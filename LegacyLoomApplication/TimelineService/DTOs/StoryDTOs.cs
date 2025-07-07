using System.ComponentModel.DataAnnotations;

namespace TimelineService.DTOs
{
    public class StoryDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(maximumLength: 50, MinimumLength = 15, ErrorMessage = "Title should be in the range of 15 to 50")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Content size is too large")]
        [StringLength(maximumLength: int.MaxValue, MinimumLength = 100, ErrorMessage = "Content length shold be of 100 to 2,147,483,647")]
        public required string Content { get; set; }

        public MediasDTO? Medias { get; set; }
    }

    public class MediasDTO
    {
        public List<ImageDTO>? Images { get; set; }
    }

    public record ImageDTO
    {
        [Required(ErrorMessage = "Image name is required")]
        [StringLength(maximumLength: 20, MinimumLength = 1)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Image notation is required")]
        public required string Notation {get; set; }

        [Required(ErrorMessage = "Image Data is required")]
        public required string Data {get; set; }    
    }
}
