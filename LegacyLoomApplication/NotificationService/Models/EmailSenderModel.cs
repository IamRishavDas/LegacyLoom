using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models
{
    public class EmailSenderModel
    {
        [Required]
        [EmailAddress]
        public required string ReceiverEmailAddress { get; set; }

        [Required]
        [StringLength(maximumLength:50, MinimumLength = 4, ErrorMessage = "Username should be in between 4 to 50")]
        public required string UserName { get; set; }
    }
}
