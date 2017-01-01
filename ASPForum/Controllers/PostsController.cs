using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASPForum.Models;
using Microsoft.AspNet.Identity;

namespace ASPForum.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: Posts
        public ActionResult PostThread(int? id)
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
            return View(posts);
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
            return RedirectToAction("PostThread", new {id = post.ThreadId});
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
            if (post == null)
                return HttpNotFound();
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

            return RedirectToAction("PostThread", new {id = post.ThreadId});
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
            return RedirectToAction("PostThread", "Posts", new {id = post.ThreadId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public string Privillege(string userId)
        {
            var user = db.Users.Find(userId);
            var im = new IdentityManager();
            if (im.isUserInRole(user.Id, "Admin"))
                return "<div style=\"color: red; text-align: center;\">Administrator</div>";
            if (user.Privileges == "Moderator")
                return "<div style=\"color: green; text-align: center;\">Moderator</div>";
            var rank = user.Rank;
            if (rank >= 0 && rank <= 20)
                return "<span style: \"color=white;\">Nowy użytkownik</span>";
            if (rank > 20 && rank <= 50)
                return "Bywalec";
            if (rank > 50 && rank <= 100)
                return "Forumowicz";

            return "Nałogowicz";
        }


        public ActionResult ReportPost(int id)
        {
            var list = new LinkedList<ApplicationUser>();

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

            var post = db.Posts.Find(id);

            var message = new Message
            {
                Date = DateTime.Now,
                Title = "Zgłoszenie postu",
                Text = "Użytkwonik " + User.Identity.GetUserName()+ " zgłosił post do moderacji. </br> <a href=\"/Posts/PostThread/" + post.ThreadId + "#" + id +"\">Sprawdź szczegóły postu</a>"
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
            return RedirectToAction("PostThread", new {id = post.ThreadId});
        }

        public static bool IsUserAdminInThread(string userId, int subjectId)
        {
            var context = new ApplicationDbContext();
            return context.Moderators.Any(x => x.UserId == userId && x.SubjectId == subjectId);
        }
    }
}