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

        // from free trial (test)
        // private string clioToken = "Bearer ";
            
        // real golbey clio token
        private string clioToken;

        private string contactsUrl;
        private string mattersUrl;

        private readonly IConfiguration _config;

        private ILogger<ClioController> _logger;

        public ClioController(IConfiguration config, ILogger<ClioController> logger) {
            _config = config;
            _logger = logger;

            clioToken = _config["ClioToken"];
            contactsUrl = _config["ClioContactsUrl"];
            mattersUrl = _config["ClioMattersUrl"];
        }

        [HttpPost]
        public async Task<IActionResult> PostNewContactAndMatter([FromBody] Contact c)
        {
            _logger.LogInformation($"{DateTime.Now} [api/clio] - Posting new Checkmarks contact to Clio");



            HttpClient client = new HttpClient();
            
            // POST new contact to clio
            client.DefaultRequestHeaders.Add("Authorization", clioToken);
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


        



        // disabled because client information is confidential

        // [HttpGet]
        // public async Task<IActionResult> Get()
        // {
        //     string url = "https://app.clio.com/api/v4/contacts.json"; 
        //     using (HttpClient client = new HttpClient())
        //     {
        //         client.DefaultRequestHeaders.Add("Authorization", clioToken);
        //         var temp = await client.GetStringAsync(url);
        //         return Ok(temp);
        //     }
        // }
    }


    
}