using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommYouNity.Controllers
{
    public class HomeController : Controller
    {
       //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Community Reimagined";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us";

            return View();
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            string cookie = "There is no cookie!";
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Cookie"))
            {
                cookie = "Yeah - Cookie: " + this.ControllerContext.HttpContext.Request.Cookies["Cookie"].Value;
            }
            ViewData["Cookie"] = cookie;
            return View();
        }
        public string CalId()
        {
            CommYouNity.Models._Default.GCalendar cal = new CommYouNity.Models._Default.GCalendar
           ("Google calendar name", "Google account name", "Google account password");
            return cal.CalId();
        }  
    }
}