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

    public class SearchController : ControllerBase
    {
        private DatabaseInterfacer<RegionModel> DatabaseInterfacer;


        public SearchController()
        {
            this.DatabaseInterfacer = new DatabaseInterfacer<RegionModel>(DatabaseEndpoints.region);

        }

        [HttpGet("name/{regionName}/{query}"), Authorize]
        public async Task<ActionResult<SearchModel<DonationItemModel[]>>> SearchRegionDonationsByName(string regionName, string query)
        {
            if (query != null){
                string regionId = Hasher.SHA256(regionName.ToLower());
                RegionModel region = await this.DatabaseInterfacer.GetModel(regionId);


                DonationItemModel[][] searchedLocations = region.Locations.Select(c => c.DonationItems.Where(d => d.Name.ToLower().Contains(query.ToLower())  ).ToArray()).Where(e => e.Length > 0).ToArray();
                
                return new SearchModel<DonationItemModel[]>(){Results = searchedLocations, AreResults = searchedLocations.Count() != 0};
            }else{
                return Conflict();
            }

        }


        [HttpGet("category/{regionName}/{query}"), Authorize]
        public async Task<ActionResult<SearchModel<DonationItemModel[]>>> SearchRegionDonationsByCategory(string regionName, ItemCategory query)
        {
                string regionId = Hasher.SHA256(regionName.ToLower());
                RegionModel region = await this.DatabaseInterfacer.GetModel(regionId);
                
                DonationItemModel[][] searchedLocations = region.Locations.Select(c => c.DonationItems.Where(d => d.Category == query).ToArray()).Where(e => e.Length > 0).ToArray();
                return new SearchModel<DonationItemModel[]>(){Results = searchedLocations, AreResults = searchedLocations.Count() != 0};
        }
       

    }
}
