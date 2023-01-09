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
            // Mi prendo il primo utente con quella email che non sia eliminato
            var user = _repository.GetFirstOrDefault(x => x.Email.Equals(request.Email) && !x.IsDeleted);
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

        public RegistrationResponse Login(LoginRequest request)
        {
            var result = new RegistrationResponse();
            // Mi prendo il primo utente con quella email che non sia eliminato
            var user = _repository.GetFirstOrDefault(x => x.Email.Equals(request.Email) && !x.IsDeleted);
            //Se non esiste nessun utente ritorno l'errore
            if(user is null)
            {
                result.IsSuccesfulRegistration = false;
                result.Error = "Non esiste nessun utente con quella email";
                return result;
            }
            // Se la password inserita non corrisponde a quella dell'utente
            // non posso far autenticare l'utente
            if(!user.Password.Equals(request.Password)) 
            {
                result.IsSuccesfulRegistration = false;
                result.Error = "La password è errata";
                return result;
            }
            //Arrivato qui sono sicuro che l'utente abbia inserito le giuste credenziali
            result.User = UserDTO.Create(user);
            result.IsSuccesfulRegistration = true;
            return result;
        }
    }
}
