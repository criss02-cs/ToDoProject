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
using ToDoProject.Models.Enums;
using ToDoProject.Client.Shared.Dialogs;

namespace ToDoProject.Client.Shared.ToDoComponent
{
    public partial class ToDoItemsTable
    {
        [Parameter]
        public List<ToDoItemDTO>? ToDoItems { get; set; }
        [Parameter]
        public Action OnAggiungiClick { get; set; }

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
                    return "background-color: " + Colors.Orange.Lighten1;
                case ToDoType.IN_CORSO:
                    return "background-color: " + Colors.Yellow.Lighten1;
                case ToDoType.FINITO:
                    return "background-color: " + Colors.LightGreen.Lighten1;
                default:
                    return "";
            }
        }
    }
}