namespace Loupedeck.LinksPlugin
{
	using System;

	public class LinksPlugin : Plugin
	{
		public override bool HasNoApplication => true;
		public override bool UsesApplicationApiOnly => true;

		public override void Load()
		{
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
