using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Models.DTO.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "L'email è obbligatoria")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "La password è obbligatoria")]
        public string? Password { get; set; }
    }
}
