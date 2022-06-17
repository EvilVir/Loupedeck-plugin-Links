
using Loupedeck.LinksPlugin.Helpers;

namespace Loupedeck.LinksPlugin
{
	class StartModernApplicationCommand : PluginDynamicCommand
	{
		public StartModernApplicationCommand() : base("Modern Applications", "Starts given modern Windows application", "Links")
		{
			MakeProfileAction("list;Application");
		}

		protected override bool OnLoad()
		{
			foreach (var (id, name) in ModernApplications.GetApplications())
			{
				this.AddParameter(id, name, "Apps");
			}

			return base.OnLoad();
		}

		protected override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
			=> CacheHelper.GetIcon(LinksPlugin.DataFolder, actionParameter, () => ModernApplications.GetIcon(actionParameter, imageSize).ToBitmapImage());

		protected override void RunCommand(string actionParameter)
			=> ModernApplications.RunApplication(actionParameter);
	}
}
