namespace TimelineService.Models
{
    public class ImageUplaodResult
    {
        public required string FileName { get; set; }
        public required string PublicUrl { get; set; }
        public required string PublicId { get; set; }
        public required string FileSize { get; set; }
    }
}
