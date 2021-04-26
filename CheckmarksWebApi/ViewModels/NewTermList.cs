using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class NewTermList
    {
        [JsonProperty("terms")]
        public NewTerm[] Terms{get;set;}
    }
}