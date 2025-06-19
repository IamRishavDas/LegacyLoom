using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public required string SendToUserId { get; set; }

        [Required(ErrorMessage = "UserEmail is required")]
        public required string SendToUserEmail { get; set; }

        [Required(ErrorMessage = "Template information is required")]
        public required string TemplateUsed { get; set; }

        public DateTime SendedAt { get; private set; } = DateTime.UtcNow;
    }
}
