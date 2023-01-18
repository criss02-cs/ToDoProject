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
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using MudBlazor;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoProject.Client.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ToDoProject.Client.Services.ToDoItems;
using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Pages
{
    public partial class Index
    {
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        ISnackbar Snackbar { get; set; }
        [Inject]
        IToDoItemService ToDoItemService { get; set; }

        private IList<ToDoItemDTO> ToDoItems { get; set; }


        protected override async Task OnInitializedAsync()
        {
            //Codice per ottenere tutte le task dell'utente
            ToDoItems = await ToDoItemService.GetToDoItemsByIdUserAsync(CurrentUser.User.Id);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (!CurrentUser.User.IsEmailConfirmed)
                {
                    Snackbar.Add("La tua mail ha bisogno di essere confermata", Severity.Warning);
                }
            }
        }

        public UserLocalStorage? CurrentUser { get => (AuthenticationStateProvider as CustomAuthenticationStateProvider)?.CurrentUser; }
    }
}