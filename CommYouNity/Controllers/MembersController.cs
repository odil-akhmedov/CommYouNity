﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommYouNity;
using CommYouNity.Models;

namespace CommYouNity.Controllers
{
    public class MembersController : Controller
    {
        private CommunityDataModel db = new CommunityDataModel();

        // GET: Members
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.Community);
            MemberTaskView result = new MemberTaskView();
            result.member = db.Members.Include(c => c.Community).ToList();
            result.memberTask = db.MemberTasks.ToList();
            return View(result);

            //var members = db.Members.Include(m => m.Community);
            //return View(members.ToList());
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
        public ActionResult Create()
        {
            ViewBag.CommunityId = new SelectList(db.Communities, "Id", "Name");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Phone,AboutMe,ImgSrc,CommunityId")] Member member)
        {
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
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Phone,AboutMe,ImgSrc,CommunityId")] Member member)
        {
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
