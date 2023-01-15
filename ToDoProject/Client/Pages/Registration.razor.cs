using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using ToDoProject.Client;
using ToDoProject.Client.Shared;
using ToDoProject.Models.DTO.Auth;
using ToDoProject.Client.Services;
using Blazored.LocalStorage;
using ToDoProject.Client.Models;
using System.Text.RegularExpressions;
using MudBlazor;

namespace ToDoProject.Client.Pages
{
    public partial class Registration
    {
        private RegistrationRequest _request = new();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public bool ShowRegistrationErrors { get; set; } = false;

        public bool IsLoading { get; set; } = false;
        public string? Error { get; set; }
        private bool showPassword = false;
        private bool showConfirmPassword = false;

        public InputType PasswordInput { get; set; } = InputType.Password;
        public InputType ConfirmPasswordInput { get; set; } = InputType.Password;
        public string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
        public string ConfirmPasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;

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
        public void ShowHideConfirmPassword()
        {
            if (showConfirmPassword)
            {
                showConfirmPassword = false;
                ConfirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                ConfirmPasswordInput = InputType.Password;
            }
            else
            {
                showConfirmPassword = true;
                ConfirmPasswordInputIcon = Icons.Material.Filled.Visibility;
                ConfirmPasswordInput = InputType.Text;
            }
        }

        public void NavigateToLogin()
        {
            NavigationManager.NavigateTo("/");
        }
        public async Task Register()
        {
            this.ShowRegistrationErrors = false;
            this.IsLoading = true;
            var result = await AuthenticationService.RegisterUser(_request);
            this.IsLoading = false;
            //_request.Name = result.User.Name;
            if (!result.IsSuccesfulRegistration)
            {
                Error = result.Error;
                ShowRegistrationErrors = true;
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
        private string? PasswordMatch(string arg)
        {
            if (_request.Password != null && !_request.Password.Equals(arg))
                return "Le password non corrispondono";
            return null;
        }
        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "La password è obbligatoria!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "La password deve avere almeno 8 caratteri";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "La password deve contenere almeno una lettera maiuscola";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "La password deve contenere almeno una lettera minuscola";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "La password deve contenere almeno un numero";
        }
    }
}