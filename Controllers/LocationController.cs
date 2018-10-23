using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using SADJZ.Models;
using SADJZ.Services;
using SADJZ.Database;
using SADJZ.Consts;
using SADJZ.Validation;
using FluentValidation.Results;
using System.Security.Claims;

namespace SADJZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LocationController : ControllerBase
    {
        private DatabaseInterfacer<LocationModel> DatabaseInterfacer;


        public LocationController()
        {
            this.DatabaseInterfacer = new DatabaseInterfacer<LocationModel>(DatabaseEndpoints.location);

        }

        [HttpGet("{locationName}"), Authorize]
        public async Task<LocationModel> GetLocations(string locationName)
        {
            LocationReader locationReader = new LocationReader();
            return await this.DatabaseInterfacer.GetModel(Hasher.SHA256(locationName.ToLower()));

        }

        [HttpPost("{locationName}"), Authorize]
        public async Task<ActionResult<bool>> AddDonationCenter(string locationName, DonationCenterModel donationCenter)
        {
            var donationCenterValidator = new DonationCenterValidator();
            string id = Hasher.SHA256(locationName.ToLower());
            LocationModel location = await this.DatabaseInterfacer.GetModel(id);

            ValidationResult response = donationCenterValidator.Validate(donationCenter);

            if (response.IsValid){
                if (location == null){
                    List<DonationCenterModel> donationCenters = new List<DonationCenterModel>();
                    donationCenters.Add(donationCenter);
                    location = new LocationModel{Name = locationName, DonationCenters = donationCenters, Id = id};
                    bool isValid = await this.DatabaseInterfacer.AddModel(location);
                    if (isValid){
                        string[] noErrors = {};
                        return Ok(noErrors);
                    }else{
                        ValidationFailure[] validationFailure = { new ValidationFailure("databaseStorageError", "Failed to add location") };
                        return Conflict(validationFailure);
                    }
                }else{
                    location.DonationCenters.Add(donationCenter);
                    LocationReader locationReader = new LocationReader();
                    if (this.DatabaseInterfacer.UpdateModel<List<DonationCenterModel>>(id, c => c.DonationCenters,location.DonationCenters)){
                        string[] noErrors = {};
                        return Ok(noErrors);
                    }else{
                        ValidationFailure[] validationFailure = { new ValidationFailure("databaseStorageError", "Failed to update location") };
                        return Conflict(validationFailure);
                    }

                }
            }else{
                return Conflict(response.Errors);
            }
                
            


        }

        



        [HttpPost, Authorize]
        public async Task<ActionResult<UserModel>> AddLocationFromCSV(ExportCSVLocationModel csvModel)
        {

            ClaimsPrincipal currentUser = HttpContext.User;

            UserType userType =  TypeFinder.TypeFromClaim(currentUser);

            if (userType == UserType.Admin){

                LocationModel locationModel = csvModel.Location;
                locationModel.Id = Hasher.SHA256(locationModel.Name.ToLower());
                LocationValidator locValidator = new LocationValidator();
                ValidationResult response = locValidator.Validate(locationModel);
                if (response.IsValid){
                    bool notExists = await this.DatabaseInterfacer.AddModel(locationModel);
                    if (notExists){
                        string[] noErrors = {};
                        return Ok(noErrors);

                    }else{
                        ValidationFailure[] validationFailure = { new ValidationFailure("duplicateLocationError", "The location already exists") };
                        return Conflict(validationFailure);
                    }
                }else{
                    return Conflict(response);
                }
            }else{
                return Unauthorized();
            }
        }


    }

}
