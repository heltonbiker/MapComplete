using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Microsoft.Maps.MapExtras
{
	internal class RasterTileCacheValue : MemoryCacheValue
	{
		public BitmapSource Image
		{
			get;
			private set;
		}

		public Rect TileSubregion
		{
			get;
			private set;
		}

		public Dictionary<string, string> Metadata
		{
			get;
			private set;
		}

		public DateTime TimeAdded
		{
			get;
			private set;
		}

		public RasterTileCacheValue(BitmapSource image, Rect tileSubregion, Dictionary<string, string> metadata)
		{
			this.Image = image;
			this.TileSubregion = tileSubregion;
			this.Metadata = metadata;
			this.TimeAdded = DateTime.UtcNow;
			base.Size = ((image == null) ? 0 : (image.PixelWidth * image.PixelHeight * ((image is WriteableBitmap) ? 8 : 4))) + 200;
		}
	}
}
