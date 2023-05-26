using ApplicationCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dto;
using WebAPI.Security;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing milestones.
    /// </summary>
    [ApiController]
    [Route("api/milestones")]
    public class MilestoneController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserEntity> _manager;

        public MilestoneController(ApplicationDbContext context, UserManager<UserEntity> manager)
        {
            _context = context;
            _manager = manager;
        }

        /// <summary>
        /// Retrieves all milestones.
        /// </summary>
        /// <returns>A list of milestone DTOs.</returns>
        [HttpGet("all")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> GetMilestones()
        {
            var milestones = await _context.Milestones.ToListAsync();
            return Ok(milestones);
        }

        /// <summary>
        /// Retrieves a milestone by its ID.
        /// </summary>
        /// <param name="id">The ID of the milestone.</param>
        /// <returns>The milestone DTO.</returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> GetMilestone(int id)
        {
            var milestone = await _context.Milestones.FindAsync(id);

            if (milestone == null)
            {
                return NotFound();
            }

            var milestoneDto = MapToDto(milestone);
            return Ok(milestone);
        }

        /// <summary>
        /// Creates a new milestone.
        /// </summary>
        /// <param name="milestoneDto">The milestone DTO containing the milestone data.</param>
        /// <returns>The created milestone DTO.</returns>
        [HttpPost("create")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> CreateMilestone(MilestoneDto milestoneDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var milestone = new Milestone
            {
                Id = milestoneDto.Id,
                Name = milestoneDto.Name,
                Description = milestoneDto.Description,
                ProjectId = milestoneDto.ProjectId
            };

            _context.Milestones.Add(milestone);
            await _context.SaveChangesAsync();

            return Ok(milestone);
        }

        /// <summary>
        /// Updates an existing milestone.
        /// </summary>
        /// <param name="id">The ID of the milestone to update.</param>
        /// <param name="milestoneDto">The milestone DTO containing the updated milestone data.</param>
        /// <returns>No content if the update is successful, NotFound if the milestone is not found, or BadRequest if the ID in the URL does not match the ID in the DTO.</returns>

        [HttpPut("{id}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> UpdateMilestone(int id, MilestoneDto milestoneDto)
        {
            if (id != milestoneDto.Id)
            {
                return BadRequest();
            }

            var milestone = await _context.Milestones.FindAsync(id);

            if (milestone == null)
            {
                return NotFound();
            }

            milestone.Name = milestoneDto.Name;
            milestone.Description = milestoneDto.Description;
            milestone.ProjectId = milestoneDto.ProjectId;

            _context.Entry(milestone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MilestoneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a milestone.
        /// </summary>
        /// <param name="id">The ID of the milestone to delete.</param>
        /// <returns>
        /// Ok with the deleted milestone DTO if the deletion is successful,
        /// NotFound if the milestone is not found,
        /// Forbid if the current user is not authorized to delete,
        /// or BadRequest if the deletion fails.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> DeleteMilestone(int id)
        {
            var milestone = await _context.Milestones.FindAsync(id);

            if (milestone == null)
            {
                return NotFound();
            }
            var authorizationHeader = HttpContext.Request.Headers["Authorization"];
            var token = authorizationHeader.ToString().Replace("Bearer ", "");

            var currentUserId = JwtTokenHelper.GetUserIdFromToken(token);
            bool isAdmin = await JwtTokenHelper.IsAdminUserAsync(currentUserId, _manager);
            if (!isAdmin)
            {
                return Forbid(); // User is not authorized to delete
            }
            _context.Milestones.Remove(milestone);
            await _context.SaveChangesAsync();

            var milestoneDto = MapToDto(milestone);
            return Ok(milestone);
        }

        private bool MilestoneExists(int id)
        {
            return _context.Milestones.Any(e => e.Id == id);
        }

        private MilestoneDto MapToDto(Milestone milestone)
        {
            return new MilestoneDto
            {
                Name = milestone.Name,
                Description = milestone.Description,
                ProjectId = milestone.ProjectId
            };
        }
    }
}