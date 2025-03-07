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

        /// <summary>
        /// Registers or updates the FCM token for push notifications.
        /// </summary>
        /// <param name="fcmTokenDto">The FCM token data transfer object.</param>
        /// <returns>HTTP 200 if successful, HTTP 400 if the token is invalid.</returns>
        [HttpPost("register-fcm-token")]
        [Authorize]
        public async Task<IActionResult> RegisterFcmToken([FromBody] FcmTokenDto fcmTokenDto)
        {
            int userId = UserHelper.GetUserIdFromToken(HttpContext);

            if (string.IsNullOrEmpty(fcmTokenDto.Token))
            {
                return BadRequest("Invalid token.");
            }

            bool success = await userService.AddFcmTokenAsync(userId, fcmTokenDto);

            if (!success)
            {
                return StatusCode(500, "Error saving token.");
            }

            return Ok();
        }
    }
}

