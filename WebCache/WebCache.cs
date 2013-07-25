// Static class for configuring WebCache.
//
// ## Usage
// All calls to configure WebCache should be made during the start up of your application (i.e. `Application_Start()` in your **Global.asax**).
//
// To *try* and delete the cache folder:
//
//		WebCache.TryDeleteCacheFolder();
//
// To register `Bundle`'s of `Asset`'s:
//
//		WebCache.Register(
//			new Bundle
//			{
//				Name = "Header",
//				Assets = new List<Asset>
//				{
//					new Asset("/assets/styles/main.min.css"),
//					new Asset("/assets/styles/app.min.css"),
//					new Asset("/assets/scripts/header.js")
//				}
//			},
//			new Bundle
//			{
//				Name = "Body",
//				Assets = new List<Asset>
//				{
//					new Asset("/assets/scripts/log.js")
//				}
//			}
//		);

//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Fluent.IO;

namespace WebCache
{
	public class WebCache
	{
		private static Dictionary<string, List<Asset>> _bundles = new Dictionary<string, List<Asset>>();

		// It is possible to directly add items to a static dictionary, so expose a read-only copy.
		public static ReadOnlyDictionary<string, List<Asset>> Bundles
		{
			get { return new ReadOnlyDictionary<string, List<Asset>>(_bundles); }
		}

		// Helper method to register bundles since there isn't a public setter for the dictionary.
		public static void Register(params Bundle[] bundles)
		{
			foreach (var bundle in bundles)
				_bundles.Add(bundle.Name, bundle.Assets);
		}

		//	Trying to recursively delete a folder can fail with an IOException (`The directory is not empty`) for
		//	a number of reasons, including: locked files, permissions, read-only files. An easy way to reproduce
		//	is to have a [subdirectory open in Windows Explorer while deleting the parent](http://stackoverflow.com/questions/4102638/directoryinfo-deletetrue-doesnt-delete-when-folder-structure-is-open-in-windo)
		//	I don't like any of the hack-arounds (although the [DeleteRecursivelyWithMagicDust()](http://stackoverflow.com/a/14933880) is amusing).
		//	I'm almost tempted to implement [a Kill 'em all](http://www.timstall.com/2009/02/killing-file-handles-but-not-process.html)...			
		public static void TryDeleteCacheFolder()
		{
			var applicationRoot = HostingEnvironment.ApplicationPhysicalPath;
			var cacheFolder = Path.Get(applicationRoot, Asset.CacheFolderName);

			if (!cacheFolder.Exists)
				return;

			try { cacheFolder.Delete(recursive: true); }
			// Oh well, better luck next time...
			catch {  }
		}
	}
}