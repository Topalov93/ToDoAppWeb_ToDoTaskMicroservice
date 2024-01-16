using DAL.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ToDoApp.Models
{
    public class ToDoTask
    {
        public ToDoTask()
        {

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; } = false;

        [BsonElement("isTodoListFree")]
        public bool IsBacklog { get; set; } = true;

        [BsonElement("addedOn")]
        public DateTime AddedOn { get; set; }

        [BsonElement("addedBy")]
        public User AddedBy { get; set; } = new User();

        [BsonElement("editedOn")]
        public DateTime? EditedOn { get; set; } = new DateTime();

        [BsonElement("editedBy")]
        public User EditedBy { get; set; } = new User();

        [BsonElement("assignedTo")]
        public User AssignedTo { get; set; }  = new User();
    }
}
