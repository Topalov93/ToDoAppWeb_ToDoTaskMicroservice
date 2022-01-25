using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ToDoApp.Models.Users;

namespace ToDoApp.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }

        public int ToDoListId { get; set; }

        public virtual ToDoList ToDoList { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public DateTime AddedOn { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime? EditedOn { get; set; }

        public int? EditedBy { get; set; }

        public virtual ICollection<UserToDoTask> AssignedUsers { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Id: {Id}");
            sb.AppendLine($"ToDo List Id: {ToDoListId}");
            sb.AppendLine($"Title: {Title}");
            sb.AppendLine($"Description: {Description}");
            sb.AppendLine($"Is Completed: {IsCompleted}");
            sb.AppendLine($"Creator Id: {UserId}");
            sb.AppendLine($"Date Of Creation: {AddedOn}");
            sb.AppendLine($"Editor Id: {EditedBy}");
            sb.AppendLine($"Date Of Last Edit: {EditedOn}");

            return sb.ToString();
        }

    }
}
