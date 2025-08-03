using System.ComponentModel.DataAnnotations;
using TimelineService.Models;

namespace TimelineService.DTOs
{
    public class CreateTimelineDTO
    {
        [Required]
        public required CreateStoryDTO Story { get; set; }
        public IFormFileCollection? Files { get; set; }
    }

    public class TimelineDTO
    {
        public required string Id {get; set;}
        public required StoryDTO StoryDTO { get; set; }
        public Guid CreatedBy {get; set;}
        public List<Guid>? SharedWith {get; set;}
        public required string Visibility {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime LastModified {get; set;}
    }

    public class TimelineLookupDTO
    {
        public required string Id { get; set; }
        public required StoryLookupDTO StoryDTO { get; set; }
        public required Guid CreatedBy { get; set; }
        public required DateTime LastModified { get; set; }
    }
}
