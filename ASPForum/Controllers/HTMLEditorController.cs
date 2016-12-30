using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASPForum.Models;

namespace ASPForum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HtmlEditorController : Controller
    {
        private readonly List<HtmlTagsViewModel> options = new List<HtmlTagsViewModel>();

        // GET: HTMLEditor
        public ActionResult Index()
        {
            try
            {
                var file = System.IO.File.ReadAllLines(AllOptionsInFile());

                foreach (var line in file)
                    options.Add(new HtmlTagsViewModel
                    {
                        Option = line
                    });

                using (var sr = new StreamReader(FilePath()))
                {
                    var contents = sr.ReadToEnd();

                    foreach (var option in options)
                        if (contents.Contains(option.Option))
                            option.IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return PartialView(options);
        }

        [HttpPost]
        public ActionResult ChangeVaue(IEnumerable<HtmlTagsViewModel> editedoptions)
        {
            var file = ReadTxtFile(AllOptionsInFile());

            foreach (var option in editedoptions)
            {
                if (option.IsEnabled == false)
                {
                    file.Remove(option.Option);
                }
            }
            System.IO.File.WriteAllLines(FilePath(), file.Cast<string>());

            return RedirectToAction("Index");
        }

        public StringCollection ReadTxtFile(string path)
        {
            var lines = System.IO.File.ReadAllLines(path);

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

        private void ConvertToConfiguration(string path)
        {
            
        }
    }
}