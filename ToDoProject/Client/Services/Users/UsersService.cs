using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ToDoProject.Client.Models;
using ToDoProject.Models;
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

        public async Task<WebApiResponse<bool>> AddUser(UserDTO userDTO)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser?.Token);
                var response = await _client.PostAsJsonAsync("api/users/AddNewUser", userDTO, _options);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                if(response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<WebApiResponse<bool>>(content, _options);
                    Console.WriteLine(result.Error);
                    Console.WriteLine(result.IsSuccesful);
                    Console.WriteLine("Result " + result.Result);
                    return result;
                }
                return new WebApiResponse<bool>
                {
                    Result = false,
                    IsSuccesful = false,
                    Error = "C'è stato un errore durante la connessione alla fonte dati"
                };
            }
            catch (Exception ex)
            {
                return new WebApiResponse<bool>
                {
                    Result = false,
                    IsSuccesful = false,
                    Error = ex.Message
                };
            }
            
        }


        private UserLocalStorage? CurrentUser
        {
            get => (_authenticationStateProvider as CustomAuthenticationStateProvider)?.CurrentUser;
        }
    }
}
