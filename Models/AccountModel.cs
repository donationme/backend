namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using SADJZ.Services;
    using static JWT.Controllers.TokenController;

    public sealed class AccountModel:DatabaseEntry
    {


        [JsonProperty("auth")]
        public LoginModel Auth { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }

        [JsonProperty("_id")]
        public override string Id { get; set; }

    }
}
