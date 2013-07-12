using System;
using System.Collections.Generic;
using System.IO.Compression;
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

			var cachedAsset = WebCacheConfig.Bundles
				.SelectMany(b => b.Assets)
				.SingleOrDefault(a => a.CachedPath == path);

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

			SetHeaders(response);

			response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
			response.ContentType = GetContentType(file.Extension);
			response.TransmitFile(file.FullPath);
		}

		private void SetHeaders(HttpResponse response)
		{
			// https://developers.google.com/speed/docs/best-practices/caching#LeverageBrowserCaching

			var now = DateTime.UtcNow;
			var expires = now.AddYears(1);
			var lastModified = now.AddMonths(-1);

			response.ClearHeaders();
			response.AppendHeader("Content-Encoding", "gzip");
			response.AppendHeader("Vary", "Accept-Encoding");
			response.AppendHeader("Last-Modified", lastModified.ToHttpDate());
			response.AppendHeader("Expires", expires.ToHttpDate());
			response.AppendHeader("Cache-Control", "public");
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