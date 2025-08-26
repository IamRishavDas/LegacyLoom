using System.ComponentModel.DataAnnotations;
using UserAuthenticationService.CustomValidations;

namespace UserAuthenticationService.DTOs.UserAuthenticationDTOs
{
    public class ResetPasswordDTO
    {
        public required Guid UserId { get; set; }

        [PasswordStrengthValidation]
        [StringLength(maximumLength: 15, MinimumLength = 8, ErrorMessage = "Length of the password should be between 8 to 15 characters!")]
        public required string Password { get; set; }
    }
}
