using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASPForum.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ASPForum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ForbiddenWordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ForbiddenWords
        public ActionResult Index(int? page)
        {
            ViewBag.Words = ReadTxtFile();
            var pageSize = 10;
            try
            {
                var userid = User.Identity.GetUserId();
                var user = db.Users.Find(userid);
                if (user.PostsOnPage != 0)
                    pageSize = user.PostsOnPage;
            }
            catch (Exception)
            {
                //
            }
            var pageNumber = (page ?? 1);
            var list = ReadTxtFile().Cast<string>().ToList();
            var pagedlist = list.ToPagedList(pageNumber, pageSize);
            return PartialView(pagedlist);
        }
        [HttpPost]
        public ActionResult Create(string newword)
        {
            var words = ReadTxtFile();
            if(words.Contains(newword)) return RedirectToAction("Index");
            words.Add(newword);
            ArrayList.Adapter(words).Sort();
            System.IO.File.WriteAllLines(FilePath(), words.Cast<string>());
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string wordstring, int? page)
        {
            ViewBag.wordEdit = wordstring;
            ViewBag.Page = page;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Edit(string oldword, string newword, int? page)
        {
            var words = ReadTxtFile();
            if (words.Contains(newword)) return RedirectToAction("Index");
            words.Remove(oldword);
            words.Add(newword);
            ArrayList.Adapter(words).Sort();
            System.IO.File.WriteAllLines(FilePath(), words.Cast<string>());
            return RedirectToAction("Index",new {page});
        }

        [HttpPost]
        public ActionResult Delete(string wordstring)
        {
            var words = ReadTxtFile();
            words.Remove(wordstring);
            ArrayList.Adapter(words).Sort();
            System.IO.File.WriteAllLines(FilePath(), words.Cast<string>());
            return RedirectToAction("Index");
        }

        public StringCollection ReadTxtFile()
        {
            var lines = System.IO.File.ReadAllLines(FilePath());

            var wordlist = new StringCollection();

            foreach (var line in lines)
                wordlist.Add(line);

            return wordlist;
        }

        private string FilePath()
        {
            return Path.Combine(Server.MapPath("~/Content"), "forbiddenwords.txt");
        }
    }
}