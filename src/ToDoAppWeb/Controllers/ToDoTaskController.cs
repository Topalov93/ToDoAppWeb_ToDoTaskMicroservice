using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.DTO.Requests;
using ToDoApp.Models.DTO.Responses;
using ToDoApp.Services.TaskService;
using ToDoAppWeb.KafkaProducer;

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
        public async Task<ActionResult<ToDoTaskResponseDTO>> GetTaskById(string toDoTaskId)
        {
            var toDoTask = await _toDoTaskService.GetTask(toDoTaskId);

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
                EditedOn = toDoTask.EditedOn,
                //EditedBy = toDoTask.EditedBy
            };

            return toDoTaskResponse;
        }

        [HttpGet]
        [Route("backlog")]
        public async Task<ActionResult<List<ToDoTaskBacklogResponseDTO>>> GetTasksBacklog()
        {
            var tasksBacklog = await _toDoTaskService.GetTasksBacklog();

            var response = new List<ToDoTaskBacklogResponseDTO>();

            foreach (var todotask in tasksBacklog)
            {
                response.Add(new ToDoTaskBacklogResponseDTO()
                {
                    Id = todotask.Id,
                    Title = todotask.Title,
                    IsCompleted = todotask.IsCompleted
                });
            }

            return response;
        }

        [HttpPut]
        [Route("{toDoTaskId}/complete")]
        public async Task<ActionResult<ToDoTaskResponseDTO>> Complete(string toDoTaskId)
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
                AddedBy = new DAL.Models.User() { Id = "1" }
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
        public async Task<ActionResult> Delete(string toDoTaskId)
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
        public async Task<ActionResult> Edit(string toDoTaskId, ToDoTaskEditRequestDTO toDoTask)
        {
            ToDoTask todoTaskToEdit = new ToDoTask
            {
                Id = toDoTaskId,
                Title = toDoTask.Title,
                Description = toDoTask.Description,
                IsCompleted = toDoTask.IsCompleted,
            };

            var resultState = await _toDoTaskService.EditTask(toDoTaskId, todoTaskToEdit);

            if (resultState.IsSuccessful)
            {
                try
                {
                    var producer = new EventProducer();
                    await producer.Produce(toDoTaskId, todoTaskToEdit);
                }
                catch (System.Exception)
                {
                    throw;
                }
                return Ok(resultState.Message);
            }
            else
            {
                return BadRequest(resultState.Message);
            }
        }

        [HttpPut]
        [Route("{toDoTaskId}/assign/user/{userId}")]
        public async Task<ActionResult<ToDoTaskResponseDTO>> AssignTask(string toDoTaskId, int userId)
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
