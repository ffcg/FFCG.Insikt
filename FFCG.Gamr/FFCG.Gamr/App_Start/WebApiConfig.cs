using System.Web.Http;

namespace FFCG.Gamr
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "battleship/{controller}/{id}", new { id = RouteParameter.Optional }
            );
        }
    }
}
