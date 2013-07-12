using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebCache
{
	public class WebCacheHttpHandler : IHttpHandler
	{
		public bool IsReusable { get { return false; } }

		public void ProcessRequest(HttpContext context)
		{
			var response = context.Response;
			var path = context.Request.Path;

			var cachedAsset = WebCacheConfig.CachedAssets.SingleOrDefault(a => a.CachedPath == path);

			if (cachedAsset == null)
			{
				response.StatusCode = (int)HttpStatusCode.NotFound;
				response.End();
				return;
			}

			var file = cachedAsset.File;

			// TODO: recreate file?
			if (!cachedAsset.File.Exists)
			{
				response.StatusCode = (int)HttpStatusCode.NotFound;
				response.End();
				return;
			}

			response.ContentType = GetContentType(file.Extension);
			response.WriteFile(file.FullPath);
		}

		private string GetContentType(string fileExtension)
		{
			switch (fileExtension)
			{
				case ".css":
					return "text/css";

				// http://stackoverflow.com/questions/9664282/difference-between-application-x-javascript-and-text-javascript-content-types
				case ".js":
					return "application/javascript";

				default:
					return "application/unknown";
			}
		}
	}
}