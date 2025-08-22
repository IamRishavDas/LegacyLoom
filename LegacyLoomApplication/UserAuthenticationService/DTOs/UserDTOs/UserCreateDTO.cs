using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UserAuthenticationService.CustomValidations;

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

        [PasswordStrengthValidation]
        [StringLength(maximumLength: 15, MinimumLength = 8, ErrorMessage = "Length of the password should be between 8 to 15 characters!")]
        public required string Password { get; set; }
    }
}
