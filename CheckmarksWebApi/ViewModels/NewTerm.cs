using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class NewTerm
    {
        [JsonProperty("id")]
        public int Id{get;set;}

        [JsonProperty("termName")]
        public string TermName{get;set;}

        [JsonProperty("termClass")]
        public int TermClass{get;set;}

        [JsonProperty("classShortName")]
        public string ClassShortName{get;set;}
    }
}