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
using PagedList;
using PagedList.Mvc;

namespace CommYouNity.Controllers
{
    public class LocationsController : Controller
    {
        private CommunityDataModel db = new CommunityDataModel();

        // GET: Locations
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ZipSortParm = sortOrder == "Zip" ? "zip_desc" : "Zip";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;
            var locations = from l in db.Locations
                            select l;
            //var communities = db.Communities.Include(c => c.Location);
            if (!String.IsNullOrEmpty(searchString)) 
            { 
                locations = locations.Where(l => l.Name.ToUpper().Contains(searchString.ToUpper()) || l.Zip.ToString().Contains(searchString.ToUpper()) || l.Email.ToUpper().Contains(searchString.ToUpper())); 
            }
            switch (sortOrder)
            {
                case "name_desc":
                    locations = locations.OrderByDescending(l => l.Name);
                    break;
                case "Zip":
                    locations = locations.OrderBy(l => l.Zip);
                    break;
                case "zip_desc":
                    locations = locations.OrderByDescending(l => l.Zip);
                    break;
                case "Email":
                    locations = locations.OrderBy(l => l.Email);
                    break;
                case "email_desc":
                    locations = locations.OrderByDescending(l => l.Email);
                    break;
                default:
                    locations = locations.OrderBy(l => l.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            LocationTaskView result = new LocationTaskView();
            result.location = locations.ToList();
            result.pagedLocation = locations.ToPagedList(pageNumber, pageSize);
            result.locationTask = db.LocationTasks.ToList();
            //var result2 = db.Locations.Include(i => i.LocationTasks);
            return View(result);
            //return View(db.Locations.ToList());
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Location location2 = db.Locations.Find(id);
            if (location2 == null)
            {
                return HttpNotFound();
            }

            LocationTaskView result = new LocationTaskView();
            result.singleLocation = location2;
            result.locationTask = db.LocationTasks.Where(i => i.LocationId == id).ToList();
   
            return View(result);
            //return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Zip,Email,Password,GoogleMap")] Location location, IEnumerable<HttpPostedFileBase> files)
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
                        var path = Path.Combine(Server.MapPath("~/img/locations"), fileName);
                        file.SaveAs(path);

                        var serverPath = "/img/locations/" + fileName;
                        imgSrc += serverPath + ";";
                        location.ImgSrc = imgSrc;
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
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }
        


        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Zip,Email,Password,GoogleMap")] Location location, IEnumerable<HttpPostedFileBase> files)
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
                        var path = Path.Combine(Server.MapPath("~/img/locations"), fileName);
                        file.SaveAs(path);

                        var serverPath = "/img/locations/" + fileName;
                        imgSrc += serverPath + ";";
                        location.ImgSrc = imgSrc;
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
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }
        [HttpPost]
        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Persist the files uploaded.
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                    file.SaveAs(path);
                }
                ViewBag.Message = "Upload successful";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Uploads");
            }
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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
