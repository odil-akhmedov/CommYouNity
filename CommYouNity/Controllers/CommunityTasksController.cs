using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommYouNity;

namespace CommYouNity.Controllers
{
    public class CommunityTasksController : Controller
    {
        private CommunityDataModel db = new CommunityDataModel();

        // GET: CommunityTasks
        public ActionResult Index()
        {
            var communityTasks = db.CommunityTasks.Include(c => c.Community);
            return View(communityTasks.ToList());
        }

        // GET: CommunityTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommunityTask communityTask = db.CommunityTasks.Find(id);
            if (communityTask == null)
            {
                return HttpNotFound();
            }
            return View(communityTask);
        }

        // GET: CommunityTasks/Create
        public ActionResult Create()
        {
            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name");
            return View();
        }

        // POST: CommunityTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,EndTime,Description,Budget,Status,Priority,Flag,CommunityId")] CommunityTask communityTask)
        {
            if (ModelState.IsValid)
            {
                db.CommunityTasks.Add(communityTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name", communityTask.CommunityId);
            return View(communityTask);
        }

        // GET: CommunityTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommunityTask communityTask = db.CommunityTasks.Find(id);
            if (communityTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name", communityTask.CommunityId);
            return View(communityTask);
        }

        // POST: CommunityTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartTime,EndTime,Description,Budget,Status,Priority,Flag,CommunityId")] CommunityTask communityTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(communityTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name", communityTask.CommunityId);
            return View(communityTask);
        }

        // GET: CommunityTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommunityTask communityTask = db.CommunityTasks.Find(id);
            if (communityTask == null)
            {
                return HttpNotFound();
            }
            return View(communityTask);
        }

        // POST: CommunityTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommunityTask communityTask = db.CommunityTasks.Find(id);
            db.CommunityTasks.Remove(communityTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
