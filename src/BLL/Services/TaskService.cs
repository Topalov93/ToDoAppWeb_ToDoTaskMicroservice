using Common;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly IToDoTaskRepository _toDoTaskRepository;

        public TaskService(IToDoTaskRepository toDoTasksRepository)
        {
            _toDoTaskRepository = toDoTasksRepository;
        }

        public async Task<ResultState> CreateTask(ToDoTask newToDoTask)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetToDoTaskByTitle(newToDoTask.Title);

            if (toDoTask is not null)
            {
                return new ResultState(false, Messages.ToDoTaskAlreadyExist);
            }

            newToDoTask.IsCompleted = false;

            try
            {
                await _toDoTaskRepository.CreateToDoTask(newToDoTask);
                return new ResultState(true, Messages.ToDoTaskCreationSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToCreateToDoTask, ex);
            }
        }

        public async Task<ResultState> DeleteTask(int taskId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetToDoTaskById(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            try
            {
                await _toDoTaskRepository.DeleteToDoTask(taskId);
                return new ResultState(true, Messages.ToDoTaskDeletedSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToDeleteToDoTask, ex);
            }
        }

        public async Task<ResultState> EditTask(int taskId, ToDoTask newInfoHolderToDoTask)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetToDoTaskById(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            newInfoHolderToDoTask.EditedOn = DateTime.UtcNow;

            try
            {
                await _toDoTaskRepository.EditToDoTask(taskId, newInfoHolderToDoTask);
                return new ResultState(true, Messages.ToDoTaskEditSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToEditToDoTask, ex);
            }
        }

        public async Task<ResultState> AssignTask(int taskId, int userId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetToDoTaskById(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            try
            {
                await _toDoTaskRepository.AssignToDoTask(taskId, userId);
                return new ResultState(true, Messages.ToDoTaskAssignedSuccessful);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToAssignToDoTask, ex);
            }
        }

        public async Task<ResultState> CompleteTask(int taskId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetToDoTaskById(taskId);

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
                await _toDoTaskRepository.CompleteToDoTask(taskId);
                return new ResultState(true, Messages.ToDoTaskCompletedSuccessful);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToCompleteToDoTask, ex);
            }
        }

        public async Task<ToDoTask> GetTaskByTitle(string title)
        {
            return await _toDoTaskRepository.GetToDoTaskByTitle(title);
        }

        public async Task<ToDoTask> GetTaskById(int taskId)
        {
            return await _toDoTaskRepository.GetToDoTaskById(taskId);
        }

        public async Task<List<ToDoTask>> GetTaskByUserId(int userId)
        {
            return await _toDoTaskRepository.GetToDoTaskByUserId(userId);
        }

        public async Task<ResultState> EditTaskUser(User newInfoHolderUser)
        {
            List<ToDoTask> toDoTasksToEdit = await _toDoTaskRepository.GetToDoTaskByUserId(newInfoHolderUser.Id);

            if (toDoTasksToEdit.Count==0)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            try
            {
                _toDoTaskRepository.EditToDoTaskUserInfo(toDoTasksToEdit, newInfoHolderUser);
                return new ResultState(true, Messages.ToDoTaskEditSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToEditToDoTask, ex);
            }
        }


    }
}
