using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SADJZ.Database;
using SADJZ.Models;
using SADJZ.Validation;

namespace SADJZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private AccountDatabase AccountDatabase;
        private AccountValidator AccountValidator;

        public AccountController()
        {
            this.AccountValidator = new AccountValidator();
            this.AccountDatabase = new AccountDatabase();
        }

        [HttpGet, Authorize]
        public async Task<UserModel> GetUser()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            AccountModel account = await this.AccountDatabase.GetAccount(currentUser.Claims.FirstOrDefault(c => c.Type == "Username").Value);

            return account.User;
        }



        // POST api/account
        [HttpPost]
        public async Task<IActionResult> CreateNormalAccount([FromBody] AccountModel model)
        {

            ClaimsPrincipal currentUser = HttpContext.User;

            string typeString = currentUser.Claims.FirstOrDefault(c => c.Type == "Type").Value;
            if (typeString != null){
                UserType userType = Enum.Parse<UserType>(typeString);
                if (userType == UserType.Admin){
                    return await CreateAccount(model, true);
                }
            }


            return await CreateAccount(model, false);



        }
        public async Task<IActionResult> CreateAccount(AccountModel model, bool isAdmin)
        {

            ValidationResult response = AccountValidator.Validate(model);


            if (response.IsValid)
            {
                if (model.User.Type == UserType.User || isAdmin)
                {
                    bool addSuccess = await this.AccountDatabase.AddAccount(model);
                    if (addSuccess)
                    {
                        return Ok("Success");
                    }
                    else
                    {
                        ValidationFailure[] validationFailure = { new ValidationFailure("duplicateUserError", "The user already exists") };
                        return Conflict(validationFailure);
                    }
                }
                else
                {
                    ValidationFailure[] validationFailure = { new ValidationFailure("privledgeError", "You are not privledged to make this account type") };
                    return Conflict(validationFailure);
                }
            }
            else
            {
                return Conflict(response.Errors.ToArray());
            }



        }

    }
}