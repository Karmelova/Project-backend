using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI.Dto
{
    public class MilestoneDto
    {
        public int Id { get; }

        [Required]
        public string Name { get; set; }

        [AllowNull]
        public string Description { get; set; }

        [Required, NotNull]
        public int ProjectId { get; set; }
    }
}