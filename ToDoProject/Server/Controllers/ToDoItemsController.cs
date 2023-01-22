using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoProject.Models;
using ToDoProject.Models.DTO;
using ToDoProject.Server.Business;

namespace ToDoProject.Server.Controllers
{
    [Route("api/todoitems")]
    [ApiController]
    [Authorize]
    public class ToDoItemsController : ControllerBase
    {
        private IConfiguration _configuration;
        private DatabaseContext _ctx;

        public ToDoItemsController(IConfiguration conf, DatabaseContext ctx)
        {
            _configuration = conf;
            _ctx = ctx;
        }
        [HttpGet, Route("GetToDoItemsByUserId/{userId:Guid}")]
        public IActionResult GetToDoItemsByUserId(Guid userId)
        {
            var manager = new ToDoManager(_ctx);
            var response = manager.GetToDoItemsByUserId(userId);
            return Ok(response);
        }

        [HttpPost, Route("AddNewToDo")]
        public ActionResult<WebApiResponse<bool>> AddNewToDo([FromBody] ToDoItemDTO todoItem)
        {
            var manager = new ToDoManager(_ctx);
            var response = manager.InsertToDoItem(todoItem);
            return Ok(response);
        }
    }
}
