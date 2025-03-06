using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace smart_doorbell_api.Services
{
    /// <summary>
    /// Service responsible for generating JWT tokens for user authentication.
    /// </summary>
    public class JwtService
    {
        private readonly string _secret;

        public JwtService(string secret)
        {
            _secret = secret;
        }

        /// <summary>
        /// Generates a JWT token containing user information.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns>A signed JWT token as a string.</returns>
        public string GenerateToken(int userId, string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(tokenDescriptor);
        }
    }
}
