using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

public class LoginModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Username { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(36)]
        public string Password { get; set; }
    }