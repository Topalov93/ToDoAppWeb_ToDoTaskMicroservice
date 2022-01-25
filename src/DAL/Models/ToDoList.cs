using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ToDoApp.Models.Users;

namespace ToDoApp.Models
{
    public class ToDoList
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public DateTime AddedOn { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime? EditedOn { get; set; }

        public int? EditedBy { get; set; }

        public virtual ICollection<ToDoTask> ToDoTasks { get; set; }

        public virtual ICollection<UserToDoList> SharedUsers { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Id: {Id}");
            sb.AppendLine($"Title: {Title}");
            sb.AppendLine($"Date Of Creation: {AddedOn}");
            sb.AppendLine($"Creator Id: {UserId}");
            sb.AppendLine($"Date Of Last Edit: {EditedOn}");
            sb.AppendLine($"EditorId: {EditedBy}");

            return sb.ToString();
        }

    }
}
