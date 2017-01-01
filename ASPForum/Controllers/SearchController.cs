using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using ASPForum.Models;

namespace ASPForum.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult SearchAll(string query)
        {
            var dbPosts = _db.Posts;
            var posts = Search(dbPosts, query);
            if (posts == null)
                return RedirectToAction("Index", "Categories");
            return View("Search", posts.ToList());
        }

        public ActionResult SearchInSubject(string query, int subjectId)
        {
            return PartialView();
        }

        public ActionResult SearchInThread(string query, int threadId)
        {
            return PartialView();
        }

        private static IQueryable<Post> Search(IQueryable<Post> dbPosts, string query)
        {
            var myquery = query.Split(' ');
            switch (myquery.Length)
            {
                case 0:
                    return null;
                case 1:
                    if (myquery[0].ToLower() == "or")
                        return null;
                    var word = myquery[0];
                    if (myquery[0].StartsWith("-"))
                    {
                        word = myquery[0].Substring(1);
                        dbPosts = dbPosts.Where(x => !x.Text.Contains(word));
                        return dbPosts;
                    }

                    dbPosts = dbPosts.Where(x => x.Text.Contains(word));
                    return dbPosts;
                case 2:
                    if (!myquery.Contains("or"))
                    {
                        var finalquery = dbPosts;
                        foreach (var w in myquery)
                            if (w.StartsWith("-"))
                            {
                                var q1 = dbPosts.Where(x => !x.Text.Contains(w.Substring(1)));
                                finalquery = finalquery.Intersect(q1);
                            }
                            else
                            {
                                var q2 = dbPosts.Where(x => x.Text.Contains(w));
                                finalquery = finalquery.Intersect(q2);
                            }
                        return finalquery;
                    }
                    foreach (var w in myquery)
                    {
                        if (w == "or") continue;
                        if (w.StartsWith("-"))
                        {
                            var q = dbPosts.Where(x => !x.Text.Contains(w.Substring(1)));
                            return q;
                        }
                        var q1 = dbPosts.Where(x => x.Text == w);
                        return q1;
                    }
                    break;
                default:
                    var finalquerym = dbPosts;
                    for (var i = 0; i < myquery.Length; i++)
                    {
                        if (myquery[i].ToLower() == "or" && i != 0)
                        {
                            i++;
                            var next = myquery[i];
                            var q = dbPosts.Where(x => x.Text.Contains(next));
                            foreach (var post in q)
                            {
                                Debug.WriteLine(post.Text);
                            }
                            finalquerym = q.Union(finalquerym);
                        }
                        else if (myquery[i].StartsWith("-"))
                        {
                            var ww = myquery[i].Substring(1);
                            var qq = dbPosts.Where(x => x.Text.Contains(ww));
                            finalquerym = finalquerym.Except(qq);
                        }
                        else
                        {
                            var ww = myquery[i];
                            finalquerym = finalquerym.Where(x => x.Text.Contains(ww));
                        }
                    }
                    return finalquerym;

            }
            return null;
        }
    }
}