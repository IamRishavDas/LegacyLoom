using System.ComponentModel.DataAnnotations;

namespace TimelineService.DTOs
{
    public record StoryDTO
    (
        [Required(ErrorMessage = "Title is required")]
        [StringLength(maximumLength: 50, MinimumLength = 15, ErrorMessage = "Title should be in the range of 15 to 50")]
        string Title,

        [Required(ErrorMessage = "Content size is too large")]
        [StringLength(maximumLength: int.MaxValue, MinimumLength = 100, ErrorMessage = "Content length shold be of 100 to 2,147,483,647")]
        string Content,

        MediasDTO? Medias
    ) { }

    public record MediasDTO
    (
        List<ImageDTO>? Images
    ) { }

    public record ImageDTO
    (
        [Required(ErrorMessage = "Image name is required")]
        [StringLength(maximumLength: 20, MinimumLength = 1)]
        string Name,

        [Required(ErrorMessage = "Image notation is required")]
        string Notation,

        [Required(ErrorMessage = "Image Data is required")]
        string Data
    ) { }
}
