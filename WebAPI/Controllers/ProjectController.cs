using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto;
using Infrastructure.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using WebAPI.Security;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing projects.
    /// </summary>
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserEntity> _manager;

        public ProjectController(ApplicationDbContext context, UserManager<UserEntity> manager)
        {
            _context = context;
            _manager = manager;
        }

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <returns>A list of projects.</returns>
        [HttpGet]
        [Authorize(Policy = "Bearer")]
        [Route("all")]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _context.Projects
                .Include(p => p.Milestones)
                .ToListAsync();

            return Ok(projects);
        }

        /// <summary>
        /// Retrieves a project by its ID.
        /// </summary>
        /// <param name="id">The ID of the project.</param>
        /// <returns>The project.</returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                return Ok(project);
            }
            return NotFound();
        }

        /// <summary>
        /// Adds a new project.
        /// </summary>
        /// <param name="projectDto">The project DTO containing the project data.</param>
        /// <returns>The created project.</returns>
        [HttpPost]
        [Authorize(Policy = "Bearer")]
        [Route("add")]
        public async Task<IActionResult> AddProject([FromBody] ProjectDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = new Project
            {
                Id = projectDto.Id,
                Name = projectDto.Name,
                Description = projectDto.Description
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="projectDto">The project DTO containing the updated project data.</param>
        /// <returns>
        /// The updated project if the update is successful,
        /// NotFound if the project is not found,
        /// or BadRequest if the update fails.
        /// </returns>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = _context.Projects.Find(id);
            if (project != null)
            {
                project.Name = projectDto.Name;
                project.Description = projectDto.Description;
                await _context.SaveChangesAsync();
                return Ok(project);
            }
            return NotFound();
        }

        /// <summary>
        /// Deletes a project.
        /// </summary>
        /// <param name="id">The ID of the project to delete.</param>
        /// <returns>
        /// Ok with a success message if the deletion is successful,
        /// NotFound if the project is not found,
        /// Forbid if the current user is not authorized to delete,
        /// or BadRequest if the deletion fails.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorizationHeader = HttpContext.Request.Headers["Authorization"];
            var token = authorizationHeader.ToString().Replace("Bearer ", "");

            var currentUserId = JwtTokenHelper.GetUserIdFromToken(token);
            bool isAdmin = await JwtTokenHelper.IsAdminUserAsync(currentUserId, _manager);
            if (!isAdmin)
            {
                return Forbid(); // User is not authorized to delete
            }
            var project = _context.Projects.Find(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return Ok("Project hes been removed from database");
            }
            return NotFound();
        }
    }
}