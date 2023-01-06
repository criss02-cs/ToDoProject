using ToDoProject.Models.DTO;
using ToDoProject.Models.DTO.Auth;
using ToDoProject.Models.Entities;
using ToDoProject.Server.Repositories;

namespace ToDoProject.Server.Business
{
    public class AuthManager
    {
        private DatabaseContext _ctx;
        private GenericRepository<User> _repository;

        public AuthManager(DatabaseContext ctx)
        {
            _ctx = ctx;
            _repository = new GenericRepository<User>(_ctx);
        }


        public RegistrationResponse Register(RegistrationRequest request)
        {
            var result = new RegistrationResponse();
            var user = _repository.GetFirstOrDefault(x => x.Email.Equals(request.Email));
            // Se esiste un utente con quella email ritorno l'errore
            if (user is not null)
            {
                result.IsSuccesfulRegistration = false;
                result.Error = "Esiste già un utente con quella email";
                return result;
            }
            user.Surname = request.Surname;
            user.Password = request.Password;
            user.Email = request.Email;
            user.Name = request.Name;
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = DateTime.Now;
            user.IsDeleted = false;
            var userManager = new UserManager(_ctx);
            var model = UserDTO.Create(user);
            userManager.Insert(model);
            result.User = model;
            return result;
        }
    }
}
