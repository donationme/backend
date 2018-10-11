using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using SADJZ.Models;
using SADJZ.Services;
using SADJZ.Database;
using SADJZ.Consts;

namespace SADJZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LocationController : ControllerBase
    {


        public LocationController()
        {

        }

        [HttpGet, Authorize]
        public List<LocationModel> GetLocations()
        {
            LocationReader locationReader = new LocationReader();
            return locationReader.ReadCSV("Assets/LocationData.csv");
        }

    }

}
