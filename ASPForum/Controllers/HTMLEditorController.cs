using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using ASPForum.Models;

namespace ASPForum.Controllers
{
    public class HtmlEditorController : Controller
    {
        private List<HtmlTagsViewModel> options = new List<HtmlTagsViewModel>();

        // GET: HTMLEditor
        public ActionResult Index()
        {
            try
            {
                var path = AllOptionsInFile();
                var file = System.IO.File.ReadAllLines(path);

                foreach (var line in file)
                {
                    options.Add(new HtmlTagsViewModel
                    {
                        Option = line
                    });
                }

                using (var sr = new StreamReader(FilePath()))
                {
                    var contents = sr.ReadToEnd();
                    foreach (var option in options)
                    {
                        if (contents.Contains(option.Option))
                        {
                            option.IsEnabled = true;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return PartialView(options);
        }
        [HttpPost]
        public ActionResult ChangeVaue(IEnumerable<HtmlTagsViewModel> editedoptions)
        {

            foreach (var option in editedoptions)
            {
                Debug.WriteLine(option.Option + " " + option.IsEnabled);
            }
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
            return Path.Combine(Server.MapPath("~/Content"), "htmltags.txt");
        }

        private string AllOptionsInFile()
        {
            var path = Path.Combine(Server.MapPath("~/Content"), "allhtmltags.txt");
            return path;
        }

        private ICollection<HtmlTagsViewModel> GetInfoFromFile()
        {
            var collection = new LinkedList<HtmlTagsViewModel>();
            var file = ReadTxtFile();

            return collection;
        }
    }
}