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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DateBirth { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public UserType UserType { get; set; }
        // Foreign key per le task
        public virtual IList<ToDoItem>? ToDoItems { get; set; }
    }
}
