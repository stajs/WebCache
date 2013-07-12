using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebCache.TestSite
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			WebCacheConfig.Add("/assets/styles/main.min.css");
			WebCacheConfig.Add("/assets/scripts/log.js");

			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
	}
}