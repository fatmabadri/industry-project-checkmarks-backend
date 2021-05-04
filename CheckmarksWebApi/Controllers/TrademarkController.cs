using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using CheckmarksWebApi.ViewModels.TrademarksModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CheckmarksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrademarkController : ControllerBase
    {
        [HttpGet("test")]
        public async Task<IActionResult> GetTrademarksTest()
        {
            string url =
                "https://www.ic.gc.ca/app/api/ic/ctr/trademarks/search/.json?dataBeanJson=%7B%22selectField1%22%3A%22tm%22%2C%22textField1%22%3A%22ubc%22%2C%22category%22%3A%22%22%2C%22type%22%3A%22%22%2C%22status%22%3A%22%22%2C%22viennaField%22%3A%5B%5D%2C%22searchDates%22%3A%5B%5D%2C%22selectMaxDoc%22%3A%22500%22%2C%22language%22%3A%22eng%22%7D&start=0&length=25";

            string response = "failed";
            TrademarkRootObject data = new TrademarkRootObject();
            using (HttpClient client = new HttpClient())
            {
                response = await client.GetStringAsync(url);
                // data = await client.GetAsync();
                // Debug.WriteLine(data);

                data = JsonConvert.DeserializeObject<TrademarkRootObject>(response);


                Console.WriteLine("hello: " + data);

                return Ok(data);
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetTrademarks(string name)
        {
            Debug.WriteLine("hi");
            string url =
                "https://www.ic.gc.ca/app/api/ic/ctr/trademarks/search/.json?dataBeanJson=%7B%22selectField1%22%3A%22tm%22%2C%22textField1%22%3A%22"; 
                
            url += name + "%22%2C%22category%22%3A%22%22%2C%22type%22%3A%22%22%2C%22status%22%3A%22%22%2C%22viennaField%22%3A%5B%5D%2C%22searchDates%22%3A%5B%5D%2C%22selectMaxDoc%22%3A%22500%22%2C%22language%22%3A%22eng%22%7D&start=0&length=99999";

            TrademarkRootObject data = new TrademarkRootObject();

            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(url);
                // data = await client.GetAsync();
                // Debug.WriteLine(data);

                data = JsonConvert.DeserializeObject<TrademarkRootObject>(response);

                data.FixUrls();

                return Ok(data);
            }
        }
    }
}