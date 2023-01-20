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
using ToDoProject.Models.Enums;

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

        private List<ToDoItemDTO> ToDoItems { get; set; }


        protected override async Task OnInitializedAsync()
        {
            //Codice per ottenere tutte le task dell'utente
            ToDoItems = await ToDoItemService.GetToDoItemsByIdUserAsync(CurrentUser.User.Id);
            if(ToDoItems is null)
            {
                ToDoItems = new List<ToDoItemDTO>();
            }
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

        private void UpdateToDoItem(MudItemDropInfo<ToDoItemDTO> droppedItem)
        {
            droppedItem.Item.Type = Enum.Parse<ToDoType>(droppedItem.DropzoneIdentifier);
        }

        private string GetNameOfToDoType(ToDoType type)
        {
            switch (type)
            {
                case ToDoType.DA_INIZIARE:
                    return "Da iniziare";
                case ToDoType.IN_CORSO:
                    return "In corso";
                case ToDoType.FINITO:
                    return "Finito";
                default:
                    return "";
            }
        }

        private string GetClassOfDropZone(ToDoType type)
        {
            switch (type)
            {
                case ToDoType.DA_INIZIARE:
                    return "background-color: " + Colors.Red.Darken1;
                case ToDoType.IN_CORSO:
                    return "background-color: " + Colors.Yellow.Darken1;
                case ToDoType.FINITO:
                    return "background-color: " + Colors.LightGreen.Darken1;
                default:
                    return "";
            }
        }

        public UserLocalStorage? CurrentUser { get => (AuthenticationStateProvider as CustomAuthenticationStateProvider)?.CurrentUser; }
    }
}