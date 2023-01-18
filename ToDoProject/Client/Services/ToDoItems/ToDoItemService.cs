using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ToDoProject.Client.Models;
using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Services.ToDoItems
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private readonly string ToDoItemEndPoint = "api/todoitems";

        public ToDoItemService(HttpClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }



        public async Task<IList<ToDoItemDTO>> GetToDoItemsByIdUserAsync(Guid userId)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser?.Token);
                var response = await _client.GetFromJsonAsync<IList<ToDoItemDTO>>($"{this.ToDoItemEndPoint}/GetToDoItemsByUserId/{userId}");
                return response;
            }
            catch (Exception)
            {
                return new List<ToDoItemDTO>();
            }
        }

        private UserLocalStorage? CurrentUser
        {
            get => (_authenticationStateProvider as CustomAuthenticationStateProvider)?.CurrentUser;
        }
    }
}
