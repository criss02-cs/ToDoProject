using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Models.DTO.Auth
{
    public class RegistrationResponse
    {
        public bool IsSuccesfulRegistration { get; set; }
        public string? Error { get; set; }
        public UserDTO? User { get; set; }
        public string? Token { get; set; }
    }
}
