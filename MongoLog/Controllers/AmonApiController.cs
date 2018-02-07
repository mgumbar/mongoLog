using MongoLog.Models;
using MongoLog.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace MongoLog.Controllers
{
    public class AmonApiController : ApiController
    {
        // GET: api/AmonApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AmonApi/5
        public string Get(int id)
        {
            return "value";
        }

        //POST: api/AmonApi
        public void Post([FromBody]string value)
        {
            ///*return*/ data.ToString();
        }

        //[System.Web.Http.HttpPost]
        //public string PostJsonString([FromBody] string text)
        //{
        //    return text;
        //}

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/workflow/{clientKey}")]
        public async Task<string> PostNewWorkflow(string clientKey)
        {
            //var status = AmonService.Instance.Insert(json, "worklow", origin);
            string result = await Request.Content.ReadAsStringAsync();
            var json = JObject.Parse(result);
            var logContext = new LogContext();

            var time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            List<Step> steps = new List<Step>();
            foreach (var item in json["workflow"]["steps"])
            {
                steps.Add(item.ToObject<Step>());
                //steps.Add(new Step(step["name"], step["label"], step["category"], step["sub_category"]));
            }
            var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(json["workflow"]["payload"].ToString());
            var document = new Workflow
            {
                ClientKey = clientKey.ToString(),
                Source = json["workflow"]["sender"]["source"].ToString().ToLower(),
                Module = json["workflow"]["sender"]["module"].ToString(),
                Payload = payload,
                Steps = steps
            };
            await logContext.Workflows.InsertOneAsync(document);
            //this.GetCollection(collection).InsertOne(document);

            return "";
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/event/{origin}/{clientKey}")]
        public async Task<string> PostEventGeneric(string origin, string clientKey)
        {
            string result = await Request.Content.ReadAsStringAsync();
            var json = JObject.Parse(result);
            var logContext = new LogContext();
            var document = new StepEvent
            {
                Message = json["message"].ToString(),
                CreatedAt = DateTime.Parse(json["created_at"].ToString()),
                Status = json["payload"]["status"].ToString(),
                EventType = json["event_type"].ToString(),
                Source = origin.ToLower(),
                ClientKey = clientKey
            };
            await logContext.StepEvents.InsertOneAsync(document);
            //this.GetCollection(collection).InsertOne(document);

            return "";
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Amon/Event/{origin}/{clientReference}/{step}")]
        public async Task<string> PostNewEvent(string origin, string clientReference, string step)
        {
            string result = await Request.Content.ReadAsStringAsync();
            var json = JObject.Parse(result);
            var status = EventService.Instance.Insert(json, "event", clientReference, step, origin);
            return "";
        }

        // PUT: api/AmonApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AmonApi/5
        public void Delete(int id)
        {
        }
    }

    public class SensorData
    {
    }
}
