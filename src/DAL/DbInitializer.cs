using Common.Constants;
using Common.Enums;
using ToDoApp.Models.Users;

namespace DAL.Data
{
    public static class DbInitializer
    {
        public static void InitializeDatabase()
        {
            ToDoAppDbContext _dbContext = new ToDoAppDbContext();

            if (_dbContext.Database.EnsureCreated())
            {
                _dbContext.Users.Add(new User
                {
                    Username = Constants.InitialLoginUsername,
                    Password = Constants.InitialLoginPassword,
                    Role = UserRolesEnum.admin.ToString()
                });

                _dbContext.SaveChanges();
            }
        }
    }
}
