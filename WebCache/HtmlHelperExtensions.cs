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

			foreach(var asset in Config.Assets)
				sb.AppendFormat("<link rel=\"stylesheet\" href=\"{0}\" />\n", asset.CachedPath);

			return new HtmlString(sb.ToString());
		}
	}
}