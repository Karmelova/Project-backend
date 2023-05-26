using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Security
{
    /// <summary>
    /// Helper class for JWT token operations and user authorization.
    /// </summary>
    public static class JwtTokenHelper
    {
        /// <summary>
        /// Retrieves the user ID from the JWT token.
        /// </summary>
        /// <param name="token">JWT token string.</param>
        /// <returns>User ID extracted from the token.</returns>
        public static string GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var currentUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return currentUserId;
        }

        /// <summary>
        /// Checks if the user with the specified ID is an admin user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="manager">UserManager instance for managing user-related operations.</param>
        /// <returns>True if the user is an admin user; otherwise, false.</returns>
        public static async Task<bool> IsAdminUserAsync(string userId, UserManager<UserEntity> manager)
        {
            var user = await manager.FindByIdAsync(userId);
            if (await manager.IsInRoleAsync(user, "ADMIN"))
            {
                return true;
            }

            return false;
        }
    }
}