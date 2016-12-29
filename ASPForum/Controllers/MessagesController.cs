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
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            var messages = db.MessageUser.ToList();
            var orderByDescending = messages.OrderByDescending(x => x.Message.Date);
            return View(orderByDescending);
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var message = db.Messeges.Find(id);
            var messageusers = db.MessageUser.First(x => x.MessageId == id);

            ViewBag.MessageInfo = messageusers;

            if (message == null)
                return HttpNotFound();
            return PartialView("Details", messageusers);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                message.Date = DateTime.Now;
                db.Messeges.Add(message);
                db.SaveChanges();

                var username = Request["toName"];
                try
                {
                    var user = db.Users.First(x => x.UserName == username);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("error", e.Message);
                    return PartialView("Create", message);
                }

                var mu = new MessageUser
                {
                    MessageId = message.Id,
                    SenderId = User.Identity.GetUserId(),
                    ReceiverId = db.Users.FirstOrDefault(x=> x.UserName == username).Id
                };
                db.MessageUser.Add(mu);
                db.SaveChanges();
                
                return RedirectToAction("Inbox", "Manage");
            }

            return PartialView("Create", message);
        }


        public ActionResult Reply(int id, string user)
        {
            var message = db.Messeges.Find(id);
            var reply = new Message()
            {
                Title = message.Title
            };
            
            var receiver = db.Users.First(x => x.Id == user);
            var msg = new MessageViewModel
            {
                Message = reply,
                User = receiver
            };
            return PartialView("Reply", msg);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var message = db.Messeges.Find(id);
            if (message == null)
                return HttpNotFound();
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var message = db.Messeges.Find(id);
            db.Messeges.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult NewMessageFriends()
        {
            var id = User.Identity.GetUserId();
            var q = db.Friends.Where(f => f.User.Id == id).ToList();
            return PartialView("NewMessageFriends", q);
        }

        public void IfViewd(int? id)
        {
            var idUser = User.Identity.GetUserId();
            var message = db.Messeges.Find(id);

            if (db.MessageUser.FirstOrDefault(mu => mu.MessageId == id).ReceiverId == idUser)
            {
                message.IsRead = true;
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public int Unread()
        {
            var id = User.Identity.GetUserId();
            var messageId = db.MessageUser.Where(mu => mu.ReceiverId == id).ToList();
            var list = new LinkedList<Message>();

            foreach (var item in messageId)
            {
                var ii = db.Messeges.FirstOrDefault(m => m.Id == item.MessageId && m.IsRead == false);
                if (ii != null)
                    list.AddFirst(ii);
            }
            var alejaa = list.Count();
            return list.Count();
        }

        public static int Unread2(string idx)
        {
            var db = new ApplicationDbContext();
            var messageId = db.MessageUser.Where(mu => mu.ReceiverId == idx).ToList();
            var list = new LinkedList<Message>();
            foreach (var item in messageId)
            {
                var ii = db.Messeges.FirstOrDefault(m => m.Id == item.MessageId && m.IsRead == false);
                if (ii != null)
                    list.AddFirst(ii);
            }

            return list.Count();
        }

        public void MarkAsRead(int id)
        {
            var m = db.Messeges.FirstOrDefault(x => x.Id == id);
            if (m.IsRead == false)
            {
                m.IsRead = true;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}