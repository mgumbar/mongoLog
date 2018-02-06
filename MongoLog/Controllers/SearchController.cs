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
        public async Task<ActionResult> Index(string host = "", string time = "", string date = "", string data = "", string logName = "")
        {
            //var logList = LogService.Instance.GetLogsAsync(host, time, date, data, logName);
            var logContext = new LogContext();

            Expression<Func<Log, bool>> filter = x => true;

            filter = x => (String.IsNullOrEmpty(host) || x.host.Equals(host))
                          && (String.IsNullOrEmpty(time) || x.time.Contains(time))
                          && (String.IsNullOrEmpty(date) || x.date.Contains(date))
                          && (String.IsNullOrEmpty(data) || x.data.Contains(data))
                          && (String.IsNullOrEmpty(logName) || x.logname.Equals(logName));

            var logs = await logContext.Logs.Find(filter)
                .SortByDescending(x => x.date)
                .Limit(500)
                .ToListAsync();

            return View(logs);
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
