using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            //var manager = new Todo
            return Ok();
        }
    }
}
