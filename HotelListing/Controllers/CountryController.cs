using AutoMapper;
using HotelListing.Core.DTOs.Country;
using HotelListing.Core.Models;
using HotelListing.Core.Repository.Interfaces;
using HotelListing.Data;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CountryController> logger;
        private readonly IMapper mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var countries = await unitOfWork.Counties.GetPagedList(paginationParams);
                var results = mapper.Map<IList<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Somthing went wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }
        //[HttpGet("{id:int}"), Name = "GetCountry"]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            // in this block of code maight to work global exception handler instead try/catch 
            var country = await unitOfWork.Counties.Get(q => q.Id == id, new List<string> { "Hotels" });
            if (country == null)
            {
                logger.LogError($"Invalid ID attemt in {nameof(GetCountry)}");
                return NotFound("Submitted country doesn't exist");
            }
            var result = mapper.Map<CountryDTO>(country);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countyDTO)
        {
            logger.LogInformation($"Registration attemt for {countyDTO}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var country = mapper.Map<Country>(countyDTO);
            await unitOfWork.Counties.Insert(country);
            await unitOfWork.Save();
            return Ok(country);
        }

        [HttpPut("{id:int}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountyDTO countyDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                logger.LogError($"Invalid UPDATE attemt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }
            var country = await unitOfWork.Counties.Get(q => q.Id == id);
            if (country == null)
            {
                logger.LogError($"Invalid UPDATE attemt in {nameof(UpdateCountry)}");
                return BadRequest("Submitted data is invalid");
            }

            mapper.Map(countyDTO, country);
            unitOfWork.Counties.Update(country);
            await unitOfWork.Save();
            return NoContent();
        }

        //[Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                logger.LogError($"Invalid DELETE attemt in {nameof(DeleteCountry)}");
                return BadRequest();
            }
            var country = await unitOfWork.Counties.Get(q => q.Id == id);
            if (country == null)
            {
                logger.LogError($"Invalid DELETE attemt in {nameof(DeleteCountry)}");
                return BadRequest("Submitted data is invalid");
            }
            await unitOfWork.Counties.Delete(id);
            await unitOfWork.Save();
            return NoContent();
        }
    }
}