using Common;
using Common.Constants;
using Common.Enums;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models.Users;

namespace ToDoApp.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UsersRepository _usersRepository;

        public User CurrentUser { get; set; }

        public UserService(UsersRepository userRepository)
        {
            _usersRepository = userRepository;
        }

        public async Task<User> GetInitialUser()
        {
            return await _usersRepository.GetUserByName(Constants.InitialLoginUsername);
        }

        public async Task<ResultState> InitialLogin(User initialUser)
        {
            while (initialUser.Username != Constants.InitialLoginUsername || initialUser.Password != Constants.InitialLoginPassword)
            {
                return new ResultState(false, Messages.WrongInitialCredentials);
            }

            initialUser.Role = UserRolesEnum.admin.ToString();

            try
            {
                await _usersRepository.CreateUser(initialUser);
                CurrentUser = initialUser;
                return new ResultState(true, Messages.SuccessfulLogin);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToCreateInitialUser, ex);
            }
        }

        public async Task<ResultState> Login(User user)
        {
            var userToLogin = await _usersRepository.GetUserByNameAndPassword(user.Username, user.Password);

            if (userToLogin is not null)
            {
                CurrentUser = userToLogin;
                return new ResultState(true, Messages.SuccessfulLogin);
            }

            return new ResultState(false, Messages.WrongCredentials);
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public async Task<ResultState> CreateUser(User newUserInfoHolder)
        {
            if (await _usersRepository.GetUserByName(newUserInfoHolder.Username) != null)
            {
                return new ResultState(false, Messages.UserAlreadyExist);
            }

            try
            {
                await _usersRepository.CreateUser(newUserInfoHolder);
                return new ResultState(true, Messages.UserCreationSuccessfull);
            }
            catch (Exception)
            {
                return new ResultState(false, Messages.UnableToCreateUser);
            }
        }

        public async Task<ResultState> EditUser(int userToEditId, User newInfoHolderUser, int currentUserId)
        {
            User userToEdit = await _usersRepository.GetUserById(userToEditId);

            if (userToEdit is null)
            {
                return new ResultState(false, Messages.UserDoesntExist);
            }

            if (userToEdit.Id == 1)
            {
                return new ResultState(false, Messages.UserInitialEditingUnsuccessful);
            }

            if (userToEdit.Id == currentUserId)
            {
                return new ResultState(false, Messages.UserCurrentEditingUnsuccessful);
            }

            newInfoHolderUser.EditedOn = DateTime.UtcNow;

            try
            {
                await _usersRepository.EditUserBy(userToEditId, newInfoHolderUser);
                return new ResultState(true, Messages.UserEditSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.UnableToEditUser, ex);
            }
        }

        public async Task<ResultState> DeleteUser(int userToDeleteId, int currentUserId)
        {
            User userToDelete = await _usersRepository.GetUserById(userToDeleteId);

            if (userToDelete is null)
            {
                return new ResultState(false, Messages.UserDoesntExist);
            }

            if (userToDelete.Id == 1)
            {
                return new ResultState(false, Messages.UserInitialAdminDeletingUnsuccessful);
            }

            if (userToDelete.Id == currentUserId)
            {
                return new ResultState(false, Messages.UserCurrentDeletingUnsuccessful);
            }

            try
            {
                await _usersRepository.DeleteUserBy(userToDeleteId);
                return new ResultState(true, Messages.UserDeletedSuccessfull);
            }
            catch (Exception)
            {
                return new ResultState(false, Messages.UnableToDeleteUser);
            }
        }

        public async Task<List<User>> ListUsers()
        {
            return await _usersRepository.GetUsers();
        }

        public async Task<User> GetUserByNameAndPassword(string username, string password)
        {
            return await _usersRepository.GetUserByNameAndPassword(username, password);
        }
    }
}
