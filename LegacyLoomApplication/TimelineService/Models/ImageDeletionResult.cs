namespace TimelineService.Models
{

    public enum ImageDeletionStatus
    {
        DELETED,
        FAILED, 
        NOT_FOUND
    }

    public class ImageDeletionResult
    {
        public required string PublicId { get; set; }
        public required ImageDeletionStatus Status { get; set; } 
        public string? ErrorMessage { get; set; }
    }
}
