using Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.Users;

namespace ToDoApp.Services.ToDoListService
{
    public interface IToDoListService
    {
        public Task<ResultState> CreateToDoList(ToDoList newToDoList, int currentUserId);

        public Task<ResultState> DeleteToDoList(int toDoListToDeleteId, int currentUserId);

        public Task<ResultState> EditToDoList(int toDoListId, ToDoList newInfoHolderToDoList, int currentUser);

        public Task<ResultState> ShareToDoList(int currentUserId, int toDoListId, int userId, List<User> users);

        public Task<List<ToDoList>> ListDoToLists();

        public Task<ToDoList> GetToDoListById(int listId);

        public Task<List<int>> UsersSharedToToDoList(int listId);


    }
}
