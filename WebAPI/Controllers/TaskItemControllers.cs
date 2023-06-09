﻿using ApplicationCore.Models;
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
    /// Controller for managing task items.
    /// </summary>
    [ApiController]
    [Route("api/taskitems")]
    public class TaskItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserEntity> _manager;

        public TaskItemController(ApplicationDbContext context, UserManager<UserEntity> manager)
        {
            _context = context;
            _manager = manager;
        }

        /// <summary>
        /// Retrieves all task items.
        /// </summary>
        /// <returns>A list of task items.</returns>
        [HttpGet("all")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> GetTaskItems()
        {
            var taskItems = await _context.TaskItems.ToListAsync();
            var taskItemDtos = taskItems.Select(MapToDto).ToList();
            return Ok(taskItemDtos);
        }

        /// <summary>
        /// Retrieves a task item by its ID.
        /// </summary>
        /// <param name="id">The ID of the task item.</param>
        /// <returns>The task item.</returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> GetTaskItem(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            var taskItemDto = MapToDto(taskItem);
            return Ok(taskItem);
        }

        /// <summary>
        /// Creates a new task item.
        /// </summary>
        /// <param name="taskItemDto">The task item DTO containing the task item data.</param>
        /// <returns>The created task item.</returns>
        [HttpPost("create")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> CreateTaskItem(TaskItemDto taskItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskItem = new TaskItem
            {
                Id = taskItemDto.Id,
                Name = taskItemDto.Name,
                Description = taskItemDto.Description,
                Priority = taskItemDto.Priority,
                MilestoneId = taskItemDto.MilestoneId
            };

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            return Ok(taskItem);
        }

        /// <summary>
        /// Updates an existing task item.
        /// </summary>
        /// <param name="id">The ID of the task item to update.</param>
        /// <param name="taskItemDto">The task item DTO containing the updated task item data.</param>
        /// <returns>
        /// NoContent if the update is successful,
        /// NotFound if the task item is not found,
        /// BadRequest if the request is invalid,
        /// or NotFound if the task item is not found after the update.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> UpdateTaskItem(int id, TaskItemDto taskItemDto)
        {
            if (id != taskItemDto.Id)
            {
                return BadRequest();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.Name = taskItemDto.Name;
            taskItem.Description = taskItemDto.Description;
            taskItem.Priority = taskItemDto.Priority;
            taskItem.MilestoneId = taskItemDto.MilestoneId;

            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
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
        /// Deletes a task item.
        /// </summary>
        /// <param name="id">The ID of the task item to delete.</param>
        /// <returns>
        /// Ok with the deleted task item if the deletion is successful,
        /// NotFound if the task item is not found,
        /// or Forbid if the user is not authorized to delete the task item.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
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

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            var taskItemDto = MapToDto(taskItem);
            return Ok(taskItem);
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }

        private TaskItemDto MapToDto(TaskItem taskItem)
        {
            return new TaskItemDto
            {
                Name = taskItem.Name,
                Description = taskItem.Description,
                Priority = taskItem.Priority,
                MilestoneId = taskItem.MilestoneId
            };
        }
    }
}