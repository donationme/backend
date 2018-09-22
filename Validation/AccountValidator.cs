using FluentValidation;
using SADJZ.Models;

namespace SADJZ.Validation{
  public class AccountValidator : AbstractValidator<AccountModel> {
    public AccountValidator() {
      RuleFor(x => x.Auth).NotEmpty().WithMessage("Please specify authentication object");
      RuleFor(x => x.User).NotEmpty().WithMessage("Please specify user object");
      RuleFor(x => x.Auth.Username).NotEmpty().WithMessage("Please specify a username");
      RuleFor(x => x.Auth.Password).NotEmpty().WithMessage("Please specify a password");
      RuleFor(x => x.User.Email).NotEmpty().WithMessage("Please specify an email");
      RuleFor(x => x.User.Email).EmailAddress().WithMessage("Not in email format");
      RuleFor(x => x.User.Name).NotEmpty().WithMessage("Please specify a name");
      RuleFor(x => x.User.Type).NotEmpty().WithMessage("Please specify a user type");
      RuleFor(x => x.User.Type).IsInEnum().WithMessage("Please specify valid user type");

    }
  }
}