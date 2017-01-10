using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPForum.Models;
using Microsoft.AspNet.Identity;
using PagedList;
namespace ASPForum.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            ViewBag.PostCount = _db.Posts.Count();
            ViewBag.ThreadCount = _db.Threads.Count();
            ViewBag.UserCount = _db.Users.Count();
            var user = _db.Users.OrderByDescending(x => x.RegistrationDate).FirstOrDefault();
            if (user != null) ViewBag.NewestUser = user.UserName;
            ViewBag.News = _db.News.OrderByDescending(t => t.Date).Take(3).ToList();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var currentUser = _db.Users.Find(userId);
                if (currentUser.LockoutEnabled)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return View("Lockout");
                }
            }

            return View(_db.Categories.ToList());
        }

        [Authorize(Roles = "Admin")]
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
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("ManageForum", "Manage", _db.Categories.ToList());
        }

        [Authorize(Roles = "Admin")]
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = _db.Categories.Find(id);
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

            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("ManageForum", "Manage", _db.Categories.ToList());
        }

        [Authorize(Roles = "Admin")]
        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = _db.Categories.Find(id);
            return PartialView("Delete", category);
        }

        [Authorize(Roles = "Admin")]
        // POST: Categories/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var category = _db.Categories.Find(id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("ManageForum", "Manage", _db.Categories.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Subjects_partial(int id)
        {
            ViewBag.ThreadsCount = _db.Threads.Count(s => s.Subject.Category.Id == id);
            ViewBag.PostCount = _db.Posts.Count(s => s.Thread.Subject.Category.Id == id);
            return PartialView("Subjects_partial", _db.Subjects.Where(s => s.Category.Id == id).ToList());
        }

        public string PostCount(int id)
        {
            return _db.Posts.Count(s => s.Thread.Subject.Id == id).ToString();
        }

        public string ThreadsCount(int id)
        {
            return _db.Threads.Count(s => s.Subject.Id == id).ToString();
        }

        public ActionResult NewPost(int id)
        {
            var post = _db.Posts.Where(t => t.Thread.SubjectId == id).OrderByDescending(t => t.Date).FirstOrDefault();
            var test = _db.Posts.Where(t => t.Thread.SubjectId == id).ToList();
            var pageSize = 10;
            try
            {
                var userid = User.Identity.GetUserId();
                var user = _db.Users.Find(userid);
                if (user.PostsOnPage != 0)
                    pageSize = user.PostsOnPage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
            var paged = test.ToPagedList(1, pageSize);
            ViewBag.Page = paged.PageCount;
            if (post != null)
                return PartialView("LastPost", post);
            return HttpNotFound("Brak postów");
        }
    }
}