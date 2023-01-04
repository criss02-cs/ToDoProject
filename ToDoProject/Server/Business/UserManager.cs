using ToDoProject.Models.DTO;
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
    }
}
