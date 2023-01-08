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
        public ILocalStorageService localStorage { get; set; }
        public bool ShowRegistrationErrors { get; set; }
        public string? Error { get; set; }

        public async Task Register()
        {
            this.ShowRegistrationErrors = false;
            var result = await AuthenticationService.RegisterUser(_request);
            //_request.Name = result.User.Name;
            if (!result.IsSuccesfulRegistration)
            {
                Error = result.Error + " dio porco";
                ShowRegistrationErrors = true;
            }
            else
            {
                var user = new UserLocalStorage
                {
                    Token = result.Token,
                    User = result.User
                };
                await localStorage.SetItemAsync("currentuser", user);
                //Console.WriteLine(result.User?.Name);
            }
        }
    }
}