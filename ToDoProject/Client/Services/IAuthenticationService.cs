using ToDoProject.Models.DTO.Auth;

namespace ToDoProject.Client.Services
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponse> RegisterUser(RegistrationRequest registrationRequest);
    }
}
