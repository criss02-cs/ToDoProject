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
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? BirthDate { get; set; }
        public UserType UserType { get; set; }
        public long? NumberOfTasks { get; set; }

        public static UserDTO Create(User entity)
        {
            return new UserDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Password = entity.Password,
                BirthDate = entity.BirthDate,
                UserType = entity.UserType,
                NumberOfTasks = entity.ToDoItems?.Count,
            };
        }

        public static User GetEntity(UserDTO model)
        {
            return new User
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                BirthDate = model.BirthDate,
                UserType = model.UserType,
            };
        }
    }
}
