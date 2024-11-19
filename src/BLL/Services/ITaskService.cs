using Common;
using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services.TaskService
{
    public interface ITaskService
    {
        public Task<ToDoTask> GetbyId(string taskId);

        public Task<List<ToDoTask>> GetAll();

        public Task<ResultState> Create(ToDoTask newToDoTask);

        public Task<ResultState> Delete(string taskId);

        public Task<ResultState> Edit(string taskId, ToDoTask newInfoHolderToDoTask);

        public Task<ResultState> AssignTask(string taskId, int userId);

        public Task<ResultState> CompleteTask(string taskId);

        public Task<ResultState> UpdateUserInfo(List<ToDoTask> toDoTasks, User userInfo);

        public Task<List<ToDoTask>> GetBacklog();
    }
}
