namespace Loupedeck.LinksPlugin
{
	using System;

	public class LinksPlugin : Plugin
	{
		public override bool HasNoApplication => true;
		public override bool UsesApplicationApiOnly => true;

		public override void Load()
		{
			this.Info.Icon16x16 = EmbeddedResources.ReadImage("Loupedeck.LinksPlugin.Resources.Icons.Icon-16.png");
			this.Info.Icon32x32 = EmbeddedResources.ReadImage("Loupedeck.LinksPlugin.Resources.Icons.Icon-32.png");
			this.Info.Icon48x48 = EmbeddedResources.ReadImage("Loupedeck.LinksPlugin.Resources.Icons.Icon-48.png");
			this.Info.Icon256x256 = EmbeddedResources.ReadImage("Loupedeck.LinksPlugin.Resources.Icons.Icon-256.png");
		}

		public override void Unload()
		{
		}

		private void OnApplicationStarted(object sender, EventArgs e)
		{
		}

		private void OnApplicationStopped(object sender, EventArgs e)
		{
		}

		public override void RunCommand(string commandName, string parameter)
		{
		}

		public override void ApplyAdjustment(string adjustmentName, string parameter, int diff)
		{
		}
	}
}
