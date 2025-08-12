using System.ComponentModel.DataAnnotations;

namespace TimelineService.DTOs
{
    public class CreateTimelineDraft
    {
        [StringLength(maximumLength: 50, ErrorMessage = "Maximum Title lenght can be 50")]
        public string? Title { get; set; }

        [StringLength(maximumLength: int.MaxValue, ErrorMessage = "Content length shold be max of 2,147,483,647")]
        public string? Content { get; set; }
    }

    public class UpdateTimelineDraft
    {
        [StringLength(maximumLength: 50, ErrorMessage = "Maximum Title lenght can be 50")]
        public string? Title { get; set; }

        [StringLength(maximumLength: int.MaxValue, ErrorMessage = "Content length shold be max of 2,147,483,647")]
        public string? Content { get; set; }
    }

    public class TimelineDraftDTO
    {
        public required string Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime LastModified { get; set; }
    }

    public class TimelineDraftLookupDTO
    {
        public required string Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime LastModified { get; set; }
    }
}
