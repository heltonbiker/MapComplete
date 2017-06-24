using System;

namespace Microsoft.Maps.MapControl.WPF
{
	public class MapEventArgs : EventArgs
	{
		private bool handled;

		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}
	}
}
