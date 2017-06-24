using System;

namespace Microsoft.Maps.MapExtras
{
	internal abstract class MemoryCacheValue
	{
		private int size;

		internal event EventHandler<MemoryCacheObjectSizeChangedEventArgs> SizeChanged;

		public int Size
		{
			get
			{
				return this.size;
			}
			protected set
			{
				if (this.size != value)
				{
					int previousSize = this.size;
					this.size = value;
					this.OnSizeChanged(previousSize);
				}
			}
		}

		protected void OnSizeChanged(int previousSize)
		{
			EventHandler<MemoryCacheObjectSizeChangedEventArgs> sizeChanged = this.SizeChanged;
			if (sizeChanged != null)
			{
				sizeChanged(this, new MemoryCacheObjectSizeChangedEventArgs(previousSize));
			}
		}
	}
}
