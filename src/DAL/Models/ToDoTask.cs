using DAL.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text;

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
        public bool IsCompleted { get; set; }

        [BsonElement("addedOn")]
        public DateTime AddedOn { get; set; }

        [BsonElement("EditedOn")]
        public DateTime? EditedOn { get; set; }

        [BsonElement("EditedBy")]
        public int? EditedBy { get; set; }

        [BsonIgnore]
        public User User { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Id: {Id}");
            sb.AppendLine($"Title: {Title}");
            sb.AppendLine($"Description: {Description}");
            sb.AppendLine($"Is Completed: {IsCompleted}");
            sb.AppendLine($"Creator Id: {User.Id}");
            sb.AppendLine($"Date Of Creation: {AddedOn}");
            sb.AppendLine($"Editor Id: {EditedBy}");
            sb.AppendLine($"Date Of Last Edit: {EditedOn}");

            return sb.ToString();
        }

    }
}
