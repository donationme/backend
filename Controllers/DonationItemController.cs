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
        private DatabaseInterfacer<LocationModel> DatabaseInterfacer;

        private DonationItemValidator DonationCenterValidator = new DonationItemValidator();

        public DonationItemController()
        {
            this.DatabaseInterfacer = new DatabaseInterfacer<LocationModel>(DatabaseEndpoints.location);

        }

        [HttpPost("add/{locationName}/{locationid}"), Authorize]
        public async Task<ActionResult<UserModel>> AddDonationItemForLocation(string locationName, string locationid, DonationItemModel donationItem)
        {

            string id = Hasher.SHA256(locationName.ToLower());
            LocationModel location = await this.DatabaseInterfacer.GetModel(id);

            ValidationResult response = DonationCenterValidator.Validate(donationItem);

            if (response.IsValid)
            {
                donationItem.LocationId = locationid;
                donationItem.Time = DateTime.Now;
                donationItem.Id = Hasher.SHA256(donationItem.Name + donationItem.Time.ToString());
                location.Locations.Where(c => c.Id == locationid).First().DonationItems.Add(donationItem);
                LocationReader locationReader = new LocationReader();
                if (this.DatabaseInterfacer.UpdateModel<List<LocationCollectionObject>>(id, c => c.Locations, location.Locations))
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
        [HttpGet("remove/{locationName}/{locationid}/{donationItemId}"), Authorize]
        public async Task<ActionResult<UserModel>> RemoveDonationItemForLocation(string locationName, string locationid, string donationItemId)
        {
            try{

            
            string id = Hasher.SHA256(locationName.ToLower());
            LocationModel location = await this.DatabaseInterfacer.GetModel(id);
            LocationCollectionObject donationCenter = location.Locations.Where(c => c.Id == locationid).First();
            donationCenter.DonationItems.Remove(donationCenter.DonationItems.Where(d => d.Id == donationItemId).First());

                if (this.DatabaseInterfacer.UpdateModel<List<LocationCollectionObject>>(id, c => c.Locations, location.Locations))
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



        [HttpPost("edit/{locationName}/{locationid}/{donationItemId}"), Authorize]
        public async Task<ActionResult<UserModel>> EditDonationItemForLocation(string locationName, string locationid,  string donationItemId, DonationItemModel donationItem)
        {

            ValidationResult response = DonationCenterValidator.Validate(donationItem);

            if (response.IsValid)
            {
                var donationCenterValidator = new DonationItemValidator();
                string id = Hasher.SHA256(locationName.ToLower());
                LocationModel location = await this.DatabaseInterfacer.GetModel(id);
                LocationCollectionObject donationCenter = location.Locations.Where(c => c.Id == locationid).First();
                donationCenter.DonationItems.Remove(donationCenter.DonationItems.Where(d => d.Id == donationItemId).First());
                donationItem.Id = donationItemId;
                donationItem.LocationId = locationid;

                donationCenter.DonationItems.Add(donationItem);

                if (this.DatabaseInterfacer.UpdateModel<List<LocationCollectionObject>>(id, c => c.Locations, location.Locations))
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
