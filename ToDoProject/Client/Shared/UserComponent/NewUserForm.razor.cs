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
using System.Text.RegularExpressions;
using ToDoProject.Client.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ToDoProject.Models.Enums;

namespace ToDoProject.Client.Shared.UserComponent
{
    public partial class NewUserForm
    {
        private UserDTO _user = new();
        public EditContext _editContext;

        [Inject]
        private AuthenticationStateProvider? StateProvider { get; set; }
        [Parameter]
        public Action<UserDTO>? OnValidSubmit { get; set; }

        protected override void OnInitialized()
        {
            _user.UserType = UserType.NORMAL;
            _editContext = new EditContext(_user);
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
        public UserLocalStorage? CurrentUser
        {
            get => (StateProvider as CustomAuthenticationStateProvider)?.CurrentUser;
        }

        public IEnumerable<string> SelectedUserType
        {
            get => Enum.GetNames(typeof(UserType)).
                Where(x => x.Equals(Enum.GetName(_user.UserType))).
                Select(x => x.ToLower());
        }

        private bool IsValid()
        {
            if(_user.Name is not null && _user.Surname is not null && 
                _user.Email is not null && _user.Password is not null)
            {
                return !_user.Name.Equals("") && !_user.Surname.Equals("") &&
                    !_user.Email.Equals("") && !_user.Password.Equals("");
            }
            return false;
        }
    }
}