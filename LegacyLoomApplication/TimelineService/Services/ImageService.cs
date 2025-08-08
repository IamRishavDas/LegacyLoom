using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using TimelineService.Models;

namespace TimelineService.Services
{
    public class ImageService : IImageService
    {

        private readonly Cloudinary _cloudinary;
        private readonly string _imageUploadDestination;
        private readonly string _maxImageUploadSizeInMB;

        public ImageService(Cloudinary cloudinary, IConfiguration configuration)
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

                if(files.Count > 4)
                {
                    throw new Exception("Maximum 4 images are allowed");
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

        public async Task<IList<ImageDeletionResult>> DeleteImagesAsync(IList<string> publicIds)
        {
            try
            {
                if (publicIds == null || publicIds.Count == 0)
                {
                    throw new Exception("No public ids are given for delete");
                }

                var deletionResults = new List<ImageDeletionResult>(publicIds.Count);

                foreach(var publicId in publicIds)
                {
                    try
                    {
                        var deletionParams = new DeletionParams(publicId)
                        {
                            ResourceType = ResourceType.Image
                        };

                        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
                        deletionResults.Add(new ImageDeletionResult()
                        {
                            PublicId = publicId,
                            Status = deletionResult.Result == "ok" ? ImageDeletionStatus.DELETED : ImageDeletionStatus.NOT_FOUND,
                            ErrorMessage = deletionResult.Result != "ok" ? deletionResult.Error?.Message : "Error while deleting image"
                        });
                    }
                    catch (Exception ex)
                    {
                        deletionResults.Add(new ImageDeletionResult
                        {
                            PublicId = publicId,
                            Status = ImageDeletionStatus.FAILED,
                            ErrorMessage = ex.Message
                        });
                    }
                }

                return deletionResults;
            }
            catch (Exception ex)
            {

                return new List<ImageDeletionResult> { new ImageDeletionResult { PublicId = "", Status = ImageDeletionStatus.FAILED, ErrorMessage = ex.Message } };
            }
        }
    }
}
