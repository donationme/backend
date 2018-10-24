namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using SADJZ.Services;
    using static JWT.Controllers.TokenController;


    public sealed class LocationModel:DatabaseEntry{

        [JsonProperty("locations")]
        public List<LocationCollectionObject> Locations { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public override string Id { get; set; }



    }


    public sealed class ExportCSVLocationModel{

        [JsonProperty("csv")]
        public string CSV { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public LocationModel Location { get{return new LocationModel{Locations = LocationReader.ReadCSV(this.CSV), Name = this.Name };}}

    }

    public sealed class LocationCollectionObject:DatabaseEntry
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
        public string Address {get;set;}

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }


        [JsonProperty("id")]
        public override string Id { get; set; }

        [JsonProperty("items")]
        public List<DonationItemModel> DonationItems { get; set; }

        public static LocationCollectionObject FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            LocationCollectionObject location = new LocationCollectionObject();
            location.Key = Convert.ToInt16(values[0]);
            location.Name = Convert.ToString(values[1]);
            location.Id = Hasher.SHA256(location.Name);
            location.Latitude = Convert.ToDouble(values[2]);
            location.Longitude = Convert.ToDouble(values[3]);
            location.Street = Convert.ToString(values[4]);
            location.City = Convert.ToString(values[5]);
            location.State = Convert.ToString(values[6]);
            location.Zip = Convert.ToInt16(values[7]);
            location.Type = Convert.ToString(values[8]);
            location.Phone = Convert.ToString(values[9]);
            location.Website = Convert.ToString(values[10]);
            location.Address = location.Street + ", " + location.City + ", " + location.State + " " + location.Zip;
            location.DonationItems = new List<DonationItemModel>();
            return location;
        }


    }
}



