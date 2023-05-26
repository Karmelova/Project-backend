using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowNull]
        public string Description { get; set; }

        [AllowNull]
        public Priority Priority { get; set; }

        public int MilestoneId { get; set; }
        public Milestone Milestone { get; set; }
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }
}