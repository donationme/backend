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
using System.Linq;
using System;

namespace SADJZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DonationItemController : ControllerBase
    {
        private DatabaseInterfacer<RegionModel> DatabaseInterfacer;

        private DonationItemValidator DonationItemValidator = new DonationItemValidator();

        public DonationItemController()
        {
            this.DatabaseInterfacer = new DatabaseInterfacer<RegionModel>(DatabaseEndpoints.region);

        }

        [HttpPost("add/{regionName}/{locationid}"), Authorize]
        public async Task<ActionResult<UserModel>> AddDonationItemForLocation(string regionName, string locationid, DonationItemModel donationItem)
        {

            string regionId = Hasher.SHA256(regionName.ToLower());
            RegionModel region = await this.DatabaseInterfacer.GetModel(regionId);

            ValidationResult response = DonationItemValidator.Validate(donationItem);

            if (response.IsValid)
            {
                donationItem.LocationId = locationid;
                donationItem.Time = DateTime.Now;
                donationItem.Id = Hasher.SHA256(donationItem.Name + donationItem.Time.ToString());
                region.Locations.Where(c => c.Id == locationid).First().DonationItems.Add(donationItem);
                RegionReader locationReader = new RegionReader();
                if (this.DatabaseInterfacer.UpdateModel<List<LocationModel>>(regionId, c => c.Locations, region.Locations))
                {
                    string[] noErrors = { };
                    return Ok(noErrors);
                }
                else
                {
                    ValidationFailure[] validationFailure = { new ValidationFailure("databaseStorageError", "Failed to update location") };
                    return Conflict(validationFailure);
                }

            }
            else
            {
                return Conflict(response.Errors);
            }

        }
        [HttpGet("remove/{regionName}/{locationid}/{donationItemId}"), Authorize]
        public async Task<ActionResult<UserModel>> RemoveDonationItemForLocation(string regionName, string locationid, string donationItemId)
        {
            try{

            
            string id = Hasher.SHA256(regionName.ToLower());
            RegionModel region = await this.DatabaseInterfacer.GetModel(id);
            LocationModel location = region.Locations.Where(c => c.Id == locationid).First();
            location.DonationItems.Remove(location.DonationItems.Where(d => d.Id == donationItemId).First());

                if (this.DatabaseInterfacer.UpdateModel<List<LocationModel>>(id, c => c.Locations, region.Locations))
                {
                    string[] noErrors = { };
                    return Ok(noErrors);
                }
                else
                {
                    ValidationFailure[] validationFailure = { new ValidationFailure("databaseStorageError", "Failed to update location") };
                    return Conflict(validationFailure);
                }


            }catch{
                return NoContent();
            }
        }



        [HttpPost("edit/{regionName}/{locationid}/{donationItemId}"), Authorize]
        public async Task<ActionResult<UserModel>> EditDonationItemForLocation(string regionName, string locationid,  string donationItemId, DonationItemModel donationItem)
        {

            ValidationResult response = DonationItemValidator.Validate(donationItem);

            if (response.IsValid)
            {


                string id = Hasher.SHA256(regionName.ToLower());
                RegionModel region = await this.DatabaseInterfacer.GetModel(id);
                LocationModel location = region.Locations.Where(c => c.Id == locationid).First();
                location.DonationItems.Remove(location.DonationItems.Where(d => d.Id == donationItemId).First());
                donationItem.Id = donationItemId;
                donationItem.LocationId = locationid;

                location.DonationItems.Add(donationItem);

                if (this.DatabaseInterfacer.UpdateModel<List<LocationModel>>(id, c => c.Locations, region.Locations))
                {
                    string[] noErrors = { };
                    return Ok(noErrors);
                }
                else
                {
                    ValidationFailure[] validationFailure = { new ValidationFailure("databaseStorageError", "Failed to update location") };
                    return Conflict(validationFailure);
                }


            }
            else
            {
                return Conflict(response.Errors);
            }



        }

    }
}
