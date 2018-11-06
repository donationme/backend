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
using System.Linq;
using System;

namespace SADJZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SearchController : ControllerBase
    {
        private DatabaseInterfacer<RegionModel> DatabaseInterfacer;


        public SearchController()
        {
            this.DatabaseInterfacer = new DatabaseInterfacer<RegionModel>(DatabaseEndpoints.region);

        }

        [HttpGet("all/name/{regionName}/{query}"), Authorize]
        public async Task<ActionResult<SearchModel<DonationItemModel>>> SearchAllRegionDonationsByName(string regionName, string query)
        {
            if (query != null){
                string regionId = Hasher.SHA256(regionName.ToLower());
                RegionModel region = await this.DatabaseInterfacer.GetModel(regionId);


                DonationItemModel[][] searchedLocationsNested = region.Locations.Select(c => c.DonationItems.Where(d => d.Name.ToLower().Contains(query.ToLower())  ).ToArray()).Where(e => e.Length > 0).ToArray();
                List<DonationItemModel> searchedLocationsList = new List<DonationItemModel>();

                foreach (DonationItemModel[] searchedLocations in searchedLocationsNested){
                    foreach (DonationItemModel searchedLocation in searchedLocations){
                        searchedLocationsList.Add(searchedLocation);
                    }
                }


                return new SearchModel<DonationItemModel>(){Results = searchedLocationsList.ToArray()};
            }else{
                return Conflict();
            }

        }


        [HttpGet("all/category/{regionName}/{query}"), Authorize]
        public async Task<ActionResult<SearchModel<DonationItemModel>>> SearchAllRegionDonationsByCategory(string regionName, ItemCategory query)
        {
                string regionId = Hasher.SHA256(regionName.ToLower());
                RegionModel region = await this.DatabaseInterfacer.GetModel(regionId);
                
                DonationItemModel[][] searchedLocationsNested = region.Locations.Select(c => c.DonationItems.Where(d => d.Category == query).ToArray()).Where(e => e.Length > 0).ToArray();
                List<DonationItemModel> searchedLocationsList = new List<DonationItemModel>();

                foreach (DonationItemModel[] searchedLocations in searchedLocationsNested){
                    foreach (DonationItemModel searchedLocation in searchedLocations){
                        searchedLocationsList.Add(searchedLocation);
                    }
                }
                

                return new SearchModel<DonationItemModel>(){Results = searchedLocationsList.ToArray()};
        }
       
        [HttpGet("specific/name/{regionName}/{locationid}/{query}"), Authorize]
        public async Task<ActionResult<SearchModel<DonationItemModel>>> SearchSpecificRegionDonationsByName(string regionName, string locationid, string query)
        {
            if (query != null){
                string regionId = Hasher.SHA256(regionName.ToLower());
                RegionModel region = await this.DatabaseInterfacer.GetModel(regionId);
                
                DonationItemModel[] donationItems = region.Locations.Where(c => c.Id == locationid).First().DonationItems.Where(d => d.Name.ToLower().Contains(query.ToLower())).ToArray();

                return new SearchModel<DonationItemModel>(){Results = donationItems.ToArray()};

            }else{
                return Conflict();
            }

        }


        [HttpGet("specific/category/{regionName}/{locationid}/{query}"), Authorize]
        public async Task<ActionResult<SearchModel<DonationItemModel>>> SearchSpecificRegionDonationsByCategory(string regionName, string locationid, ItemCategory query)
        {
                string regionId = Hasher.SHA256(regionName.ToLower());
                RegionModel region = await this.DatabaseInterfacer.GetModel(regionId);
                
                DonationItemModel[] donationItems = region.Locations.Where(c => c.Id == locationid).First().DonationItems.Where(d => d.Category == query).ToArray();

                return new SearchModel<DonationItemModel>(){Results = donationItems.ToArray()};
        }

    }
}
