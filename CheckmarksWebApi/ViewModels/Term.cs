using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class Term
    {
        [JsonProperty("id")]
        public int Id{get;set;}

        [JsonProperty("termName")]
        public string TermName{get;set;}
    }
}