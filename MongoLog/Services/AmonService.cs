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
    public class AmonService
    {
        // -----------------------------------------------------------
        // Singleton
        // -----------------------------------------------------------

        private static object syncRoot = new object();

        private static AmonService instance;

        public static AmonService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AmonService(ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString);
                        }
                    }
                }

                return instance;
            }
        }

        // ------------------------------------------------------------


        private string connectionString;

        public AmonService(string inConnectionString)
        {
            connectionString = inConnectionString;
        }

        private IMongoCollection<BsonDocument> GetCollection(String collection)
        {
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("amon");
            return database.GetCollection<BsonDocument>(collection);
        }

        private IMongoCollection<Amon> GetAmonCollection(String collection)
        {
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("amon");
            return database.GetCollection<Amon>(collection);
        }

        public List<BsonDocument> GetAmons(string origin, string startTime, string endTime)
        {
            // CHECK EVENT DATETIME IN MONGO DB
            var dteFmt = "yyyy-MM-dd HH:mm:ss";
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("amon");
            var collection = database.GetCollection<BsonDocument>("worklow");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Ne("sender.source", origin);
                        //& builder.Regex("updated_at", new BsonRegularExpression(".*" + startTime + ".*"));
            //& builder.Eq("logname", logName);
            var sort = Builders<BsonDocument>.Sort.Ascending("updated_at");
            var result = collection.Find(filter).Limit(500).Sort(sort).ToList();
            return result;
        }

        public string Insert(JObject json, String collection, string origin)
        {
            var time = DateTime.Now;
            var document = new BsonDocument
            {
                { "client_key", json["client_key"].ToString() },
                { "sender", json["workflow"]["sender"].ToString() },
                { "steps", json["workflow"]["steps"].ToString() },
                { "payload", json["workflow"]["payload"].ToString() },
                { "origin", origin },
                { "created_at", time },
                { "updated_at", time }
            };
            this.GetCollection(collection).InsertOne(document);
            return "";
        }

        public JObject Update(JObject json, String collection, String clientKey, String stepName)
        {

            //var filter = Builders<BsonDocument>.Filter.Eq("client_key", clientKey);
            //var update = Builders<BsonDocument>.Update.Set("steps.events." + stepName, json["message"]).CurrentDate("updated_at");

            //var update = Builders<BsonDocument>.Update.Set(x => x.GetValue("steps.name") == stepName, "test").CurrentDate("updated_at");
            //var result = this.GetCollection(collection).UpdateOne(filter, update);


            var filter = Builders<BsonDocument>.Filter.Eq("client_key", clientKey);
            var sort = Builders<BsonDocument>.Sort.Descending("client_key");
            var result = this.GetCollection(collection).Find(filter).Sort(sort).ToList().FirstOrDefault();
            Debug.WriteLine(result.ToJson());
            var temp = result.GetElement("steps").ToString();
            var tjson = JObject.Parse(result.GetElement("steps").ToJson());
            tjson["name"] = json;
            return tjson;
            //var amon = this.GetAmonCollection(collection);
            //amon.FindOneAndUpdate(
            //c => c.ClientKey == clientKey && c.Steps.Any(s => s.name == stepName), // find this match
            //Builders<Amon>.Update.Set(c => c.Steps[-1].events, json["message"]));



            //var amonCollection = this.GetAmonCollection(collection);
            //var builder = Builders<Amon>.Filter;
            ////var filter = builder.Eq("client_key", clientKey);
            //var amonToUpdate = amonCollection.Find(a => a.ClientKey == clientKey).First();




            //var temp = JObject.Parse(amonToUpdate.GetElement("steps").ToJson());
            //var temp = JObject.Parse(amonToUpdate.GetElement("steps").ToJson());
            //var val = temp["Value"];
            //var toto = amonToUpdate.GetElement("steps").ToJson();


            //var stepToUpdate = amonToUpdate.Steps.First(s => s.Name == stepName);
            //var stepEvent = new StepEvent(json["message"].ToString(),
            //                                json["payload"]["createdAt"].ToString(),
            //                                json["payload"]["status"].ToString(),
            //                                json["event_type"].ToString());
            //stepToUpdate.Events.Add(stepEvent);
            //var result = amonCollection.ReplaceOne(a => a.ClientKey == amonToUpdate.ClientKey, amonToUpdate);
        }
    }
}

