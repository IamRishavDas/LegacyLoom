using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace TimelineService.Models
{
    public class TimelineDraft
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(maximumLength: 50, MinimumLength = 15, ErrorMessage = "Title should be in the range of 15 to 50")]
        public required string? Title { get; set; }

        [Required(ErrorMessage = "Content size is too large")]
        [StringLength(maximumLength: int.MaxValue, MinimumLength = 100, ErrorMessage = "Content length shold be of 100 to 2,147,483,647")]
        public required string? Content { get; set; }
        public required string CreatedBy { get; set; } // it is Guid
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
