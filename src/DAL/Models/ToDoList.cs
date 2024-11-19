using System;

namespace DAL.Models
{
    public class ToDoList
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
    }
}
