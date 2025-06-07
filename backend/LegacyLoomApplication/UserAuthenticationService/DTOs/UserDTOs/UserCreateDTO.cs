using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UserAuthenticationService.Enums;

namespace UserAuthenticationService.DTOs.UserDTOs
{
    public class UserCreateDTO
    {
        [Required]
        [NotNull]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Length of the user name should be between 4 to 50 characters!")]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "Length of the password should be between 6 to 50 characters!")]
        public required string Password { get; set; }
    }
}
