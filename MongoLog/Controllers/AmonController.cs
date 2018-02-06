using MongoDB.Driver;
using MongoLog.Models;
using MongoLog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MongoLog.Controllers
{
    public class AmonController : Controller
    {
        // GET: Amon
        public async Task<ActionResult> Index(string source = "", string startDate = "", string endDate = "", string search = "")
        {
            var logList = AmonService.Instance.GetAmons(source, startDate, endDate);
            var logContext = new LogContext();
            if (String.IsNullOrEmpty(startDate))
                startDate = DateTime.Now.AddDays(-10).ToString();
            if (String.IsNullOrEmpty(endDate))
                endDate = DateTime.Now.ToString();
            Expression<Func<Workflow, bool>> filter = x => true;

            filter = x => (String.IsNullOrEmpty(source) || x.Source.Equals(source))
                          && (String.IsNullOrEmpty(startDate) || x.CreatedAt >= DateTime.Parse(startDate))
                          && (String.IsNullOrEmpty(endDate) || x.CreatedAt <= DateTime.Parse(endDate))
                          && (
                             (String.IsNullOrEmpty(search) || x.Steps.Any(s => s.Label.Contains(search)))
                             || (String.IsNullOrEmpty(search) || x.Steps.Any(s => s.Category.Contains(search)))
                             || (String.IsNullOrEmpty(search) || x.Steps.Any(s => s.SubCategory.Contains(search)))
                             || (String.IsNullOrEmpty(search) || x.Steps.Any(s => s.Status.Contains(search)))
                             || (String.IsNullOrEmpty(search) || x.Payload["filename"].Contains(search))
                          );

            var worklows = await logContext.Workflows.Find(filter)
                .SortByDescending(x => x.CreatedAt)
                .Limit(500)
                .ToListAsync();

            //worklows = await GetWorkflowsAsync(source, startDate, endDate, search);
            //worklows.Select(w => w.Payload.ContainsValue(search)).;
            return View(worklows);
        }

        public async Task<List<Workflow>> GetWorkflowsAsync(string source = "", string startDate = "", string endDate = "", string search = "")
        {
            var logList = AmonService.Instance.GetAmons(source, startDate, endDate);
            var logContext = new LogContext();
            if (String.IsNullOrEmpty(startDate))
                startDate = DateTime.Now.AddDays(-1).ToString();
            if (String.IsNullOrEmpty(endDate))
                endDate = DateTime.Now.ToString();
            Expression<Func<Workflow, bool>> filter = x => true;

            filter = x => (String.IsNullOrEmpty(source) || x.Source.Equals(source))
                          && (String.IsNullOrEmpty(startDate) || x.CreatedAt >= DateTime.Parse(startDate))
                          && (String.IsNullOrEmpty(endDate) || x.CreatedAt <= DateTime.Parse(endDate));

            var worklows = await logContext.Workflows.Find(x => (String.IsNullOrEmpty(search) || x.Payload["filename"].Contains(search)))
                .SortByDescending(x => x.CreatedAt)
                .Limit(500)
                .ToListAsync();
            return worklows;
        }



        // GET: Amon/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Amon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Amon/Create
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

        // GET: Amon/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Amon/Edit/5
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

        // GET: Amon/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Amon/Delete/5
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
