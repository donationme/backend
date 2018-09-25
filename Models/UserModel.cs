namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public sealed class UserModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserType Type { get; set; }


    }

    public enum UserType{
        Admin,
        User,
        LocationEmployee,
        Manager
    }

}
