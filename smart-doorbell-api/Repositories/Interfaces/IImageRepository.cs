using smart_doorbell_api.Models;

namespace smart_doorbell_api.Repositories.Interfaces
{
    /// <summary>
    /// Defines the contract for an image repository, providing methods to manage image data.
    /// </summary>
    public interface IImageRepository
    {
        /// <summary>
        /// Retrieves all images associated with a specific user by their user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of images belonging to the specified user.</returns>
        Task<IEnumerable<Image>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Adds a new image to the repository.
        /// </summary>
        /// <param name="image">The image entity to be added.</param>
        /// <returns>Returns true if the image is added successfully; otherwise, false.</returns>
        Task<bool> AddAsync(Image image);

        /// <summary>
        /// Deletes an image by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the image to be deleted.</param>
        /// <returns>Returns true if the image is deleted successfully; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
