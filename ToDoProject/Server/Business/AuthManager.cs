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
            user = new User
            {
                Password = request.Password,
                Email = request.Email,
                Name = request.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false,
                BirthDate = request.BirthDate,
                Surname = request.Surname
            };
            var userManager = new UserManager(_ctx);
            var model = UserDTO.Create(user);
            userManager.Insert(user);
            result.User = model;
            result.User.Id = user.Id;
            result.IsSuccesfulRegistration = true;
            return result;
        }
    }
}
