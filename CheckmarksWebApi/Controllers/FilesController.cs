using System;
using System.Collections.Generic;
using System.IO;
using CheckmarksWebApi.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CheckmarksWebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("Policy")]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private ILogger<FilesController> _logger;
        
        public FilesController(IWebHostEnvironment hostingEnvironment, ILogger<FilesController> logger) {
            _hostingEnvironment=hostingEnvironment;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Upload(FileUploadForm form) {

            var img = form.FileToUpload;

            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath + "/images");
            string datedFilename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + img.FileName;
            string filePath = Path.Combine(uploadsFolder,datedFilename);

            bool successfulUpload = false;
            try {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                img.CopyTo(fs);
                fs.Close();
                successfulUpload = true;
            } catch(Exception e) {
                datedFilename = "File not uploaded";
                _logger.LogError($"{DateTime.Now} [api/files] - Failed to upload file. {e.ToString()}");
            }
            

            FilenameResponse response = new FilenameResponse() {
                filename = datedFilename
            };

            if (successfulUpload) {
                _logger.LogInformation($"{DateTime.Now} [api/files] - successfully uploaded {datedFilename} to wwwroot/images .");
                return Ok(response);
            } else {
                _logger.LogError($"{DateTime.Now} [api/files] - ERROR: {datedFilename} not uploaded.");
                return BadRequest(response);
            }
        }

        // to-do: get by name

        
        
        
        
        // to-do: delete by name


    }
}