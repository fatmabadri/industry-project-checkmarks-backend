using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class Contact
    {
        [JsonProperty("data")]
        public ContactData Data { get; set; }

    }
}
