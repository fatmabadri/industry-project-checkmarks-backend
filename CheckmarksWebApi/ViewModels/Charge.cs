using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class Charge
    {
        [JsonProperty("amount")]
        public string Amount{get;set;}

        [JsonProperty("method")]
        public string Method{get;set;}

        [JsonProperty("account_id")]
        public string AccountId{get;set;}
        
    }
}