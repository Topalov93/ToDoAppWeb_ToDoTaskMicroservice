using Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services.TaskService
{
    public interface ITaskService
    {
        public Task<ToDoTask> GetTask(string taskId);

        public Task<List<ToDoTask>> GetTasks();

        public Task<ResultState> CreateTask(ToDoTask newToDoTask);

        public Task<ResultState> DeleteTask(string taskId);

        public Task<ResultState> EditTask(string taskId, ToDoTask newInfoHolderToDoTask);

        public Task<ResultState> AssignTask(string taskId, int userId);

        public Task<ResultState> CompleteTask(string taskId);
    }
}
