using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;

namespace WebCache
{
	public class WebCache
	{
		private static Dictionary<string, List<Asset>> _bundles = new Dictionary<string, List<Asset>>();
		public static ReadOnlyDictionary<string, List<Asset>> Bundles
		{
			get { return new ReadOnlyDictionary<string, List<Asset>>(_bundles); }
		}

		public static void Register(params Bundle[] bundles)
		{
			foreach (var bundle in bundles)
				_bundles.Add(bundle.Name, bundle.Assets);
		}
	}
}