using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Models.DTO.Auth
{
    public class ConfirmEmail
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public bool IsTokenValid { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
