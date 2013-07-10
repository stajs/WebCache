using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Path = Fluent.IO.Path;

namespace WebCache
{
	public class CachedAsset
	{
		public string OriginalPath { get; private set; }
		public string CachedPath { get; private set; }
		public Path File { get; private set; }

		public CachedAsset(string virtualPath)
		{
			OriginalPath = virtualPath;
			var originalFile = GetFile(virtualPath);
			var hash = GetHash(originalFile);
			File = CreateFile(originalFile, hash);
			CachedPath = string.Format("{0}/{1}", virtualPath.Substring(0, virtualPath.LastIndexOf("/", StringComparison.Ordinal)), File.FileName);
		}
		
		private Path CreateFile(Path file, string hash)
		{
			var newFileName = string.Format("{0}.{1}.webcache{2}", file.FileNameWithoutExtension, hash, file.Extension);
			var newFile = file.Copy(file.Combine(file.DirectoryName, newFileName));
			return newFile;
		}

		private Path GetFile(string virtualPath)
		{
			var path = HostingEnvironment.MapPath(virtualPath);
			
			var file = Path.Get(path);

			if (!file.Exists)
				throw new FileNotFoundException();

			return file;
		}

		private string GetHash(Path file)
		{
			byte[] bytes;

			using (var md5 = MD5.Create())
			using (var stream = System.IO.File.OpenRead(file.FullPath))
				bytes = md5.ComputeHash(stream);

			return BitConverter.ToString(bytes)
				.Replace("-", string.Empty)
				.ToLower();
		}
	}

	public class Config
	{
		public static List<CachedAsset> Assets = new List<CachedAsset>();

		public static void Add(string virtualPath)
		{
			Assets.Add(new CachedAsset(virtualPath));
		}
	}
}