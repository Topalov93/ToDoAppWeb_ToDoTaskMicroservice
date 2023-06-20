using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace DAL.Repositories
{
    public class TaskServiceRepository : ITasksServiceRepository
    {
        private readonly IMongoCollection<ToDoTask> _toDoTasks;

        public TaskServiceRepository(
            IOptions<TaskServiceDatabaseSettings> taskServiceDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                taskServiceDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                taskServiceDatabaseSettings.Value.DatabaseName);

            _toDoTasks = mongoDatabase.GetCollection<ToDoTask>(
                taskServiceDatabaseSettings.Value.TasksCollectionName);
        }

        public async Task<List<ToDoTask>> GetAsync() =>
            await _toDoTasks.Find(_ => true).ToListAsync();

        public async Task<ToDoTask?> GetAsync(string id) =>
            await _toDoTasks.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ToDoTask newBook) =>
            await _toDoTasks.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, ToDoTask updatedBook) =>
            await _toDoTasks.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _toDoTasks.DeleteOneAsync(x => x.Id == id);
    }
}
