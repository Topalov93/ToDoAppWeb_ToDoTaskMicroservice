using Common.Constants;

namespace DAL.Data
{
    public static class DbInitializer
    {
        public static void InitializeDatabase()
        {
            ToDoAppDbContext _dbContext = new ToDoAppDbContext();

            if (_dbContext.Database.EnsureCreated()) { }
         }
    }
}
