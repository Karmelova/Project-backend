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

        [HttpGet("all")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> GetMilestones()
        {
            var milestones = await _context.Milestones.ToListAsync();
            var milestoneDtos = milestones.Select(MapToDto).ToList();
            return Ok(milestoneDtos);
        }

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
            return Ok(milestoneDto);
        }

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
                Name = milestoneDto.Name,
                Description = milestoneDto.Description,
                ProjectId = milestoneDto.ProjectId
            };

            _context.Milestones.Add(milestone);
            await _context.SaveChangesAsync();

            return Ok(milestoneDto);
        }

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
            return Ok(milestoneDto);
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