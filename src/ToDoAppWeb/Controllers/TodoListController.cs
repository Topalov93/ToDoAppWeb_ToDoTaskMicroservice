using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.DTO.Requests;
using ToDoApp.Models.DTO.Responses;
using ToDoApp.Models.Users;
using ToDoApp.Services.ToDoListService;
using ToDoApp.Services.UserService;
using ToDoAppWeb.Auth;

namespace ToDoAppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        public ToDoListService _todoListService;

        public UserService _userService;

        public TodoListController() : base()
        {
            _todoListService = new ToDoListService(new ToDoListsRepository());
            _userService = new UserService(new UsersRepository());
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoListResponseDTO>>> GetAll()
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var toDoLists = await _todoListService.ListDoToLists();

            List<TodoListResponseDTO> toDoListsResponse = new List<TodoListResponseDTO>();

            foreach (var toDoList in toDoLists)
            {
                toDoListsResponse.Add(new TodoListResponseDTO()
                {
                    Id = toDoList.Id,
                    Title = toDoList.Title,
                    AddedOn = toDoList.AddedOn,
                    AddedBy = toDoList.UserId,
                    EditedOn = toDoList.EditedOn,
                    EditedBy = toDoList.EditedBy
                });
            }
            return toDoListsResponse;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ToDoListRequestDTO toDoList)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            ToDoList todoListToAdd = new ToDoList
            {
                Title = toDoList.Title,
                UserId = currentUser.Id
            };

            var resultState = await _todoListService.CreateToDoList(todoListToAdd, currentUser.Id);

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
        [Route("{toDoListId}")]
        public async Task<ActionResult> Edit(int toDoListId, ToDoListRequestDTO toDoList)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            ToDoList todoListToEdit = new ToDoList
            {
                Title = toDoList.Title,
                EditedBy = currentUser.Id
            };

            var resultState = await _todoListService.EditToDoList(toDoListId, todoListToEdit, currentUser.Id);

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
        [Route("{toDoListId}")]
        public async Task<ActionResult> Delete(int toDolistId)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var resultState = await _todoListService.DeleteToDoList(toDolistId, currentUser.Id);

            if (resultState.IsSuccessful)
            {

                return Ok(resultState.Message);
            }
            else
            {
                return BadRequest(resultState.Message);
            }
        }

        [HttpPost]
        [Route("{toDolistId}/share/user/{userId}")]
        public async Task<ActionResult> Share(int toDolistId, int userId)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var resultState = await _todoListService.ShareToDoList(currentUser.Id, toDolistId, userId, await _userService.ListUsers());

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
