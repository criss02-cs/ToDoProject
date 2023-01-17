using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ToDoProject.Client.Models;
using ToDoProject.Models.DTO.Auth;
using ToDoProject.Models.Entities;

namespace ToDoProject.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticationService(HttpClient client, ILocalStorageService localStorageService)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _localStorageService = localStorageService;
        }

        public async Task<RegistrationResponse> LoginUser(LoginRequest loginRequest)
        {
            var loginResult = await _client.PostAsJsonAsync("api/auth/Login", loginRequest);
            var loginContent = await loginResult.Content.ReadAsStringAsync();
            if(loginResult.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RegistrationResponse>(loginContent, _options);
                var user = new UserLocalStorage
                {
                    Token = result?.Token,
                    User = result?.User
                };
                await _localStorageService.SetItemAsync("currentuser", user);
                return result;
            }
            return new RegistrationResponse { IsSuccesfulRegistration = false };
        }

        public async Task<ConfirmEmail> ConfirmEmail(ConfirmEmail confirmEmail)
        {
            var result = await _client.PostAsJsonAsync("api/auth/ConfirmEmail", confirmEmail);
            var content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<ConfirmEmail>(content, _options);
                return response;
            }
            return new ConfirmEmail { Email = confirmEmail.Email, Token = confirmEmail.Token, IsEmailConfirmed = false, IsTokenValid = false };
        }

        public async Task<RegistrationResponse> RegisterUser(RegistrationRequest registrationRequest)
        {
            var registrationResult = await _client.PostAsJsonAsync("api/auth/Register", registrationRequest);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();

            if (registrationResult.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RegistrationResponse>(registrationContent, _options);
                var user = new UserLocalStorage
                {
                    Token = result.Token,
                    User = result.User
                };
                await _localStorageService.SetItemAsync("currentuser", user);
                return result;
            }
            return new RegistrationResponse { IsSuccesfulRegistration = false };
        }
    }
}
