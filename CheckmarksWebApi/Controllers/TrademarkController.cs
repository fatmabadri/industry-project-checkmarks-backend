using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckmarksWebApi.Models;
using Microsoft.AspNetCore.Cors;
using CheckmarksWebApi.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using CheckmarksWebApi.Data.CIPOResponse;
using Newtonsoft.Json;

namespace CheckmarksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy")]
    public class TrademarkController : ControllerBase
    { 
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private ILogger<EmailController> _logger;

        public TrademarkController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context, IConfiguration configuration, ILogger<EmailController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/trademark
        //[HttpGet("{searchString}")]
        public async Task<ActionResult<IEnumerable<Trademark>>> Index(string searchString)
        {
            try {
                var trademarksDb = _context.Trademarks;

                if (!String.IsNullOrEmpty(searchString))
                {
                    // tQ: query cache first
                    var trademarksQuery = trademarksDb
                        .Where(tm => (tm.Title.Contains(searchString) || tm.Owner.Contains(searchString)));
                    if (trademarksQuery.ToList().Count < 1 ) { 
                        var trademarksLst = await GetTMsFromCIPO(searchString);
                        
                        // tQ: TODO add to db
                    }


                    var trademarksRet = trademarksQuery.Select(tm => new {
                        tm.Title,
                        tm.FileDate,
                        tm.RegDate,
                        tm.IntrnlRenewDate,
                        tm.Owner,
                        tm.StatusDescEn,
                        tm.NiceClasses,
                        tm.TmType,
                        tm.ApplicationNumberL,
                        tm.MediaUrls
                    });
                    return Ok(trademarksRet);
                } 
                else
                {
                    return BadRequest("Search string empty");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{searchString}")]
        public async Task<ActionResult<IEnumerable<Trademark>>> GetTMsFromCIPO(string searchString)
        {
            // tQ: full string
            var cipoSearchString = $"https://www.ic.gc.ca/app/api/ic/ctr/trademarks/search/.json?dataBeanJson=%7B%22selectField1%22%3A%22all%22%2C%22textField1%22%3A%22{searchString}%22%2C%22category%22%3A%22%22%2C%22type%22%3A%22%22%2C%22status%22%3A%22%22%2C%22viennaField%22%3A%5B%5D%2C%22searchDates%22%3A%5B%5D%2C%22selectMaxDoc%22%3A%22500%22%2C%22language%22%3A%22eng%22%7D&draw=1&columns%5B0%5D%5Bdata%5D=applicationNumber&columns%5B0%5D%5Bname%5D=&columns%5B0%5D%5Bsearchable%5D=true&columns%5B0%5D%5Borderable%5D=true&columns%5B0%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B0%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B1%5D%5Bdata%5D=intrnlRegNum&columns%5B1%5D%5Bname%5D=&columns%5B1%5D%5Bsearchable%5D=true&columns%5B1%5D%5Borderable%5D=true&columns%5B1%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B1%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B2%5D%5Bdata%5D=title&columns%5B2%5D%5Bname%5D=&columns%5B2%5D%5Bsearchable%5D=true&columns%5B2%5D%5Borderable%5D=true&columns%5B2%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B2%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B3%5D%5Bdata%5D=tmType&columns%5B3%5D%5Bname%5D=&columns%5B3%5D%5Bsearchable%5D=true&columns%5B3%5D%5Borderable%5D=false&columns%5B3%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B3%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B4%5D%5Bdata%5D=statusDescEn&columns%5B4%5D%5Bname%5D=&columns%5B4%5D%5Bsearchable%5D=true&columns%5B4%5D%5Borderable%5D=true&columns%5B4%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B4%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B5%5D%5Bdata%5D=niceClasses&columns%5B5%5D%5Bname%5D=&columns%5B5%5D%5Bsearchable%5D=true&columns%5B5%5D%5Borderable%5D=false&columns%5B5%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B5%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B6%5D%5Bdata%5D=mediaUrls&columns%5B6%5D%5Bname%5D=&columns%5B6%5D%5Bsearchable%5D=true&columns%5B6%5D%5Borderable%5D=false&columns%5B6%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B6%5D%5Bsearch%5D%5Bregex%5D=false&start=0&length=500&search%5Bvalue%5D=&search%5Bregex%5D=false&_=1619560007739";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(cipoSearchString);

            if (response.IsSuccessStatusCode)
            {
                string dataResponse = await response.Content.ReadAsStringAsync();

                Root apiObjects = JsonConvert.DeserializeObject<Root>(dataResponse);

                foreach (var tm in apiObjects.data)
                {
                    _context.Trademarks.Add(
                        new Trademark(
                            tm.title,
                            tm.fileDate,
                            tm.regDate,
                            tm.intrnlRenewDate,
                            tm.owner,
                            tm.statusDescEn,
                            tm.niceClasses,
                            tm.tmType,
                            tm.applicationNumberL,
                            tm.mediaUrls
                        )
                    );

                    // int[] NiceClasses
                    // int[] TmType 
                    // string[] ApplicationNumberL 
                    // string[] MediaUrls 

                    HashSet<int> set = new HashSet<int>();

                    foreach (var nc in tm.niceClasses)
                    {
                        set.Add(nc);
                    }

                    foreach (var nc in set)
                    {
                        tm.niceClasses.Add(new MovieGenre(m.id, g));

                    }
                }
            }
            else
            {
                return BadRequest("There was a problem getting results from the CIPO API");
            }
        }
    }
}
