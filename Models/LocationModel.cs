namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using static JWT.Controllers.TokenController;

    public sealed class LocationModel:DatabaseEntry
    {

        [JsonProperty("Key")]
        public int Key { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }

        [JsonProperty("Street")]
        public string Street { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Zip")]
        public int Zip { get; set; }

        [JsonProperty("Address")]
        public string Address { get{return this.Street + ", " + this.City + ", " + this.State + " " + this.Zip; } set{ this.Address = value;} }

        [JsonProperty("Type")]
        public string Type { get; set; }


        [JsonProperty("Phone")]
        public string Phone { get; set; }


        [JsonProperty("Website")]
        public string Website { get; set; }


        public static LocationModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            LocationModel location = new LocationModel();
            location.Key = Convert.ToInt16(values[0]);
            location.Name = Convert.ToString(values[1]);
            location.Latitude = Convert.ToDouble(values[2]);
            location.Longitude = Convert.ToDouble(values[3]);
            location.Street = Convert.ToString(values[4]);
            location.City = Convert.ToString(values[5]);
            location.State = Convert.ToString(values[6]);
            location.Zip = Convert.ToInt16(values[7]);
            location.Type = Convert.ToString(values[8]);
            location.Phone = Convert.ToString(values[9]);
            location.Website = Convert.ToString(values[10]);
            return location;
        }


    }
}



