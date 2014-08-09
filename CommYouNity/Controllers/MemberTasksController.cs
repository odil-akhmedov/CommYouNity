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
            var x1 = db.Members;
            var x2 = x1.Where(i => i.Id == memberTask.MemberId);
            var x3 = x2.Select(e => e.Email);
            var fromMemberAddress = x3.FirstOrDefault();

            var x4 = x2.Select(f => f.FirstName);
            var x5 = x2.Select(l => l.LastName);
            
            string fromMemberName = x4.FirstOrDefault() + " " + x5.FirstOrDefault();

            var fromAddress = new MailAddress(fromMemberAddress, fromMemberName);
            string fromPassword = db.Members
                .Where(i => i.Id == memberTask.MemberId)
                .Select(p => p.Password).
                FirstOrDefault()
                .ToString();
            string subject = memberTask.Name;
            string taskType;
            if (memberTask.Flag == true)
                taskType = "task";
            else
                taskType = "alert";

            string body = "Member \"" + fromMemberName + "\" posted new " + taskType + ":\n";
            body += "You can see that on <a href='/location'>";

            var memberCommunityId = x2.Select(c => c.CommunityId).FirstOrDefault();
            var toCommunityEmail = db.Communities.Where(i => i.Id == memberCommunityId).Select(e => e.Email).FirstOrDefault();
            var toCommunityName = db.Communities.Where(i => i.Id == memberCommunityId).Select(n => n.Name).FirstOrDefault();

            //for (int index = 0; index < toMembersEmailList.Count; index++)
            //{
            //    string toEmail = toMembersEmailList[index];
            //    string toName = toNameList[index];
            //    if (toEmail != null)
            //    {

                    var toAddress = new MailAddress(toCommunityEmail, toCommunityName);
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
               //}
            //}
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
