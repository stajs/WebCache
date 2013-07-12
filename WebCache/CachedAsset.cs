﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
			var originalFileSystemPath = HostingEnvironment.MapPath(virtualPath);
			var originalFile = GetFile(originalFileSystemPath);
			var hash = GetHash(originalFile);
			File = CreateFile(originalFile, hash);
			CachedPath = string.Format("{0}/{1}", virtualPath.Substring(0, virtualPath.LastIndexOf("/", StringComparison.Ordinal)), File.FileName);
		}
		
		private Path CreateFile(Path file, string hash)
		{
			var applicationRoot = HostingEnvironment.ApplicationPhysicalPath;
			var relativePath = file
				.DirectoryName
				.Split(new [] { applicationRoot }, 2, StringSplitOptions.None)
				.Last();
			var newFileName = string.Format("{0}.{1}.webcache{2}", file.FileNameWithoutExtension, hash, file.Extension);
			var newFilePath = Path.Get(applicationRoot, "webcache", relativePath, newFileName);
			var newFile = file.Copy(newFilePath);

			return newFile;
		}

		private Path GetFile(string path)
		{
			var file = Path.Get(path);

			if (!file.Exists)
				throw new FileNotFoundException("Could not find file to cache: " + path);

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
}