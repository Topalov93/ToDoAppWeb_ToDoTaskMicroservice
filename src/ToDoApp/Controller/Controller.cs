using Common;
using Common.Enums;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.Users;
using ToDoApp.Services.TaskService;
using ToDoApp.Services.ToDoListService;
using ToDoApp.Services.UserService;
using ToDoApp.UI;

namespace ToDoApp.Controller
{
    public class Controller
    {
        private UserInterface _userInterface;

        private UserService _userService;

        private ToDoListService _toDoListService;

        private TaskService _taskService;

        internal async Task Run()
        {
            InitializeApplication();

            await InitialLogin();

            await MainMenu();
        }

        private void InitializeApplication()
        {
            DbInitializer.InitializeDatabase();

            UsersRepository usersRepository = new UsersRepository();
            ToDoListsRepository toDoListsRepository = new ToDoListsRepository();
            ToDoTasksRepository toDoTasksRepository = new ToDoTasksRepository();

            _userService = new UserService(usersRepository);
            _toDoListService = new ToDoListService(toDoListsRepository);
            _taskService = new TaskService(toDoTasksRepository);
            _userInterface = new UserInterface();
        }

        private async Task InitialLogin()
        {
            User initialUser = await _userService.GetInitialUser();

            if (initialUser != null)
            {
                return;
            }

            Console.WriteLine(Messages.AppName);
            _userInterface.RenderInitialLoginMenu();

            bool isExit = false;

            string command;

            while (!isExit)
            {
                command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        await FirstExecutionLogin();
                        isExit = true;
                        break;
                    case "0":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine(Messages.InvalidCommad);
                        _userInterface.RenderInitialLoginMenu();
                        break;
                }
            }
        }

        private async Task MainMenu()
        {
            Console.WriteLine(Messages.AppName);
            Console.WriteLine(Messages.Instructions);

            _userInterface.RenderMainMenu(_userService.CurrentUser);

            bool isExit = false;

            string command;

            while (!isExit)
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        if (_userService.CurrentUser is null)
                        {
                            await Login();
                        }
                        else
                        {
                            Logout();
                        }
                        break;
                    case "2":
                        await OpenUserManagementView();
                        break;
                    case "3":
                        await OpenToDoManagementView();
                        break;
                    case "4":
                        await OpenTaskManagementView();
                        break;
                    case "0":
                        isExit = true;
                        return;
                    default:
                        Console.WriteLine(Messages.InvalidCommad);
                        break;
                }
                _userInterface.ReadKey();
                Console.WriteLine(Messages.Instructions);
                if (_userService.CurrentUser != null)
                {
                    _userInterface.ShowCurrentUser(_userService.CurrentUser);
                }

