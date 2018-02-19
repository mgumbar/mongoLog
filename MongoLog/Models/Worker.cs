using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoLog.Services;
using Newtonsoft.Json;

namespace MongoLog.Models
{
    [BsonIgnoreExtraElements]
    public class Worker
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("date_time")]
        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }
        [BsonElement("updated_at")]
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [BsonElement("payload")]
        //public Dictionary<string, string> Payload { get; set; }
        public string Payload { get; set; }
        [BsonElement("status")]
        public string Satus { get; set; }
        [BsonElement("job_name")]
        public string JobName { get; set; }
        [BsonElement("progress")]
        public float Progress { get; set; }
        [BsonElement("exception")]
        public string Exception { get; set; }

        public virtual void StartProcessing(LoggerService logger, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        private void ProcessCancellation()
        {
            throw new NotImplementedException();
        }

        public string GetProgress()
        {
            return Convert.ToInt32(Progress).ToString();
        }


    }
}