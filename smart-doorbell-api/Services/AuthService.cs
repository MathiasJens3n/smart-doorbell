using smart_doorbell_api.Models;
using smart_doorbell_api.Repositories.Interfaces;
using smart_doorbell_api.Tools;

namespace smart_doorbell_api.Services
{
    /// <summary>
    /// Provides authentication services including user registration and login.
    /// </summary>
    public class AuthService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtService jwtService;

        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
        }

        /// <summary>
        /// Registers a new user if the username does not already exist.
        /// </summary>
        /// <param name="login">The login request containing username and password.</param>
        /// <returns>True if registration is successful; otherwise, false.</returns>
        public async Task<bool> RegisterAsync(LoginRequest login)
        {
            var existingUser = await userRepository.GetByUsernameAsync(login.Username);

            if (existingUser != null) return false; // User already exists

            var user = new User
            {
                Username = login.Username,
                Password = PasswordHasher.HashPassword(login.Password),
                Registration_Code = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()
            };

            return await userRepository.AddAsync(user);
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token if credentials are valid.
        /// </summary>
        /// <param name="login">The login request containing username and password.</param>
        /// <returns>A JWT token if authentication is successful; otherwise, null.</returns>
        public async Task<string?> LoginAsync(LoginRequest login)
        {
            var user = await userRepository.GetByUsernameAsync(login.Username);

            if (user == null || !PasswordHasher.VerifyPassword(login.Password, user.Password))
            {
                return null; // Invalid credentials
            }

            return jwtService.GenerateToken(user.Id, user.Username);
        }
    }
}
