using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.DTOs.Country
{
    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Contry name is too long!")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 5, ErrorMessage = "Short Contry name is too long!")]
        public string ShortName { get; set; }
    }
}