using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoLog.Models
{
    //[BsonDiscriminator("events")]
    //[BsonIgnoreExtraElements]
    public class StepEvent
    {
        [BsonElement("message")]
        public string Message { get; set; }
        [BsonElement("created_at")]
        public string CreatedAt { get; set; }
        [BsonElement("status")]
        public string Status { get; set; }
        [BsonElement("event_type")]
        public string EventType { get; set; }

        //[BsonConstructorAttribute("events")]
        //public StepEvent(string message, string createdAt, string status, string eventType)
        //{
        //    this.Message = message;
        //    this.CreatedAt = createdAt;
        //    this.Status = status;
        //    this.EventType = eventType;
        //}

    }
}
