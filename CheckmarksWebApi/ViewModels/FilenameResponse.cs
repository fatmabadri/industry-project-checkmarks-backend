using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class FilenameResponse
    {
        [JsonProperty("filename")]
        public string filename{get;set;}
    }
}