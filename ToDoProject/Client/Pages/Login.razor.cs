using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ToDoProject.Client.Models;
using ToDoProject.Client.Services;
using ToDoProject.Client.Shared.Dialogs;
using ToDoProject.Models.DTO.Auth;

namespace ToDoProject.Client.Pages
{
    public partial class Login
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ILocalStorageService localStorage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private LoginRequest _request = new LoginRequest();
        public bool ShowErrors { get; set; } = false;

        public bool IsLoading { get; set; } = false;
        public string? Error { get; set; }

        public async Task LoginUser()
        {
            this.ShowErrors = false;
            this.IsLoading = true;
            var result = await AuthenticationService.LoginUser(_request);
            this.IsLoading = false;
            if (!result.IsSuccesfulRegistration)
            {
                Error = result.Error;
                ShowErrors = true;
            }
            else
            {
                var user = new UserLocalStorage
                {
                    Token = result.Token,
                    User = result.User
                };
                // Salvo l'utente e il token nel local storage
                // Marco l'utente come autenticato così da poter accedere ai suoi dati anche da altre pagine
                ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(user);
                // Torno alla home
                NavigationManager.NavigateTo("/home");
                //Console.WriteLine(result.User?.Name);
            }
        }
        public void NavigateToRegister()
        {
            NavigationManager.NavigateTo("/registration");
        }
    }
}
