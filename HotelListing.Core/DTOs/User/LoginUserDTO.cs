using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.DTOs.User
{
    public class LoginUserDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limeted to {2} to {1}", MinimumLength = 2)]
        public string Password { get; set; }
    }
}