                _userInterface.RenderMainMenu(_userService.CurrentUser);
            }
        }

        #region User Management 

        private async Task FirstExecutionLogin()
        {
            bool isSuccessful = false;

            while (!isSuccessful)
            {
                User newUser = _userInterface.GetCredentials();

                var resultState = await _userService.InitialLogin(newUser);

                Console.WriteLine(resultState.Message);

                if (resultState.IsSuccessful)
                {
                    isSuccessful = true;
                    _userInterface.ReadKey();
                    Console.Clear();
                }
            }
        }

        private async Task Login()
        {
            User user = _userInterface.GetCredentials();

            var resultState = await _userService.Login(user);

            Console.WriteLine(resultState.Message);
        }

        private void Logout()
        {
            _userService.Logout();
            Console.WriteLine(Messages.SuccessfulLogout);
        }

        private async Task OpenUserManagementView()
        {
            if (_userService.CurrentUser is null)
            {
                Console.WriteLine(Messages.NotLogged);
                return;
            }

            if (_userService.CurrentUser.Role != UserRolesEnum.admin.ToString())
            {
                Console.WriteLine(Messages.AdminAccessDenied);
                return;
            }

            _userInterface.ShowCurrentUser(_userService.CurrentUser);
            _userInterface.RenderUserManagementView();
            Console.WriteLine();
            string command = Console.ReadLine();

            bool isBackToMainMenu = false;

            while (!isBackToMainMenu)
            {
                switch (command)
                {
                    case "1":
                        await CreateUser();
                        break;
                    case "2":
                        await EditUser();
                        break;
                    case "3":
                        await DeleteUserById();
                        break;
                    case "4":
                        await ListUsers();
                        break;
                    case "0":
                        isBackToMainMenu = true;
                        return;
                    default:
                        Console.WriteLine(Messages.InvalidCommad);
                        break;
                }
                _userInterface.ReadKey();
                _userInterface.ShowCurrentUser(_userService.CurrentUser);
                _userInterface.RenderUserManagementView();
                command = Console.ReadLine();
            }

        }

        private async Task CreateUser()
        {
            User newUser = _userInterface.GetUserCreatonInfo();

            var resultState = await _userService.CreateUser(newUser);

            Console.WriteLine(resultState.Message);
        }

        private async Task DeleteUserById()
        {
            int userId = _userInterface.GetId("user");

            if (userId == -1)
            {
                Console.WriteLine(Messages.UserInvalidId);
                return;
            }

            var resultState = await _userService.DeleteUser(userId, _userService.CurrentUser.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task EditUser()
        {
            int userId = _userInterface.GetId("user");

            if (userId == -1)
            {
                Console.WriteLine(Messages.UserInvalidId);
                return;
            }

            User newInfoHolderUser = _userInterface.GetUserEditingInfo();

            var resultState = await _userService.EditUser(userId, newInfoHolderUser, _userService.CurrentUser.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task ListUsers()
        {
            var users = await _userService.ListUsers();

            if (!users.Any())
            {
                Console.WriteLine(Messages.UsersEmptyCollection);
                return;
            }

            Console.WriteLine(Messages.UsersList);

            foreach (var user in users)
            {
                Console.WriteLine();
                Console.WriteLine(user.ToString());
                Console.WriteLine("--------------------------------------------");
            }
        }

        #endregion

        #region ToDo List Management

        private async Task OpenToDoManagementView()
        {
            if (_userService.CurrentUser is null)
            {
                Console.WriteLine(Messages.NotLogged);
                return;
            }

            _userInterface.ShowCurrentUser(_userService.CurrentUser);
            _userInterface.RenderToDoListManagementView();

            string command = Console.ReadLine();

            bool isBackToMainMenu = false;

            while (!isBackToMainMenu)
            {
                switch (command)
                {
                    case "1":
                        await CreateToDoList();
                        break;
                    case "2":
                        await EditToDoList();
                        break;
                    case "3":
                        await DeleteToDoList();
                        break;
                    case "4":
                        await ListToDoList();
                        break;
                    case "5":
                        await ShareToDoList();
                        break;
                    case "0":
                        isBackToMainMenu = true;
                        return;
                    default:
                        Console.WriteLine(Messages.InvalidCommad);
                        break;
                }
                _userInterface.ReadKey();
                _userInterface.ShowCurrentUser(_userService.CurrentUser);
                _userInterface.RenderToDoListManagementView();
                command = Console.ReadLine();
            }
        }

        private async Task CreateToDoList()
        {
            ToDoList newToDoList = _userInterface.GetToDoListCreationInfo();

            var resultState = await _toDoListService.CreateToDoList(newToDoList, _userService.CurrentUser.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task DeleteToDoList()
        {
            int listId = _userInterface.GetId("ToDo List");

            if (listId == -1)
            {
                Console.WriteLine(Messages.ToDoListInvalidId);
                return;
            }

            var resultState = await _toDoListService.DeleteToDoList(listId, _userService.CurrentUser.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task EditToDoList()
        {
            int listId = _userInterface.GetId("ToDo List");

            if (listId == -1)
            {
                Console.WriteLine(Messages.ToDoListInvalidId);
                return;
            }

            ToDoList newInfoHolderToDoList = _userInterface.GetToDoListEditingInfo();

            var resultState = await _toDoListService.EditToDoList(listId, newInfoHolderToDoList, _userService.CurrentUser.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task ListToDoList()
        {
            List<ToDoList> toDoLists = await _toDoListService.ListDoToLists();

            if (!toDoLists.Any())
            {
                Console.WriteLine(Messages.ToDoListsEmptyCollection);
                return;
            }

            Console.WriteLine(Messages.ToDoListsList);

            foreach (var toDoList in toDoLists)
            {
                Console.WriteLine();
                Console.WriteLine(toDoList.ToString());
                Console.WriteLine("--------------------------------------------");
            }
        }

        private async Task ShareToDoList()
        {
            int listId = _userInterface.GetId("ToDo List");

            if (listId == -1)
            {
                Console.WriteLine(Messages.ToDoListInvalidId);
                return;
            }

            int userId = _userInterface.GetId("User");

            if (userId == -1)
            {
                Console.WriteLine(Messages.UserInvalidId);
                return;
            }

            List<User> usersList = await _userService.ListUsers();

            var resultState = await _toDoListService.ShareToDoList(_userService.CurrentUser.Id, listId, userId, usersList);

            Console.WriteLine(resultState.Message);
        }

        #endregion

        #region Task Management

        private async Task OpenTaskManagementView()

        {
            if (_userService.CurrentUser is null)
            {
                Console.WriteLine(Messages.NotLogged);
                return;
            }

            List<ToDoList> toDoLists = await _toDoListService.ListDoToLists();

            if (!toDoLists.Any())
            {
                Console.WriteLine(Messages.ToDoListsEmptyCollection);
                return;
            }

            int listId = _userInterface.GetId("ToDo List");

            if (listId == -1)
            {
                Console.WriteLine(Messages.ToDoListInvalidId);
                return;
            }

            ToDoList toDoList = await _toDoListService.GetToDoListById(listId);

            if (toDoList is null)
            {
                Console.WriteLine(Messages.ToDoListDoesntExist, listId);
                return;
            }

            List<int> usersSharedIds = await _toDoListService.UsersSharedToToDoList(listId);

            if (_userService.CurrentUser.Id != toDoList.UserId)
            {
                if (!usersSharedIds.Contains(_userService.CurrentUser.Id))
                {
                    Console.WriteLine(Messages.AdminOrSharedAccessDenied);
                    return;
                }
            }

            Console.WriteLine(Messages.ToDoListAccessedSuccessful, listId);
            _userInterface.ReadKey();
            _userInterface.ShowCurrentUser(_userService.CurrentUser);
            _userInterface.ShowCurrentToDolist(toDoList);
            _userInterface.RenderTaskManagementView();

            string command = Console.ReadLine();

            bool isBackToMainMenu = false;

            while (!isBackToMainMenu)
            {
                switch (command)
                {
                    case "1":
                        await CreateTask(toDoList);
                        break;
                    case "2":
                        await EditTask(toDoList);
                        break;
                    case "3":
                        await DeleteTask(toDoList);
                        break;
                    case "4":
                        await ListTasks(toDoList);
                        break;
                    case "5":
                        await AssignTask(toDoList);
                        break;
                    case "6":
                        await CompleteTask(toDoList);
                        break;
                    case "0":
                        isBackToMainMenu = true;
                        return;
                    default:
                        Console.WriteLine(Messages.InvalidCommad);
                        break;
                }
                _userInterface.ReadKey();
                _userInterface.ShowCurrentUser(_userService.CurrentUser);
                _userInterface.ShowCurrentToDolist(toDoList);
                _userInterface.RenderTaskManagementView();
                command = Console.ReadLine();
            }
        }

        private async Task CreateTask(ToDoList toDoList)
        {
            ToDoTask newToDoTask = _userInterface.GetToDoTaskCreationInfo();

            var resultState = await _taskService.CreateTask(newToDoTask, toDoList.Id, _userService.CurrentUser.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task EditTask(ToDoList toDoList)
        {
            int taskId = _userInterface.GetId("task");

            if (taskId == -1)
            {
                Console.WriteLine(Messages.ToDoTaskInvalidId);
                return;
            }

            ToDoTask newInfoHolderToDoTask = _userInterface.GetTaskEditingInfo();

            var resultState = await _taskService.EditTask(taskId, toDoList.Id, newInfoHolderToDoTask, _userService.CurrentUser.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task DeleteTask(ToDoList toDoList)
        {
            int taskId = _userInterface.GetId("task");

            if (taskId == -1)
            {
                Console.WriteLine(Messages.ToDoTaskInvalidId);
                return;
            }

            var resultState = await _taskService.DeleteTask(taskId, toDoList.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task ListTasks(ToDoList toDoList)
        {
            List<ToDoTask> toDoTask = await _taskService.ListToDoTasks(toDoList.Id);

            if (!toDoTask.Any())
            {
                Console.WriteLine(Messages.ToDoTasksEmptyCollection);
                return;
            }

            Console.WriteLine(Messages.ToDoListsList);

            foreach (var task in await _taskService.ListToDoTasks(toDoList.Id))
            {
                Console.WriteLine();
                Console.WriteLine(task.ToString());
                Console.WriteLine("--------------------------------------------");
            }
        }

        private async Task AssignTask(ToDoList toDoList)
        {
            int taskId = _userInterface.GetId("task");

            if (taskId == -1)
            {
                Console.WriteLine(Messages.ToDoTaskInvalidId);
                return;
            }

            int userId = _userInterface.GetId("User");

            if (userId == -1)
            {
                Console.WriteLine(Messages.UserInvalidId);
                return;
            }

            var resultState = await _taskService.AssignTask(taskId, toDoList, userId, _userService.CurrentUser.Id);

            Console.WriteLine(resultState.Message);
        }

        private async Task CompleteTask(ToDoList toDoList)
        {
            int taskId = _userInterface.GetId("task");

            if (taskId == -1)
            {
                Console.WriteLine(Messages.ToDoTaskInvalidId);
                return;
            }

            var resultState = await _taskService.CompleteTask(taskId);

            Console.WriteLine(resultState.Message);
        }

        #endregion

    }
}


