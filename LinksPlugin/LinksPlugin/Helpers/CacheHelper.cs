using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Loupedeck.LinksPlugin.Helpers
{
	internal static class CacheHelper
	{
		private static readonly string CacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Loupedeck-Links");
		private static readonly ConcurrentDictionary<string, BitmapImage> Cache = new ConcurrentDictionary<string, BitmapImage>();

		static CacheHelper()
		{
			Directory.CreateDirectory(CacheFolder);
		}

		public static BitmapImage GetIcon(string key, Func<BitmapImage> factory)
		{
			key = EncodeKey(key);
			var cachePath = Path.Combine(CacheFolder, key);

			return Cache.GetOrAdd(key, k =>
			{
				if (File.Exists(cachePath))
				{
					return BitmapImage.FromFile(cachePath);
				}
				else
				{
					var bitmapImage = factory();
					bitmapImage.SaveToFile(cachePath);
					return bitmapImage;
				}
			});
		}

		public static string EncodeKey(string key)
		{
			using (var md5 = MD5.Create())
			{
				var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
				return BitConverter.ToString(hash).Replace("-", string.Empty);
			}
		}
	}
}
