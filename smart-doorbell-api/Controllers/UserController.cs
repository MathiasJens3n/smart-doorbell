using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using smart_doorbell_api.Dto;
using smart_doorbell_api.Models;
using smart_doorbell_api.Services;
using smart_doorbell_api.Tools;
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
        /// Retrieves the authenticated user's information.
        /// </summary>
        /// <returns>Returns the user's details if found; otherwise, returns an unauthorized or not found response.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            int userIdFromToken = UserHelper.GetUserIdFromToken(HttpContext);

            if (userIdFromToken == -1)
            {
                return Unauthorized("Invalid or missing user token.");
            }

            var user = await userService.GetUserByIdAsync(userIdFromToken);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        /// <summary>
        /// Updates the authenticated user's information.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing the updated information.</param>
        /// <returns>Returns a no-content response if successful, a not found response if the user does not exist, or an error response if the request is invalid.</returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody]UserDTO userDto)
        {
            int userIdFromToken = UserHelper.GetUserIdFromToken(HttpContext);

            if (userIdFromToken == -1)
            {
                return Unauthorized("Invalid or missing user token.");
            }

            if (userDto == null)
            {
                return BadRequest("User data is required.");
            }

            var result = await userService.UpdateUserAsync(userIdFromToken, userDto);

            if (result == false)
            {
                return NotFound("User not found.");
            }

            return NoContent();

        }
    }
}
