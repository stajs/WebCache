using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WebCache
{
	public class WebCacheConfig
	{
		public static List<CachedAsset> Assets = new List<CachedAsset>();

		public static void Add(string virtualPath)
		{
			Assets.Add(new CachedAsset(virtualPath));
		}
	}
}