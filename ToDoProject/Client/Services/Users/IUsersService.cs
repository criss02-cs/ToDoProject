using ToDoProject.Models;
using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Services.Users
{
    public interface IUsersService
    {
        Task<IList<UserDTO>> GetAllUsers();
        Task<WebApiResponse<bool>> AddUser(UserDTO userDTO);
    }
}
