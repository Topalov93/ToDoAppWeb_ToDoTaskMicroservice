﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace DAL.Data
{
    public class ToDoTasksRepository : ToDoAppDbContext
    {
        public async Task CreateToDoTask(ToDoTask newToDoTask)
        {
            await ToDoTasks.Add(newToDoTask).ReloadAsync();
            SaveChanges();
        }

        public async Task EditToDoTask(int taskId, ToDoTask newInfoHolderToDoTask)
        {
            ToDoTask toDoTask = await ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            toDoTask.Title = newInfoHolderToDoTask.Title;
            toDoTask.Description = newInfoHolderToDoTask.Description;
            toDoTask.IsCompleted = newInfoHolderToDoTask.IsCompleted;
            toDoTask.EditedBy = newInfoHolderToDoTask.EditedBy;
            toDoTask.EditedOn = newInfoHolderToDoTask.EditedOn;

            SaveChanges();
        }

        public async Task DeleteToDoTask(int taskId)
        {
            var taskToDelete = await ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            ToDoTasks.Remove(taskToDelete);

            SaveChanges();
        }

        //public async Task AssignToDoTask(int taskId, int userId)
        //{
            //await UsersToDoTasks.Add(new UserToDoTask { ToDoTaskId = taskId, UserId = userId }).ReloadAsync();

            //SaveChanges();
        //}

        public async Task CompleteToDoTask(int taskId)
        {
            ToDoTask toDoTask = await ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            toDoTask.IsCompleted = true;

            SaveChanges();
        }

        public async Task<ToDoTask> GetToDoTaskById(int taskId)
        {
            return await ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ToDoTask> GetToDoTaskByTitle(string title)
        {
            return await ToDoTasks.FirstOrDefaultAsync(t => t.Title == title);
        }
    }
}
