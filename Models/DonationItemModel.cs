namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public sealed class DonationItemModel: DatabaseEntry
    {

        [JsonProperty("id")]
        public override string Id { get; set; }

        [JsonProperty("timeDonated")]
        public DateTime TimeDonated { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }


        [JsonProperty("title")]
        public string Title { get; set; }


        [JsonProperty("quantity")]
        public long Quantity { get; set; }


        [JsonConverter(typeof(StringEnumConverter))]
        public ItemCategory Category { get; set; }



    }

    public enum ItemCategory{
        Food,
        Clothing,
        Furniture,
        Drink,
        Accessory,
        Electronics,
        Other
    }

   
}
