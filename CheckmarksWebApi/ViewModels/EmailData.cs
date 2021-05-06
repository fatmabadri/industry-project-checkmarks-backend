using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class EmailData
    {
        [JsonProperty("name")]
        public string Name {get;set;}
        
        [JsonProperty("type")]
        public string Type {get;set;}

        [JsonProperty("organizationName")]
        public string OrganizationName{get;set;}

        [JsonProperty("amountPaid")]
        public int AmountPaid {get;set;}
        
        [JsonProperty("trademarkInfo")]
        public TrademarkInfo TrademarkInfo {get;set;}
        
        [JsonProperty("prevTrademarkInfo")]
        public PreviousTrademark PreviousTrademark {get;set;}
        
        [JsonProperty("contactInfo")]
        public ContactInfo ContactInfo {get;set;}

        [JsonProperty("addressInfo")]
        public Address AddressInfo {get;set;}

        [JsonProperty("IdFileName")]
        public string IdFileName { get; set; }
    }

    public class ContactInfo
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress {get;set;}

        [JsonProperty("phoneNumber")]
        public string Phone {get;set;}

        [JsonProperty("fax")]
        public string Fax {get;set;}

        [JsonProperty("matterId")]
        public string MatterId {get;set;}
    }

    public class PreviousTrademark
    {
        [JsonProperty("filedInOtherCountry")]
        public string FiledInOtherCountry {get;set;}
        
        [JsonProperty("countryOfFiling")]
        public string Country {get;set;}

        // filing date??
        [JsonProperty("filingDate")]
        public string FilingDate {get;set;}

        [JsonProperty("applicationNum")]
        public string ApplicationNumber {get;set;}

        
    }

    public class TrademarkInfo
    {
        [JsonProperty("trademarkType")]
        public IList<String> Types {get;set;}

        [JsonProperty("characterText")]
        public string Text {get;set;}

        [JsonProperty("fileName")]
        public string FileName {get;set;}

        //
        //todo: niceclasses
        //

        [JsonProperty("classes")]
        public IList<Int32> ClassIds {get;set;}

        [JsonProperty("terms")]
        public IList<IList<String>> TermNames {get;set;}
    }
}