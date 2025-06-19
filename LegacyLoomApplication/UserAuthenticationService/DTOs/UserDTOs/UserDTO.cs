using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UserAuthenticationService.Enums;

namespace UserAuthenticationService.DTOs.UserDTOs
{
    public class UserDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [NotNull]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Length of the user name should be between 4 to 50 characters!")]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public required string Role { get; set; }
    }
}
