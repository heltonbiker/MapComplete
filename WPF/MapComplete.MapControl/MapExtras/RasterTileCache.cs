using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Microsoft.Maps.MapExtras
{
	internal delegate TileId TransformTileId(TileId tileId);

	internal class RasterTileCache
	{
		private static int nextRasterTileCacheId;

		private int rasterTileCacheId;

		private TilePyramidDescriptor tilePyramidDescriptor;

		private RasterTileDownloader rasterTileDownloader;

		private TransformTileId transformTileId;

		private bool useGlobalMemoryCache;

		private Dictionary<TileId, RasterTileCacheValue> rasterTileCacheValues;

		private HashSet<TileId> relevantTransformedTileIds;

		private List<TileId> rasterTileCacheValuesToRemove;

		private HashSet<TileId> pendingDownloads = new HashSet<TileId>();

		public event EventHandler NewTilesAvailable;

		public RasterTileCache(TilePyramidDescriptor tilePyramidDescriptor, RasterTileDownloader rasterTileDownloader, TransformTileId transformTileId, bool useGlobalMemoryCache)
		{
			this.tilePyramidDescriptor = tilePyramidDescriptor;
			this.rasterTileDownloader = rasterTileDownloader;
			this.transformTileId = transformTileId;
			this.useGlobalMemoryCache = useGlobalMemoryCache;
			this.rasterTileCacheId = RasterTileCache.nextRasterTileCacheId++;
			if (this.transformTileId == null)
			{
				this.transformTileId = ((TileId tileId) => tileId);
			}
			if (!useGlobalMemoryCache)
			{
				this.rasterTileCacheValues = new Dictionary<TileId, RasterTileCacheValue>();
				this.relevantTransformedTileIds = new HashSet<TileId>();
				this.rasterTileCacheValuesToRemove = new List<TileId>();
			}
		}

		public RasterTileCacheValue GetValue(TileId tileId)
		{
			TileId tileId2 = this.transformTileId(tileId);
			if (this.useGlobalMemoryCache)
			{
				return MemoryCache.Instance.GetValue<RasterTileCacheValue>(new RasterTileCacheKey(this.rasterTileCacheId, tileId2));
			}
			RasterTileCacheValue result = null;
			this.rasterTileCacheValues.TryGetValue(tileId2, out result);
			return result;
		}

		public void SetRelevantTileSet(RelevantTileSet relevantTileSet)
		{
			foreach (Tuple<TileId, int?> current in relevantTileSet.RelevantTiles)
			{
				TileId tileId = this.transformTileId(current.Item1);
				if (!this.useGlobalMemoryCache)
				{
					this.relevantTransformedTileIds.Add(tileId);
				}
				if (this.pendingDownloads.Contains(tileId))
				{
					if (current.Item2.HasValue)
					{
						this.rasterTileDownloader.UpdateTileDownloadPriority(tileId, current.Item2.Value);
					}
					else
					{
						this.rasterTileDownloader.CancelTileDownload(tileId);
						this.pendingDownloads.Remove(tileId);
					}
				}
				else if (current.Item2.HasValue && this.GetValue(current.Item1) == null)
				{
					this.pendingDownloads.Add(tileId);
					this.rasterTileDownloader.DownloadTile(tileId, this.tilePyramidDescriptor.GetTileEdgeFlags(current.Item1), tileId, new RasterTileAvailableDelegate(this.RasterTileImageAvailable), current.Item2.Value);
				}
			}
			foreach (TileId current2 in relevantTileSet.RemovedTiles)
			{
				TileId tileId2 = this.transformTileId(current2);
				if (this.pendingDownloads.Contains(tileId2))
				{
					this.rasterTileDownloader.CancelTileDownload(tileId2);
					this.pendingDownloads.Remove(tileId2);
				}
			}
			if (!this.useGlobalMemoryCache)
			{
				foreach (KeyValuePair<TileId, RasterTileCacheValue> current3 in this.rasterTileCacheValues)
				{
					if (!this.relevantTransformedTileIds.Contains(current3.Key))
					{
						this.rasterTileCacheValuesToRemove.Add(current3.Key);
					}
				}
				foreach (TileId current4 in this.rasterTileCacheValuesToRemove)
				{
					this.rasterTileCacheValues.Remove(current4);
				}
				this.rasterTileCacheValuesToRemove.Clear();
				this.relevantTransformedTileIds.Clear();
			}
		}

		private void RasterTileImageAvailable(BitmapSource image, Rect tileSubregion, Dictionary<string, string> metadata, object token)
		{
			TileId tileId = (TileId)token;
			RasterTileCacheValue rasterTileCacheValue = new RasterTileCacheValue(image, tileSubregion, metadata);
			if (this.useGlobalMemoryCache)
			{
				MemoryCache.Instance.Replace(new RasterTileCacheKey(this.rasterTileCacheId, tileId), rasterTileCacheValue);
			}
			else
			{
				this.rasterTileCacheValues[tileId] = rasterTileCacheValue;
			}
			if (this.pendingDownloads.Remove(tileId) && this.NewTilesAvailable != null)
			{
				this.NewTilesAvailable(this, EventArgs.Empty);
			}
		}
	}
}
