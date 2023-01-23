using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ToDoProject.Client.Models;
using ToDoProject.Models;
using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Services.ToDoItems
{
    public class ToDoItemService : IToDoItemService
    {
        public event EventHandler<List<ToDoItemDTO>> ListChanged;


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



        public async Task<List<ToDoItemDTO>> GetToDoItemsByIdUserAsync(Guid userId)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser?.Token);
                var response = await _client.GetFromJsonAsync<List<ToDoItemDTO>>($"{this.ToDoItemEndPoint}/GetToDoItemsByUserId/{userId}");
                return response;
            }
            catch (Exception ex)
            {
                return new List<ToDoItemDTO>();
            }
        }

        public async Task<WebApiResponse<bool>> AddToDoItem(ToDoItemDTO dto)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser?.Token);
                var response = await _client.PostAsJsonAsync("api/todoitems/AddNewToDo", dto, _options);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<WebApiResponse<bool>>(content, _options);
                    if (result.IsSuccesful)
                    {
                        var todos = await GetToDoItemsByIdUserAsync(CurrentUser.User.Id);
                        ListChanged?.Invoke(this, todos);
                    }
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
