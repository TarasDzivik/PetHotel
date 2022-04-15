using HotelListing.Core.DTOs.Hotel;
using System.Collections.Generic;

namespace HotelListing.Core.DTOs.Country
{
    public class UpdateCountyDTO : CreateCountryDTO
    {
        public IList<CreateHotelDTO> Hotels { get; set; }
    }
}