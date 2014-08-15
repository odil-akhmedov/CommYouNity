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
using PagedList;
using PagedList.Mvc;

namespace CommYouNity.Controllers
{
    public class CommunitiesController : Controller
    {
        private CommunityDataModel db = new CommunityDataModel();

        // GET: Communities
        public ActionResult Index(int? id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.SearchString = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LocationSortParm = sortOrder == "Location" ? "location_desc" : "Location";
            ViewBag.OfficerNameSortParm = sortOrder == "OfficerName" ? "officerName_desc" : "OfficerName";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;
            var communities = db.Communities.Include(c => c.Location);
            if (!String.IsNullOrEmpty(searchString)) 
                { 
                    communities = communities.Where(c => c.Name.ToUpper().Contains(searchString.ToUpper()) || c.Location.Name.ToUpper().Contains(searchString.ToUpper()) || c.OfficerName.ToUpper().Contains(searchString.ToUpper()) || c.Email.ToUpper().Contains(searchString.ToUpper())); 
                }
            switch (sortOrder)
            {
                case "name_desc":
                    communities = communities.OrderByDescending(c => c.Name);
                    break;
                case "Location":
                    communities = communities.OrderBy(c => c.Location.Name);
                    break;
                case "location_desc":
                    communities = communities.OrderByDescending(c => c.Location.Name);
                    break;
                case "OfficerName":
                    communities = communities.OrderBy(c => c.OfficerName);
                    break;
                case "officerName_desc":
                    communities = communities.OrderByDescending(c => c.OfficerName);
                    break;
                case "Email":
                    communities = communities.OrderBy(c => c.Email);
                    break;
                case "email_desc":
                    communities = communities.OrderByDescending(c => c.Email);
                    break;
                default:
                    communities = communities.OrderBy(c => c.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            CommunityTaskView result = new CommunityTaskView();
            result.communityTask = db.CommunityTasks.ToList();
            if (id != null)
            {
                result.community = communities.Where(i => i.LocationId == id).ToList();
            }
            else 
            {
                result.community = communities.ToList();
            }

            result.pagedCommunity = communities.ToPagedList(pageNumber, pageSize);
            return View(result);
          
            //return View(communities.ToList());
        }
        // GET: Communities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Community community2 = db.Communities.Find(id);
            if (community2 == null)
            {
                return HttpNotFound();
            }
            CommunityTaskView result = new CommunityTaskView();
            result.singleCommunity = community2;
            result.communityTask = db.CommunityTasks.Where(i => i.CommunityId == id).ToList();

            return View(result);
            
            //return View(community);
        }

        // GET: Communities/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            return View();
        }

        // POST: Communities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,OfficerName,Email,Password,LocationId")] Community community, IEnumerable<HttpPostedFileBase> files)
        {
            var imgFiles = files.ToList();
            string imgSrc = "";
            foreach (var file in imgFiles)
            {
                try
                {
                    if (file.ContentLength > 0 && file.FileName != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = DateTime.Now.ToFileTimeUtc().ToString() + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/img/communities"), fileName);
                        file.SaveAs(path);

                        var serverPath = "/img/communities/" + fileName;
                        imgSrc += serverPath + ";";
                        community.ImgSrc = imgSrc;
                    }
                    ViewBag.Message = "Upload successful";
                    //return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "Upload failed";
                    //location.ImgSrc = "";
                    //return RedirectToAction("Uploads");
                }
            }
            if (ModelState.IsValid)
            {
                db.Communities.Add(community);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", community.LocationId);
            return View(community);
        }

        // GET: Communities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Community community = db.Communities.Find(id);
            if (community == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", community.LocationId);
            return View(community);
        }

        // POST: Communities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,OfficerName,Email,Password,LocationId")] Community community, IEnumerable<HttpPostedFileBase> files)
        {

            var imgFiles = files.ToList();
            string imgSrc = "";
            foreach (var file in imgFiles)
            {
                try
                {
                    if (file.ContentLength > 0 && file.FileName != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = DateTime.Now.ToFileTimeUtc().ToString() + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/img/communities"), fileName);
                        file.SaveAs(path);

                        var serverPath = "/img/communities/" + fileName;
                        imgSrc += serverPath + ";";
                        community.ImgSrc = imgSrc;
                    }
                    ViewBag.Message = "Upload successful";
                    //return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "Upload failed";
                    //location.ImgSrc = "";
                    //return RedirectToAction("Uploads");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(community).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", community.LocationId);
            return View(community);
        }

        // GET: Communities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Community community = db.Communities.Find(id);
            if (community == null)
            {
                return HttpNotFound();
            }
            return View(community);
        }

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Community community = db.Communities.Find(id);
            db.Communities.Remove(community);
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
