using System;

namespace Microsoft.Maps.MapExtras
{
	internal class MemoryCacheObjectSizeChangedEventArgs : EventArgs
	{
		public int PreviousSize
		{
			get;
			private set;
		}

		public MemoryCacheObjectSizeChangedEventArgs(int previousSize)
		{
			this.PreviousSize = previousSize;
		}
	}
}
