using Loupedeck.LinksPlugin.Helpers;

namespace Loupedeck.LinksPlugin
{
	class StartClassicApplicationCommand : PluginDynamicCommand
	{
		public StartClassicApplicationCommand() : base("Classic Applications", "Starts given classic Windows applications", "Links")
		{
			MakeProfileAction("text;Path");
		}

		protected override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
			=> ClassicApplications.GetIcon(actionParameter, imageSize).ToBitmapImage();

		protected override void RunCommand(string actionParameter)
			=> ClassicApplications.RunApplication(actionParameter);
	}
}
