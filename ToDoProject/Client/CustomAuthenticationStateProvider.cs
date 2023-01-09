using Blazored.LocalStorage;
using System.Security.Claims;
using ToDoProject.Client.Models;
using ToDoProject.Models.DTO;

namespace ToDoProject.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentUser = await _localStorage.GetItemAsync<UserLocalStorage>("currentuser");
            ClaimsIdentity identity;
            if (currentUser != null && currentUser.User != null)
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, currentUser.User.Email),
                    new Claim(ClaimTypes.Name , currentUser.User.Name),
                    new Claim(ClaimTypes.Role, currentUser.User.UserType.ToString()),
                });
            }
            else
            {
                identity = new ClaimsIdentity();
            }
            //identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }

        public void MarkUserAsAuthenticated(UserDTO user)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name , user.Name),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
            }, "apiauth_type");
            var u = new ClaimsPrincipal(identity);
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
