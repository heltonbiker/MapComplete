using System;
using System.Windows.Threading;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	internal class MapConfigurationGetSectionRequest
	{
		public string Version
		{
			get;
			private set;
		}

		public string SectionName
		{
			get;
			private set;
		}

		public string Culture
		{
			get;
			private set;
		}

		public Dispatcher CallbackDispatcher
		{
			get;
			private set;
		}

		public MapConfigurationCallback Callback
		{
			get;
			private set;
		}

		public object UserState
		{
			get;
			private set;
		}

		public MapConfigurationGetSectionRequest(string version, string sectionName, string culture, MapConfigurationCallback callback, object userState)
		{
			this.Version = version;
			this.SectionName = sectionName;
			this.Culture = culture;
			this.Callback = callback;
			this.UserState = userState;
			this.CallbackDispatcher = Dispatcher.CurrentDispatcher;
		}
	}
}
