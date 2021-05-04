using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckmarksWebApi.ViewModels.TrademarksModel
{
    public class TrademarkData
    {
        public string Heading { get; set; }
        public DateTime? FileDate { get; set; }
        public DateTime? RegDate { get; set; }  //register date
        public DateTime? IntrnlRenewDate { get; set; }
        public string Owner { get; set; }
        public string StatusDescEn { get; set; } //registered, expunged, etc

        public int[] NiceClasses { get; set; }
        public int[] TmType { get; set; }
        public string[] ApplicationNumberL { get; set; }

        public string[] MediaUrls { get; set; }

        public void FixUrls()
        {
            if (MediaUrls != null)
            {
                for (int i = 0; i < MediaUrls.Length; i++)
                {
                    MediaUrls[i] = "https://www.ic.gc.ca/" + MediaUrls[i] + ".png";
                }
            }
        }

    }
}
