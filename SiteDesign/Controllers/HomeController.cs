using Microsoft.AspNet.Identity;
using SiteDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteDesign.Controllers
{
    public class HomeController : Controller
    {
        private Site_DesignDBEntities db = new Site_DesignDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult BuildSite()
        {
            return View();
        }
        public ActionResult Privacy()
        {
            return View();
        }
        public ActionResult Templates()
        {
            return RedirectToAction("Index", "Templates");
        }
        [Authorize]
        public ActionResult UserProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = User.Identity.GetUserId();
            }
            var siteUser = db.SiteUsers.Find(id);
            return View(siteUser);
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