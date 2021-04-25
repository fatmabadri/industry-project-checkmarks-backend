using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CheckmarksWebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy")]
    public class PingController : ControllerBase {

        private readonly IConfiguration _configuration;

        private ILogger<PingController> _logger;

        public PingController(IConfiguration configuration, ILogger<PingController> logger) {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult TakeThis() {
            string msg = _configuration["TestMessage"];
            _logger.LogInformation($"{DateTime.Now} - ping logged!");
            return Ok(msg);
        }
    }
}