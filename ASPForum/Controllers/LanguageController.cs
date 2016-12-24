using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ASPForum.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult Change(string language)
        {
            if (language != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }
            var cookie = new HttpCookie("Language");
            cookie.Value = language;
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}