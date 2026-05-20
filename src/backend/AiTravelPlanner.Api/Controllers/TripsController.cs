using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AiTravelPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        /// <summary>
        /// Get all trips
        /// </summary>
        [HttpGet]
        public IActionResult GetTrips()
        {
            return Ok();
        }
    }
}
