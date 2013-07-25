// HttpHandler to look for cached assets, and if they exist, send them
// compressed with long-lived HTTP cache headers.
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

			var cachedAsset = WebCache.Bundles
				.SelectMany(b => b.Value)
				.SingleOrDefault(a => a.CachedVirtualPath == path);

			if (cachedAsset == null)
			{
				response.StatusCode = (int)HttpStatusCode.NotFound;
				response.End();
				return;
			}

			var file = cachedAsset.File;

			if (!cachedAsset.File.Exists)
			{
				response.StatusCode = (int)HttpStatusCode.NotFound;
				response.End();
				return;
			}

			SetHeaders(response);
			response.ContentType = GetContentType(file.Extension);
			// TODO: [Add support for content negotiation](https://github.com/stajs/WebCache/issues/10)
			response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
			// `TransmitFile()` is awesome and doesn't buffer the file in memory. This is the reason cached assets are written to disk.
			response.TransmitFile(file.FullPath);
		}

		// Based on [Google-suggested best practice](https://developers.google.com/speed/docs/best-practices/caching#LeverageBrowserCaching).
		private void SetHeaders(HttpResponse response)
		{
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

				// There are [two other obsoleted content types for JavaScript](http://stackoverflow.com/a/9664327).
				case ".js":					
					return "application/javascript"; 

				default:
					return "application/unknown";
			}
		}
	}
}