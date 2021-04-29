using CheckmarksWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckmarksWebApi.Data.CIPOResponse
{
    public class TMResult
    {
        public string title { get; set; }
        public DateTime fileDate { get; set; }
        public DateTime regDate { get; set; }
        public DateTime intrnlRenewDate { get; set; }
        public string owner { get; set; }
        public string statusDescEn { get; set; }
        public string[] niceClasses { get; set; }
        public string[] tmType { get; set; }
        public string[] applicationNumberL { get; set; }
        public string[] mediaUrls { get; set; }
    }
}
