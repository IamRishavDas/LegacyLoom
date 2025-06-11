using System.ComponentModel.DataAnnotations;

namespace EventModelsShared
{
    public interface UserRegistered 
    {
        [Required]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
