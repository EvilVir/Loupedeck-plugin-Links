using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.IconLib;
using System.IO;
using System.Linq;

namespace Loupedeck.LinksPlugin.Helpers
{
	internal static class ClassicApplications
	{
		public static void RunApplication(string pathAndParams)
		{
			try
			{
				var (path, parameters, iconIndex) = ParsePathAndParams(pathAndParams);

				var process = new Process()
				{
					StartInfo = new ProcessStartInfo(path, parameters)
					{
						LoadUserProfile = true,
						UseShellExecute = true
					}
				};

				process.Start();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ERROR while running classic application: {0}", ex.Message);
			}
		}

		private static (string ExecutablePath, string Parameters, int iconIndex) ParsePathAndParams(string pathAndParams)
		{
			pathAndParams = pathAndParams.Trim();
			var isQuoted = pathAndParams.StartsWith('"');
			int splitPoint;

			if (isQuoted)
			{
				pathAndParams = pathAndParams.Substring(1);
				splitPoint = pathAndParams.IndexOf('"');
			}
			else
			{
				splitPoint = pathAndParams.IndexOf(' ');
			}

			var path = splitPoint > -1 ? pathAndParams.Substring(0, splitPoint) : pathAndParams;
			var prms = splitPoint > -1 ? pathAndParams.Substring(splitPoint + 1) : null;
			var iconIndex = 0;

			if (path.IndexOf(',') is int commaIdx && commaIdx > -1)
			{
				iconIndex = int.Parse(path.Substring(commaIdx + 1));
				path = path.Substring(0, commaIdx);
			}

			return (path.Trim(), string.IsNullOrEmpty(prms) ? null : prms.Trim(), iconIndex);
		}

		public static Bitmap GetIcon(string pathAndParams, PluginImageSize imageSize)
		{
			var (width, height) = imageSize.GetSize();
			return GetIcon(pathAndParams, width, height);
		}

		private static Bitmap GetIcon(string pathAndParams, int width, int height)
		{
			try
			{
				var (path, _, iconIndex) = ParsePathAndParams(pathAndParams);

				switch (Path.GetExtension(path))
				{
					case ".exe":
						return GetIconFromExe(path, iconIndex, width, height);
					default:
						return null;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ERROR while getting classing application icon: {0}", ex.Message);
				return null;
			}
		}

		private static Bitmap GetIconFromExe(string path, int index, int width, int height)
		{
			var bitmap = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.FillRectangle(Constants.ICON_BACKGROUND_BRUSH, new Rectangle(0, 0, width, height));

				var mi = new MultiIcon();
				mi.Load(path);

				if (mi.Skip(index).FirstOrDefault() is SingleIcon singleIcon && singleIcon.Any())
				{
					var w = (int)(width * Constants.ICON_SCALE);
					var h = (int)(height * Constants.ICON_SCALE);

					var ordered = singleIcon.OrderByDescending(x => x.Size.Width).ThenByDescending(x => x.Size.Height);
					var selected = ordered.Where(x => x.Size.Width <= w && x.Size.Height <= h).FirstOrDefault() ?? ordered.First();

					var targetRect = new Rectangle((width / 2) - (w / 2), (height / 2) - (h / 2), w, h);
					graphics.DrawIcon(selected.Icon, targetRect);
				}
			}

			return bitmap;
		}
	}
}
