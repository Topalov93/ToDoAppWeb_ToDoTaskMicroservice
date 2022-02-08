using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace DAL.Data
{
    public class ToDoTasksRepository : IToDoTaskRepository
    {
        private ToDoAppDbContext _context;

        public ToDoTasksRepository(ToDoAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateToDoTask(ToDoTask newToDoTask)
        {
            await _context.ToDoTasks.Add(newToDoTask).ReloadAsync();
            _context.SaveChanges();
        }

        public async Task EditToDoTask(int taskId, ToDoTask newInfoHolderToDoTask)
        {
            ToDoTask toDoTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            toDoTask.Title = newInfoHolderToDoTask.Title;
            toDoTask.Description = newInfoHolderToDoTask.Description;
            toDoTask.IsCompleted = newInfoHolderToDoTask.IsCompleted;
            toDoTask.EditedBy = newInfoHolderToDoTask.EditedBy;
            toDoTask.EditedOn = newInfoHolderToDoTask.EditedOn;

            _context.SaveChanges();
        }

        public async Task DeleteToDoTask(int taskId)
        {
            var taskToDelete = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            _context.ToDoTasks.Remove(taskToDelete);

            _context.SaveChanges();
        }

        public async Task AssignToDoTask(int taskId, int userId)
        {
            var taskToAssign = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            taskToAssign.UserId = userId;

            _context.SaveChanges();
        }

        public async Task CompleteToDoTask(int taskId)
        {
            ToDoTask toDoTaskTocomplete = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            toDoTaskTocomplete.IsCompleted = true;

            _context.SaveChanges();
        }

        public async Task<ToDoTask> GetToDoTaskById(int taskId)
        {
            return await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ToDoTask> GetToDoTaskByTitle(string title)
        {
            return await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Title == title);
        }

        public async Task<List<ToDoTask>> GetToDoTaskByUserId(int userId)
        {
            return await _context.ToDoTasks.Where(x => x.UserId == userId).ToListAsync();
        }

        public void EditToDoTaskUserInfo(List<ToDoTask> tasksToEdit, User newInfoHolderUser)
        {
            foreach (var toDoTask in tasksToEdit)
            {
                toDoTask.UserId = newInfoHolderUser.Id;
                toDoTask.UserUsername = newInfoHolderUser.Username;
                toDoTask.UserFirstname = newInfoHolderUser.FirstName;
                toDoTask.UserLastname = newInfoHolderUser.LastName;
            }

            _context.SaveChanges();
        }
    }
}
