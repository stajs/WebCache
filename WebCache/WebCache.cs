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
		public static ReadOnlyDictionary<string, List<Asset>> Bundles
		{
			get { return new ReadOnlyDictionary<string, List<Asset>>(_bundles); }
		}

		public static void Register(params Bundle[] bundles)
		{
			foreach (var bundle in bundles)
				_bundles.Add(bundle.Name, bundle.Assets);
		}

		public static void TryDeleteCacheFolder()
		{
			var applicationRoot = HostingEnvironment.ApplicationPhysicalPath;
			var cacheFolder = Path.Get(applicationRoot, Asset.CacheFolderName);

			if (!cacheFolder.Exists)
				return;

			/* Trying to recurively delete a folder can fail with an IOException ("The directory is not empty") for
			 * a number of reasons, including: locked files, permissions, read-only files. An easy way to reproduce
			 * is to have a subdirectory open in Windows Explorer while deleting the parent:
			 * 
			 *	- http://stackoverflow.com/questions/4102638/directoryinfo-deletetrue-doesnt-delete-when-folder-structure-is-open-in-windo
			 *		
			 * I don't like any of the hack-arounds out there (although the DeleteRecursivelyWithMagicDust() is amusing):
			 * 
			 * - http://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true
			 * 
			 * I'm almost tempted to implement this:
			 *  
			 * - http://www.timstall.com/2009/02/killing-file-handles-but-not-process.html
			 */
			
			try { cacheFolder.Delete(recursive: true); }
			catch { /* Oh well, better luck next time... */ }
		}
	}
}