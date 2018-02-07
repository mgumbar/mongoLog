using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MongoLog.Models
{
    [BsonIgnoreExtraElements]
    public class Log
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int line { get; set; }
        public string name { get; set; }
        public string data { get; set; }
        public string date { get; set; }
        [BsonElement("date_time")]
        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }
        public string host { get; set; }
        public string logname { get; set; }
        public string user { get; set; }
        public string time { get; set; }
        public string path { get; set; }
        public string request { get; set; }
        public string status { get; set; }
        public string responseSize { get; set; }
        public string referrer { get; set; }
        public string userAgent { get; set; }
        public string process { get; set; }
    }
}