using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebCache
{
	public static class HtmlHelperExtensions
	{
		public static IHtmlString RenderCss(this HtmlHelper helper)
		{
			var sb = new StringBuilder();

			foreach (var asset in WebCacheConfig.CachedAssets.Where(a => a.File.Extension == ".css"))
				sb.AppendFormat("<link rel=\"stylesheet\" href=\"{0}\" />\n", asset.CachedPath);

			return new HtmlString(sb.ToString());
		}

		public static IHtmlString RenderJs(this HtmlHelper helper)
		{
			var sb = new StringBuilder();

			foreach (var asset in WebCacheConfig.CachedAssets.Where(a => a.File.Extension == ".js"))
				sb.AppendFormat("<script src=\"{0}\" /></script>\n", asset.CachedPath);

			return new HtmlString(sb.ToString());
		}
	}
}