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

        [JsonProperty("locationid")]
        public string LocationId { get; set; }

        [JsonProperty("id")]
        public override string Id { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("name")]
        public string Name { get; set; }


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
