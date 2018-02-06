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
    public class EventService
    {
        // -----------------------------------------------------------
        // Singleton
        // -----------------------------------------------------------

        private static object syncRoot = new object();

        private static EventService instance;

        public static EventService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new EventService(ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString);
                        }
                    }
                }

                return instance;
            }
        }

        // ------------------------------------------------------------


        private string connectionString;

        public EventService(string inConnectionString)
        {
            connectionString = inConnectionString;
        }

        private IMongoCollection<BsonDocument> GetCollection(String collection)
        {
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("amon");
            return database.GetCollection<BsonDocument>(collection);
        }

        public string Insert(JObject json, String collection, String clientKey, String stepName, String origin)
        {
            var time = DateTime.Now;
            var document = new BsonDocument
            {
                { "client_key", clientKey },
                { "message", json["message"].ToString() },
                { "event_created_at", json["payload"]["createdAt"].ToString() },
                { "status", json["payload"]["status"].ToString() },
                { "event_type", json["event_type"].ToString() },
                { "origin", origin },
                { "created_at", time },
                { "updated_at", time }
            };
            this.GetCollection(collection).InsertOne(document);
            return "";
        }

    }
}