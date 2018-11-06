namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public sealed class DonationItemModel: DatabaseEntry
    {
        public string LocationId { get; set; }
        public override string Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Description { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Name { get; set; }

        [Required]
        [Range(1,100)]
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
