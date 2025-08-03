
using TimelineService.Models;

namespace TimelineService.Services
{
    public interface IImageService
    {
        Task<IList<ImageUplaodResult>> UploadImagesAsync(IFormFileCollection files);
        Task<IList<ImageDeletionResult>> DeleteImagesAsync(IList<string> publicIds);

    }
}