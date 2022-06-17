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

			return (Environment.ExpandEnvironmentVariables(path).Trim(), string.IsNullOrEmpty(prms) ? null : prms.Trim(), iconIndex);
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
					case ".lnk":
						return GetIconFromLnk(path, iconIndex, width, height);
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

		private static Bitmap GetIconFromLnk(string path, int index, int width, int height)
		{
			var lnk = Lnk.Lnk.LoadFile(path);
			var icoPath = Environment.ExpandEnvironmentVariables(lnk.IconLocation);

			switch (Path.GetExtension(icoPath))
			{
				case ".dll":
				case ".exe":
					return GetIconFromExe(icoPath, index, width, height);

				default:
				case ".ico":
					return GetIconFromIco(icoPath, width, height);
			}
		}

		private static Bitmap GetIconFromExe(string path, int index, int width, int height)
		{
			Icon icon = null;

			var mi = new MultiIcon();
			mi.Load(path);

			if (mi.Skip(index).FirstOrDefault() is SingleIcon singleIcon && singleIcon.Any())
			{
				var w = (int)(width * Constants.ICON_SCALE);
				var h = (int)(height * Constants.ICON_SCALE);
				var ordered = singleIcon.OrderByDescending(x => x.Size.Width).ThenByDescending(x => x.Size.Height);
				var selected = ordered.Where(x => x.Size.Width <= w && x.Size.Height <= h).FirstOrDefault() ?? ordered.First();
				icon = selected.Icon;
			}

			return CreateButtonIcon(icon, width, height);
		}

		private static Bitmap GetIconFromIco(string path, int width, int height)
			=> CreateButtonIcon(new Icon(path), width, height);

		private static Bitmap CreateButtonIcon(Icon icon, int width, int height)
		{
			var bitmap = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.FillRectangle(Constants.ICON_BACKGROUND_BRUSH, new Rectangle(0, 0, width, height));

				if (icon != null)
				{
					var w = (int)(width * Constants.ICON_SCALE);
					var h = (int)(height * Constants.ICON_SCALE);
					var targetRect = new Rectangle((width / 2) - (w / 2), (height / 2) - (h / 2), w, h);
					graphics.DrawIcon(icon, targetRect);
				}
			}

			return bitmap;
		}
	}
}
