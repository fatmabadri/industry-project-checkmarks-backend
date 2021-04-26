using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class ClassJson
    {
        [JsonProperty("id")]
        public int Id{get;set;}

        [JsonProperty("name")]
        public string Name{get;set;}
    }
}