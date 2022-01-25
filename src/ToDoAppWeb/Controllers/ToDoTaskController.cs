using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.DTO.Requests;
using ToDoApp.Models.DTO.Responses;
using ToDoApp.Models.Users;
using ToDoApp.Services.TaskService;
using ToDoApp.Services.ToDoListService;
using ToDoApp.Services.UserService;
using ToDoAppWeb.Auth;

namespace ToDoAppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        public TaskService _toDoTaskService;

        public UserService _userService;

        public ToDoListService _toDoListService;

        public ToDoTaskController() : base()
        {
            _toDoTaskService = new TaskService(new ToDoTasksRepository());
            _userService = new UserService(new UsersRepository());
            _toDoListService = new ToDoListService(new ToDoListsRepository());
        }

        [HttpGet]
        [Route("{toDoListId}")]
        public async Task<ActionResult<List<ToDoTaskResponseDTO>>> GetAll(int toDoListId)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var toDoTasks = await _toDoTaskService.ListToDoTasks(toDoListId);

            List<ToDoTaskResponseDTO> toDoTaskResponse = new List<ToDoTaskResponseDTO>();

            foreach (var toDoTask in toDoTasks)
            {
                toDoTaskResponse.Add(new ToDoTaskResponseDTO()
                {
                    Id = toDoTask.Id,
                    ToDoListId = toDoTask.ToDoListId,
                    Title = toDoTask.Title,
                    Description = toDoTask.Description,
                    IsCompleted = toDoTask.IsCompleted,
                    AddedOn = toDoTask.AddedOn,
                    UserId = toDoTask.UserId,
                    EditedOn = toDoTask.EditedOn,
                    EditedBy = toDoTask.EditedBy
                });
            }

            return toDoTaskResponse;
        }

        [HttpPut]
        [Route("{toDoTaskId}/complete")]
        public async Task<ActionResult<ToDoTaskResponseDTO>> Complete(int toDoTaskId)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var resultState = await _toDoTaskService.CompleteTask(toDoTaskId);

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
        public async Task<ActionResult<ToDoTaskResponseDTO>> Post(ToDoTaskCreateRequestDTO toDoTask)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            ToDoTask toDoTaskToAdd = new ToDoTask
            {
                ToDoListId = toDoTask.ToDoListId,
                Title = toDoTask.Title,
                Description = toDoTask.Description
            };

            var resultState = await _toDoTaskService.CreateTask(toDoTaskToAdd, currentUser.Id);

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
        [Route("{toDoTaskId}")]
        public async Task<ActionResult> Delete(int toDoTaskId)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var resultState = await _toDoTaskService.DeleteTask(toDoTaskId);

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
        [Route("{toDoTaskId}")]
        public async Task<ActionResult> Edit(int toDoTaskId, ToDoTaskEditRequestDTO toDoTask)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            ToDoTask todoTaskToEdit = new ToDoTask
            {
                Title = toDoTask.Title,
                Description = toDoTask.Description,
                IsCompleted = toDoTask.IsCompleted,
                EditedBy = currentUser.Id
            };

            var resultState = await _toDoTaskService.EditTask(toDoTaskId, todoTaskToEdit, currentUser.Id);

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
        [Route("{toDoTaskId}/assign/user/{userId}")]
        public async Task<ActionResult<ToDoTaskResponseDTO>> AssignTask(int toDoTaskId, int userId)
        {
            User currentUser = AuthHelper.AuthenticateUser(Request);

            if (currentUser is null)
            {
                return Unauthorized();
            }

            var toDoList = await _toDoListService.GetToDoListByToDoTaskId(toDoTaskId);

            var resultState = await _toDoTaskService.AssignTask(toDoTaskId, toDoList, userId, currentUser.Id);

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
