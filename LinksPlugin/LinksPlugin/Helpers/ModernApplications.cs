using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using Windows.Management.Deployment;

namespace Loupedeck.LinksPlugin.Helpers
{
	internal static class ModernApplications
	{
		private static readonly PackageManager pkgManager = new PackageManager();

		public static IEnumerable<(string Id, string Name)> GetApplications()
		{
			try
			{
				return pkgManager.FindPackagesForUserWithPackageTypes(string.Empty, PackageTypes.Main)
						  .Where(x => !string.IsNullOrEmpty(x.Id.FullName))
						  .DistinctBy(x => x.Id.FullName)
						  .Select(x => (x.Id.FullName, x.DisplayName))
						  .OrderBy(x => x.DisplayName);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ERROR while enumerating modern applications: {0}", ex.Message);
				return Enumerable.Empty<(string, string)>();
			}
		}

		public static async void RunApplication(string packageFullName)
		{
			try
			{
				await pkgManager.FindPackageForUser(string.Empty, packageFullName)?.GetAppListEntries().FirstOrDefault()?.LaunchAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ERROR while running modern application: {0}", ex.Message);
			}
		}

		public static Bitmap GetIcon(string pathAndParams, PluginImageSize imageSize)
		{
			try
			{
				var (width, height) = imageSize.GetSize();
				return GetIcon(pathAndParams, width, height);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ERROR while getting modern application icon: {0}", ex.Message);
				return null;
			}
		}

		public static Bitmap GetIcon(string packageFullName, int width, int height)
		{
			var bitmap = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.FillRectangle(Constants.ICON_BACKGROUND_BRUSH, new Rectangle(0, 0, width, height));

				var iconPath = pkgManager.FindPackageForUser(string.Empty, packageFullName)?.Logo;
				if (iconPath != null && iconPath.IsFile)
				{
					var icon = Image.FromFile(iconPath.LocalPath, true);

					var w = (int)(width * Constants.ICON_SCALE);
					var h = (int)(height * Constants.ICON_SCALE);

					var targetRect = new Rectangle((width / 2) - (w / 2), (height / 2) - (h / 2), w, h);
					graphics.DrawImage(icon, targetRect);
				}
			}

			return bitmap;

		}
	}
}
