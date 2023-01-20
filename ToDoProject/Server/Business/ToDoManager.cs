using ToDoProject.Models;
using ToDoProject.Models.DTO;
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

        public List<ToDoItemDTO> GetToDoItemsByUserId(Guid userId)
        {
            try
            {
                var response = new List<ToDoItemDTO>();
                // Mi prendo tutti gli items che non sono stati eliminati
                // e che sono stati creati dall'utente passato per parametro
                var items = _repository.Get(x => x.UserId == userId && !x.IsDeleted);
                if(items != null)
                {
                    // Li trasformo in DTO e li aggiungo alla lista
                    foreach (var item in items)
                    {
                        var model = ToDoItemDTO.Create(item);
                        response.Add(model);
                    }
                }
                // Al massimo ritorno una lista vuota
                return response;
            }
            catch (Exception)
            {
                return new List<ToDoItemDTO>();
            }
        }

        public WebApiResponse<bool> InsertToDoItem(ToDoItemDTO toDoItem)
        {
            try
            {
                var response = new WebApiResponse<bool>();
                var entity = ToDoItemDTO.GetEntity(toDoItem);
                var userRepository = new GenericRepository<User>(_ctx);
                // Mi prendo l'utente tramite il suo id
                var user = userRepository.GetById(entity.UserId);
                //Controllo che l'utente esista veramente
                if (user is not null)
                {
                    // Inserisco il todo nel database
                    _repository.Insert(entity);
                    // Aggancio l'utente alla mia entity
                    entity.User = user;
                    // Aggiungo alla lista di todoitems il todo da inserire
                    user?.ToDoItems?.Add(entity);
                    userRepository.Update(user);
                    var rows = _ctx.SaveChanges();
                    if(rows > 0)
                    {
                        response.Result = true;
                        response.IsSuccesful= true;
                        response.Error = "";
                        return response;
                    }
                    response.Result = false;
                    response.IsSuccesful = true;
                    response.Error = "C'è stato un errore con l'inserimento, riprova";
                    return response;
                }
                response.Result = false;
                response.IsSuccesful = true;
                response.Error = "L'utente associato non esiste";
                return response;
            }
            catch (Exception e)
            {
                return new WebApiResponse<bool>
                {
                    Error = e.Message,
                    IsSuccesful = false,
                    Result = false,
                };
            }
        }
    }
}
