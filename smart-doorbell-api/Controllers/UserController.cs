using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using smart_doorbell_api.Dto;
using smart_doorbell_api.Models;
using smart_doorbell_api.Services;
using System.Security.Claims;

namespace smart_doorbell_api.Controllers
{
    /// <summary>
    /// Controller responsible for managing user-related operations.
    /// Requires authentication to access its endpoints.
    /// </summary>

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Retrieves user information based on the provided user ID.
        /// Ensures that a user can only access their own data.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>Returns the user details if found; otherwise, returns a 404 error or access is denied.</returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Deny access if user tries to access another user's data
            if (userIdFromToken != id)
            {
                return Forbid();
            }

            var user = await userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        /// <summary>
        /// Updates user information.
        /// Ensures that a user can only update their own profile.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing updated user information.</param>
        /// <returns>Returns a success message if updated successfully; otherwise, returns an error status.</returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody]UserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is required.");
            }

            var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Deny access if user tries to access another user's data
            if (userIdFromToken != id)
            {
                return Forbid();
            }

            var result = await userService.UpdateUserAsync(id, userDto);

            if (result == false)
            {
                return NotFound("User not found.");
            }

            return NoContent();

        }
    }
}
