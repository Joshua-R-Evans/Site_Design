using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SiteDesign.Models;

namespace SiteDesign.Controllers
{
    public class TemplatesController : Controller
    {
        private Site_DesignDBEntities db = new Site_DesignDBEntities();

        // GET: Templates
        public ActionResult Index()
        {
            var templates = db.Templates.Include(t => t.SiteUser);
            return View(templates.ToList());
        }
        public ActionResult IndexFiltered(string id)
        {
            if (id == User.Identity.GetUserId()) 
            {
                ViewBag.headerText = "Your ";
            }
            else
            {
                ViewBag.headerText = "Uploaded ";
            }
            var siteUser = db.SiteUsers.Find(id);
            var templates = siteUser.Templates;
            return PartialView("Index", templates.ToList());
        }

        // GET: Templates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = db.Templates.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            var htmlPath = Path.Combine(Server.MapPath(Template.UploadLocation), template.FolderName, template.HtmlFile);
            ViewBag.Html_res = System.IO.File.ReadAllText(htmlPath);
            return View(template);
        }

        // GET: Templates/Create
        [Authorize]
        public ActionResult Create()
        {
            var template = new Template { UserId = User.Identity.GetUserId(), CssFile = "DefaultPlaceholder" };
            return View(template);
        }

        // POST: Templates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Template template, HttpPostedFileBase htmlUpload, HttpPostedFileBase cssUpload, HttpPostedFileBase imgUpload)
        {
            var isValid = ModelState.IsValid;
            if (htmlUpload == null || htmlUpload.ContentLength == 0)
            {
                isValid = false;
                ModelState.AddModelError("HtmlFile", "Html File Required");
            }
            else if(!(htmlUpload.ContentType == "text/html" || htmlUpload.ContentType == "text/plain"))
            {
                isValid = false;
                ModelState.AddModelError("HtmlFile", "Invalid File Type");
            }
            if (cssUpload == null || cssUpload.ContentLength == 0)
            {
                isValid = false;
                ModelState.AddModelError("CssFile", "Css File Required");
            }
            else if (!(cssUpload.ContentType == "text/css" || cssUpload.ContentType == "text/plain"))
            {
                isValid = false;
                ModelState.AddModelError("CssFile", "Invalid File Type");
            }
            if (imgUpload != null && imgUpload.ContentLength > 0 && !imgUpload.ContentType.StartsWith("image/"))
            {
                isValid = false;
                ModelState.AddModelError("Image", "Invalid File Type");
            }
            if(db.Templates.Any(T => T.FolderName == template.FolderName))
            {
                isValid = false;
                ModelState.AddModelError("FolderName", "Invalid Folder Name/Folder Name already exists");
            }
            if (isValid)
            {
                var basePath = Server.MapPath(Template.UploadLocation);
                var zipPath = Path.Combine(basePath, template.FolderName + ".zip");
                var templateFolder = Path.Combine(basePath, template.FolderName);
                Directory.CreateDirectory(templateFolder);
                template.HtmlFile = htmlUpload.FileName;
                template.CssFile = cssUpload.FileName;
                template.Image = imgUpload.FileName;
                var htmlFilePath = Path.Combine(templateFolder, template.HtmlFile);               
                var cssFilePath = Path.Combine(templateFolder, template.CssFile);               
                var imgFilePath = Path.Combine(templateFolder, template.Image);
                htmlUpload.SaveAs(htmlFilePath);
                cssUpload.SaveAs(cssFilePath);
                imgUpload.SaveAs(imgFilePath);
                ZipFile.CreateFromDirectory(templateFolder, zipPath);
                db.Templates.Add(template);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.SiteUsers, "UserId", "Email", template.UserId);
            return View(template);
        }
        // GET: Templates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = db.Templates.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.SiteUsers, "UserId", "Email", template.UserId);
            return View(template);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TemplateId,UserId,FolderName,HtmlFile,CssFile,Image,Description")] Template template)
        {
            if (ModelState.IsValid)
            {
                db.Entry(template).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.SiteUsers, "UserId", "Email", template.UserId);
            return View(template);
        }

        // GET: Templates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = db.Templates.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Template template = db.Templates.Find(id);
            db.Templates.Remove(template);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DownloadFile(string folderName, string fileName)
        {
            string file;
            if (string.IsNullOrEmpty(folderName))
            {
                file = Path.Combine(Server.MapPath(Template.UploadLocation), fileName);
            }
            else
            {
                file = Path.Combine(Server.MapPath(Template.UploadLocation), folderName, fileName);
            }
            var fileBytes = System.IO.File.ReadAllBytes(file);
           
            
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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
