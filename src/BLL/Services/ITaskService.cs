using Common;
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

        //public Task<ResultState> AssignTask(int taskId, ToDoList toDoList, int userId, int currentUserId);

        public Task<ResultState> CompleteTask(int taskId);

        //public Task<List<ToDoTask>> ListToDoTasks();

        public Task<ToDoTask> GetTaskByTitle(string title);

        public Task<ToDoTask> GetTaskById(int taskId);

    }
}
