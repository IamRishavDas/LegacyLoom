using System.ComponentModel.DataAnnotations;

namespace UserAuthenticationService.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Length of the user name should be between 4 to 50 characters!")]

        public string  Username { get; set; }

        [Required]
        [EmailAddress]
        public string  Email { get; set; }
    }
}
