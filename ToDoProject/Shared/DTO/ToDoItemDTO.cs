using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Models.Entities;
using ToDoProject.Models.Enums;

namespace ToDoProject.Models.DTO
{
    public class ToDoItemDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Il ToDo deve avere un titolo")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Il ToDo deve avere una priorità")]
        public ToDoPriority Priority { get; set; }
        [Required(ErrorMessage = "Il ToDo deve avere un tipo")]
        public ToDoType Type { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }

        public static ToDoItemDTO Create(ToDoItem entity)
        {
            return new ToDoItemDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Priority = entity.Priority,
                Type = entity.Type,
                UserId = entity.UserId,
                UserName = entity.User?.Name,
                EndDate = entity.EndDate,
            };
        }

        public static ToDoItem GetEntity(ToDoItemDTO model)
        {
            return new ToDoItem
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Priority = model.Priority,
                Type = model.Type,
                UserId = model.UserId,
                EndDate = model.EndDate,
            };
        }
    }
}
