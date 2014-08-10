using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommYouNity;
using CommYouNity.Models;
using System.IO;
using System.Net.Mail;

namespace CommYouNity.Controllers
{
    public class MembersController : Controller
    {
        private CommunityDataModel db = new CommunityDataModel();

        // GET: Members
        public ActionResult Index(int? id, string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CommunitySortParm = sortOrder == "Community" ? "community_desc" : "Community";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            var members = db.Members.Include(m => m.Community);
            if (!String.IsNullOrEmpty(searchString)) 
            {
                members = members.Where(m => m.LastName.ToUpper().Contains(searchString.ToUpper()) || m.Phone.ToString().ToUpper().Contains(searchString.ToUpper()) || m.Email.ToUpper().Contains(searchString.ToUpper()) || m.Community.Name.ToUpper().Contains(searchString.ToUpper()) || m.FirstName.ToUpper().Contains(searchString.ToUpper())); 
            }
            switch (sortOrder)
            {
                case "name_desc":
                    members = members.OrderByDescending(m => m.LastName);
                    break;
                case "Community":
                    members = members.OrderBy(m => m.Community.Name);
                    break;
                case "community_desc":
                    members = members.OrderByDescending(m => m.Community.Name);
                    break;
                case "Email":
                    members = members.OrderBy(m => m.Email);
                    break;
                case "email_desc":
                    members = members.OrderByDescending(m => m.Email);
                    break;
                default:
                    members = members.OrderBy(m => m.LastName);
                    break;
           }
         
            MemberTaskView result = new MemberTaskView();
            //result.member = db.Members.Include(c => c.Community).ToList();
            if (id != null)
            {
                result.member = members.Where(i => i.CommunityId == id).ToList();
            }
            else
            {
                result.member = members.ToList();
            } 


            result.memberTask = db.MemberTasks.ToList();
            return View(result);
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member2 = db.Members.Find(id);
            if (member2 == null)
            {
                return HttpNotFound();
            }
            MemberTaskView result = new MemberTaskView();
            result.singleMember = member2;
            result.memberTask = db.MemberTasks.Where(i => i.MemberId == id).ToList();

            return View(result);
            //return View(member);
        }

        // GET: Members/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Phone,Email,Password,NotifyByEmail,NotifyBySMS,AboutMe,CommunityId")] Member member, HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = DateTime.Now.ToFileTimeUtc().ToString() + "_" + fileName;
                    var path = Path.Combine(Server.MapPath("~/img/members"), fileName);
                    file.SaveAs(path);

                    var serverPath = "/img/members/" + fileName;
                    member.ImgSrc = serverPath;
                }
                ViewBag.Message = "Upload successful";
                //return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                member.ImgSrc = "";
                //return RedirectToAction("Uploads");
            }
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name", member.CommunityId);
            return View(member);
        }

        // GET: Members/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name", member.CommunityId);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Phone,Email,Password,NotifyByEmail,NotifyBySMS,AboutMe,CommunityId")] Member member, HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = DateTime.Now.ToFileTimeUtc().ToString() + "_" + fileName;
                    var path = Path.Combine(Server.MapPath("~/img/members"), fileName);
                    file.SaveAs(path);

                    var serverPath = "/img/members/" + fileName;
                    member.ImgSrc = serverPath;
                }
                ViewBag.Message = "Upload successful";
                //return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                member.ImgSrc = "";
                //return RedirectToAction("Uploads");
            }
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name", member.CommunityId);
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SendEmail() {
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
