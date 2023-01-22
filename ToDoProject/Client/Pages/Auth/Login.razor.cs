using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ToDoProject.Client.Models;
using ToDoProject.Client.Services;
using ToDoProject.Client.Shared.Dialogs;
using ToDoProject.Models.DTO.Auth;

namespace ToDoProject.Client.Pages.Auth
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
        [Inject]
        public IDialogService DialogService { get; set; }

        private LoginRequest _request = new LoginRequest();
        private bool showPassword = false;

        public InputType PasswordInput { get; set; } = InputType.Password;
        public string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
        public bool ShowErrors { get; set; } = false;

        public bool IsLoading { get; set; } = false;
        public string? Error { get; set; }

        public void ShowHidePassword()
        {
            if (showPassword)
            {
                showPassword = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                showPassword = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

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
            }
        }
        public void NavigateToRegister()
        {
            NavigationManager.NavigateTo("/registration");
        }

        public void TestDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("ErrorText", "Esiste già un utente con quella email!");
            var options = new DialogOptions()
            {
                CloseButton = true,
                CloseOnEscapeKey = true,
                //FullWidth= true,
            };
            DialogService.Show<ErrorDialog>("Errore", parameters, options);
        }
    }
}
