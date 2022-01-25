using Common.Constants;
using Common.FileStorage;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoApp.Models.Users;
using ToDoApp.Services.UserService;
using ToDoAppWeb.Auth;

namespace ToDoAppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public UserService _userService;

        public LoginController() : base()
        {
            _userService = new UserService(new UsersRepository());
        }

        [HttpGet]
        public async Task<ActionResult<int>> Login()
        {
            User currentUser = await _userService.GetUserFromDbByCredentials(Request);

            if (currentUser is not null)
            {
                JsonFileStorage.Write(Constants.CurrentUserLogFileName, currentUser);

                return currentUser.Id;
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult<int> Logout()
        {
            User currentUser = AuthHelper.GetCurrentUserFromLogByCredentials(Request);

            if (currentUser is not null)
            {
                JsonFileStorage.Write(Constants.CurrentUserLogFileName, new object());
            
                return currentUser.Id;
            }

            return Unauthorized();
        }
    }
}
