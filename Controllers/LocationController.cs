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
using System;

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
            LocationModel locations = await this.DatabaseInterfacer.GetModel(Hasher.SHA256(locationName.ToLower()));
            return locations;

        }

        [HttpPost("{locationName}"), Authorize]
        public async Task<ActionResult<bool>> AddDonationCenter(string locationName, LocationCollectionObject locationCollectionObject)
        {
            var donationCenterValidator = new DonationCenterValidator();
            string id = Hasher.SHA256(locationName.ToLower());
            LocationModel locationModel = await this.DatabaseInterfacer.GetModel(id);

            ValidationResult response = donationCenterValidator.Validate(locationCollectionObject);

            locationModel.Id =  Hasher.SHA256(locationModel.Name.ToLower());


            if (response.IsValid){
                if (locationModel == null){
                    List<LocationCollectionObject> donationCenters = new List<LocationCollectionObject>();
                    donationCenters.Add(locationCollectionObject);
                    locationModel = new LocationModel{Name = locationName, Locations = donationCenters, Id = id};
                    bool isValid = await this.DatabaseInterfacer.AddModel(locationModel);
                    if (isValid){
                        string[] noErrors = {};
                        return Ok(noErrors);
                    }else{
                        ValidationFailure[] validationFailure = { new ValidationFailure("databaseStorageError", "Failed to add location") };
                        return Conflict(validationFailure);
                    }
                }else{
                    locationModel.Locations.Add(locationCollectionObject);
                    LocationReader locationReader = new LocationReader();
                    if (this.DatabaseInterfacer.UpdateModel<List<LocationCollectionObject>>(id, c => c.Locations,locationModel.Locations)){
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
                locationModel.Id =  Hasher.SHA256(csvModel.Name.ToLower());

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
