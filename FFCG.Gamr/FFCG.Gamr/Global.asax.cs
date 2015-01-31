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

        public static BingoGame BingoGame; 

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}