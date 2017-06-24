using System;

namespace Microsoft.Maps.MapExtras
{
	internal delegate LevelOfDetailSettings? ChooseLevelOfDetailSettings(int renderLevelOfDetail, int levelOfDetail, int minimumLevelOfDetail);

	internal struct LevelOfDetailSettings
	{
		public bool Visible
		{
			get;
			set;
		}

		public double TargetOpacity
		{
			get;
			set;
		}

		public int? DownloadPriority
		{
			get;
			set;
		}

		public LevelOfDetailSettings(bool visible, double targetOpacity, int? downloadPriority)
		{
			this = default(LevelOfDetailSettings);
			this.Visible = visible;
			this.TargetOpacity = targetOpacity;
			this.DownloadPriority = downloadPriority;
		}
	}
}
