namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public sealed class UserModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [MinLength(1)]
        [MaxLength(36)]
        public string Email { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserType UserType { get; set; }


    }

    public enum UserType{
        Admin,
        User,
        LocationEmployee,
        Manager
    }

}
