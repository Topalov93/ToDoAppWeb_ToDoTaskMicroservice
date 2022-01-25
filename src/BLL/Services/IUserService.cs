using Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models.Users;

namespace ToDoApp.Services.UserService
{
    public interface IUserService
    {
        public Task<User> GetInitialUser();

        public Task<ResultState> InitialLogin(User initialUser);

        public Task<ResultState> Login(User user);

        public void Logout();

        public Task<ResultState> CreateUser(User newUserInfoHolder);

        public Task<ResultState> EditUser(int userToEditId, User newInfoHolderUser, int currentUserId);

        public Task<ResultState> DeleteUser(int userToDeleteId, int currentUserId);

        public Task<List<User>> ListUsers();


    }
}
