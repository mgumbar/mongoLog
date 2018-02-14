using MongoDB.Driver;
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

        public async void StartFileProcessing()
        {
            if (FilePath.ToLower().Contains("fundlook"))
            {
                StartFileProcessing();
            }
            else if (FilePath.ToLower().Contains("activemonitoring.log")
                     || FilePath.ToLower().Contains("businessrulelibrary.log"))
            {
                StartCrGenericAProcessing();
            }
            else if (FilePath.ToLower().Contains("documentgeneration.log")
                     || FilePath.ToLower().Contains("livingdocumentdata.log")
                     || FilePath.ToLower().Contains("livingdocumentservice.log")
                     || FilePath.ToLower().Contains("serice.log")
                     || FilePath.ToLower().Contains("workflow.log"))
            {
                StartCrGenericBProcessing();
            }
            //else if (FilePath.ToLower().Contains("disseminationdownloadrule.log"))
            //{
            //    // TO BE CHECKED
            //    StartCrGenericAProcessing();
            //}
            //else if (FilePath.ToLower().Contains("disseminationupoadrule.log"))
            //{
            //    // TO BE CHECKED
            //    StartCrGenericAProcessing();
            //}
            //else if (FilePath.ToLower().Contains("livingdocumentmvc.log"))
            //{
            //    // TO BE CHECKED
            //    StartCrGenericAProcessing();
            //}
            else
            {
                WorkerService.Instance.UpdateProgress(ClientKey, 100);
                WorkerService.Instance.UpdateStatus(ClientKey, "danger");
                WorkerService.Instance.UpdateException(ClientKey, String.Format("Unable to process file: {0}", FilePath));
            }
        }

        public async void StartCrGenericBProcessing()
        {
            /*var client = new MongoClient(@"mongodb://admin:admin@cluster0-shard-00-00-hudu2.mongodb.net:27017,cluster0-shard-00-01-hudu2.mongodb.net:27017,cluster0-shard-00-02-hudu2.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin");*/
            WorkerService.Instance.UpdateException(ClientKey, "Starting " + DateTime.Now.ToString());
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString);
            var database = client.GetDatabase("log");
            var collection = database.GetCollection<Log>("log");
            var timeOne = DateTime.Now.ToString();
            var nbLines = File.ReadLines(FilePath).Count();
            int counter = 1;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(FilePath);
            DateTime dateTime;
            DateTime previousDate = StartDate; //DateTime.Parse("31/07/2017 00:00:01");
            string process;
            string logStatus = "";
            float progress = 0;
            int updateProgress = 0;
            string[] words;
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    words = line.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        dateTime = DateTime.Parse(words[2] + " " + words[3].Replace(",", "."));
                        process = words[2];
                    }
                    catch (Exception)
                    {
                        dateTime = previousDate;
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
                        ApplicationName = "coreact",
                        Host = "",
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
                progress = ((float)counter / (float)nbLines) * 100;
                updateProgress++;
                if (updateProgress >= 100)
                {
                    updateProgress = 0;
                    WorkerService.Instance.UpdateProgress(ClientKey, progress);
                }

                counter++;
            }
            if (progress >= 100)
            {
                WorkerService.Instance.UpdateProgress(ClientKey, 100);
                WorkerService.Instance.UpdateStatus(ClientKey, "success");
                WorkerService.Instance.UpdateException(ClientKey, "Successfully imported");
            }
            file.Close();
        }

        public async void StartCrGenericAProcessing()
        {
            /*var client = new MongoClient(@"mongodb://admin:admin@cluster0-shard-00-00-hudu2.mongodb.net:27017,cluster0-shard-00-01-hudu2.mongodb.net:27017,cluster0-shard-00-02-hudu2.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin");*/
            WorkerService.Instance.UpdateException(ClientKey, "Starting " + DateTime.Now.ToString());
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString);
            var database = client.GetDatabase("log");
            var collection = database.GetCollection<Log>("log");
            var timeOne = DateTime.Now.ToString();
            var nbLines = File.ReadLines(FilePath).Count();
            int counter = 1;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(FilePath);
            DateTime dateTime;
            DateTime previousDate = StartDate; //DateTime.Parse("31/07/2017 00:00:01");
            string process;
            string logStatus = "";
            float progress = 0;
            int updateProgress = 0;
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    try
                    {
                        dateTime = DateTime.Parse(line.Substring(0, 23).Replace(",", "."));
                        process = line.Substring(line.IndexOf("[") + 1, (line.IndexOf("]") - line.IndexOf("[")) - 1);
                    }
                    catch (Exception)
                    {
                        dateTime = previousDate;
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
                        ApplicationName = "coreact",
                        Host = "",
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
                progress = ((float)counter / (float)nbLines) * 100;
                updateProgress++;
                if (updateProgress >= 100)
                {
                    updateProgress = 0;
                    WorkerService.Instance.UpdateProgress(ClientKey, progress);
                }

                counter++;
            }
            if (progress >= 100)
            {
                WorkerService.Instance.UpdateProgress(ClientKey, 100);
                WorkerService.Instance.UpdateStatus(ClientKey, "success");
                WorkerService.Instance.UpdateException(ClientKey, "Successfully imported");
            }
            file.Close();
        }

        public async void StartFlProcessing()
        {
            /*var client = new MongoClient(@"mongodb://admin:admin@cluster0-shard-00-00-hudu2.mongodb.net:27017,cluster0-shard-00-01-hudu2.mongodb.net:27017,cluster0-shard-00-02-hudu2.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin");*/
            WorkerService.Instance.UpdateException(ClientKey, "Starting " + DateTime.Now.ToString());
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString);
            var database = client.GetDatabase("log");
            var collection = database.GetCollection<Log>("log");
            var timeOne = DateTime.Now.ToString();
            var nbLines = File.ReadLines(FilePath).Count();
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
            float progress = 0;
            while ((line = file.ReadLine()) != null)
            {
                try
                {
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
                progress = ((float)counter / (float)nbLines) * 100;
                WorkerService.Instance.UpdateProgress(ClientKey, progress);

                counter++;
            }
            if (progress >= 100)
            {
                WorkerService.Instance.UpdateProgress(ClientKey, 100);
                WorkerService.Instance.UpdateStatus(ClientKey, "success");
                WorkerService.Instance.UpdateException(ClientKey, "Successfully imported");
            }
            file.Close();
        }
    }
}