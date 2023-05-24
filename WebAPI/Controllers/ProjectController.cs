using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto;
using ApplicationCore.Interfaces.ProjectService;
using Infrastructure.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "Bearer")]
        [Route("all")]
        public async Task<IActionResult> GetProjects()
        {
            return Ok(await _context.Projects.ToListAsync());
        }

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
    }

    //public ProjectController(IProjectService projectService)
    //{
    //    _projectService = projectService;
    //}

    //[HttpPost]
    //public ActionResult CreateProject([FromBody] ProjectDto projectDto)
    //{
    //    if (projectDto == null)
    //    {
    //        return BadRequest();
    //    }

    //    var project = new Project
    //    {
    //        Name = projectDto.Name,
    //        Description = projectDto.Description
    //    };

    //    _projectService.Create(project);

    //    return Ok();
    //}
    //public async Task<IActionResult> CreateProject([FromBody] ProjectDto project)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return Unauthorized();
    //    }
    //    var logged = await _manager.FindByNameAsync(user.LoginName);
    //    if (await _manager.CheckPasswordAsync(logged, user.Password))
    //    {
    //        return Ok(new { Token = CreateToken(logged) });
    //    }
    //    return Unauthorized();
    //}

    //[HttpGet("{id}")]
    //public ActionResult<ProjectDto> GetProjectById(int id)
    //{
    //    var project = _projectService.GetById(id);
    //    if (project == null)
    //    {
    //        return NotFound();
    //    }

    //    return Ok(project);
    //}

    //[HttpPut("{id}")]
    //public IActionResult UpdateProject(int id, [FromBody] ProjectDto projectDto)
    //{
    //    if (projectDto == null)
    //    {
    //        return BadRequest();
    //    }

    //    var project = _projectService.GetById(id);
    //    if (project == null)
    //    {
    //        return NotFound();
    //    }

    //    project.Name = projectDto.Name;
    //    project.Description = projectDto.Description;

    //    _projectService.Update(project);

    //    return Ok();
    //}

    //[HttpDelete("{id}")]
    //public IActionResult DeleteProject(int id)
    //{
    //    var project = _projectService.GetById(id);
    //    if (project == null)
    //    {
    //        return NotFound();
    //    }

    //    _projectService.Delete(project);
    //    _projectService.Delete(project);

    //    return Ok();
    //}
}