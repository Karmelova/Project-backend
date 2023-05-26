using ApplicationCore.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI.Dto
{
    public class TaskItemDto
    {
        public int Id { get; }

        [Required, NotNull]
        public string Name { get; set; }

        [AllowNull]
        public string Description { get; set; }

        [AllowNull]
        public Priority Priority { get; set; }

        [Required, NotNull]
        public int MilestoneId { get; set; }
    }
}