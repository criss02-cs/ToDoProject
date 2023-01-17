using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDoProject.Client.Models;
using ToDoProject.Models.DTO;

namespace ToDoProject.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public UserLocalStorage CurrentUser { get; private set; }

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _jwtSecurityTokenHandler = new();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var currentUser = await _localStorage.GetItemAsync<UserLocalStorage>("currentuser");
                ClaimsIdentity identity;
                // Controllo che effettivamente ci siano dei dati all'interno dello storage
                if (currentUser != null && currentUser.User != null && !string.IsNullOrWhiteSpace(currentUser.Token))
                {
                    // Vado a leggermi il token per verificare che non sia scaduto
                    JwtSecurityToken jwtSecurityToken =
                        _jwtSecurityTokenHandler.ReadJwtToken(currentUser.Token);
                    var expires = jwtSecurityToken.ValidTo;
                    if (expires < DateTime.UtcNow)
                    {
                        await _localStorage.RemoveItemAsync("currentuser");
                        identity = new ClaimsIdentity();
                    }
                    else
                    {
                        identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Email, currentUser.User.Email),
                            new Claim(ClaimTypes.Name , currentUser.User.Name),
                            new Claim(ClaimTypes.Role, currentUser.User.UserType.ToString()),
                        }, "apiauth_type");
                        CurrentUser = currentUser;
                    }
                }
                else
                {
                    identity = new ClaimsIdentity();
                }
                //identity = new ClaimsIdentity();
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            catch (Exception)
            {
                return new AuthenticationState(
                                    new ClaimsPrincipal(
                                        new ClaimsIdentity()));
            }
        }

        public void MarkUserAsAuthenticated(UserLocalStorage current)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, current.User.Email),
                new Claim(ClaimTypes.Name , current.User.Name),
                new Claim(ClaimTypes.Role, current.User.UserType.ToString()),
            }, "apiauth_type");
            var u = new ClaimsPrincipal(identity);
            CurrentUser = current;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(u)));
        }

        public void MarkUserAsLoggetOut()
        {
            _localStorage.RemoveItemAsync("currentuser");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
