using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoProject.Models.DTO;
using ToDoProject.Server.Business;

namespace ToDoProject.Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IConfiguration _configuration;
        private DatabaseContext _ctx;

        public UsersController(IConfiguration conf, DatabaseContext ctx)
        {
            _configuration = conf;
            _ctx = ctx;
        }

        [HttpGet, Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var manager = new UserManager(_ctx);
            var result = manager.GetUsers();
            return Ok(result);
        }

        [HttpPost, Route("AddNewUser")]
        public IActionResult AddNewUser([FromBody] UserDTO user)
        {
            var manager = new UserManager(_ctx);
            var result = manager.Insert(UserDTO.GetEntity(user));
            return Ok(result);
        }
    }
}
