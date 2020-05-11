using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SiteDesign.Models;

namespace SiteDesign.Controllers
{
    public class RatingsController : Controller
    {
        private Site_DesignDBEntities db = new Site_DesignDBEntities();

        // GET: Ratings
        public ActionResult Index(int? id)
        {
            var ratings = db.Ratings.Where(r => r.TemplateId == id);
            ViewBag.Template = db.Templates.Find(id);
            return PartialView(ratings.ToList());
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }
        // GET: Ratings/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            var rating = new Rating { TemplateId = id.Value, UserId = User.Identity.GetUserId()};
            return View(rating);
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RatingId,UserId,TemplateId,Stars,Comments")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Details", "Templates", new { id = rating.TemplateId });
            }
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.TemplateId = new SelectList(db.Templates, "TemplateId", "UserId", rating.TemplateId);
            ViewBag.UserId = new SelectList(db.SiteUsers, "UserId", "Email", rating.UserId);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RatingId,UserId,TemplateId,Stars,Comments")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TemplateId = new SelectList(db.Templates, "TemplateId", "UserId", rating.TemplateId);
            ViewBag.UserId = new SelectList(db.SiteUsers, "UserId", "Email", rating.UserId);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
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
