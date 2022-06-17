using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Loupedeck.LinksPlugin.Helpers
{
	internal static class CacheHelper
	{
		public static BitmapImage GetIcon(string cacheFolder, string key, Func<BitmapImage> factory)
		{
			key = EncodeKey(key) + ".png";
			var cachePath = Path.Combine(cacheFolder, key);

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
