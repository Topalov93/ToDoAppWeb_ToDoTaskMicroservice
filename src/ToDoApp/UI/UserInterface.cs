using System;
using Common;
using ToDoApp.Models;
using ToDoApp.Models.Users;

namespace ToDoApp.UI
{
    public class UserInterface
    {
        public void ReadKey()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        public void RenderInitialLoginMenu()
        {
            Console.WriteLine(Messages.Instructions);
            Console.WriteLine("1. Login" + Environment.NewLine);
            Console.WriteLine("0. Exit");
        }

        public void RenderMainMenu(User currentUser)
        {
            if (currentUser is null)
            {
                Console.WriteLine("1. Login" + Environment.NewLine);
            }
            else
            {
                Console.WriteLine("1. Logout" + Environment.NewLine);
            }

            Console.WriteLine("2. User Managment" + Environment.NewLine);
            Console.WriteLine("3. ToDo Lists Managment" + Environment.NewLine);
            Console.WriteLine("4. Tasks Managment" + Environment.NewLine);
            Console.WriteLine("0. Exit" + Environment.NewLine);
        }

        public void RenderUserManagementView()
        {
            Console.WriteLine("-------User Managment View-------");
            Console.WriteLine("Select option from the menu and press enter" + Environment.NewLine);
            Console.WriteLine("1. Create user" + Environment.NewLine);
            Console.WriteLine("2. Edit user" + Environment.NewLine);
            Console.WriteLine("3. Delete user" + Environment.NewLine);
            Console.WriteLine("4. List users" + Environment.NewLine);
            Console.WriteLine("0. Back to main menu");
        }

        public void RenderToDoListManagementView()
        {
            Console.WriteLine("-------ToDo Managment View-------");
            Console.WriteLine("Select option from the menu and press enter" + Environment.NewLine);
            Console.WriteLine("1. Create ToDo List" + Environment.NewLine);
            Console.WriteLine("2. Edit ToDo List" + Environment.NewLine);
            Console.WriteLine("3. Delete ToDo List" + Environment.NewLine);
            Console.WriteLine("4. List Todo Lists" + Environment.NewLine);
            Console.WriteLine("5. Share ToDoList" + Environment.NewLine);
            Console.WriteLine("0. Back to main menu" + Environment.NewLine);
        }

        public void RenderTaskManagementView()
        {
            Console.WriteLine("-------Task Managment View-------");
            Console.WriteLine("Select option from the menu and press enter" + Environment.NewLine);
            Console.WriteLine("1. Create task" + Environment.NewLine);
            Console.WriteLine("2. Edit task" + Environment.NewLine);
            Console.WriteLine("3. Delete task" + Environment.NewLine);
            Console.WriteLine("4. List tasks" + Environment.NewLine);
            Console.WriteLine("5. Assign task" + Environment.NewLine);
            Console.WriteLine("6. Complete task" + Environment.NewLine);
            Console.WriteLine("0. Back to main menu" + Environment.NewLine);
        }

        public User GetCredentials()
        {
            User newUser = new User();

            Console.WriteLine("Enter Username:");
            newUser.Username = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            newUser.Password = Console.ReadLine();

            return newUser;
        }

        public User GetUserCreatonInfo()
        {
            User newUser = new User();

            Console.WriteLine("Enter Username:");
            newUser.Username = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            newUser.Password = Console.ReadLine();

            Console.WriteLine("Enter First name:");
            newUser.FirstName = Console.ReadLine();

            Console.WriteLine("Enter Last name:");
            newUser.LastName = Console.ReadLine();

            Console.WriteLine("Enter Role (admin or regular):");
            newUser.Role = Console.ReadLine();

            return newUser;
        }

        public User GetUserEditingInfo()
        {
            User newInfoHolderUser = new User();

            Console.WriteLine("Enter new Username:");
            newInfoHolderUser.Username = Console.ReadLine();

            Console.WriteLine("Enter new Password:");
            newInfoHolderUser.Password = Console.ReadLine();

            Console.WriteLine("Enter new First name:");
            newInfoHolderUser.FirstName = Console.ReadLine();

            Console.WriteLine("Enter new Last name:");
            newInfoHolderUser.LastName = Console.ReadLine();

            return newInfoHolderUser;
        }

        public ToDoList GetToDoListCreationInfo()
        {
            ToDoList newToDoList = new ToDoList();

            Console.WriteLine("Enter Title:");

            newToDoList.Title = Console.ReadLine();

            return newToDoList;
        }

        public ToDoList GetToDoListEditingInfo()
        {
            ToDoList newInfoHolderToDoList = new ToDoList();

            Console.WriteLine("Enter new Title:");
            newInfoHolderToDoList.Title = Console.ReadLine();

            return newInfoHolderToDoList;
        }

        public ToDoTask GetToDoTaskCreationInfo()
        {
            ToDoTask newToDoTask = new ToDoTask();

            Console.WriteLine("Enter Title:");

            newToDoTask.Title = Console.ReadLine();

            Console.WriteLine("Enter Description:");

            newToDoTask.Description = Console.ReadLine();

            return newToDoTask;
        }

        public ToDoTask GetTaskEditingInfo()
        {
            ToDoTask newInfoHolderToDoTask = new ToDoTask();

            Console.WriteLine("Enter new Title:");
            newInfoHolderToDoTask.Title = Console.ReadLine();

            Console.WriteLine("Enter new Description:");
            newInfoHolderToDoTask.Description = Console.ReadLine();

            bool completeStatus;

            Console.WriteLine("Enter new 'is completed' status (true or false):");

            while (!bool.TryParse(Console.ReadLine(), out completeStatus))
            {
                Console.WriteLine("Is completed status can be only true or false!");
                Console.WriteLine("Enter new is completed status:");
            }

            newInfoHolderToDoTask.IsCompleted = completeStatus;

            return newInfoHolderToDoTask;

        }

        public int GetId(string type)
        {
            Console.WriteLine($"Enter {type} Id:");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                return -1;
            }
            return id;
        }

        public void ShowCurrentUser(User currentUser)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Current user: {currentUser.Username}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void ShowCurrentToDolist(ToDoList todoList)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Current ToDoList: {todoList.Title}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
