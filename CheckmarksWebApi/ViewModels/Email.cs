using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class Email
    {
        [JsonProperty("data")]
        public EmailData Data {get;set;}
    }
}