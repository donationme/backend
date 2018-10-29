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
        private DatabaseInterfacer<RegionModel> DatabaseInterfacer;


        public LocationController()
        {
            this.DatabaseInterfacer = new DatabaseInterfacer<RegionModel>(DatabaseEndpoints.region);

        }

        [HttpGet("{regionName}"), Authorize]
        public async Task<RegionModel> GetLocations(string regionName)
        {
            RegionReader locationReader = new RegionReader();
            RegionModel locations = await this.DatabaseInterfacer.GetModel(Hasher.SHA256(regionName.ToLower()));
            return locations;

        }

        [HttpPost("{regionName}"), Authorize]
        public async Task<ActionResult<bool>> AddDonationCenter(string regionName, LocationModel location)
        {
            var donationCenterValidator = new DonationCenterValidator();
            string regionId = Hasher.SHA256(regionName.ToLower());
            RegionModel regionModel = await this.DatabaseInterfacer.GetModel(regionId);

            ValidationResult validationResp = donationCenterValidator.Validate(location);

            regionModel.Id =  Hasher.SHA256(regionModel.Name.ToLower());


            if (validationResp.IsValid){
                if (regionModel == null){
                    List<LocationModel> locations = new List<LocationModel>();
                    locations.Add(location);
                    regionModel = new RegionModel{Name = regionName, Locations = locations, Id = regionId};
                    bool isValid = await this.DatabaseInterfacer.AddModel(regionModel);
                    if (isValid){
                        string[] noErrors = {};
                        return Ok(noErrors);
                    }else{
                        ValidationFailure[] validationFailure = { new ValidationFailure("databaseStorageError", "Failed to add location") };
                        return Conflict(validationFailure);
                    }
                }else{
                    regionModel.Locations.Add(location);
                    RegionReader regionReader = new RegionReader();
                    if (this.DatabaseInterfacer.UpdateModel<List<LocationModel>>(regionId, c => c.Locations,regionModel.Locations)){
                        string[] noErrors = {};
                        return Ok(noErrors);
                    }else{
                        ValidationFailure[] validationFailure = { new ValidationFailure("databaseStorageError", "Failed to update location") };
                        return Conflict(validationFailure);
                    }

                }
            }else{
                return Conflict(validationResp.Errors);
            }
                
            


        }

        



        [HttpPost, Authorize]
        public async Task<ActionResult<UserModel>> AddLocationFromCSV(ExportCSVRegionModel csvModel)
        {

            ClaimsPrincipal currentUser = HttpContext.User;

            UserType userType =  TypeFinder.TypeFromClaim(currentUser);

            if (userType == UserType.Admin){

                RegionModel region = csvModel.Location;
                region.Id =  Hasher.SHA256(csvModel.Name.ToLower());

                RegionValidator regionValidator = new RegionValidator();
                ValidationResult response = regionValidator.Validate(region);
                if (response.IsValid){
                    bool notExists = await this.DatabaseInterfacer.AddModel(region);
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
