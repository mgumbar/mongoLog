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
using System.Web.Hosting;

namespace MongoLog.Controllers
{
    public class WorkerController : Controller
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("worker/{state}")]
        public async Task<ActionResult> Index(string state = null)
        {
            var logContext = new LogContext();
            Expression<Func<Worker, bool>> filter = x => true;
            var startDate = DateTime.Now.AddDays(-365).ToString();
            filter = x => ((String.IsNullOrEmpty(startDate) || x.DateTime >= DateTime.Parse(startDate))
                            && (String.IsNullOrEmpty(state) || x.Satus == state));
            var workers = await logContext.Workers.Find(filter)
                .Limit(2000)
                .ToListAsync();
            var workerListed = workers.OrderBy(p => p.DateTime).ToList();
            return View(workerListed);
        }

        // GET: Worker/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Worker/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Worker/Create
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

        // GET: Worker/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Worker/Edit/5
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

        // GET: Worker/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Worker/Delete/5
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
