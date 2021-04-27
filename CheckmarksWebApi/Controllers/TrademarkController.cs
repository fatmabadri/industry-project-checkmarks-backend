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
    public class TrademarkController
    { 
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private ILogger<EmailController> _logger;
        private string uploadsFolder;

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
        public async Task<ActionResult<IEnumerable<TradeMark>>> Index(string searchString)
        {
            return await _context.Trademarks.ToListAsync();
        }

        
    }
}
