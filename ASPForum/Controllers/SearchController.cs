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
            IQueryable<Post> dbPosts = _db.Posts;

            var myquery = query.Split(' ');
            switch (myquery.Length)
            {
                case 0:
                    return RedirectToAction("Index", "Categories");
                case 1:
                    if (myquery[0].ToLower() == "or")
                        return RedirectToAction("Index", "Categories");
                    var word = myquery[0];
                    if (myquery[0].StartsWith("-"))
                    {
                        word = myquery[0].Substring(1);
                        dbPosts = dbPosts.Where(x => !x.Text.Contains(word));
                        return View("Search", dbPosts.ToList());
                    }

                    dbPosts = dbPosts.Where(x => x.Text.Contains(word));
                    return View("Search", dbPosts.ToList());
                case 2:
                    if (!myquery.Contains("or"))
                    {
                        var queries = new List<IQueryable<Post>>();
                        var finalquery = dbPosts;
                        foreach (var w in myquery)
                        {
                            if (w.StartsWith("-"))
                            {
                                var q1 = dbPosts.Where(x => !x.Text.Contains(w.Substring(1)));

                                finalquery = finalquery.Intersect(q1);
                                foreach (var post in finalquery)
                                {
                                    Debug.WriteLine(post.Text);
                                }
                            }
                            else
                            {
                                var q2 = dbPosts.Where(x => x.Text.Contains(w));
                                finalquery = finalquery.Intersect(q2);
                            }
                            
                        }
                        return View("Search", finalquery.ToList());
                    }
                    foreach (var w in myquery)
                    {
                        if (w == "or") continue;
                        if (w.StartsWith("-"))
                        {
                            var q = dbPosts.Where(x => !x.Text.Contains(w.Substring(1)));
                            return View("Search", q.ToList());
                        }
                        var q1 = dbPosts.Where(x => x.Text == w);
                        return View("Search", q1.ToList());
                    }
                    break;
                default:
                    break;
            }

            /*if (!query.ToLower().Contains(" or ") && !query.Contains(" -"))
            {
                var myquery = query.Split(' ');
                if (myquery.Length > 1)
                {
                    for (var i = 0; i < myquery.Length - 1; i++)
                    {
                        var left = myquery[i];
                        var right = myquery[i + 1];
                        dbPosts = dbPosts.Where(x => x.Text.Contains(left) && x.Text.Contains(right));
                    }
                }
                else
                {
                    dbPosts = dbPosts.Where(x => x.Text.Contains(query));
                    return View("Search", dbPosts);
                }
                
                return View("Search", dbPosts);
            }
            var result = query.Split(' ');
            
            for (var i = 0; i < result.Length-1; i++)
            {
                if (result[i].ToLower() == "or" && i != 0)
                {
                    var leftor = result[i - 1];
                    var rightor = result[i + 1];
                    if(leftor.Contains("-") || rightor.StartsWith("-")) continue;
                    dbPosts = dbPosts.Where(x => x.Text.Contains(leftor) || x.Text.Contains(rightor));
                }
                else
                {

                    if (!result[i + 1].StartsWith("-") || result[i+1].ToLower()==" or ")
                    {
                        var left = result[i];
                        var right = result[i + 1];
                        var q = dbPosts.Where(x => x.Text.Contains(left) || x.Text.Contains(right));
                        dbPosts = dbPosts.Union(q);

                    }

                    else if (result[i + 1].StartsWith("-"))
                    {
                        var left = result[i];
                        var right = result[i + 1];
                        var q = dbPosts.Where(x => x.Text.Contains(left) && !x.Text.Contains(right));
                        dbPosts = dbPosts.Union(q);

                    }


                    else if (result[i].StartsWith("-"))
                    {
                        var word = result[i].Substring(1);
                        dbPosts = dbPosts.Where(x => !x.Text.Contains(word));
                    }

                }
            }*/

            return View("Search", dbPosts.ToList());
        }

        public ActionResult SearchInSubject(string query, int subjectId)
        {
            return PartialView();
        }
    }
}