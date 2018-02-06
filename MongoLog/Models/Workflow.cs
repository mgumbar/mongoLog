using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MongoLog.Models
{
    public class Workflow
    {
        public ObjectId _id { get; set; }
        [BsonElement("client_key")]
        public string ClientKey { get; set; }
        [BsonElement("source")]
        public string Source { get; set; }
        [BsonElement("module")]
        public string Module { get; set; }
        [BsonElement("payload")]
        public Dictionary<string, string> Payload { get; set; }
        [BsonElement("steps")]
        public List<Step> Steps { get; set; }
        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public Workflow()
        {
            var timeNow = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            this.CreatedAt = timeNow;
            this.UpdatedAt = timeNow;
        }

        
        public bool PayloadContains(string value)
        {
            return this.Payload.ContainsValue(value);
        }
    }
}