namespace SADJZ.Models{
    using System.ComponentModel.DataAnnotations;
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

        [Required]
        public LoginModel Auth { get; set; }
        [Required]
        public UserModel User { get; set; }
        public override string Id { get; set; }

    }
}
