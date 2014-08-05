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
    public class MemberTasksController : Controller
    {
        private CommunityDataModel db = new CommunityDataModel();

        // GET: MemberTasks
        public ActionResult Index()
        {
            var memberTasks = db.MemberTasks.Include(m => m.Member);
            return View(memberTasks.ToList());
        }

        // GET: MemberTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberTask memberTask = db.MemberTasks.Find(id);
            if (memberTask == null)
            {
                return HttpNotFound();
            }
            return View(memberTask);
        }

        // GET: MemberTasks/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName");
            return View();
        }

        // POST: MemberTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,EndTime,Description,Budget,Status,Priority,Flag,MemberId")] MemberTask memberTask)
        {
            if (ModelState.IsValid)
            {
                db.MemberTasks.Add(memberTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName", memberTask.MemberId);
            return View(memberTask);
        }

        // GET: MemberTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberTask memberTask = db.MemberTasks.Find(id);
            if (memberTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName", memberTask.MemberId);
            return View(memberTask);
        }

        // POST: MemberTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartTime,EndTime,Description,Budget,Status,Priority,Flag,MemberId")] MemberTask memberTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName", memberTask.MemberId);
            return View(memberTask);
        }

        // GET: MemberTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberTask memberTask = db.MemberTasks.Find(id);
            if (memberTask == null)
            {
                return HttpNotFound();
            }
            return View(memberTask);
        }

        // POST: MemberTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberTask memberTask = db.MemberTasks.Find(id);
            db.MemberTasks.Remove(memberTask);
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
