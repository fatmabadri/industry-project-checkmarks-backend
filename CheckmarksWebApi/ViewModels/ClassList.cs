using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class ClassList
    {
        [JsonProperty("classes")]
        public ClassJson[] Classes {get;set;}
    }
}