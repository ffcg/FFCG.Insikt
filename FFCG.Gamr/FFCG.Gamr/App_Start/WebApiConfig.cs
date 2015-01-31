using System.Web.Http;

namespace FFCG.Gamr
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("Api1", "battleship/{controller}/{action}");
            config.Routes.MapHttpRoute("Api2", "battleship/{controller}/{action}/{value}", new { id = RouteParameter.Optional }
            );
        }
    }
}
