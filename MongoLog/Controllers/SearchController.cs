using MongoLog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoLog.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoLog.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public async Task<ActionResult> Index(string host = "", string startDate = "", string endDate = "", string data = "", string logName = "")
        {
            //var logList = LogService.Instance.GetLogsAsync(host, time, date, data, logName);
            var logContext = new LogContext();
            if (String.IsNullOrEmpty(startDate))
                startDate = DateTime.Now.AddDays(-365).ToString();
            if (String.IsNullOrEmpty(endDate))
                endDate = DateTime.Now.ToString();
            Expression<Func<Log, bool>> filter = x => true;

            filter = x => ((String.IsNullOrEmpty(startDate) || x.DateTime >= DateTime.Parse(startDate))
                          && (String.IsNullOrEmpty(host) || x.host.Equals(host))
                          && (String.IsNullOrEmpty(endDate) || x.DateTime <= DateTime.Parse(endDate))
                          && (String.IsNullOrEmpty(data) || x.data.Contains(data))
                          && (String.IsNullOrEmpty(logName) || x.logname.Equals(logName)));

            var logs = await logContext.Logs.Find(filter)
                .Limit(2000)
                .ToListAsync();
            var logListed = logs.OrderBy(p => p.DateTime).ToList();
            return View(logListed);
        }

        // GET: Search/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Search/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Search/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Search/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Search/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Search/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Search/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
