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

namespace CheckmarksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy")]
    public class TrademarkController
    {

    }
}
