using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WebCache
{
	public class WebCacheConfig
	{
		public static List<CachedAsset> CachedAssets = new List<CachedAsset>();

		public static void Add(string virtualPath)
		{
			CachedAssets.Add(new CachedAsset(virtualPath));
		}
	}
}