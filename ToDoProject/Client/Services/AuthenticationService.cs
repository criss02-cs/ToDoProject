using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ToDoProject.Models.DTO.Auth;

namespace ToDoProject.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public AuthenticationService(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<RegistrationResponse> RegisterUser(RegistrationRequest registrationRequest)
        {
            var content = JsonSerializer.Serialize(registrationRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            //var registrationResult = await _client.PostAsync("api/auth/Register", bodyContent);
            var registrationResult = await _client.PostAsJsonAsync("api/auth/Register", registrationRequest);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();

            if (registrationResult.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RegistrationResponse>(registrationContent, _options);
                return result;
            }
            return new RegistrationResponse { IsSuccesfulRegistration = false };
        }
    }
}
