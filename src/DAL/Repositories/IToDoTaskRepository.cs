using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace DAL.Repositories
{
    public interface IToDoTaskRepository
    {
        public Task CreateToDoTask(ToDoTask newToDoTask);

        public Task EditToDoTask(int taskId, ToDoTask newInfoHolderToDoTask);

        public Task DeleteToDoTask(int taskId);

        public Task AssignToDoTask(int taskId, int userId);

        public Task CompleteToDoTask(int taskId);

        public Task<ToDoTask> GetToDoTaskById(int taskId);

        public Task<ToDoTask> GetToDoTaskByTitle(string title);

        public Task<List<ToDoTask>> GetToDoTaskByUserId(int userId);

        public void EditToDoTaskUserInfo(List<ToDoTask> tasksToEdit, User newInfoHolderUser);
    }
}
