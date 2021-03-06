using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.DTO.Requests;
using ToDoApp.Models.DTO.Responses;
using ToDoApp.Services.TaskService;

namespace ToDoAppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        public ITaskService _toDoTaskService;

        public ToDoTaskController(ITaskService taskService) : base()
        {
            _toDoTaskService = taskService;
        }

        [HttpGet]
        [Route("{toDoTaskId}")]
        public async Task<ActionResult<ToDoTaskResponseDTO>> GetTaskById(int toDoTaskId)
        {
            var toDoTask = await _toDoTaskService.GetTaskById(toDoTaskId);

            if (toDoTask is null)
            {
                return BadRequest();
            }

            var toDoTaskResponse = new ToDoTaskResponseDTO()
            {
                Id = toDoTask.Id,
                Title = toDoTask.Title,
                Description = toDoTask.Description,
                IsCompleted = toDoTask.IsCompleted,
                AddedOn = toDoTask.AddedOn,
                UserId = toDoTask.UserId,
                UserUsername = toDoTask.UserUsername,
                UserFirstname = toDoTask.UserFirstname,
                UserLastname = toDoTask.UserLastname,
                EditedOn = toDoTask.EditedOn,
                EditedBy = toDoTask.EditedBy
            };

            return toDoTaskResponse;
        }

        [HttpPut]
        [Route("{toDoTaskId}/complete")]
        public async Task<ActionResult<ToDoTaskResponseDTO>> Complete(int toDoTaskId)
        {
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
            ToDoTask toDoTaskToAdd = new ToDoTask
            {
                Title = toDoTask.Title,
                Description = toDoTask.Description,
                UserId = toDoTask.UserId,
                UserUsername = toDoTask.UserUsername,
                UserFirstname = toDoTask.UserFirstname,
                UserLastname = toDoTask.UserLastname
            };

            var resultState = await _toDoTaskService.CreateTask(toDoTaskToAdd);

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
            ToDoTask todoTaskToEdit = new ToDoTask
            {
                Title = toDoTask.Title,
                Description = toDoTask.Description,
                IsCompleted = toDoTask.IsCompleted,
            };

            var resultState = await _toDoTaskService.EditTask(toDoTaskId, todoTaskToEdit);

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
            var resultState = await _toDoTaskService.AssignTask(toDoTaskId, userId);

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
