using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace DAL.Data
{
    public class ToDoListsRepository : ToDoAppDbContext
    {
        public async Task CreateToDoList(ToDoList newToDoList)
        {
            await ToDoLists.Add(newToDoList).ReloadAsync();
            SaveChanges();
        }

        public async Task DeleteToDoList(int toDoListId)
        {
            var taskInToDoList = await ToDoTasks.Where(l => l.ToDoListId == toDoListId).ToListAsync();

            foreach (var toDoTask in taskInToDoList)
            {
                foreach (var item in UsersToDoTasks.Where(i => i.ToDoTaskId == toDoTask.Id))
                {
                    UsersToDoTasks.Remove(item);
                }
            }

            foreach (var toDoTask in taskInToDoList)
            {
                ToDoTasks.Remove(toDoTask);
            }

            var listsUsersToRemove = await UsersToDoLists.Where(t => t.ToDoListId == toDoListId).ToListAsync();

            foreach (var item in listsUsersToRemove)
            {
                UsersToDoLists.Remove(item);
            }

            var listToDelete = await ToDoLists.FirstOrDefaultAsync(l => l.Id == toDoListId);

            ToDoLists.Remove(listToDelete);

            SaveChanges();
        }

        public async Task ShareToDoList(int toDoListId, int userId)
        {
            await UsersToDoLists.Add(new Models.UserToDoList { ToDoListId = toDoListId, UserId = userId }).ReloadAsync();
            SaveChanges();
        }

        public async Task RemoveSharing(int toDoListId, int userId)
        {
            UsersToDoLists.Remove(await UsersToDoLists.FirstOrDefaultAsync(ul => ul.ToDoListId == toDoListId && ul.UserId == userId));
            SaveChanges();
        }

        public async Task EditToDoList(int toDoListId, ToDoList newInfoHolderToDoList)
        {
            ToDoList toDoList = await ToDoLists.FirstOrDefaultAsync(l => l.Id == toDoListId);

            toDoList.Title = newInfoHolderToDoList.Title;
            toDoList.EditedBy = newInfoHolderToDoList.EditedBy;
            toDoList.EditedOn = newInfoHolderToDoList.EditedOn;

            SaveChanges();
        }

        public Task<List<ToDoList>> GetToDoLists()
        {
            return ToDoLists.ToListAsync();
        }

        public Task<ToDoList> GetToDoListById(int toDoListId)
        {
            return ToDoLists.FirstOrDefaultAsync(l => l.Id == toDoListId);
        }

        public Task<ToDoList> GetToDoListByTitle(string title)
        {
            return ToDoLists.FirstOrDefaultAsync(l => l.Title == title);
        }

        public Task<List<int>> GetUsersSharedToToDoList(int toDoListId)
        {
            return UsersToDoLists.Where(ul => ul.ToDoListId == toDoListId).Select(ul => ul.UserId).ToListAsync();
        }

        public Task<ToDoList> GetToDoListByToDoTaskId(int toDoTaskId)
        {
            return ToDoLists.FirstOrDefaultAsync(l => l.ToDoTasks.Select(t => t.Id).FirstOrDefault() == toDoTaskId);
        }
    }
}