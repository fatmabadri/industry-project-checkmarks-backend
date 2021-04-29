using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckmarksWebApi.Models
{
    public class Trademark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ApplicationNumber { get; set; }
        public string Title { get; set; }
        // "NIKE"
        public DateTime FileDate { get; set; }
        // "2000-05-24T00:00:00"
        public DateTime RegDate { get; set; }
        // "2003-06-11T00:00:00"
        public DateTime IntrnlRenewDate { get; set; }
        // "2033-06-11T00:00:00"
        public string Owner { get; set; }
        // "NIKE INNOVATE C.V."
        public string StatusDescEn { get; set; }
        // "REGISTERED"
        public string[] NiceClasses { get; set; }
        // 0	9
        // 1	10
        // 2	16
        // 3	18
        // 4	20
        // 5	28
        public string[] TmType { get; set; }
        // 0	1
        public string[] ApplicationNumberL { get; set; }
        // 0	"1060300"
        // 1	"106030000"
        // 2	"1060300-00"
        public string[] MediaUrls { get; set; }
        // null

        public Trademark(string applicationnumber,
            string title,
            DateTime fileDate,
            DateTime regDate,
            DateTime intrnlRenewDate,
            string owner,
            string statusDescEn,
            string[] niceClasses,
            string[] tmType,
            string[] applicationNumberL,
            string[] mediaUrls)
        {
            ApplicationNumber = applicationnumber;
            Title = title;
            FileDate = fileDate;
            RegDate = regDate;
            IntrnlRenewDate = intrnlRenewDate;
            Owner = owner;
            StatusDescEn = statusDescEn;
            NiceClasses = niceClasses;
            TmType = tmType;
            ApplicationNumberL = applicationNumberL;
            MediaUrls = mediaUrls;
        }
    }
}
