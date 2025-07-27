using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using TimelineService.Models;

namespace TimelineService.Services
{
    public class ImageUploadService : IImageUploadService
    {

        private readonly Cloudinary _cloudinary;
        private readonly string _imageUploadDestination;
        private readonly string _maxImageUploadSizeInMB;

        public ImageUploadService(Cloudinary cloudinary, IConfiguration configuration)
        {
            _cloudinary = cloudinary;
            _imageUploadDestination = configuration["Cloudinary:ImageUploadDestination"] ?? throw new ArgumentNullException("Image upload destination not found!");
            _maxImageUploadSizeInMB = configuration["Cloudinary:MaxImageUploadSize"] ?? throw new ArgumentNullException("Image upload size not found!");
        }

        public async Task<IList<ImageUplaodResult>> UploadImagesAsync(IFormFileCollection files)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    throw new Exception("No files are there for upload");
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var maxFileSize = int.Parse(_maxImageUploadSizeInMB) * 1024 * 1024;
                var uploadResults = new List<ImageUplaodResult>();

                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        throw new Exception($"Invalid file type for {file.FileName}. Only JPG, JPEG, PNG, and GIF are allowed.");
                    }

                    if (file.Length > maxFileSize)
                    {
                        throw new Exception($"{file.FileName} exceeds the length of {maxFileSize}MB limit!");
                    }

                    using var stream = file.OpenReadStream();

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        PublicId = Path.GetFileNameWithoutExtension(file.FileName),
                        Folder = _imageUploadDestination
                    };

                    ImageUploadResult? uploadResult = await _cloudinary.UploadAsync(uploadParams) ?? throw new Exception("Upload filed!");

                    uploadResults.Add
                    (
                        new ImageUplaodResult()
                        {
                            FileName = file.FileName,
                            PublicUrl = uploadResult.SecureUrl.ToString(),
                            PublicId = uploadResult.PublicId,
                            FileSize = (file.Length * 0.000001).ToString()
                        }  
                    );
                }

                return uploadResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ImageUplaodResult>();
            }
        }
    }
}
