﻿using System;
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
        public ActionResult Create(int? id)
        {
            ViewBag.CommunityId = new SelectList(db.Communities.Where(i => i.Id == id), "Id", "Name");
            return View();
        }

        // POST: CommunityTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,EndTime,Description,Budget,Status,Priority,Flag,CommunityId")] CommunityTask communityTask)
        {
            var x1 = db.Communities;
            var x2 = x1.Where(i => i.Id == communityTask.CommunityId);
            var x3 = x2.Select(e => e.Email);
            var fromCommunityAddress = x3.FirstOrDefault();

            var x4 = x2.Select(n => n.Name);
            var fromCommunityName = x4.FirstOrDefault();

            var x5 = db.Members;
            var x6 = db.Communities;

            var fromAddress = new MailAddress(fromCommunityAddress, fromCommunityName);
            string fromPassword = db.Locations
                .Where(i => i.Id == communityTask.CommunityId)
                .Select(p => p.Password).
                FirstOrDefault()
                .ToString();
            string subject = communityTask.Name;
            string taskType;
            if (communityTask.Flag == true)
                taskType = "task";
            else
                taskType = "alert";

            string body = "Location \"" + fromCommunityName + "\" posted new " + taskType + ":\n";
            body += "You can see that on <a href='/location'>";
            var toMembersEmailList = db.Members.Where(i => i.CommunityId == communityTask.CommunityId).Select(e => e.Email).ToList();
            var toNameList = db.Members.Where(i => i.CommunityId == communityTask.CommunityId).Select(n => n.FullName).ToList();

            for (int index = 0; index < toMembersEmailList.Count; index++)
            {
                string toEmail = toMembersEmailList[index];
                string toName = toNameList[index];
                if (toEmail != null)
                {

                    var toAddress = new MailAddress(toEmail, toName);
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
                }
            }
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
            var x1 = db.Communities;
            var x2 = x1.Where(i => i.Id == communityTask.CommunityId);
            var x3 = x2.Select(e => e.Email);
            var fromCommunityAddress = x3.FirstOrDefault();

            var x4 = x2.Select(n => n.Name);
            var fromCommunityName = x4.FirstOrDefault();

            var x5 = db.Members;
            var x6 = db.Communities;

            var fromAddress = new MailAddress(fromCommunityAddress, fromCommunityName);
            string fromPassword = db.Locations
                .Where(i => i.Id == communityTask.CommunityId)
                .Select(p => p.Password).
                FirstOrDefault()
                .ToString();
            string subject = communityTask.Name;
            string taskType;
            if (communityTask.Flag == true)
                taskType = "task";
            else
                taskType = "alert";

            string body = "Location \"" + fromCommunityName + "\" posted new " + taskType + ":\n";
            body += "You can see that on <a href='/location'>";
            var toMembersEmailList = db.Members.Where(i => i.CommunityId == communityTask.CommunityId).Select(e => e.Email).ToList();
            var toNameList = db.Members.Where(i => i.CommunityId == communityTask.CommunityId).Select(n => n.FullName).ToList();

            for (int index = 0; index < toMembersEmailList.Count; index++)
            {
                string toEmail = toMembersEmailList[index];
                string toName = toNameList[index];
                if (toEmail != null)
                {

                    var toAddress = new MailAddress(toEmail, toName);
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
                }
            }
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
