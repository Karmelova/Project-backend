using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Security
{
    public static class JwtTokenHelper
    {
        public static string GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var currentUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return currentUserId;
        }

        public static async Task<bool> IsAdminUserAsync(string userid, UserManager<UserEntity> manager)
        {
            var user = await manager.FindByIdAsync(userid);
            if (await manager.IsInRoleAsync(user, "ADMIN"))
            {
                return true;
            }

            return false;
        }
    }
}