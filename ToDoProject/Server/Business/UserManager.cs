using ToDoProject.Models;
using ToDoProject.Models.DTO;
using ToDoProject.Models.DTO.Auth;
using ToDoProject.Models.Entities;
using ToDoProject.Server.Repositories;

namespace ToDoProject.Server.Business
{
    public class UserManager
    {
        private DatabaseContext _ctx;
        private IGenericRepository<User> _repository;

        public UserManager(DatabaseContext ctx)
        {
            _ctx = ctx;
            _repository = new GenericRepository<User>(ctx);
        }

        public IList<UserDTO>? GetUsers()
        {
            try
            {
                var user = _repository.Get(x => !x.IsDeleted, includes: x => x.ToDoItems);
                if (user.Count > 0)
                {
                    var usersDto = new List<UserDTO>();
                    foreach (var item in user)
                    {
                        var userDto = UserDTO.Create(item);
                        usersDto.Add(userDto);
                    }
                    return usersDto;
                }
                return new List<UserDTO>();
            }
            catch (Exception)
            {
                return new List<UserDTO>();
            }
        }

        public bool InsertDTO(UserDTO model)
        {
            User entity = UserDTO.GetEntity(model);
            _repository.Insert(entity);
            var righe = _ctx.SaveChanges();
            return righe > 0;
        }
        public WebApiResponse<bool> Insert(User entity)
        {
            try
            {
                // Controllo se non c'è già un utente con questa email
                var user = _repository.Get(x => !x.IsDeleted && x.Email.Equals(entity.Email));
                // Se non ci sono utenti con quella email procedo a inserire l'utente
                if (user.Count == 0)
                {
                    entity.CreatedDate = DateTime.Now;
                    entity.UpdatedDate = DateTime.Now;
                    _repository.Insert(entity);
                    var righe = _ctx.SaveChanges();
                    return new WebApiResponse<bool>()
                    {
                        Result = righe > 0,
                        IsSuccesful = true,
                        Error = ""
                    };
                }
                return new WebApiResponse<bool>
                {
                    Result = false,
                    IsSuccesful = false,
                    Error = "Esiste già un utente con quella email!"
                };
            }
            catch (Exception e)
            {
                return new WebApiResponse<bool>
                {
                    Result = false,
                    IsSuccesful = false,
                    Error = e.Message
                };
            }
        }

        public UserDTO GetUserById(Guid id)
        {
            var entity = _repository.GetById(id);
            var model = UserDTO.Create(entity);
            return model;
        }

        public bool DeleteLogical(Guid id)
        {
            var model = this.GetUserById(id);
            var entity = UserDTO.GetEntity(model);
            entity.IsDeleted = true;
            _repository.DeleteLogical(entity);
            var righe = _ctx.SaveChanges();
            return righe > 0;
        }
        public bool Update(UserDTO model)
        {
            User entity = UserDTO.GetEntity(model);
            _repository.Update(entity);
            var righe = _ctx.SaveChanges();
            return righe > 0;
        }
    }
}
