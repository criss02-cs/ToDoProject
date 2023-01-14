using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Services.Users
{
    public interface IUsersService
    {
        Task<IList<UserDTO>> GetAllUsers();
        Task<bool> AddUser(UserDTO userDTO);
    }
}
