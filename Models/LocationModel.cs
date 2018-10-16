namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using static JWT.Controllers.TokenController;


    public sealed class LocationModel:DatabaseEntry{

        [JsonProperty("locations")]
        public List<LocationListObject> Locations { get; set; }

    }


    public sealed class LocationListObject
    {

        [JsonProperty("key")]
        public int Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        public int Zip { get; set; }

        [JsonProperty("address")]
        public string Address { get{return this.Street + ", " + this.City + ", " + this.State + " " + this.Zip; } set{ this.Address = value;} }

        [JsonProperty("type")]
        public string Type { get; set; }


        [JsonProperty("phone")]
        public string Phone { get; set; }


        [JsonProperty("website")]
        public string Website { get; set; }


        public static LocationListObject FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            LocationListObject location = new LocationListObject();
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



