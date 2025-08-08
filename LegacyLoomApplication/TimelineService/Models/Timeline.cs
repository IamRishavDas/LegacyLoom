using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TimelineService.Models
{
    public class Timeline
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [Required]
        public required Story Story { get; set; }

        [Required(ErrorMessage = "User id is required")]
        public required string CreatedBy { get; set; } // should be a Guid value
        public required HashSet<string> Likes { get; set; } // should be a list of Guid value
        public required HashSet<string> Dislikes { get; set; } // should be a list of Guid value
        public List<string>? SharedWith { get; set; }  // should be a Guid value

        [Required]
        public TimelineVisibility Visibility { get; set; } = TimelineVisibility.PUBLIC;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }

    public enum TimelineVisibility
    {
        PUBLIC,
        SHARED,
        PRIVATE
    }
}
