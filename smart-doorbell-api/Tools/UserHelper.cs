using System.Security.Claims;

namespace smart_doorbell_api.Tools
{
    public static class UserHelper
    {
        /// <summary>
        /// Retrieves the user ID from the claims in the current HTTP context.
        /// </summary>
        /// <param name="httpContext">The current HTTP context containing user claims.</param>
        /// <returns>The user ID as an integer. Returns 0 if the claim is missing or invalid.</returns>
        public static int GetUserIdFromToken(HttpContext httpContext)
        {
            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return 0;
            }

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }
    }
}
