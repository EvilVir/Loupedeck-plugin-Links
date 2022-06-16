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
			var (path, parameters) = ParsePathAndParams(pathAndParams);

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

		private static (string ExecutablePath, string Parameters) ParsePathAndParams(string pathAndParams)
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

			return (path.Trim(), string.IsNullOrEmpty(prms) ? null : prms.Trim());
		}

		public static Bitmap GetIcon(string pathAndParams, PluginImageSize imageSize)
		{
			var (width, height) = imageSize.GetSize();
			return GetIcon(pathAndParams, width, height);
		}

		private static Bitmap GetIcon(string pathAndParams, int width, int height)
		{
			var (path, _) = ParsePathAndParams(pathAndParams);

			switch (Path.GetExtension(path))
			{
				case ".exe":
					return GetIconFromExe(path, width, height);
				default:
					return null;
			}
		}

		private static Bitmap GetIconFromExe(string path, int width, int height)
		{
			var bitmap = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.FillRectangle(Constants.ICON_BACKGROUND_BRUSH, new Rectangle(0, 0, width, height));

				var mi = new MultiIcon();
				mi.Load(path);

				if (mi.FirstOrDefault() is SingleIcon singleIcon && singleIcon.Any())
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
