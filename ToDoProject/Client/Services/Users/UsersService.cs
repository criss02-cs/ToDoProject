using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ToDoProject.Client.Models;
using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UsersService(HttpClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task<IList<UserDTO>> GetAllUsers()
        {
            try
            {
                //Setto l'header
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser?.Token);
                var response = await _client.GetFromJsonAsync<IList<UserDTO>>("api/users/GetAllUsers");
                return response;
            }
            catch (Exception)
            {
                return new List<UserDTO>();
            }
        }


        private UserLocalStorage? CurrentUser
        {
            get => (_authenticationStateProvider as CustomAuthenticationStateProvider)?.CurrentUser;
        }
    }
}
