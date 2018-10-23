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

        [HttpPost("add/{locationName}/{donationCenterName}"), Authorize]
        public async Task<ActionResult<UserModel>> AddDonationItemForLocation(string locationName, string donationCenterName, DonationItemModel donationItem)
        {

            string id = Hasher.SHA256(locationName.ToLower());
            LocationModel location = await this.DatabaseInterfacer.GetModel(id);

            ValidationResult response = DonationCenterValidator.Validate(donationItem);

            if (response.IsValid)
            {
                donationItem.TimeDonated = DateTime.Now;
                donationItem.Id = Hasher.SHA256(donationItem.Title + donationItem.TimeDonated.ToString());
                location.DonationCenters.Where(c => c.Name == donationCenterName).First().DonationItems.Add(donationItem);
                LocationReader locationReader = new LocationReader();
                if (this.DatabaseInterfacer.UpdateModel<List<DonationCenterModel>>(id, c => c.DonationCenters, location.DonationCenters))
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
        [HttpPost("remove/{locationName}/{donationCenterName}/{donationItemId}"), Authorize]
        public async Task<ActionResult<UserModel>> RemoveDonationItemForLocation(string locationName, string donationCenterName, string donationItemId)
        {
            try{

            
            string id = Hasher.SHA256(locationName.ToLower());
            LocationModel location = await this.DatabaseInterfacer.GetModel(id);
            DonationCenterModel donationCenter = location.DonationCenters.Where(c => c.Name == donationCenterName).First();
            donationCenter.DonationItems.Remove(donationCenter.DonationItems.Where(d => d.Id == donationItemId).First());

                if (this.DatabaseInterfacer.UpdateModel<List<DonationCenterModel>>(id, c => c.DonationCenters, location.DonationCenters))
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



        [HttpPost("edit/{locationName}/{donationCenterName}/{donationItemId}"), Authorize]
        public async Task<ActionResult<UserModel>> EditDonationItemForLocation(string locationName, string donationCenterName,  string donationItemId, DonationItemModel donationItem)
        {

            ValidationResult response = DonationCenterValidator.Validate(donationItem);

            if (response.IsValid)
            {
                var donationCenterValidator = new DonationItemValidator();
                string id = Hasher.SHA256(locationName.ToLower());
                LocationModel location = await this.DatabaseInterfacer.GetModel(id);
                DonationCenterModel donationCenter = location.DonationCenters.Where(c => c.Name == donationCenterName).First();
                donationCenter.DonationItems.Remove(donationCenter.DonationItems.Where(d => d.Id == donationItemId).First());
                donationItem.Id = donationItemId;
                donationCenter.DonationItems.Add(donationItem);

                if (this.DatabaseInterfacer.UpdateModel<List<DonationCenterModel>>(id, c => c.DonationCenters, location.DonationCenters))
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
