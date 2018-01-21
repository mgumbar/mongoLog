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
        public List<Log> GetLogs(string host, string time)
        {
            var client = new MongoClient(this.connectionString);
            var database = client.GetDatabase("log");
            var collection = database.GetCollection<Log>("back-end");
            var filter = Builders<Log>.Filter.Eq("host", host);
            var list = collection.Find(x => x.host == host).ToList();

            return list;
        }
    }
}