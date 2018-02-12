using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB.Driver;
namespace MongoLog.Models
{
    public class LogContext
    {
        public const string CONNECTION_STRING_NAME = "MongoServer";
        public const string DATABASE_NAME = "log";
        public const string LOGS_COLLECTION_NAME = "log";
        public const string WORKFLOWS_COLLECTION_NAME = "workflow";
        public const string STEPS_COLLECTION_NAME = "step_event";
        public const string WORKER_COLLECTION_NAME = "worker";

        // This is ok... Normally, they would be put into
        // an IoC container.
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static LogContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(DATABASE_NAME);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoCollection<Log> Logs
        {
            get { return _database.GetCollection<Log>(LOGS_COLLECTION_NAME); }
        }

        public IMongoCollection<Workflow> Workflows
        {
            get { return _database.GetCollection<Workflow>(WORKFLOWS_COLLECTION_NAME); }
        }

        public IMongoCollection<StepEvent> StepEvents
        {
            get { return _database.GetCollection<StepEvent>(STEPS_COLLECTION_NAME); }
        }

        public IMongoCollection<Worker> Workers
        {
            get { return _database.GetCollection<Worker>(WORKER_COLLECTION_NAME); }
        }
    }
}