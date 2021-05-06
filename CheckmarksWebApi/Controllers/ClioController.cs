using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CheckmarksWebApi.Models;
using CheckmarksWebApi.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CheckmarksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy")]
    public class ClioController : ControllerBase
    {

        private string refreshToken;
        private string contactsUrl;
        private string mattersUrl;
        private string tokenUrl;

        private HttpClient client;

        private readonly IConfiguration _config;

        private ILogger<ClioController> _logger;

        public ClioController(IConfiguration config, ILogger<ClioController> logger) {
            _config = config;
            _logger = logger;

            refreshToken = _config["ClioRefreshToken"];
            contactsUrl = _config["ClioContactsUrl"];
            mattersUrl = _config["ClioMattersUrl"];
            tokenUrl = _config["ClioTokenUrl"];
            client = new HttpClient();
        }

        [HttpPost]
        public async Task<IActionResult> PostNewContactAndMatter([FromBody] Contact c)
        {
            _logger.LogInformation($"{DateTime.Now} [api/clio] - Posting new Checkmarks contact to Clio");
            
            var newtoken = "Bearer ";
            try {
                var tmp = await getNewToken();
                // append the new token to 'Bearer '
                newtoken += tmp;
                _logger.LogInformation($"{DateTime.Now} [api/clio] - got a new Clio Access Token: {newtoken}");
            } catch(Exception e) {
                _logger.LogError($"{DateTime.Now} [api/clio] - Token Refresh Failed; {e.StackTrace}");
                return BadRequest($"Token Refresh Failed; {e.StackTrace}");
            }
            

            // POST new contact to clio
            client.DefaultRequestHeaders.Add("Authorization", newtoken);
            var temp = await client.PostAsync(contactsUrl,
                new StringContent(JsonConvert.SerializeObject(c), Encoding.UTF8, "application/json"));
            
            var response = await temp.Content.ReadAsStringAsync();

            if (!temp.IsSuccessStatusCode) {
                _logger.LogError($"{DateTime.Now} [api/clio] - Error in posting new contact.");
                _logger.LogError(response);
                return BadRequest(temp);
            }

            // POST new contact successful

            _logger.LogInformation($"{DateTime.Now} [api/clio] - Clio POST Contact:\n"+temp 
                + "\nContact Creation Response:\n" + response+"\n");

            
            // get new contact's ID
            Contact createdContact = JsonConvert.DeserializeObject<Contact>(response);

            // create a new Matter
            Matter newMatter = new Matter() {
                MatterData = new MatterData() {
                    Client = new MatterDataClient() {
                        Id = createdContact.Data.Id
                    },
                    Description = "Checkmarks.ca trademark registration"
                },
            };

            var postNewMatter = await client.PostAsync(mattersUrl,
                new StringContent(JsonConvert.SerializeObject(newMatter), Encoding.UTF8, "application/json"));
            
            var matterResponse = await postNewMatter.Content.ReadAsStringAsync();
            
            if (!postNewMatter.IsSuccessStatusCode) {
                _logger.LogError($"{DateTime.Now} [api/clio] - Error in posting new matter.");
                _logger.LogError(matterResponse);
                return BadRequest(postNewMatter);
            }

            // POST new matter successful
            _logger.LogInformation($"{DateTime.Now} [api/clio] - Clio POST Matter:\n"+postNewMatter 
                + "\nMatter Creation Response:\n" + matterResponse+"\n");


            return Ok(matterResponse);
        }

        // gets a new token
        private async Task<String> getNewToken() {
            var client_id = _config["ClientID"];
            var client_secret = _config["ClientSecret"];

            // build refresh request
            var form = new Dictionary<string, string>();
            form.Add("grant_type", "refresh_token");
            form.Add("refresh_token", $"{refreshToken}");
            form.Add("client_id", $"{client_id}");
            form.Add("client_secret", $"{client_secret}");
            var formEncoded = new FormUrlEncodedContent(form);

            // send request
            var tokenRefreshResponse = await client.PostAsync(tokenUrl, formEncoded);
            
            if (tokenRefreshResponse.IsSuccessStatusCode) {
                var content = await tokenRefreshResponse.Content.ReadAsStringAsync();
                TokenRefreshResponse trr = JsonConvert.DeserializeObject<TokenRefreshResponse>(content);
                return trr.access_token;
            } else {
                Console.WriteLine(tokenRefreshResponse);
                throw new Exception(tokenRefreshResponse.ReasonPhrase);
            }
        }
    }

    public class TokenRefreshResponse {
        [JsonProperty("access_token")]
        public string access_token;
    }
}