using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Models.DTO.Auth
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "Devi inserire il tuo nome")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "La password è obbligatoria")]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Le 2 password non corrispondono")]
        public string? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "L'email è obbligatoria")]
        public string? Email { get; set; }
        public string? Surname { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
