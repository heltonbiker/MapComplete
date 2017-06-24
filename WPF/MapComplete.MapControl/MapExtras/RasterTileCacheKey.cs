using System;

namespace Microsoft.Maps.MapExtras
{
	internal class RasterTileCacheKey
	{
		private int rasterTileCacheId;

		private TileId tileId;

		public override bool Equals(object obj)
		{
			RasterTileCacheKey rasterTileCacheKey = obj as RasterTileCacheKey;
			return rasterTileCacheKey != null && this.rasterTileCacheId == rasterTileCacheKey.rasterTileCacheId && this.tileId == rasterTileCacheKey.tileId;
		}

		public override int GetHashCode()
		{
			return this.rasterTileCacheId ^ this.tileId.GetHashCode();
		}

		public RasterTileCacheKey(int rasterTileCacheId, TileId tileId)
		{
			this.rasterTileCacheId = rasterTileCacheId;
			this.tileId = tileId;
		}
	}
}
