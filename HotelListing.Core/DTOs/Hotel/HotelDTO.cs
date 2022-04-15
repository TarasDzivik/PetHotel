using HotelListing.Core.DTOs.Country;

namespace HotelListing.Core.DTOs.Hotel
{
    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }

        public CountryDTO Country { get; set; }
    }
}