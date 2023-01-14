using ToDoProject.Models.Entities;
using ToDoProject.Server.Repositories;

namespace ToDoProject.Server.Business
{
    public class ToDoManager
    {
        private DatabaseContext _ctx;
        private IGenericRepository<ToDoItem> _repository;

        public ToDoManager(DatabaseContext ctx)
        {
            _ctx = ctx;
            _repository = new GenericRepository<ToDoItem>(ctx);
        }
    }
}
