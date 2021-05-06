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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CheckmarksWebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("Policy")]
    public class EmailController : ControllerBase
    {
        private string fromAddress;

        // app specific password        
        private string fromPassword;

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        
        private ILogger<EmailController> _logger;

        private string uploadsFolder;

        public EmailController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context, IConfiguration configuration, ILogger<EmailController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _configuration = configuration;
            _logger = logger;

            // local
            uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath + "/images/");

            // constants
            fromAddress = _configuration["EmailFrom"];
            fromPassword = _configuration["EmailFromPwd"];
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] Email email)
        {
            EmailData ed = email.Data;

            _logger.LogInformation($"{DateTime.Now} [api/email] - Attempting to build email to {ed.ContactInfo.EmailAddress}");

            MimeMessage msg = new MimeMessage();
            MailboxAddress from = new MailboxAddress("Checkmarks", fromAddress);
            msg.From.Add(from);
            msg.Bcc.Add(from);
            
            MailboxAddress to = new MailboxAddress(ed.Name, ed.ContactInfo.EmailAddress);
            msg.To.Add(to);

            msg.Subject = "[Checkmarks] Trademark Registration Request";

            BodyBuilder bodyBuilder = new BodyBuilder();

            string heading = "<h1>Checkmarks</h1>"
                + $"<h2>By Golbey Law</h2>"
                + "<br/><br/>";

            string applicationInfo = "<p><b>** Important ** </b>Please follow up with the parties in this email to progress with the trademark registration.</p>"
                + "<p>A trademark registration was submitted via Checkmarks.ca</p>"
                + $"<b>Registering as:</b> {ed.Type}<br/>"
                ;

            if (!string.IsNullOrEmpty(ed.OrganizationName) && ed.Type != "Person")
            {
                applicationInfo += $"<b>      Company Name:</b> {ed.OrganizationName}<br/>";
            }

            string matterid = string.IsNullOrEmpty(ed.ContactInfo.MatterId) ? "no matter ID" : ed.ContactInfo.MatterId;
            applicationInfo += $"<b>Amount Paid to Trust:</b> ${ed.AmountPaid}.00<br/>"
                + $"The above amount will be checked by a representative before work on this registration commences.<br/><br/>"
                + $"<b>Matter ID:</b> {matterid}<br/>"
                + "<br/><br/>"
                ;

            string primaryContact = "<h2>Primary Contact Information</h2>"
                + $"<b>Name:</b> {ed.Name}<br/>"
                + $"<b>Email Address:</b> {ed.ContactInfo.EmailAddress}<br/>"
                + $"<b>Phone Number:</b> {ed.ContactInfo.Phone}<br/>"
                + $"<b>Fax:</b> {ed.ContactInfo.Fax}<br/>"
                ;

            string addressOnFile = "";
            if (ed.AddressInfo != null)
            {
                addressOnFile = "<h3>Contact Address on File:</h3>"
                + $"<b>Street Address:</b> {ed.AddressInfo.Street}<br/>"
                + $"<b>City:</b> {ed.AddressInfo.City}<br/>"
                + $"<b>Province / State:</b> {ed.AddressInfo.Province}<br/>"
                + $"<b>Country:</b> {ed.AddressInfo.Country}<br/>"
                + $"<b>Postal Code:</b> {ed.AddressInfo.PostalCode}<br/><br/>"
                ;
            }

            string trademarkDetails = "";
            if (ed.TrademarkInfo != null)
            {
                trademarkDetails = "<h2>Trademark Details</h2>";

                trademarkDetails += "<h3>Selected Trademark Types:</h3><ul>";
                bool choseDesign = false;
                bool choseStandardChars = false;
                foreach (var tmType in ed.TrademarkInfo.Types)
                {
                    if (tmType == "Design/Logo") choseDesign = true;
                    if (tmType == "Standard Characters") choseStandardChars = true;
                    trademarkDetails += $"<li>{tmType}</li>";
                }
                trademarkDetails += "</ul><br/>";

                if (choseStandardChars) {
                    trademarkDetails += $"<b>Character Text:</b> {ed.TrademarkInfo.Text}<br/>";
                }

                if (!string.IsNullOrEmpty(ed.TrademarkInfo.FileName) && choseDesign)
                {
                    string fn = uploadsFolder + ed.TrademarkInfo.FileName;
                    if (System.IO.File.Exists(fn)) {
                        try {
                            bodyBuilder.Attachments.Add(fn);
                            ed.TrademarkInfo.FileName += $" (see attached).";
                        } catch (Exception e) {
                            ed.TrademarkInfo.FileName += $" (failed to attach)."
                                // debug statement in email
                                // + $"Error {e}"
                                ;
                            _logger.LogError($"{DateTime.Now} [api/email] - Failed to attach file.\n{e.ToString()}");
                        }
                    }
                    _logger.LogInformation($"{DateTime.Now} [api/email] - Attached {ed.TrademarkInfo.FileName} to email.");
                }
                else
                {
                    ed.TrademarkInfo.FileName = "none";
                }
                
                trademarkDetails += $"<b>Image File:</b> {ed.TrademarkInfo.FileName}<br/>";



                trademarkDetails += "<h3>Selected Trademark Terms:</h3>";

                for (int i = 0; i < ed.TrademarkInfo.ClassIds.Count; i++)
                {
                    var id = ed.TrademarkInfo.ClassIds[i];
                    trademarkDetails += $"<b>Class {id}: </b>";
                    var nicecl = await _context.NICEClasses.FindAsync(id);
                    trademarkDetails += $"{nicecl.ShortName}<ul>";

                    foreach (var term in ed.TrademarkInfo.TermNames[i])
                    {
                        trademarkDetails += $"<li>{term}</li>";
                    }
                    trademarkDetails += "</ul><br/>";
                }

            }

            string prevTrademarkInfo = "";
            if (ed.PreviousTrademark != null)
            {
                prevTrademarkInfo = "<h2>Previous Trademark Information</h2>";
                prevTrademarkInfo += $"<b>Was this filed in another country?:</b> {ed.PreviousTrademark.FiledInOtherCountry}<br/>";
                
                if (ed.PreviousTrademark.FiledInOtherCountry == "Yes") {prevTrademarkInfo += $"<b>Country:</b> {ed.PreviousTrademark.Country}<br/>"
                    + $"<b>Filing Date:</b> {ed.PreviousTrademark.FilingDate}<br/>"
                    + $"<b>Application Number:</b> {ed.PreviousTrademark.ApplicationNumber}<br/>"
                    ;
                }
            }

            string signOff = "<br/><p><b>** Important ** </b>Please follow up with the parties in this email to progress with the trademark registration.</p>";


            bodyBuilder.HtmlBody = heading + applicationInfo + primaryContact + addressOnFile + trademarkDetails + prevTrademarkInfo + signOff;
            msg.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();

            // try sending
            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(fromAddress, fromPassword);
                client.Send(msg);
            }
            catch (Exception e)
            {
                string errmsg = $"{DateTime.Now} [api/email] - Something went wrong with the SMTP Client: ";
                _logger.LogError(errmsg + e.ToString());

                // delete pic from filesystem after sending it
                if (!string.IsNullOrEmpty(ed.TrademarkInfo.FileName))
                {
                    deleteFile(uploadsFolder + ed.TrademarkInfo.FileName);
                }

                // quick check to delete old files
                deleteOldPics();

                return BadRequest(errmsg);
            }


            client.Disconnect(true);
            client.Dispose();

            _logger.LogInformation($"{DateTime.Now} [api/email] - Email successfully sent to {ed.ContactInfo.EmailAddress}");

            // delete pic from filesystem after sending it
            if (ed.TrademarkInfo != null && !string.IsNullOrEmpty(ed.TrademarkInfo.FileName))
            {
                deleteFile(uploadsFolder + ed.TrademarkInfo.FileName);
            }

            // quick check to delete old files
            deleteOldPics();

            return Ok("email successfully sent.");
        }


        // searches through wwwroot/images to see if there are files older than 1hr. if so, delete that file
        private void deleteOldPics()
        {

            var filenames = System.IO.Directory.GetFiles(uploadsFolder);

            foreach (var filename in filenames)
            {

                var creationTime = System.IO.File.GetCreationTime(filename);
                if (DateTime.Compare(creationTime, DateTime.Now.AddHours(-1)) < 0)
                {
                    _logger.LogInformation($"{DateTime.Now} [api/email] - Old file detected: {filename} @ {creationTime}\n");
                    try
                    {
                        // delete the file
                        System.IO.File.Delete(filename);
                        _logger.LogInformation($"{DateTime.Now} [api/email] - Deleted old file {filename}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"{DateTime.Now} [api/email] - Error deleting {filename}: {e.ToString()}");
                    }
                }
            }
        }

        // checks system if file exists, then delete it
        private void deleteFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                try
                {
                    System.IO.File.Delete(filename);
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"{DateTime.Now} [api/email] - could not delete file {filename}: {e.ToString()}");
                }
            }
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] string testToAddress) {

            SmtpClient smtp = new SmtpClient();
            MimeMessage msg = createTestMessage(fromAddress, testToAddress);

            try {
                smtp.Connect("smtp.gmail.com",465,true);
                smtp.Authenticate(fromAddress, fromPassword);
                smtp.Send(msg);
            } catch (Exception e) {
                _logger.LogInformation(e.ToString());
                return BadRequest("something went wrong");
            }


            return Ok("test email successful");
        }

        private MimeMessage createTestMessage(string fromm, string too) {
            MimeMessage msg = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Tester", fromm);
            msg.From.Add(from);

            MailboxAddress to = new MailboxAddress("User", too);
            msg.To.Add(to);

            msg.Subject = "test subject";

            BodyBuilder bodyBuilder = new BodyBuilder();

            bodyBuilder.TextBody = "test text body";

            msg.Body = bodyBuilder.ToMessageBody();

            return msg;
        }

        // private MimeMessage createJustinNotificationMessage(string matterId) {
        //     MimeMessage msg = new MimeMessage();
            
        //     MailboxAddress from = new MailboxAddress("Checkmarks", fromAddress);
        //     msg.From.Add(from);
        //     msg.To.Add(from);

        //     msg.Subject = "New Checkmarks Application";

        //     BodyBuilder bodyBuilder = new BodyBuilder();

        //     bodyBuilder.TextBody = $"A new checkmarks application has come in. Check Clio for matter ID {matterId}.";

        //     msg.Body = bodyBuilder.ToMessageBody();

        //     return msg;
        // }
    }
}