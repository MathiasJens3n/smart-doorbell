using smart_doorbell_api.Models;

namespace smart_doorbell_api.Repositories.Interfaces
{
    /// <summary>
    /// Interface for user repository operations.
    /// Provides methods for retrieving, adding, and updating user data.
    /// </summary>

    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        Task<User?> GetByUsernameAsync(string username);
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        Task<User?> GetByIdAsync(int id);
        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user entity to be added.</param>
        /// <returns>Returns true if the user was added successfully; otherwise, returns false.</returns>
        Task<bool> AddAsync(User user);
        /// <summary>
        /// Updates an existing user in the repository.
        /// </summary>
        /// <param name="user">The user entity with updated information.</param>
        /// <returns>Returns true if the user was updated successfully; otherwise, returns false.</returns>
        Task<bool> UpdateAsync(User user);

        /// <summary>
        /// Adds or updates the Firebase Cloud Messaging (FCM) token for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="fcmToken">The FCM token to be stored.</param>
        /// <returns>Returns true if the token is successfully added or updated; otherwise, false.</returns>
        Task<bool> AddFcmTokenAsync(int userId, string fcmToken);

        /// <summary>
        /// Retrieves the FCM token for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>Returns the fcm token if found, otherwise null</returns>
        Task<string?> GetFcmTokenByUserIdAsync(int userId);
    }
}
