using Microsoft.AspNetCore.Mvc;
using smart_doorbell_api.Models;
using smart_doorbell_api.Services;

namespace smart_doorbell_api.Controllers
{
    /// <summary>
    /// Handles authentication-related API endpoints such as user registration and login.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="login">The login request containing username and password.</param>
        /// <returns>HTTP 200 if successful, HTTP 400 if the user already exists.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest login)
        {
            var result = await authService.RegisterAsync(login);
            if (!result)
            {
                return BadRequest("User already exists.");
            }

            return Created();
        }

        /// <summary>
        /// Logs in a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="login">The login request containing username and password.</param>
        /// <returns>HTTP 200 with JWT token if successful, HTTP 401 if authentication fails.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var token = await authService.LoginAsync(login);
            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { Token = token });
        }
    }
}