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

namespace MongoLog.Services
{
    public class LogService
    {
        // -----------------------------------------------------------
        // Singleton
        // -----------------------------------------------------------

        private static object syncRoot = new object();

        private static LogService instance;

        public static LogService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new LogService(ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString);
                        }
                    }
                }

                return instance;
            }
        }

        // ------------------------------------------------------------


        private string connectionString;

        public LogService(string inConnectionString)
        {
            connectionString = inConnectionString;

        }

        /// <summary>
        /// Récupère les logs de la base de données
        /// </summary>
        /// <returns></returns>
        public List<BsonDocument> GetLogsAsync(string host, string time, string date, string data, string logName)
        {
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("log");
            var collection = database.GetCollection<BsonDocument>("coreact");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("name", "coreact")
                         & builder.Eq("host", host)
                         & builder.Regex("time", new BsonRegularExpression(".*" + time + ".*"))
                         & builder.Regex("date", new BsonRegularExpression(".*" + date + ".*"))
                         & builder.Regex("data", new BsonRegularExpression(".*" + data + ".*"))
                         & builder.Regex("logname", new BsonRegularExpression(".*" + logName + ".*"));
                         //& builder.Eq("logname", logName);
            var sort = Builders<BsonDocument>.Sort.Ascending("line");
            var result = collection.Find(filter).Limit(5000).Sort(sort).ToList();
            return result;
        }
    }
}