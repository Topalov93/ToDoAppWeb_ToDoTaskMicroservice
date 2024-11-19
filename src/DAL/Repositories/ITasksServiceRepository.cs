using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace DAL.Repositories
{
    public interface ITasksServiceRepository
    {
        public Task<List<ToDoTask>> GetAsync();

        public Task<ToDoTask?> GetAsync(string id);

        public Task CreateAsync(ToDoTask newBook);

        public Task UpdateAsync(string id, ToDoTask updatedBook);

        public Task RemoveAsync(string id);

        public Task<List<ToDoTask>> GetbyUserIdAsync(string userId);

        public Task<List<ToDoTask>> GetBacklogAsync();
    }
}