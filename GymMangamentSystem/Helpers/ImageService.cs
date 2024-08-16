using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GymMangamentSystem.Core.IServices;

namespace GymMangamentSystem.Apis.Helpers
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string[] _allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

        public ImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
        }

        public async Task<Tuple<int, string>> UploadImageAsync(IFormFile imageFile)
        {
            try
            {
                var ext = Path.GetExtension(imageFile.FileName).ToLower();
                if (!_allowedExtensions.Contains(ext))
                {
                    string msg = $"Only {string.Join(", ", _allowedExtensions)} extensions are allowed";
                    return new Tuple<int, string>(0, msg);
                }

                await using var stream = imageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(imageFile.FileName, stream)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new Tuple<int, string>(1, uploadResult.SecureUrl.AbsoluteUri);
                }
                else
                {
                    string errorMessage = $"Error occurred while uploading the image. Status Code: {uploadResult.StatusCode}, Error: {uploadResult.Error?.Message}";
                    Console.WriteLine(errorMessage);
                    return new Tuple<int, string>(0, errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new Tuple<int, string>(0, $"Error has occurred: {ex.Message}");
            }
        }
        public async Task DeleteImageAsync(string imageUrl)
        {
            try
            {
                var publicId = GetPublicIdFromUrl(imageUrl);
                var deletionParams = new DeletionParams(publicId)
                {
                    Invalidate = true
                };

                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                Console.WriteLine($"Deletion result: {deletionResult.Result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DeleteImageAsync: {ex.Message}");
            }
        }

        private string GetPublicIdFromUrl(string url)
        {
            return Path.GetFileNameWithoutExtension(new Uri(url).AbsolutePath);
        }

    }
}
