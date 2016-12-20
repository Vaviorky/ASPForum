using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ASPForum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace ASPForum.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            ViewBag.PostCount = db.Posts.Count();
            ViewBag.ThreadCount = db.Threads.Count();
            ViewBag.UserCount = db.Users.Count();
            var user = db.Users.OrderByDescending(x => x.RegistrationDate).FirstOrDefault();
            if (user != null) ViewBag.NewestUser = user.UserName;
            ViewBag.News =  db.News.OrderByDescending(t => t.Date).Take(3).ToList();
            
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var currentUser = db.Users.Find(userId);
                if (currentUser.LockoutEnabled)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return View("Lockout");
                }
            }

            return View(db.Categories.ToList());
        }

        [Authorize(Roles ="Admin")]
        // GET: Categories/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (category == null)
                return HttpNotFound();
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("ManageForum","Manage", db.Categories.ToList());
        }
        [Authorize(Roles = "Admin")]
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = db.Categories.Find(id);
            return PartialView("Edit", category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (category == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ManageForum", "Manage", db.Categories.ToList());

        }
        [Authorize(Roles = "Admin")]
        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = db.Categories.Find(id);
            return PartialView("Delete", category);
        }
        [Authorize(Roles = "Admin")]
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var category = db.Categories.Find(id);
            db.Categories.Remove(category);
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

        public ActionResult Subjects_partial(int id)
        {
            ViewBag.ThreadsCount = db.Threads.Count(s => s.Subject.Category.Id == id);
            ViewBag.PostCount = db.Posts.Count(s => s.Thread.Subject.Category.Id == id);
            return PartialView("Subjects_partial", db.Subjects.Where(s => s.Category.Id == id).ToList());
        }

        public string PostCount(int id)
        {
            return db.Posts.Count(s => s.Thread.Subject.Id == id).ToString();
        }
        public string ThreadsCount(int id)
        {
            return db.Threads.Count(s => s.Subject.Id == id).ToString();
        }
        public ActionResult NewPost(int id)
        {
            var post = db.Posts.Where(t => t.Thread.SubjectId == id).OrderByDescending(t => t.Date).FirstOrDefault();
            if (post != null)
            {
                return PartialView("LastPost", post);
            }
            else
                return HttpNotFound("Brak postów");
            
        }
    }
}
