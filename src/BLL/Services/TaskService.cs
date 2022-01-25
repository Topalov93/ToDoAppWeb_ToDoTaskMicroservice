using Common;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ToDoTasksRepository _toDoTaskRepository;

        public TaskService(ToDoTasksRepository toDoTaskRepository)
        {
            _toDoTaskRepository = toDoTaskRepository;
        }

        public async Task<ResultState> CreateTask(ToDoTask newToDoTask, int currentUserId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetToDoTaskByTitle(newToDoTask.Title, newToDoTask.ToDoListId);

            if (toDoTask is not null)
            {
                return new ResultState(false, Messages.ToDoTaskAlreadyExist);
            }

            newToDoTask.IsCompleted = false;
            newToDoTask.UserId = currentUserId;

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

        public async Task<ResultState> EditTask(int taskId, ToDoTask newInfoHolderToDoTask, int currentUserId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetToDoTaskById(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            newInfoHolderToDoTask.EditedBy = currentUserId;
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

        public async Task<ResultState> AssignTask(int taskId, ToDoList toDoList, int userId, int currentUserId)
        {
            ToDoTask toDoTask = await _toDoTaskRepository.GetToDoTaskById(taskId);

            if (toDoTask is null)
            {
                return new ResultState(false, Messages.ToDoTaskDoesntExist);
            }

            List<int> usersSharedToToDoList = await _toDoTaskRepository.UsersSharedToToDoList(toDoList.Id);

            if (userId == toDoList.UserId || usersSharedToToDoList.Contains(userId))
            {
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

            return new ResultState(false, Messages.UserDontHaveAccessOrDoestExist);
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

        public async Task<List<ToDoTask>> ListToDoTasks(int toDoListId)
        {
            return await _toDoTaskRepository.GetToDoTasks(toDoListId);
        }
    }
}
