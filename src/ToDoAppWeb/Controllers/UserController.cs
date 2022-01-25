using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models.DTO.Requests;
using ToDoApp.Models.DTO.Responses;
using ToDoApp.Models.Users;
using ToDoApp.Services.UserService;
using ToDoAppWeb.Auth;

namespace ToDoAppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserService _userService;

        public UserController() : base()
        {
            _userService = new UserService(new UsersRepository());
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponseDTO>>> GetAll()
        {
            User currentUser = AuthHelper.AuthenticateAndAuthorizeUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var users = await _userService.ListUsers();

            List<UserResponseDTO> usersResponse = new List<UserResponseDTO>();

            foreach (var user in users)
            {
                usersResponse.Add(new UserResponseDTO()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    AddedOn = user.AddedOn,
                    AddedBy = user.AddedBy,
                    EditedOn = user.EditedOn,
                    EditedBy = user.EditedBy
                });
            }
            return usersResponse;
        }

        [HttpPost]
        public async Task<ActionResult> Post(UserWithRoleRequestDTO user)
        {
            User currentUser = AuthHelper.AuthenticateAndAuthorizeUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            User userToAdd = new User
            {
                Username = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                AddedBy = currentUser.Id
            };

            var resultState = await _userService.CreateUser(userToAdd);

            if (resultState.IsSuccessful)
            {

                return Ok(resultState.Message);
            }
            else
            {
                return BadRequest(resultState.Message);
            }

        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<ActionResult> Edit(int userId, UserRequestDTO user)
        {
            User currentUser = AuthHelper.AuthenticateAndAuthorizeUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            User userToEdit = new User
            {
                Username = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EditedBy = currentUser.Id
            };

            var resultState = await _userService.EditUser(userId, userToEdit, currentUser.Id);

            if (resultState.IsSuccessful)
            {

                return Ok(resultState.Message);
            }
            else
            {
                return BadRequest(resultState.Message);
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult> Delete(int userId)
        {
            User currentUser = AuthHelper.AuthenticateAndAuthorizeUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var resultState = await _userService.DeleteUser(userId, currentUser.Id);

            if (resultState.IsSuccessful)
            {

                return Ok(resultState.Message);
            }
            else
            {
                return BadRequest(resultState.Message);
            }
        }
    }
}
