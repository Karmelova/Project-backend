using Infrastructure.EF.Entities;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Configuration;
using WebAPI.Dto;
using WebAPI.Security;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller responsible for authentication-related actions.
    /// </summary>
    [ApiController, Route("/api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<UserEntity> _manager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger _logger;

        public AuthenticationController(UserManager<UserEntity> manager, ILogger<AuthenticationController> logger, IConfiguration configuration, JwtSettings jwtSettings)
        {
            _manager = manager;
            _logger = logger;
            _jwtSettings = jwtSettings;
        }

        /// <summary>
        /// Authenticates a user by generating and returning a JWT token.
        /// </summary>
        /// <param name="user">User credentials.</param>
        /// <returns>JWT token if authentication is successful, otherwise Unauthorized.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized();
            }
            var logged = await _manager.FindByNameAsync(user.LoginName);
            if (await _manager.CheckPasswordAsync(logged, user.Password))
            {
                return Ok(new { Token = CreateToken(logged) });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">User entity.</param>
        /// <returns>JWT token.</returns>
        private string CreateToken(UserEntity user)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
                .AddClaim(JwtRegisteredClaimNames.Name, user.UserName)
                .AddClaim(JwtRegisteredClaimNames.Gender, "male")
                .AddClaim(JwtRegisteredClaimNames.Email, user.Email)
                .AddClaim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds())
                .AddClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid())
                .AddClaim(ClaimTypes.NameIdentifier, user.Id)
                .Audience(_jwtSettings.Audience)
                .Issuer(_jwtSettings.Issuer)
                .Encode();
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDto">User information.</param>
        /// <returns>Ok if registration is successful, otherwise BadRequest with errors.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new UserEntity
            {
                UserName = userDto.Username,
                Email = userDto.Email
            };

            var result = await _manager.CreateAsync(user, userDto.Password);
            _manager.AddToRoleAsync(user, "USER");
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                // Registration failed, return appropriate error response
                var errors = result.Errors.Select(error => error.Description);
                return BadRequest(errors);
            }
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>Ok if deletion is successful, NotFound if user is not found, Forbid if user is not authorized, or BadRequest with errors.</returns>
        [HttpDelete("users/{userId}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _manager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var authorizationHeader = HttpContext.Request.Headers["Authorization"];
            var token = authorizationHeader.ToString().Replace("Bearer ", "");

            var currentUserId = JwtTokenHelper.GetUserIdFromToken(token);
            if (currentUserId == userId)
            {
                return Forbid(); // User is not authorized to delete
            }

            bool isAdmin = await JwtTokenHelper.IsAdminUserAsync(currentUserId, _manager);
            if (!isAdmin)
            {
                return Forbid(); // User is not authorized to delete
            }
            var result = await _manager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User was deleted succesfully");
            }
            else
            {
                // Failed to delete user, return appropriate error response
                var errors = result.Errors.Select(error => error.Description);
                return BadRequest(errors);
            }
        }
    }
}