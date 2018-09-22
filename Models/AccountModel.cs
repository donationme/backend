namespace SADJZ.Models{

    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using static JWT.Controllers.TokenController;

    public sealed class AccountModel
    {

        [JsonProperty("auth")]
        public LoginModel Auth { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }

        [JsonProperty("_id")]
        public ObjectId _id { get; set; }

    }
}
