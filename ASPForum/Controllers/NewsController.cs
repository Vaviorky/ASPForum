using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASPForum.Models;
using Microsoft.AspNet.Identity;

namespace ASPForum.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.News.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var news = db.News.Find(id);
            if (news == null)
                return HttpNotFound();
            return View(news);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(News news)
        {
            news.UserId = User.Identity.GetUserId();
            news.Date = DateTime.Now;
            db.News.Add(news);
            db.SaveChanges();
            return RedirectToAction("ManageNews", "Manage", db.News.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var news = db.News.Find(id);
            return PartialView("Edit", news);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(News news)
        {
            if (news == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            news.Date = DateTime.Now;
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ManageNews", "Manage", db.News.ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var news = db.News.Find(id);
            return PartialView("Delete", news);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("ManageNews", "Manage", db.News.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}