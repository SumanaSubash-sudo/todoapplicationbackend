using Microsoft.AspNetCore.Mvc;
using ToDoListapi.Database;
using ToDoListapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Http;

namespace ToDoListapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Todolist _toDoList;
        public UserController(Todolist toDoList)
        {
            this._toDoList = toDoList;
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var User = await _toDoList.User.ToListAsync();
            return Ok(User);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            user.UserId = Guid.NewGuid();
            await _toDoList.User.AddAsync(user);
            await _toDoList.SaveChangesAsync();
            return Ok(user);

        }
        [HttpPost]
        [Route("Login")]
        public IActionResult LoginRequest(LoginModel _user)
        {
            var isUserExist = _toDoList.User.SingleOrDefault(u => u.Email == _user.Email && u.Password == _user.Password);
            if (isUserExist == null)
            {
                return StatusCode(401, "wrong credentials");
            }
            else
            {
                return StatusCode(200, _user);

            }
           
        }

    }
}
