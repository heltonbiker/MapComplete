using System;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	public class MapConfigurationLoadedEventArgs : EventArgs
	{
		public Exception Error
		{
			get;
			private set;
		}

		public MapConfigurationLoadedEventArgs(Exception error)
		{
			this.Error = error;
		}
	}
}
