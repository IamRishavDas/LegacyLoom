using System.ComponentModel.DataAnnotations;

namespace TimelineService.Models
{
    public class Story
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(maximumLength: 50, MinimumLength = 15, ErrorMessage = "Title should be in the range of 15 to 50")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Content size is too large")]
        [StringLength(maximumLength: int.MaxValue, MinimumLength = 100, ErrorMessage = "Content length shold be of 100 to 2,147,483,647")]
        public required string Content { get; set; }

        public Medias? Medias { get; set; }

        [Required(ErrorMessage = "Word count is required")]
        [Range(minimum: 10, maximum: int.MaxValue, ErrorMessage = "Word count should be in the range of 10 to 2,147,483,647")]
        public required long WordCount { get; set; }
    }

    public class Medias
    {
        public List<Image>? Images { get; set; }
    }

    public class Image
    {
        [Required(ErrorMessage = "Image name is required")]
        [StringLength(maximumLength: 20, MinimumLength = 1)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Image notation is required")]
        public required string Notation { get; set; }

        [Required(ErrorMessage = "Image Data is required")]
        public required string Data { get; set; }

        [Required(ErrorMessage = "Image size is required")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Image size should be in the range of 1 to 2,147,483,647")]
        public required int Size { get; set; }
    }
}
