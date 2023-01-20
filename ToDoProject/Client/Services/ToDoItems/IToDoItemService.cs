using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Services.ToDoItems
{
    public interface IToDoItemService
    {
        Task<List<ToDoItemDTO>> GetToDoItemsByIdUserAsync(Guid userId);
    }
}
