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
        public int Progress { get; set; }
        [BsonElement("exception")]
        public string Exception { get; set; }

        string filePath = @"c:/test.txt";

        public void StartProcessing(LoggerService logger, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                logger.StartFlLog();
            }
            catch (Exception ex)
            {
                ProcessCancellation();
                string errorMsg = "Error Occured : " + ex.GetType().ToString() + " : " + ex.Message;
                WorkerService.Instance.UpdateException(logger.ClientKey, errorMsg);
                //File.AppendAllText(filePath, "Error Occured : " + ex.GetType().ToString() + " : " + ex.Message);
            }
        }
        private void ProcessCancellation()
        {
            //Thread.Sleep(10000);
            //File.AppendAllText(filePath, "Process Cancelled");
        }


    }
}