using AutoMapper;
using HotelListing.Core.DTOs.Hotel;
using HotelListing.Core.Models;
using HotelListing.Core.Repository.Interfaces;
using HotelListing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/hotel")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<HotelController> logger;
        private readonly IMapper mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var hotels = await unitOfWork.Hotels.GetPagedList(paginationParams);
                var results = mapper.Map<IList<HotelDTO>>(hotels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Somthing went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
                if (hotel == null)
                {
                    logger.LogError($"Invalid ID attemt in {nameof(GetHotel)}");
                    return NotFound("Submitted Hotel doesn't exist");
                }
                var result = mapper.Map<HotelDTO>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Somthing went wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }
        //[Authorize(Roles = "Admin")] //авторизація не підключена
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            logger.LogInformation($"Registration attemt for {hotelDTO} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var hotel = mapper.Map<Hotel>(hotelDTO);
            await unitOfWork.Hotels.Insert(hotel);
            await unitOfWork.Save();
            return Ok(hotel);


        }
        //[Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                logger.LogError($"Invalid UPDATE attemt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }
            var hotel = await unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                logger.LogError($"Invalid UPDATE attemt in {nameof(UpdateHotel)}");
                return BadRequest("Submitted data is invalid");
            }

            mapper.Map(hotelDTO, hotel);
            unitOfWork.Hotels.Update(hotel);
            await unitOfWork.Save();
            return NoContent();

        }
        //[Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                logger.LogError($"Invalid DELETE attemt in {nameof(DeleteHotel)}");
                return BadRequest();
            }
            var hotel = await unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                logger.LogError($"Invalid DELETE attemt in {nameof(DeleteHotel)}");
                return BadRequest("Submitted data is invalid");
            }
            await unitOfWork.Hotels.Delete(id);
            await unitOfWork.Save();
            return NoContent();
        }
    }
}