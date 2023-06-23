using Common;
using DAL.Repositories;
using System;
using System.Collections.Generic;
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
        public async Task<ToDoTask> GetTasksByUserId(string userId)
        {
            return await _toDoTaskRepository.GetAsync(userId);
        }

    }
}
