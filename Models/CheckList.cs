using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckListAPI.Models
{
    public class CheckList
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? ChecklistName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}