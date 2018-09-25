using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SADJZ.Database;
using SADJZ.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Controllers
{
  [Route("api/[controller]")]
  public class TokenController : Controller
  {
    private IConfiguration _config;
    private AccountDatabase AccountDatabase;

    public TokenController(IConfiguration config)
    {
      _config = config;
      this.AccountDatabase = new AccountDatabase();

    }


    [HttpPost]
    public async Task<IActionResult> CreateToken([FromBody]LoginModel login)
    {
      IActionResult response = Unauthorized();
      var account = await Authenticate(login);

      if (account != null)
      {
        var tokenString = BuildToken(account);
        response = Ok(new { token = tokenString });
      }

      return response;
    }

    private string BuildToken(AccountModel account)
{

    var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, account.User.Name),
        new Claim("Username", account.Auth.Username),
        new Claim("Type", account.User.Type.ToString()),

        new Claim(JwtRegisteredClaimNames.Email, account.User.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
      _config["Jwt:Issuer"],
      claims,
      expires: DateTime.Now.AddMinutes(30),
      signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

     private async Task<AccountModel> Authenticate(LoginModel login)
     {
        AccountModel account = await this.AccountDatabase.GetAccount(login.Username);
        if (account != null){
            if (login.Username == account.Auth.Username && login.Password == account.Auth.Password)
            {
                return account;
            }
        }
        
        return null;

        
        
     }


  }
}