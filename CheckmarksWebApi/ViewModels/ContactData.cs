using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class ContactData
    {
        [JsonProperty("id")]
        public int Id {get;set;}

        [JsonProperty("initials")]
        public string Initials {get;set;}

        [JsonProperty("etag")]
        public string Etag{get;set;}

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("email_addresses")]
        public EmailAddress[] EmailAddresses { get; set; }
        
        [JsonProperty("phone_numbers")]
        public PhoneNumber[] PhoneNumbers { get; set; }
        
        [JsonProperty("addresses")]
        public Address[] Addresses { get; set; }
    }

    public class Address
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }
                
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }
    }

    public class PhoneNumber
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }
    }

    public class EmailAddress
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
