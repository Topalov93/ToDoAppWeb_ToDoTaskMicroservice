﻿using DAL.Models;
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

        [BsonElement("addedBy")]
        public User AddedBy { get; set; } = new User();

        [BsonElement("EditedOn")]
        public DateTime? EditedOn { get; set; }

        [BsonElement("EditedBy")]
        public string EditedBy { get; set; }

        [BsonIgnore]
        public User AssignedTo { get; set; }  = new User();
    }
}
