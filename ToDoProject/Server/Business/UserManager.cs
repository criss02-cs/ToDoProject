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
            return null;
        }

        public bool Insert(UserDTO model)
        {
            User entity = UserDTO.GetEntity(model);
            _repository.Insert(entity);
            var righe = _ctx.SaveChanges();
            return righe > 0;
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
