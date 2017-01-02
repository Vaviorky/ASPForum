using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASPForum.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ASPForum.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: Posts
        public ActionResult PostThread(int? id, int? page)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var thread = db.Threads.FirstOrDefault(t => t.Id == id);

            thread.ViewCount++;
            db.Entry(thread).State = EntityState.Modified;
            db.SaveChanges();

            var posts = db.Posts.Where(t => t.Thread.Id == id).ToList();
            ViewBag.ThreadTitle = thread.Title;
            ViewBag.Title = thread.Title;
            ViewBag.CategoryTitle = thread.Subject.Category.Title;
            ViewBag.SubjectTitle = thread.Subject.Title;
            var pageSize = 10;
            try
            {
                var userid = User.Identity.GetUserId();
                var user = db.Users.Find(userid);
                if (user.PostsOnPage != 0)
                    pageSize = user.PostsOnPage;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            var pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Thread).Include(p => p.User);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var post = db.Posts.Find(id);
            if (post == null)
                return HttpNotFound();
            return View(post);
        }

        [Authorize]
        // GET: Posts/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var firstOrDefault = db.Threads.FirstOrDefault(t => t.Id == id);
            if (firstOrDefault != null)
                ViewBag.ThreadTitle = firstOrDefault.Title;
            ViewBag.ThreadId = id;
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Text,ThreadId")] Post post)
        {
            post.UserId = User.Identity.GetUserId();
            var user = db.Users.Find(post.UserId);
            post.Date = DateTime.Now;
            if (!ModelState.IsValid) return View(post);
            db.Posts.Add(post);
            if (user.IfAdminChangedRank == false)
            {
                user.Rank++;
                db.Entry(user).State = EntityState.Modified;
            }
            db.SaveChanges();
            var test = db.Posts.Where(t => t.ThreadId == post.ThreadId).ToList();
            var paged = test.ToPagedList(1, user.PostsOnPage);
            return RedirectToAction("PostThread", new { id = post.ThreadId, page = paged.PageCount });
        }

        [Authorize]
        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var post = db.Posts.Find(id);
            ViewBag.ThreadId = post.ThreadId;
            ViewBag.Date = post.Date;
            ViewBag.UserId = post.UserId;
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,Date,UserId,ThreadId")] Post post)
        {
            if (!ModelState.IsValid) return View(post);

            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            var user = db.Users.Find(post.UserId);
            var test = db.Posts.Where(t => t.ThreadId == post.ThreadId).ToList();
            var paged = test.ToPagedList(1, user.PostsOnPage);
            return RedirectToAction("PostThread", new { id = post.ThreadId, page = paged.PageCount });
        }

        [Authorize]
        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var post = db.Posts.Find(id);
            if (post == null)
                return HttpNotFound();
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("PostThread", "Posts", new { id = post.ThreadId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Privillege(string userId)
        {
            var user = db.Users.Find(userId);
            var im = new IdentityManager();
            if (im.isUserInRole(user.Id, "Admin"))
                return Content("/Content/Images/Ranks/admin.png");
            if (user.Privileges == "Moderator")
                return Content("/Content/Images/Ranks/moderator.png");
            var rank = user.Rank;
            if (rank >= 0 && rank <= 25)
                return Content("/Content/Images/Ranks/level1.png");
            if (rank > 25 && rank <= 75)
                return Content("/Content/Images/Ranks/level2.png");
            if (rank > 75 && rank <= 150)
                return Content("/Content/Images/Ranks/level3.png");
            if (rank > 150 && rank <= 250)
                return Content("/Content/Images/Ranks/level4.png");
            if (rank > 250 && rank <= 500)
                return Content("/Content/Images/Ranks/level5.png");
            if (rank > 500 && rank <= 750)
                return Content("/Content/Images/Ranks/level6.png");
            if (rank > 750 && rank <= 1000)
                return Content("/Content/Images/Ranks/level7.png");
            if (rank > 1000 && rank <= 1500)
                return Content("/Content/Images/Ranks/level8.png");
            if (rank > 1500 && rank <= 2000)
                return Content("/Content/Images/Ranks/level9.png");
            //else
            return Content("/Content/Images/Ranks/level10.png");
        }

        public ActionResult PostCount(string id)
        {
            var count = db.Posts.Count(x => x.UserId == id);
            return Content(count.ToString());
        }

        public ActionResult ReportPost(int id, int page)
        {
            var list = new LinkedList<ApplicationUser>();
            var post = db.Posts.Find(id);
            var subjectid = post.Thread.SubjectId;
            var moderators = db.Moderators.Where(x => x.SubjectId == subjectid).ToList();
            foreach (var moderator in moderators)
            {
                list.AddFirst(moderator.User);
            }
            try
            {
                var admins = db.Users.ToList();
                var im = new IdentityManager();
                foreach (var item in admins)
                    if (item != null)
                        if (im.isAdmin(item))
                        {
                            var wtf = item.Privileges;
                            list.AddFirst(item);
                        }
            }
            catch (Exception)
            {
                return HttpNotFound();
            }



            var message = new Message
            {
                Date = DateTime.Now,
                Title = "Zgłoszenie postu",
                Text = "Użytkwonik " + User.Identity.GetUserName() + " zgłosił post do moderacji. </br> <a href=\"/Posts/PostThread/" + post.ThreadId + "#" + id + "\">Sprawdź szczegóły postu</a>"
            };
            db.Messeges.Add(message);
            db.SaveChanges();
            foreach (var item in list)
            {
                var mu = new MessageUser
                {
                    MessageId = message.Id,
                    SenderId = User.Identity.GetUserId(),
                    ReceiverId = item.Id
                };
                db.MessageUser.Add(mu);
                db.SaveChanges();
            }

            return RedirectToAction("PostThread", new { id = post.ThreadId, page });
        }

        public static bool IsUserAdminInThread(string userId, int subjectId)
        {
            var context = new ApplicationDbContext();
            return context.Moderators.Any(x => x.UserId == userId && x.SubjectId == subjectId);
        }
    }
}