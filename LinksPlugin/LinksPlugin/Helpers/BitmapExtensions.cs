using System.Drawing;

namespace Loupedeck.LinksPlugin
{
	internal static class BitmapExtensions
	{
		public static BitmapImage ToBitmapImage(this Bitmap bitmap)
			=> bitmap != null ? BitmapImage.FromArray((byte[])new ImageConverter().ConvertTo(bitmap, typeof(byte[]))) : null;
	}
}
