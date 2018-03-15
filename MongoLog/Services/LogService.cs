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
using MongoDB.Bson.Serialization;

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
                         & builder.Eq("logname", new BsonRegularExpression(".*" + logName + ".*"));
                         //& builder.Eq("logname", logName);
            var sort = Builders<BsonDocument>.Sort.Ascending("line");
            var result = collection.Find(filter).Limit(500).Sort(sort).ToList();
            return result;
        }

        public List<Log> GetLogsJson(string applicationName, DateTime starDate, DateTime endDate, string data, string logName)
        {
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("log");
            var collection = database.GetCollection<BsonDocument>("log");
            if (applicationName.ToLower() == "global")
                applicationName = null;
            var applicationNameQuery = String.IsNullOrEmpty(applicationName) ? String.Format("application_name: {{ $ne:null }}") : String.Format("application_name: '{0}'", applicationName);
            var dataQuery = String.IsNullOrEmpty(data) ? String.Format("data: {{ $ne:null }}") : String.Format("data: RegExp('{0}')", data);
            var logNameQuery = String.IsNullOrEmpty(logName) ? String.Format("logname: {{ $ne:null }}") : String.Format("logname: '{0}", logName);
            var filter = String.Format(@"{{{0}, 
                                           date_time: {{$gte: ISODate('{1}'), $lte: ISODate('{2}')}},
                                           {3},
                                           {4}
                                         }}",
                                         applicationNameQuery, 
                                         starDate.AddHours(-2).ToString("yyyy-MM-ddTHH:mm:ssZ"), 
                                         endDate.AddHours(-2).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                                         dataQuery,
                                         logNameQuery);

            //& builder.Eq("logname", logName);
            var documentArray = collection.Find(filter).Limit(500).ToList();
            var result = new List<Log>();
            foreach (var document in documentArray)
            {
                var log = BsonSerializer.Deserialize<Log>(document);
                result.Add(log);
            }
            return result;
        }

        public List<BsonDocument> GetLogsJsonV1(string host, string time, string date, string data, string logName)
        {
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("log");
            var collection = database.GetCollection<BsonDocument>("log");

            var filter = @"{$and : [" +
                            "{ $and : [ {name: 'coreact'} ] }," +
                            "{ $and : [ {time: { $regex: '/.*" + time + ".*/', $options: '-i' }} ] }," +
                            "{ $and: [ { date: { $regex: '/.*" + date + ".*/', $options: '-i' } } ] }," +
                            "{ $and: [ { data: { $regex: '/.*" + data + ".*/', $options: '-i' } } ] }," +
                            "{ $and: [ { logname: '" + logName + "' } ] }]}"; //", "coreact", time, date, data, logName

            //& builder.Eq("logname", logName);
            var sort = Builders<BsonDocument>.Sort.Ascending("line");
            var result = collection.Find(filter).Limit(500).Sort(sort).ToList();
            return result;
        }
    }
}