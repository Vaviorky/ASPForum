using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ASPForum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ForbiddenWordsController : Controller
    {
        // GET: ForbiddenWords
        public ActionResult Index()
        {
            ViewBag.Words = ReadTxtFile();
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(string newword)
        {
            var words = ReadTxtFile();
            words.Add(newword);
            ArrayList.Adapter(words).Sort();
            System.IO.File.WriteAllLines(FilePath(), words.Cast<string>());
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string wordstring)
        {
            ViewBag.wordEdit = wordstring;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Edit(string oldword, string newword)
        {
            var words = ReadTxtFile();
            words.Remove(oldword);
            words.Add(newword);
            ArrayList.Adapter(words).Sort();
            System.IO.File.WriteAllLines(FilePath(), words.Cast<string>());
            return RedirectToAction("Index");
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