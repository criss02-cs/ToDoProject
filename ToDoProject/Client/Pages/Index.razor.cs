using Microsoft.AspNetCore.Components;
using MudBlazor;
using ToDoProject.Client.Models;
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

        public UserLocalStorage? CurrentUser { get => (AuthenticationStateProvider as CustomAuthenticationStateProvider)?.CurrentUser; }
    }
}