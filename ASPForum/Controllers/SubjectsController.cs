using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPForum.Models;

namespace ASPForum.Controllers
{
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subjects
        public ActionResult Index()
        {
            return View(db.Subjects.ToList());
        }

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.CategoryId = id;
            return PartialView("Create");
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Subject subject)
        {
            if (subject == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            db.Subjects.Add(subject);
            db.SaveChanges();
            return RedirectToAction("ManageForum", "Manage", db.Categories.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var subject = db.Subjects.Find(id);
            ViewBag.CategoryId = subject.CategoryId;
            return PartialView("Edit", subject);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Subject subject)
        {
            if (subject == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            db.Entry(subject).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ManageForum", "Manage", db.Categories.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var subject = db.Subjects.Find(id);
            return PartialView("Delete", subject);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
            db.SaveChanges();
            return RedirectToAction("ManageForum", "Manage", db.Categories.ToList());
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
