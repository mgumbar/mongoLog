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
    public class WorkerLogger : Worker
    {
        public override void StartProcessing(LoggerService logger, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                logger.StartFileProcessing();
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