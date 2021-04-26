using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class Matter
    {
        [JsonProperty("data")]
        public MatterData MatterData {get;set;}
    }

    public class MatterData
    {
        [JsonProperty("client")]
        public MatterDataClient Client {get;set;}

        [JsonProperty("description")]
        public string Description {get;set;}
    }

    public class MatterDataClient
    {
        [JsonProperty("id")]
        public int Id {get;set;}
    }
}