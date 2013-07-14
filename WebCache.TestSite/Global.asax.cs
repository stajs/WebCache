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
			WebCache.Register(
				new Bundle
				{
					Name = "Header",
					Assets = new List<Asset>
					{
						new Asset("/assets/styles/main.min.css"),
						new Asset("/assets/styles/app.min.css"),
						new Asset("/assets/scripts/header.js")
					}
				},
				new Bundle
				{
					Name = "Body",
					Assets = new List<Asset>
					{
						new Asset("/assets/scripts/log.js")
					}
				}
			);

			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
	}
}