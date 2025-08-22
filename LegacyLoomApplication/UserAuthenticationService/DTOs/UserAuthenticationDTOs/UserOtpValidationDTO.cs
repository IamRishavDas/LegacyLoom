using System.ComponentModel.DataAnnotations;

namespace UserAuthenticationService.DTOs.UserAuthenticationDTOs
{
    public class UserOtpValidationDTO
    {
        public required string UserNameOrEmail { get; set; }

        [StringLength(maximumLength: 6, MinimumLength = 6, ErrorMessage = "OTP length must be 6")]
        public required string OTP { get; set; }
    }
}
