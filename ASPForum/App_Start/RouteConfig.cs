using System.Web.Mvc;
using System.Web.Routing;

namespace ASPForum
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Categories", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}