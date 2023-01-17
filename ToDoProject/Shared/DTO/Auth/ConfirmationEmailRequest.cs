using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Models.DTO.Auth
{
    public class ConfirmationEmailRequest
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
    }
}
