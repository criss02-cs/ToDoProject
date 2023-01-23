﻿using ToDoProject.Models;
using ToDoProject.Models.DTO;

namespace ToDoProject.Client.Services.ToDoItems
{
    public interface IToDoItemService
    {
        event EventHandler<List<ToDoItemDTO>> ListChanged;
        Task<List<ToDoItemDTO>> GetToDoItemsByIdUserAsync(Guid userId);
        Task<WebApiResponse<bool>> AddToDoItem(ToDoItemDTO dto);
    }
}
