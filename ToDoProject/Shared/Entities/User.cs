using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Models.Enums;

namespace ToDoProject.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public UserType UserType { get; set; }
        public bool IsEmailConfirmed { get; set; }
        // Foreign key per le task
        public virtual IList<ToDoItem>? ToDoItems { get; set; }
    }
}
