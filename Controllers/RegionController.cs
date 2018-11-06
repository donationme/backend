using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using SADJZ.Models;
using SADJZ.Services;
using SADJZ.Database;
using SADJZ.Consts;
using FluentValidation.Results;
using System.Security.Claims;
using System;

namespace SADJZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegionController : ControllerBase
    {
        private DatabaseInterfacer<RegionModel> DatabaseInterfacer;


        public RegionController()
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
            string regionId = Hasher.SHA256(regionName.ToLower());
            RegionModel regionModel = await this.DatabaseInterfacer.GetModel(regionId);


            regionModel.Id =  Hasher.SHA256(regionModel.Name.ToLower());


                if (regionModel == null){
                    List<LocationModel> locations = new List<LocationModel>();
                    locations.Add(location);
                    regionModel = new RegionModel{Name = regionName, Locations = locations, Id = regionId};
                    bool isValid = await this.DatabaseInterfacer.AddModel(regionModel);
                    if (isValid){
                        string[] noErrors = {};
                        return Ok(noErrors);
                    }else{
                        var resp = Content("{" + '"' + "DatabaseStorageError" + '"' + ":[" +'"' + "Failed to add location" + '"' + "]}");
                        resp.StatusCode = 400;
                        return resp;
                    }
                }else{
                    regionModel.Locations.Add(location);
                    RegionReader regionReader = new RegionReader();
                    if (this.DatabaseInterfacer.UpdateModel<List<LocationModel>>(regionId, c => c.Locations,regionModel.Locations)){
                        string[] noErrors = {};
                        return Ok(noErrors);
                    }else{
                        var resp = Content("{" + '"' + "DatabaseStorageError" + '"' + ":[" +'"' + "Failed to add location" + '"' + "]}");
                        resp.StatusCode = 400;
                        return resp;
                    }
            
                }

        }

        



        [HttpPost, Authorize]
        public async Task<ActionResult<UserModel>> AddRegionFromCSV(ExportCSVRegionModel csvModel)
        {

            ClaimsPrincipal currentUser = HttpContext.User;

            UserType userType =  TypeFinder.TypeFromClaim(currentUser);

            if (userType == UserType.Admin){

                RegionModel region = csvModel.Region;
                region.Id =  Hasher.SHA256(csvModel.Name.ToLower());

                    bool notExists = await this.DatabaseInterfacer.AddModel(region);
                    if (notExists){
                        string[] noErrors = {};
                        return Ok(noErrors);

                    }else{
                        var resp = Content("{" + '"' + "Location" + '"' + ":[" +'"' + "The region already exists" + '"' + "]}");
                        resp.StatusCode = 400;
                        return resp;
                    }
                
            }else{
                return Unauthorized();
            }
        }


    }

}
