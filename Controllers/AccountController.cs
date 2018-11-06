using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SADJZ.Consts;
using SADJZ.Database;
using SADJZ.Models;
using SADJZ.Services;

namespace SADJZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private bool privligedMode = false;
        private DatabaseInterfacer<AccountModel> DatabaseInterfacer;

        public AccountController()
        {
            this.DatabaseInterfacer = new DatabaseInterfacer<AccountModel>(DatabaseEndpoints.account);
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<UserModel>> GetUser()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string username = currentUser.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            if (username != null){
                AccountModel account = await this.DatabaseInterfacer.GetModel(Hasher.SHA256(username));
                return account.User;
            }

            return Unauthorized();
        }

        // POST api/account
        [HttpPost]
        public async Task<IActionResult> PostCreateAccount([FromBody] AccountModel model)
        {


            
            ClaimsPrincipal currentUser = HttpContext.User;

            UserType userType =  TypeFinder.TypeFromClaim(currentUser);

            if (userType == UserType.Admin){
                return await CreateAccount(model, true);
            }


            return await CreateAccount(model, false);



        }
        public async Task<IActionResult> CreateAccount(AccountModel model, bool isAdmin)
        {




                if ((model.User.Type == UserType.User || privligedMode == false) || isAdmin)
                {
                    model.Id = Hasher.SHA256(model.Auth.Username);
                    bool addSuccess = await this.DatabaseInterfacer.AddModel(model);
                    if (addSuccess)
                    {   
                        string[] noErrors = {};
                        return Ok(noErrors);
                    }
                    else
                    {
                        var resp = Content("{" + '"' + "DuplicateUser" + '"' + ":[" +'"' + "The user already exists" + '"' + "]}");
                        resp.StatusCode = 400;
                        return resp;
                    }
                }
                else
                {            
                    var resp = Content("{" + '"' + "PrivledgeError" + '"' + ":[" +'"' + "You are not privledged to make this account type" + '"' + "]}");
                    resp.StatusCode = 400;
                    return resp;
                }
            
           



        }

    }
}