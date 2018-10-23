using FluentValidation;
using SADJZ.Models;

namespace SADJZ.Validation{
  public class DonationItemValidator : AbstractValidator<DonationItemModel> {
    public DonationItemValidator() {
      RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Please specify quantity");
      RuleFor(x => x.ShortDescription).NotEmpty().WithMessage("Please specify description");
      RuleFor(x => x.Title).NotEmpty().WithMessage("Please specify a title");
      RuleFor(x => x.Category).IsInEnum().WithMessage("Please specify valid category");
    }
  }
}