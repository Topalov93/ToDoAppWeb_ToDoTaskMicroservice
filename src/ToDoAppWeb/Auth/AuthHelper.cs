using Common.Constants;
using Common.Enums;
using Common.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using ToDoApp.Models.Users;
using ToDoApp.Services.UserService;

namespace ToDoAppWeb.Auth
{

    public static class AuthHelper
    {
        public async static Task<User> GetUserFromDbByCredentials(this UserService userService, HttpRequest request)
        {
            request.Headers.TryGetValue("Username", out StringValues username);
            request.Headers.TryGetValue("Password", out StringValues password);

            if (!StringValues.IsNullOrEmpty(username) && !StringValues.IsNullOrEmpty(password))
            {
                return await userService.GetUserByNameAndPassword(username, password);
            }

            return null;
        }

        public static User GetCurrentUserFromLogById(HttpRequest request)
        {
            request.Headers.TryGetValue("Authenticate_UserId", out StringValues userId);

            if (!StringValues.IsNullOrEmpty(userId))
            {
                User currentUser = JsonFileStorage.Read<User>(Constants.CurrentUserLogFileName);

                if (currentUser.Id == int.Parse(userId))
                {
                    return currentUser;
                }

                return null;
            }

            return null;
        }

        public static User GetCurrentUserFromLogByCredentials(HttpRequest request)
        {
            request.Headers.TryGetValue("Username", out StringValues username);
            request.Headers.TryGetValue("Password", out StringValues password);

            if (!StringValues.IsNullOrEmpty(username) && !StringValues.IsNullOrEmpty(password))
            {
                User currentUser = JsonFileStorage.Read<User>(Constants.CurrentUserLogFileName);

                if (currentUser.Username==username && currentUser.Password==password)
                {
                    return currentUser;
                }

                return null;
            }

            return null;
        }

        public static User AuthenticateUser(HttpRequest request)
        {
            return GetCurrentUserFromLogById(request);
        }

        public static User AuthenticateAndAuthorizeUser(HttpRequest request)
        {
            User currentUser = GetCurrentUserFromLogById(request);

            if (currentUser is null || currentUser.Role != UserRolesEnum.admin.ToString())
            {
                return null;
            }

            return currentUser;
        }
    }
}
