using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASPForum.Models;
using Microsoft.AspNet.Identity;

namespace ASPForum.Controllers
{
    public class ThreadsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ThreadsSubject(int id)
        {
            var threads = db.Threads.Where(t => t.SubjectId == id).OrderByDescending(t => t.Date).ToList();
            var threadsNotPinned = threads.Where(t => t.IsPinned == false);
            var threadsPinned = threads.Where(t => t.IsPinned);

            var model = new ThreadsWithPinnedViewModel
            {
                UnpinnedThreads = threadsNotPinned,
                PinnedThreads = threadsPinned
            };

            var firstOrDefault = db.Subjects.FirstOrDefault(s => s.Id == id);
            if (firstOrDefault == null) return View(model);
            ViewBag.Title = firstOrDefault.Title;
            ViewBag.CategoryTitle = firstOrDefault.Category.Title;
            return View(model);
        }


        // GET: Threads
        public ActionResult Index()
        {
            return View(db.Threads.ToList());
        }

        // GET: Threads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var thread = db.Threads.Find(id);
            if (thread == null)
                return HttpNotFound();
            return View(thread);
        }

        // GET: Threads/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.SubjectId = id;
            return View();
        }

        // POST: Threads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Content,SubjectId,Date")]Thread thread)
        {
            thread.Date = DateTime.Now;
            thread.UserId = User.Identity.GetUserId();
            var user = db.Users.Find(thread.UserId);
            try
            {
                db.Threads.Add(thread);
                if (user.IfAdminChangedRank == false)
                {
                    user.Rank += 2;
                    db.Entry(user).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("ThreadsSubject", new {id = thread.SubjectId});
            }
            catch (DbEntityValidationException)
            {
                return View(thread);
            }
        }

        // GET: Threads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var thread = db.Threads.Find(id);
            ViewBag.SubjectId = thread.SubjectId;
            ViewBag.UserId = thread.UserId;
            ViewBag.Date = thread.Date;
            return View(thread);
        }

        // POST: Threads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,IsPinned,SubjectId,UserId,Date")]Thread thread)
        {
            db.Entry(thread).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ThreadsSubject", new {id = thread.SubjectId});
        }

        // GET: Threads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var thread = db.Threads.Find(id);
            if (thread == null)
                return HttpNotFound();
            return View(thread);
        }

        // POST: Threads/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var thread = db.Threads.Find(id);
            db.Threads.Remove(thread);
            db.SaveChanges();
            return RedirectToAction("ThreadsSubject", new {id = thread.SubjectId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public string PostCount(int id)
        {
            return db.Posts.Count(s => s.Thread.Id == id).ToString();
        }

        public string ViewCount(int id)
        {
            var thread = db.Threads.FirstOrDefault(x => x.Id == id);
            return thread.ViewCount.ToString();
        }

        public ActionResult LastPost(int id)
        {
            var post = db.Posts.Where(t => t.ThreadId == id).OrderByDescending(t => t.Date).FirstOrDefault();
            if (post != null)
                return PartialView("LastPost", post);
            ViewBag.NoPost = "Brak postów";
            return HttpNotFound("");
        }
    }
}