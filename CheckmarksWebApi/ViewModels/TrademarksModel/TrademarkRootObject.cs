using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckmarksWebApi.ViewModels.TrademarksModel
{
    public class TrademarkRootObject
    {
        public List<TrademarkData> Data { get; set; }

        public void FixUrls()
        {
            if (Data != null)
            {
                foreach (TrademarkData data in Data)
                {
                    data.FixUrls();
                }
            }
        }

        public void FixTitle()
        {
            if (Data != null)
            {
                foreach (TrademarkData data in Data)
                {
                    data.FixTitle();
                }
            }
        }
    }
}
