using MongoDB.Bson;
using MongoDB.Driver;
using MongoLog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using Newtonsoft.Json.Linq;

namespace MongoLog.Services
{
    public class WorkerService
    {
        // -----------------------------------------------------------
        // Singleton
        // -----------------------------------------------------------

        private static object syncRoot = new object();

        private const string WORKER_COLLECTION_NAME = "worker";

        private static WorkerService instance;

        public static WorkerService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new WorkerService(ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString);
                        }
                    }
                }

                return instance;
            }
        }

        // ------------------------------------------------------------


        private string connectionString;

        public WorkerService(string inConnectionString)
        {
            connectionString = inConnectionString;
        }

        private IMongoCollection<BsonDocument> GetCollection(String collection)
        {
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("log");
            return database.GetCollection<BsonDocument>(collection);
        }

        public string Insert(JObject json, string clientKey)
        {
            var time = DateTime.Now;
            var document = new BsonDocument
            {
                { "client_key", clientKey },
                { "date_time", DateTime.Now },
                { "payload", json["payload"].ToString() },
                { "job_name", json["jobName"].ToString() },
                { "status", "danger" },
                { "progress", 0 },
                { "exception", "" },
                { "retry", 0 }
            };
            this.GetCollection(WORKER_COLLECTION_NAME).InsertOne(document);
            return "";
        }

        public string UpdateProgress(string clientKey, int progress)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("client_key", clientKey);
            var update = Builders<BsonDocument>.Update.Set("progress", progress).CurrentDate("updated_at");
            var result = this.GetCollection(WORKER_COLLECTION_NAME).UpdateOne(filter, update);
            return "";
        }

        public string UpdateException(string clientKey, string exception)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("client_key", clientKey);
            var update = Builders<BsonDocument>.Update.Set("exception", exception).CurrentDate("updated_at");
            var result = this.GetCollection(WORKER_COLLECTION_NAME).UpdateOne(filter, update);
            return "";
        }
    }
}