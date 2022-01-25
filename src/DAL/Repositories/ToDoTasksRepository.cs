using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace DAL.Data
{
    public class ToDoTasksRepository : ToDoAppDbContext
    {
        public async Task CreateToDoTask(ToDoTask newToDoTask)
        {
            await ToDoTasks.Add(newToDoTask).ReloadAsync();
            SaveChanges();
        }

        public async Task EditToDoTask(int taskId, ToDoTask newInfoHolderToDoTask)
        {
            ToDoTask toDoTask = await ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            toDoTask.Title = newInfoHolderToDoTask.Title;
            toDoTask.Description = newInfoHolderToDoTask.Description;
            toDoTask.IsCompleted = newInfoHolderToDoTask.IsCompleted;
            toDoTask.EditedBy = newInfoHolderToDoTask.EditedBy;
            toDoTask.EditedOn = newInfoHolderToDoTask.EditedOn;

            SaveChanges();
        }

        public async Task DeleteToDoTask(int taskId)
        {
            var tasksAssignedToRemove = await UsersToDoTasks.Where(ut => ut.ToDoTaskId == taskId).ToListAsync();

            foreach (var item in tasksAssignedToRemove)
            {
                UsersToDoTasks.Remove(item);
            }

            var taskToDelete = await ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            ToDoTasks.Remove(taskToDelete);

            SaveChanges();
        }

        public async Task AssignToDoTask(int taskId, int userId)
        {
            await UsersToDoTasks.Add(new UserToDoTask { ToDoTaskId = taskId, UserId = userId }).ReloadAsync();

            SaveChanges();
        }

        public async Task CompleteToDoTask(int taskId)
        {
            ToDoTask toDoTask = await ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            toDoTask.IsCompleted = true;

            SaveChanges();
        }

        public Task<List<ToDoTask>> GetToDoTasks(int toDoListId)
        {
            return ToDoTasks.Where(t => t.ToDoListId == toDoListId).ToListAsync();
        }

        public Task<ToDoTask> GetToDoTaskById(int taskId)
        {
            return ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public Task<ToDoTask> GetToDoTaskByTitle(string title, int toDoListId)
        {
            return ToDoTasks.FirstOrDefaultAsync(t => t.Title == title && t.ToDoListId == toDoListId);
        }

        public Task<List<int>> UsersSharedToToDoList(int toDoListId)
        {
            return UsersToDoLists.Where(ul => ul.ToDoListId == toDoListId).Select(ul => ul.UserId).ToListAsync();
        }
    }
}
