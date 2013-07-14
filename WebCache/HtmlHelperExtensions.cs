﻿using System;
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
		public static IHtmlString RenderBundle(this HtmlHelper helper, string name)
		{
			if (!WebCache.Bundles.ContainsKey(name))
				throw new KeyNotFoundException("Bundle not found: " + name);

			var bundle = WebCache.Bundles[name];
			
			if (bundle == null)
				throw new NullReferenceException("Bundle is null: " + name);

			var sb = new StringBuilder();

			foreach (var asset in bundle.Where(a => a.File.Extension == ".css"))
				sb.AppendFormat("<link rel=\"stylesheet\" href=\"{0}\" />\n", asset.CachedPath);

			foreach (var asset in bundle.Where(a => a.File.Extension == ".js"))
				sb.AppendFormat("<script src=\"{0}\" /></script>\n", asset.CachedPath);

			return new HtmlString(sb.ToString());
		}
	}
}