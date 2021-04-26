using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class TermList
    {
        [JsonProperty("terms")]
        public Term[] Terms{get;set;}
    }
}