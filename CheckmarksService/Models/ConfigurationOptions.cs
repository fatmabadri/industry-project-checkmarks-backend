using System;
using System.Collections.Generic;
using System.Text;

namespace CheckmarksService.Models
{
    public class ConfigurationOptions
    {
        public string CIPOClassUrl { get; set; }
        public string CIPOTermsBaseUrl { get; set; }
        public string ConnString { get; set; }
        public int ScheduledIntervalInMinutes { get; set; }

        // tQ: added
        public string AzureConnection { get; set; }
        public string CipoUserKey { get; set; }
    }
}
