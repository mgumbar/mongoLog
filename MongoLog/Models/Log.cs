using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoLog.Models
{
    public class Log
    {
        public ObjectId Id { get; set; }
        public string line { get; set; }
        public string Name { get; set; }
        public string host { get; set; }
        public string logname { get; set; }
        public string user { get; set; }
        public string time { get; set; }
        public string path { get; set; }
        public string request { get; set; }
        public string status { get; set; }
        public string response_size { get; set; }
        public string referrer { get; set; }
        public string user_agent { get; set; }
    }
}