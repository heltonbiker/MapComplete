using System;
using System.Xml;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	internal class MapConfigurationFromFile : MapConfigurationProvider
	{
		private XmlReader config;

		public override event EventHandler<MapConfigurationLoadedEventArgs> Loaded;

		public MapConfigurationFromFile(XmlReader config, string culture, string version)
		{
			this._culture = culture;
			this._version = version;
			this.config = config;
		}

		public override void LoadConfiguration()
		{
			try
			{
				base.Sections = base.ParseConfiguration(this.config);
				if (this.Loaded != null)
				{
					this.Loaded(this, new MapConfigurationLoadedEventArgs(null));
				}
			}
			catch (Exception error)
			{
				if (this.Loaded != null)
				{
					this.Loaded(this, new MapConfigurationLoadedEventArgs(error));
				}
			}
		}

		public override void Cancel()
		{
		}

		public override void GetConfigurationSection(string version, string sectionName, string culture, MapConfigurationCallback callback, bool reExecuteCallback, object userState)
		{
			this._version = version;
			this._culture = culture;
			if (callback != null)
			{
				callback(base.GetSection(version, sectionName, culture), userState);
			}
		}
	}
}
