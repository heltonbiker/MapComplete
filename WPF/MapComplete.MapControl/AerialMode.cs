using Microsoft.Maps.MapControl.WPF.Core;
using Microsoft.Maps.MapControl.WPF.PlatformServices;
using System;

namespace Microsoft.Maps.MapControl.WPF
{
	public class AerialMode : MapMode
	{
		private const string aerialUriFormatKey = "AERIALWITHOUTLABELS";

		private const string aerialWithLabelsUriFormatKey = "AERIALWITHLABELS";

		private bool labels;

		private string aerialTileUriFormat = string.Empty;

		private string aerialWithLabelsTileUriFormat = string.Empty;

		private string aerialUriSubdomains = string.Empty;

		private string aerialWithLablesUriSubdomains = string.Empty;

		public bool Labels
		{
			get
			{
				return this.labels;
			}
			set
			{
				this.labels = value;
			}
		}

		internal override string TileUriFormat
		{
			get
			{
				string text = this.labels ? this.aerialWithLabelsTileUriFormat : this.aerialTileUriFormat;
				return text.Replace("{Culture}", base.Culture);
			}
		}

		internal override string Subdomains
		{
			get
			{
				if (!this.labels)
				{
					return this.aerialUriSubdomains;
				}
				return this.aerialWithLablesUriSubdomains;
			}
		}

		internal override MapStyle? MapStyle
		{
			get
			{
				return new MapStyle?(this.labels ? Microsoft.Maps.MapControl.WPF.PlatformServices.MapStyle.AerialWithLabels : Microsoft.Maps.MapControl.WPF.PlatformServices.MapStyle.Aerial);
			}
		}

		public AerialMode() : this(false)
		{
		}

		public AerialMode(bool labels)
		{
			this.labels = labels;
		}

		internal override void AsynchronousConfigurationLoaded(MapConfigurationSection config, object userState)
		{
			if (config != null)
			{
				bool flag = false;
				string text = config["AERIALWITHLABELS"];
				string text2 = config["AERIALWITHOUTLABELS"];
				if (text2.IndexOf("{0-3}") != -1)
				{
					this.aerialUriSubdomains = "0,1,2,3";
					text2 = text2.Replace("{0-3}", "{subdomain}");
				}
				if (text2.IndexOf("{0-7}") != -1)
				{
					this.aerialUriSubdomains = "0,2,4,6 1,3,5,7";
					text2 = text2.Replace("{0-7}", "{subdomain}");
				}
				if (text.IndexOf("{0-3}") != -1)
				{
					this.aerialWithLablesUriSubdomains = "0,1,2,3";
					text = text.Replace("{0-3}", "{subdomain}");
				}
				if (text.IndexOf("{0-7}") != -1)
				{
					this.aerialWithLablesUriSubdomains = "0,2,4,6 1,3,5,7";
					text = text.Replace("{0-7}", "{subdomain}");
				}
				flag = (flag || text2 != this.aerialTileUriFormat || text != this.aerialWithLabelsTileUriFormat);
				this.aerialTileUriFormat = text2;
				this.aerialWithLabelsTileUriFormat = text;
				if (flag)
				{
					base.RebuildTileSource();
				}
			}
		}
	}
}
