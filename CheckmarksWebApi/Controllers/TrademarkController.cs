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
        // tQ: initial solution only--GET requests should not affect server state (results should be cached)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trademark>>> Index(string searchString)
        {
            try {
                var trademarks = _context.Trademarks;
                if (!String.IsNullOrEmpty(searchString))
                {
                    var trademarks_query = trademarks
                        .Where(tm => (tm.Title.Contains(searchString) || tm.Owner.Contains(searchString)));
                    var trademarks_ret = trademarks_query.Select(tm => new {
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
                    return Ok(trademarks_ret);
                } else
                {
                    return BadRequest("Search string empty");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        
    }
}
