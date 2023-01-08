using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Models
{
    public class UserLocalStorage
    {
        public string? Token { get; set; }
        public UserDTO? User { get; set; }
    }
}
