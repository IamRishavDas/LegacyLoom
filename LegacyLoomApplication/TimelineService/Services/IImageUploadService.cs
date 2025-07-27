
using TimelineService.Models;

namespace TimelineService.Services
{
    public interface IImageUploadService
    {
        Task<IList<ImageUplaodResult>> UploadImagesAsync(IFormFileCollection files);
    }
}