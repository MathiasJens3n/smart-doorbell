using smart_doorbell_api.Dto;
using smart_doorbell_api.Models;
using smart_doorbell_api.Repositories.Interfaces;

namespace smart_doorbell_api.Services
{
    /// <summary>
    /// Service responsible for handling image-related business logic.
    /// </summary>
    public class ImageService
    {
        private readonly IImageRepository imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        /// <summary>
        /// Adds a new image to the system after processing the base64-encoded data.
        /// </summary>
        /// <param name="imageDto">The image data transfer object containing base64-encoded image data and user ID.</param>
        /// <returns>Returns true if the image is successfully added; otherwise, false.</returns>
        public async Task<bool> AddImageAsync(ImageDTO imageDto)
        {
            // Remove prefix if present
            if (imageDto.Data.Contains(','))
            {
                imageDto.Data = imageDto.Data.Split(',')[1]; // Remove "data:image/jpeg;base64,"
            }

            byte[] imageBytes = Convert.FromBase64String(imageDto.Data);

            // Convert DTO to Domain Model
            var image = new Image
            {
                Data = imageBytes,
                User_Id = imageDto.UserId
            };

            return await imageRepository.AddAsync(image);
        }

        /// <summary>
        /// Deletes an image by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the image to be deleted.</param>
        /// <returns>Returns true if the image is successfully deleted; otherwise, false.</returns>
        public async Task<bool> DeleteImageAsync(int id)
        {
            return await imageRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves all images associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of images belonging to the specified user.</returns>
        public async Task<IEnumerable<Image>> GetImagesByUserIdAsync(int userId)
        {
            return await imageRepository.GetByUserIdAsync(userId);
        }
    }
}
