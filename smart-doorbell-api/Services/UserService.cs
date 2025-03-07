using Serilog;
using smart_doorbell_api.Dto;
using smart_doorbell_api.Models;
using smart_doorbell_api.Repositories.Interfaces;
using smart_doorbell_api.Tools;

namespace smart_doorbell_api.Services
{
    /// <summary>
    /// Service class responsible for user-related business logic.
    /// Provides methods to retrieve and update user information.
    /// </summary>

    public class UserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await userRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// Ensures that the user exists before performing the update.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="userDto">The user data transfer object containing updated user information.</param>
        /// <returns>Returns true if the update was successful; otherwise, returns false.</returns>
        public async Task<bool> UpdateUserAsync(int id, UserDTO userDto)
        {
            var existingUser = await userRepository.GetByIdAsync(id);

            if(existingUser == null)
            {
                return false;
            }

            // Convert DTO to Domain Model
            var user = new User
            {
                Id = id,
                Username = userDto.Username,
                Password = PasswordHasher.HashPassword(userDto.Password)
            };

            return await userRepository.UpdateAsync(user);
        }
    }
}
