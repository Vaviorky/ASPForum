using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPForum.Models;

namespace ASPForum.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult Search(string query)
        {
            var result = query.Split(' ');

            var posts = _db.Posts;
            return RedirectToAction("Index", "Categories");
        }
    }
}