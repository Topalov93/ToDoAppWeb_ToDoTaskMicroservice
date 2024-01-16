using System;

namespace DAL.Models
{
    public class ToDoList
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime EditedOn { get; set; }
    }
}
