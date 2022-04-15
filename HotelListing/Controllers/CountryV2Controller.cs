using HotelListing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    // controller for testing a version control
    [ApiVersion("2.0")]
    [Route("api/{v:apiversion}/country")] // {v:apiversion} isn't nesessary when we are using header Key: api-version 2.0
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private AppDbContext _context;
        public CountryV2Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCountries()
        {
            return Ok(_context.Countries);
        }
    }
}