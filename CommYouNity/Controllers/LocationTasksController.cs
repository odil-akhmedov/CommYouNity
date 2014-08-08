using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommYouNity;
using System.Net.Mail;

namespace CommYouNity.Controllers
{
    public class LocationTasksController : Controller
    {
        private CommunityDataModel db = new CommunityDataModel();

        // GET: LocationTasks
        public ActionResult Index()
        {
            var locationTasks = db.LocationTasks.Include(l => l.Location);
            return View(locationTasks.ToList());
        }

        // GET: LocationTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationTask locationTask = db.LocationTasks.Find(id);
            if (locationTask == null)
            {
                return HttpNotFound();
            }
            return View(locationTask);
        }

        // GET: LocationTasks/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            return View();
        }

        // POST: LocationTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,EndTime,Description,Budget,Status,Priority,Flag,LocationId")] LocationTask locationTask)
        {
            var fromAddress = new MailAddress("akhmedoff.odil@gmail.com", "From Name");
            var toAddress = new MailAddress("akhmedoff.o.k@gmail.com", "To Name");
            const string fromPassword = "paramaribo";
            const string subject = "Subject";
            const string body = "CommYouNity";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            if (ModelState.IsValid)
            {
                db.LocationTasks.Add(locationTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", locationTask.LocationId);
            return View(locationTask);
        }

        // GET: LocationTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationTask locationTask = db.LocationTasks.Find(id);
            if (locationTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", locationTask.LocationId);
            return View(locationTask);
        }

        // POST: LocationTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartTime,EndTime,Description,Budget,Status,Priority,Flag,LocationId")] LocationTask locationTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locationTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", locationTask.LocationId);
            return View(locationTask);
        }

        // GET: LocationTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationTask locationTask = db.LocationTasks.Find(id);
            if (locationTask == null)
            {
                return HttpNotFound();
            }
            return View(locationTask);
        }

        // POST: LocationTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LocationTask locationTask = db.LocationTasks.Find(id);
            db.LocationTasks.Remove(locationTask);
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
