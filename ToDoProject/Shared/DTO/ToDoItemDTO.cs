using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Models.Entities;
using ToDoProject.Shared.Enums;

namespace ToDoProject.Models.DTO
{
    public class ToDoItemDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ToDoPriority Priority { get; set; }
        public ToDoType Type { get; set; }
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
                UserName = entity.User?.Name
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
            };
        }
    }
}
