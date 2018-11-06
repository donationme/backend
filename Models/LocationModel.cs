namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using SADJZ.Services;
    using static JWT.Controllers.TokenController;


    public sealed class RegionModel:DatabaseEntry{
        [Required]
        public List<LocationModel> Locations { get; set; }

        public Coords RegionCoords {get; set;}
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Name { get; set; }
        public override string Id { get; set; }



    }


    public sealed class Coords{
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }

    public sealed class ExportCSVRegionModel{
        [Required]
        public string CSV { get; set; }
        [Required]
        public Coords RegionCoords {get; set;}
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Name { get; set; }
        [Required]
        public RegionModel Region { get{return new RegionModel{Locations = RegionReader.ReadCSV(this.CSV), Name = this.Name, RegionCoords = this.RegionCoords };}}

    }

    public sealed class LocationModel:DatabaseEntry
    {
        [Required]
        public int Key { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(200)]        
        public string Name { get; set; }
        [Required]
        public Coords Coords { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Street { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string City { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string State { get; set; }
        [Required]
        public int Zip { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Address {get;set;}
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Type { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Phone { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Website { get; set; }

        public override string Id { get; set; }
        [Required]

        public List<DonationItemModel> DonationItems { get; set; }

        public static LocationModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            LocationModel location = new LocationModel();
            location.Key = Convert.ToInt16(values[0]);
            location.Name = Convert.ToString(values[1]);
            location.Id = Hasher.SHA256(location.Name);
            location.Coords = new Coords{Latitude = Convert.ToDouble(values[2]), Longitude = Convert.ToDouble(values[3])};
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



