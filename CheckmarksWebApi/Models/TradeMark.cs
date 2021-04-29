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

        public string _niceClasses { get; set; }
        [NotMapped]
        public int[] NiceClasses
        {
            get
            {
                var strings = _niceClasses.Split(",");
                return Array.ConvertAll(strings, int.Parse);
            }
            set
            {
                _niceClasses = string.Join(",", value);
            }
        }
        // 0	9
        // 1	10
        // 2	16
        // 3	18
        // 4	20
        // 5	28
        public string _tmType { get; set; }
        [NotMapped]
        public int[] TmType
        {
            get {
                var strings = _tmType.Split(",");
                return Array.ConvertAll(strings, int.Parse);
            }
            set
            {
                _tmType = string.Join(",", value);
            }
        }
        // 0	1
        public string _applicationNumberL { get; set; }
        [NotMapped]
        public string[] ApplicationNumberL
        {
            get { return _applicationNumberL.Split(","); }
            set
            {
                _applicationNumberL = string.Join(",", value);
            }
        }
        // 0	"1060300"
        // 1	"106030000"
        // 2	"1060300-00"
        public string _mediaUrls { get; set; }
        [NotMapped]
        public string[] MediaUrls
        {
            get { return _mediaUrls.Split(","); }
            set
            {
                _mediaUrls = string.Join(",", value);
            }
        }
        // null

        public Trademark(string title,
            DateTime fileDate,
            DateTime regDate,
            DateTime intrnlRenewDate,
            string owner,
            string statusDescEn,
            int[] niceClasses,
            int[] tmType,
            string[] applicationNumberL,
            string[] mediaUrls)
        {
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
