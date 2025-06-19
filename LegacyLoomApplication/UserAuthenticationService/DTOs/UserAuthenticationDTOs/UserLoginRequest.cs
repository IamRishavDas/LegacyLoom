using System.ComponentModel.DataAnnotations;

namespace UserAuthenticationService.DTOs.UserAuthenticationDTOs
{
    public class UserLoginRequestByUsername
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
    
    public class UserLoginRequestByEmail
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
