﻿using MongoDB.Driver;
using MongoLog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace MongoLog.Services
{
    public class LoggerService
    {
        public string FilePath { get; set; }
        public string ApplicationName { get; set; }
        public DateTime StartDate { get; set; }
        public string ClientKey { get; set; }

        public async void StartFlLog()
        {
            /*var client = new MongoClient(@"mongodb://admin:admin@cluster0-shard-00-00-hudu2.mongodb.net:27017,cluster0-shard-00-01-hudu2.mongodb.net:27017,cluster0-shard-00-02-hudu2.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin");*/
            WorkerService.Instance.UpdateException(ClientKey, "Starting " + DateTime.Now.ToString());
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString);
            var database = client.GetDatabase("log");
            var collection = database.GetCollection<Log>("log");
            var timeOne = DateTime.Now.ToString();
            var nbLines = File.ReadLines(FilePath).Count();
            WorkerService.Instance.UpdateException(ClientKey, "nbLines " + nbLines.ToString());
            int counter = 1;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(FilePath);
            DateTime dateTime;
            DateTime previousDate = StartDate; //DateTime.Parse("31/07/2017 00:00:01");
            string[] words;
            string server;
            string process;
            string logStatus = "";

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    WorkerService.Instance.UpdateException(ClientKey, "counter " + counter.ToString());
                    dateTime = DateTime.Parse(StartDate.ToString("yyyy-MM-") + line.Substring(4, 12));
                    words = line.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    server = words[3];
                    try
                    {
                        process = words[4].Substring(words[4].IndexOf("[") + 1, (words[4].IndexOf("]") - words[4].IndexOf("[")) - 1);
                    }
                    catch (Exception)
                    {
                        process = "";
                    }

                        previousDate = dateTime;
                    var lower = line.ToLower();
                    if (lower.Contains("exception") || lower.Contains("error") || lower.Contains("fatal"))
                        logStatus = "danger";
                    else if (lower.Contains("info"))
                        logStatus = "info";
                    else if (lower.Contains("warning"))
                        logStatus = "warning";
                    else if (lower.Contains("success"))
                        logStatus = "success";
                    else logStatus = "";

                    await collection.InsertOneAsync(new Log
                    {
                        ApplicationName = "fundlook",
                        Host = server,
                        Logname = Path.GetFileName(FilePath),
                        Date = dateTime.ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)[0].Replace("/", "-"),
                        Time = dateTime.ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)[1],
                        DateTime = dateTime,
                        Line = counter,
                        Data = line,
                        Status = logStatus,
                        Process = process
                    });
                }
                catch (Exception)
                {
                    //Console.WriteLine("Error");
                    dateTime = previousDate;
                    counter++;

                }
                WorkerService.Instance.UpdateProgress(ClientKey, (counter / nbLines * 100));

                counter++;
            }
            file.Close();
        }
    }
}