using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CheckmarksWebApi.Models;
using CheckmarksWebApi.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;

namespace CheckmarksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy")]
    public class PaymentController : ControllerBase
    {
        private string url;
        private string auth;

        private readonly IConfiguration _config;

        private ILogger<PaymentController> _logger;

        public PaymentController(IConfiguration config, ILogger<PaymentController> logger) {
            _config = config;
            _logger = logger;

            url = _config["PaymentUrl"];
            auth = _config["PaymentAuth"];
        }
        

        [HttpPost]
        public async Task<IActionResult> PostCharge([FromBody] Charge pc)
        {
            _logger.LogInformation($"{DateTime.Now} [api/payment] - Charging {pc.Amount}cents to method {pc.Method} going to account {pc.AccountId} .");
            
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", auth);
                
                var temp = await client.PostAsync(url,
                  new StringContent(JsonConvert.SerializeObject(pc), Encoding.UTF8, "application/json"));

                if (temp.IsSuccessStatusCode) {
                    var contents = await temp.Content.ReadAsStringAsync();
                    _logger.LogInformation($"{DateTime.Now} [api/payment] - Charge successfuly posted.");
                    return Ok(contents);
                } else {
                    _logger.LogError($"{DateTime.Now} [api/payment] - Charge failed to post.");
                    return BadRequest("Charge unsuccessful");
                }

                
            }
        }

    }
}