using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Shared.Enums;

namespace ToDoProject.Models.Entities
{
    public class ToDoItem : BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public ToDoPriority Priority { get; set; }
        [Required]
        public ToDoType Type { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
