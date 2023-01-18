using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Services.ToDoItems
{
    public interface IToDoItemService
    {
        Task<IList<ToDoItemDTO>> GetToDoItemsByIdUserAsync(Guid userId);
    }
}
