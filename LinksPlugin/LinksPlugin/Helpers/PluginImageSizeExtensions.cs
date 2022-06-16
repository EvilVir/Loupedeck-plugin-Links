namespace Loupedeck.LinksPlugin.Helpers
{
	internal static class PluginImageSizeExtensions
	{
		public static (int Width, int Height) GetSize(this PluginImageSize imageSize)
		{
			switch (imageSize)
			{
				case PluginImageSize.Width60:
					return (60, 60);

				default:
				case PluginImageSize.Width90:
					return (90, 90);
			}
		}
	}
}
