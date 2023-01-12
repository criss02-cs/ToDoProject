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
using System.Security.Claims;
using ToDoProject.Client.Models;
using ToDoProject.Client.Services;
using ToDoProject.Models.DTO;
using ToDoProject.Models.Enums;
using ToDoProject.Shared.Enums;

namespace ToDoProject.Client.Shared
{
    public partial class NavMenu
    {
        private bool collapseNavMenu = true;
        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;
        private UserLocalStorage CurrentUser { get => (AuthenticationService as CustomAuthenticationStateProvider)?.CurrentUser; }

        private MudDropContainer<ToDoItemDTO> _dropContainer;
        private readonly string?[] _sections = new string?[3] {
            Enum.GetName(ToDoType.DA_INIZIARE),
            Enum.GetName(ToDoType.IN_CORSO),
            Enum.GetName(ToDoType.FINITO)
        };

        private List<ToDoItemDTO> _tasks;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        // Da completare dopo aver finito la parte degli utenti o dopo aver completato il manager dei ToDoItems
        //protected override Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if(firstRender)
        //    {

        //    }
        //}
    }
}