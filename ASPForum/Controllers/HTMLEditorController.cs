using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASPForum.Models;
using static System.String;

namespace ASPForum.Controllers
{
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
            catch (Exception)
            {
                // nothing
            }
            return PartialView(options);
        }

        [HttpPost]
        public ActionResult ChangeVaue(IEnumerable<HtmlTagsViewModel> editedoptions)
        {
            var file = ReadTxtFile(AllOptionsInFile());

            foreach (var option in editedoptions)
                if (option.IsEnabled == false)
                    file.Remove(option.Option);
            System.IO.File.WriteAllLines(FilePath(), file.Cast<string>());
            ConvertToConfiguration();
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
            return Path.Combine(Server.MapPath("~/Content"), "allhtmltags.txt");
        }

        private void ConvertToConfiguration()
        {
            var alloptions = ReadTxtFile(AllOptionsInFile());
            var selectedoptions = ReadTxtFile(FilePath());
            var content = "";
            content += "toolbar1: '";
            for (var i = 0; i < 16; i++)
            {
                if (i == 2) content += "| ";
                if (i == 3) content += "| ";
                if (i == 4) content += "| ";
                if (i == 6) content += "| ";
                if (i == 10) content += "| ";
                if (i == 14) content += "| ";

                if (selectedoptions.Contains(alloptions[i]))
                    content += alloptions[i] + " ";
            }
            content += "',\n";
            content += "toolbar2: '";
            for (var i = 16; i < alloptions.Count; i++)
            {
                if (i == 19) content += "| ";
                if (i == 22) content += "| ";

                if (selectedoptions.Contains(alloptions[i]))
                    content += alloptions[i] + " ";
            }
            content += "',";

            System.IO.File.WriteAllText(ConfiguredFile(), content);
        }

        private string ConfiguredFile()
        {
            return Path.Combine(Server.MapPath("~/Content"), "tinymce_conf.txt");
        }

        public ActionResult FileContent()
        {
            string content;
            try
            {
                using (var stream = new StreamReader(ConfiguredFile()))
                {
                    content = stream.ReadToEnd();
                }
            }
            catch (Exception exc)
            {
                return Content("");
            }
            Debug.WriteLine(content);
            return Content(content);
        }
    }
}