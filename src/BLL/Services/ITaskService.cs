using Common;
using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services.TaskService
{
    public interface ITaskService
    {
        public Task<ResultState> CreateTask(ToDoTask newToDoTask);

        public Task<ResultState> DeleteTask(int taskId);

        public Task<ResultState> EditTask(int taskId, ToDoTask newInfoHolderToDoTask);

        public Task<ResultState> AssignTask(int taskId, int userId);

        public Task<ResultState> CompleteTask(int taskId);

        public Task<ToDoTask> GetTaskByTitle(string title);

        public Task<ToDoTask> GetTaskById(int taskId);

        public Task<List<ToDoTask>> GetTaskByUserId(int userId);

        public  Task<ResultState> EditTaskUser(User newInfoHolderUser);
    }
}
