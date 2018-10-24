using FluentValidation;
using SADJZ.Models;
using System.Linq;
namespace SADJZ.Validation{
  public class LocationValidator : AbstractValidator<LocationModel> {
    public LocationValidator() {
      RuleFor(x => x.Name).NotEmpty().WithMessage("Please provide region name");
      RuleFor(x => x.Locations).NotEmpty().WithMessage("Please provide location information");
      RuleForEach(x => x.Locations).SetValidator(new DonationCenterValidator());
    }
  }

  public class DonationCenterValidator : AbstractValidator<LocationCollectionObject> {
    public DonationCenterValidator() {
      RuleFor(x => x.Key).NotEmpty().WithMessage("Please provide Key");
      RuleFor(x => x.Latitude).NotEmpty().WithMessage("Please provide Latitude");
      RuleFor(x => x.Longitude).NotEmpty().WithMessage("Please provide Longitude");
      RuleFor(x => x.Name).NotEmpty().WithMessage("Please provide Name");
      RuleFor(x => x.Phone).NotEmpty().WithMessage("Please provide Phone");
      RuleFor(x => x.State).NotEmpty().WithMessage("Please provide State");
      RuleFor(x => x.Street).NotEmpty().WithMessage("Please provide Street");
      RuleFor(x => x.Zip).NotEmpty().WithMessage("Please provide Zip");
      RuleFor(x => x.Type).NotEmpty().WithMessage("Please provide Type");
      RuleFor(x => x.Website).NotEmpty().WithMessage("Please provide Website");



    }
  }


}