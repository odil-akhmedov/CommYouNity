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
    public class LocationsController : Controller
    {
        private CommunityDataModel db = new CommunityDataModel();

        // GET: Locations
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var locations = from l in db.Locations
                            select l;
            switch (sortOrder)
            {
                case "name_desc":
                    locations = locations.OrderByDescending(l => l.Name);
                    break;
                default:
                    locations = locations.OrderBy(l => l.Name);
                    break;
            }
            LocationTaskView result = new LocationTaskView();
            result.location = locations.ToList();
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
            var file = files.ToList()[0];
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = DateTime.Now.ToFileTimeUtc().ToString() + "_" + fileName;
                    var path = Path.Combine(Server.MapPath("~/img/locations"), fileName);
                    file.SaveAs(path);

                    var serverPath = "/img/locations/" + fileName;
                    location.ImgSrc = serverPath;
                }
                ViewBag.Message = "Upload successful";
                //return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                location.ImgSrc = "";
                //return RedirectToAction("Uploads");
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
        public ActionResult Edit([Bind(Include = "Id,Name,Zip,Email,Password,GoogleMap")] Location location, HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = DateTime.Now.ToFileTimeUtc().ToString() + "_" + fileName;
                    var path = Path.Combine(Server.MapPath("~/img/locations"), fileName);
                    file.SaveAs(path);

                    var serverPath = "/img/locations/" + fileName;
                    location.ImgSrc = serverPath;
                }
                ViewBag.Message = "Upload successful";
                //return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                location.ImgSrc = "";
                //return RedirectToAction("Uploads");
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
