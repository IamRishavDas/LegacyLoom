using System.ComponentModel.DataAnnotations;
using TimelineService.Models;

namespace TimelineService.DTOs
{
    public record CreateTimelineDTO
    (
        [Required]
        Story Story
    ){ }

    public record TimelineDTO
    (
        string Id,
        StoryDTO StoryDTO,
        Guid CreatedBy,
        List<Guid>? SharedWith,
        string Visibility,
        DateTime CreatedAt,
        DateTime LastModified
    ) { }
}
