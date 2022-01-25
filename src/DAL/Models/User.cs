using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToDoApp.Models.Users
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Username { get; set; }

        [Required]
        [MaxLength(150)]
        public string Password { get; set; }

        [MaxLength(150)]
        public string FirstName { get; set; }

        [MaxLength(150)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }

        [Required]
        public DateTime AddedOn { get; set; }

        [Required]
        public int AddedBy { get; set; }

        public DateTime? EditedOn { get; set; }

        public int? EditedBy { get; set; }

        public virtual ICollection<ToDoList> CreatedToDoLists { get; set; }

        public virtual ICollection<ToDoTask> CreatedToDoTasks { get; set; }

        public virtual ICollection<UserToDoList> ToDoListsShared { get; set; }

        public virtual ICollection<UserToDoTask> ToDoTasksAssigned { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Id: {Id}");
            sb.AppendLine($"Username: {Username}");
            sb.AppendLine($"Password: {Password}");
            sb.AppendLine($"First Name: {FirstName}");
            sb.AppendLine($"Last Name: {LastName}");
            sb.AppendLine($"Role: {Role}");
            sb.AppendLine($"Date Of Creation: {AddedOn}");
            sb.AppendLine($"Creator Id: {AddedBy}");
            sb.AppendLine($"Date Of Last Edit: {EditedOn}");
            sb.AppendLine($"Editor Id: {EditedBy}");

            return sb.ToString();
        }

    }
}
