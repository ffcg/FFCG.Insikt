using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using FFCG.Brun7;

namespace FFCG.Gamr
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        public static List<BingoGame> BingoGames; 

        protected void Application_Start()
        {
            BingoGames = new List<BingoGame>();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}