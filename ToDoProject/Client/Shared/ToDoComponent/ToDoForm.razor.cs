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
using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Shared.ToDoComponent
{
    public partial class ToDoForm
    {
        private ToDoItemDTO _todoItem = new ToDoItemDTO();
        [Parameter]
        public Action<ToDoItemDTO>? OnValidSubmit { get; set; }
    }
}