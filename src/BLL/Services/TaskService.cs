using Common;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ITasksServiceRepository _toDoTaskRepository;

        public TaskService(ITasksServiceRepository toDoTasksRepository)
        {
            _toDoTaskRepository = toDoTasksRepository;
        }

        public async Task<ToDoTask> GetTask(string taskId)
        {
            return await _toDoTaskRepository.GetAsync(taskId);
        }

        public async Task<List<ToDoTask>> GetTasks()
        {
            return await _toDoTaskRepository.GetAsync();
        }

        public async Task<ResultState> CreateTask(ToDoTask newToDoTask)
        {
            newToDoTask.IsCompleted = false;
            newToDoTask.AssignedTo = newToDoTask.AddedBy;

            try
            {
                await _toDoTaskRepository.CreateAsync(newToDoTask);
                return new ResultState(true, Messages.ToDoTaskCreationSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToCreateToDoTask, ex);
            }
        }

        public async Task<ResultState> DeleteTask(string taskId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetAsync(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            try
            {
                await _toDoTaskRepository.RemoveAsync(taskId);
                return new ResultState(true, Messages.ToDoTaskDeletedSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToDeleteToDoTask, ex);
            }
        }

        public async Task<ResultState> EditTask(string taskId, ToDoTask newInfoHolderToDoTask)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetAsync(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            newInfoHolderToDoTask.EditedOn = DateTime.UtcNow;

            try
            {
                await _toDoTaskRepository.UpdateAsync(taskId, newInfoHolderToDoTask);
                return new ResultState(true, Messages.ToDoTaskEditSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToEditToDoTask, ex);
            }
        }

        public async Task<ResultState> AssignTask(string taskId, int userId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetAsync(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            try
            {
                //await _toDoTaskRepository.AssignToDoTask(taskId, userId);
                return new ResultState(true, Messages.ToDoTaskAssignedSuccessful);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToAssignToDoTask, ex);
            }
        }

        public async Task<ResultState> CompleteTask(string taskId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetAsync(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            if (toDoTask.IsCompleted)
            {
                return new ResultState(false, Messages.ToDoTaskAlreadyCompleted);
            }

            try
            {
                //await _toDoTaskRepository.CompleteToDoTask(taskId);
                return new ResultState(true, Messages.ToDoTaskCompletedSuccessful);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToCompleteToDoTask, ex);
            }
        }

        public async Task<List<ToDoTask>> GetTasksByUserId(string userId)
        {
            return await _toDoTaskRepository.GetbyUserIdAsync(userId);
        }

        public async Task<ResultState> UpdateUserInfo(List<ToDoTask> toDoTasks, User userInfo)
        {
            foreach (var toDoTask in toDoTasks)
            {
                toDoTask.AssignedTo = userInfo;

                try
                {
                    await _toDoTaskRepository.UpdateAsync(toDoTask.Id, toDoTask);
                }
                catch (Exception ex)
                {
                    return new ResultState(false, Messages.UnableToEditToDoTask, ex);
                }
            }

            return new ResultState(true, Messages.ToDoTaskEditSuccessfull);
        }

        public async Task<List<ToDoTask>> GetTasksBacklog()
        {
            return await _toDoTaskRepository.GetTasksBacklogAsync();
        }

        public async Task<ResultState> AddTaskToList(string toDoTaskId, string toDoListId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetAsync(toDoListId);

            if (toDoTask is null)
            {
                return new ResultState(false, "ToDoTask doesn't exist");
            }

            var lists = await GetToDoLists();
            if (lists is null) return new ResultState(false, "Unsuccessful fetching of ToDo Lists");

            var todoList = lists.Find(x => x.Id == toDoTaskId);
            if (todoList is null) return new ResultState(false, "Unsuccessful list pick");

            try
            {
                //await _toDoListRepository.UpdateAsync(toDoListId, toDoTask);
                return new ResultState(true, "Successful");
            }
            catch (Exception ex)
            {
                return new ResultState(false, "Unable to add task to ToDoList", ex);
            }
        }

        public async Task<List<ToDoList>> GetToDoLists()
        {
            using var client = new HttpClient();
            List<ToDoList> toDoLists = new List<ToDoList>();
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                response = await client.GetAsync("http://localhost:5002/api/ToDoList");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (response.IsSuccessStatusCode)
            {
                var toDoListsAsString = await response.Content.ReadAsStringAsync();
                toDoLists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ToDoList>>(toDoListsAsString);
            }
            else
            {
                await Console.Out.WriteLineAsync(response.Content.ToString());
            }

            return toDoLists;
        }
    }
}
