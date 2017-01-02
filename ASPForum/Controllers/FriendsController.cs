using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPForum.Models;
using Microsoft.AspNet.Identity;

namespace ASPForum.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Friends
        public ActionResult Index()
        {
            return View(db.Friends.ToList());
        }

        // GET: Friends/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return PartialView("Details", users);
        }

        // GET: Friends/Create
        public ActionResult Add()
        {
            return View(db.Users.ToList());
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult UserList()
        {
            var name = Request["FriendName"];
            var userList = db.Users.ToList();
            var searchResult = new List<ApplicationUser>();

            if (name != null)
            {
                userList.Remove(db.Users.Find(User.Identity.GetUserId()));
                foreach (var iter in userList)
                {
                    if (iter.UserName.ToLower().Contains(name.ToLower()))
                    {
                        searchResult.Add(iter);
                    }
                }
            }
            return PartialView("UserList", searchResult);
        }


        // GET: Friends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friends friends = db.Friends.Find(id);
            if (friends == null)
            {
                return HttpNotFound();
            }
            return View(friends);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] Friends friends)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friends).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(friends);
        }

        // GET: Friends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friends friends = db.Friends.Find(id);
            if (friends == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete",friends);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var friends = db.Friends.Find(id);
            db.Friends.Remove(friends);
            db.SaveChanges();
            return RedirectToAction("Inbox", "Manage");
        }
        public ActionResult AddFriend(string id)
        {
            var idd = User.Identity.GetUserId();
            var friend = new Friends
            {
                User = db.Users.Find(idd),
                Friend = db.Users.Find(id)
            };
            if(db.Friends.FirstOrDefault(f=>f.Friend.UserName==friend.Friend.UserName && f.User.UserName == friend.User.UserName) != null)
            {
                return RedirectToAction("Inbox", "Manage");
            }
            db.Friends.Add(friend);
            db.SaveChanges();
            return RedirectToAction("Inbox", "Manage");
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
