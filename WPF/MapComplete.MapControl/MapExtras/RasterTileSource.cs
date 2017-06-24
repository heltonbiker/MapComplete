using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Microsoft.Maps.MapExtras
{
	internal class RasterTileSource : TileSource
	{
		private static Pool<Size, Image> imagePool = new Pool<Size, Image>();

		private long finestLodWidth;

		private long finestLodHeight;

		private int tileWidth;

		private int tileHeight;

		private int minimumLevelOfDetail;

		private int maximumLevelOfDetail;

		//	private Matrix3D transform;

		//	private TransformTileId tileIdTransform;

		//	private TilePyramidDescriptor tilePyramidDescriptor;

		private RelevantTileSet relevantTileSet;

		private Dictionary<TileId, TileRenderable> tileRenderables;

		private RasterTileCache rasterTileImageCache;

		//	public override event EventHandler NewTilesAvailable;

		public override TimeSpan? TileFadeInDuration
		{
			get;
			set;
		}

		public override long FinestLodWidth
		{
			get
			{
				return this.finestLodWidth;
			}
		}

		public override long FinestLodHeight
		{
			get
			{
				return this.finestLodHeight;
			}
		}

		public override int TileWidth
		{
			get
			{
				return this.tileWidth;
			}
		}

		public override int TileHeight
		{
			get
			{
				return this.tileHeight;
			}
		}

		public override int MinimumLevelOfDetail
		{
			get
			{
				return this.minimumLevelOfDetail;
			}
		}

		public override int MaximumLevelOfDetail
		{
			get
			{
				return this.maximumLevelOfDetail;
			}
		}

		//	public override Rect? Clip
		//	{
		//		get;
		//		set;
		//	}

		//	public override Matrix3D Transform
		//	{
		//		get
		//		{
		//			return this.transform;
		//		}
		//	}

		//	public Dictionary<string, string> MostRelevantTileMetadata
		//	{
		//		get
		//		{
		//			foreach (Tuple<TileId, int?> current in this.relevantTileSet.RelevantTiles)
		//			{
		//				RasterTileCacheValue value = this.rasterTileImageCache.GetValue(current.Item1);
		//				if (value != null)
		//				{
		//					return value.Metadata;
		//				}
		//			}
		//			return null;
		//		}
		//	}

		public RasterTileSource(long finestLodWidth, long finestLodHeight, int tileWidth, int tileHeight, int minimumLevelOfDetail, RasterTileDownloader rasterTileDownloader, bool useGlobalMemoryCache = true)
	{
	//		this.transform = Matrix3D.Identity;
	//		this.relevantTileSet = new RelevantTileSet();
	//		this.tileRenderables = new Dictionary<TileId, TileRenderable>();
	//		base..ctor();
	//		if (finestLodWidth <= 0L)
	//		{
	//			throw new ArgumentOutOfRangeException("finestLodWidth");
	//		}
	//		if (finestLodHeight <= 0L)
	//		{
	//			throw new ArgumentOutOfRangeException("finestLodHeight");
	//		}
	//		if (tileWidth <= 0)
	//		{
	//			throw new ArgumentOutOfRangeException("tileWidth");
	//		}
	//		if (tileHeight <= 0)
	//		{
	//			throw new ArgumentOutOfRangeException("tileHeight");
	//		}
	//		if (rasterTileDownloader == null)
	//		{
	//			throw new ArgumentNullException("rasterTileDownloader");
	//		}
	//		this.finestLodWidth = finestLodWidth;
	//		this.finestLodHeight = finestLodHeight;
	//		this.tileWidth = tileWidth;
	//		this.tileHeight = tileHeight;
	//		this.minimumLevelOfDetail = minimumLevelOfDetail;
	//		this.ConstructCommon(rasterTileDownloader, useGlobalMemoryCache);
	}

	public RasterTileSource(long logicalFinestLodWidth, long logicalFinestLodHeight, int tileWidth, int tileHeight, int logicalMinimumLevelOfDetail, RasterTileDownloader rasterTileDownloader, TileWrap tileWrap, int log2DuplicatePyramidCount, bool useGlobalMemoryCache = true)
	{
	//		RasterTileSource.<> c__DisplayClass1 <> c__DisplayClass = new RasterTileSource.<> c__DisplayClass1();
	//		<> c__DisplayClass.tileWrap = tileWrap;
	//		<> c__DisplayClass.log2DuplicatePyramidCount = log2DuplicatePyramidCount;
	//		this.transform = Matrix3D.Identity;
	//		this.relevantTileSet = new RelevantTileSet();
	//		this.tileRenderables = new Dictionary<TileId, TileRenderable>();
	//		base..ctor();
	//		if (logicalFinestLodWidth <= 0L)
	//		{
	//			throw new ArgumentOutOfRangeException("logicalFinestLodWidth");
	//		}
	//		if (logicalFinestLodHeight <= 0L)
	//		{
	//			throw new ArgumentOutOfRangeException("logicalFinestLodHeight");
	//		}
	//		if (tileWidth <= 0)
	//		{
	//			throw new ArgumentOutOfRangeException("tileWidth");
	//		}
	//		if (tileHeight <= 0)
	//		{
	//			throw new ArgumentOutOfRangeException("tileHeight");
	//		}
	//		if (rasterTileDownloader == null)
	//		{
	//			throw new ArgumentNullException("rasterTileDownloader");
	//		}
	//		if (<> c__DisplayClass.tileWrap == TileWrap.None)
	//		{
	//			throw new ArgumentException("tileWrap must horizontal, vertical, or both.");
	//		}
	//		if (<> c__DisplayClass.log2DuplicatePyramidCount < 1)
	//		{
	//			throw new ArgumentOutOfRangeException("log2DuplicatePyramidCount");
	//		}
	//		this.finestLodWidth = logicalFinestLodWidth << <> c__DisplayClass.log2DuplicatePyramidCount;
	//		this.finestLodHeight = logicalFinestLodHeight << <> c__DisplayClass.log2DuplicatePyramidCount;
	//		this.tileWidth = tileWidth;
	//		this.tileHeight = tileHeight;
	//		this.minimumLevelOfDetail = logicalMinimumLevelOfDetail + <> c__DisplayClass.log2DuplicatePyramidCount;
	//		TilePyramidDescriptor logicalTilePyramidDescriptor = new TilePyramidDescriptor(logicalFinestLodWidth, logicalFinestLodHeight, logicalMinimumLevelOfDetail, tileWidth, tileHeight);
	//		this.tileIdTransform = delegate (TileId tileId)
	//		{
	//			int lod = tileId.LevelOfDetail - <> c__DisplayClass.log2DuplicatePyramidCount;
	//			long levelOfDetailWidthInTiles = logicalTilePyramidDescriptor.GetLevelOfDetailWidthInTiles(lod);
	//			long levelOfDetailHeightInTiles = logicalTilePyramidDescriptor.GetLevelOfDetailHeightInTiles(lod);
	//			return new TileId(tileId.LevelOfDetail - <> c__DisplayClass.log2DuplicatePyramidCount, (<> c__DisplayClass.tileWrap == TileWrap.Horizontal || <> c__DisplayClass.tileWrap == TileWrap.Both) ? (tileId.X % levelOfDetailWidthInTiles) : tileId.X, (<> c__DisplayClass.tileWrap == TileWrap.Vertical || <> c__DisplayClass.tileWrap == TileWrap.Both) ? (tileId.Y % levelOfDetailHeightInTiles) : tileId.Y);
	//		};
	//		this.transform = VectorMath.TranslationMatrix3D((double)((<> c__DisplayClass.tileWrap == TileWrap.Horizontal || <> c__DisplayClass.tileWrap == TileWrap.Both) ? (-(double)this.finestLodWidth / 2L) : 0L), (double)((<> c__DisplayClass.tileWrap == TileWrap.Vertical || <> c__DisplayClass.tileWrap == TileWrap.Both) ? (-(double)this.finestLodHeight / 2L) : 0L), 0.0) * VectorMath.ScalingMatrix3D((double)(1 << <> c__DisplayClass.log2DuplicatePyramidCount), (double)(1 << <> c__DisplayClass.log2DuplicatePyramidCount), 1.0);
	//		this.ConstructCommon(rasterTileDownloader, useGlobalMemoryCache);
	}

	//	private void ConstructCommon(RasterTileDownloader rasterTileDownloader, bool useGlobalMemoryCache)
	//	{
	//		this.tilePyramidDescriptor = new TilePyramidDescriptor(this.finestLodWidth, this.finestLodHeight, this.minimumLevelOfDetail, this.tileWidth, this.tileHeight);
	//		this.maximumLevelOfDetail = this.tilePyramidDescriptor.FinestLevelOfDetail;
	//		if (this.minimumLevelOfDetail < 0 || this.minimumLevelOfDetail > this.maximumLevelOfDetail)
	//		{
	//			throw new ArgumentException("minimumLevelOfDetail must be in range [0, finest level of detail].");
	//		}
	//		this.rasterTileImageCache = new RasterTileCache(this.tilePyramidDescriptor, rasterTileDownloader, this.tileIdTransform, useGlobalMemoryCache);
	//		this.rasterTileImageCache.NewTilesAvailable += delegate (object sender, EventArgs e)
	//		{
	//			if (this.NewTilesAvailable != null)
	//			{
	//				this.NewTilesAvailable(this, EventArgs.Empty);
	//			}
	//		};
	//		this.TileFadeInDuration = new TimeSpan?(TimeSpan.FromMilliseconds(300.0));
	//	}

	public override void SetRelevantTiles(IList<Tuple<TileId, int?>> relevantTiles)
	{
	//		this.relevantTileSet.SetRelevantTiles(relevantTiles);
	//		this.rasterTileImageCache.SetRelevantTileSet(this.relevantTileSet);
	//		foreach (TileId current in this.relevantTileSet.RemovedTiles)
	//		{
	//			TileRenderable tileRenderable;
	//			if (this.tileRenderables.TryGetValue(current, out tileRenderable))
	//			{
	//				Image image = tileRenderable.Element as Image;
	//				if (image != null)
	//				{
	//					RasterTileSource.imagePool.Add(new Size(image.Width, image.Height), image);
	//				}
	//				tileRenderable.DetachFromElement();
	//				this.tileRenderables.Remove(current);
	//			}
	//		}
	}

	public override void GetTile(TileId tileId, out TileRenderable tileRenderable, out bool tileWillNeverBeAvailable)
	{
		if (!this.relevantTileSet.Contains(tileId))
		{
			throw new InvalidOperationException("Cannot get a tile that is not currently set as relevant.");
		}
		tileRenderable = null;
		tileWillNeverBeAvailable = false;
		RasterTileCacheValue value = this.rasterTileImageCache.GetValue(tileId);
		if (value != null)
		{
			if (value.Image == null)
			{
				tileWillNeverBeAvailable = true;
				return;
			}
			if (!this.tileRenderables.TryGetValue(tileId, out tileRenderable))
			{
				bool newToCache = DateTime.UtcNow.Subtract(value.TimeAdded).TotalMilliseconds < 500.0;
				tileRenderable = this.CreateTileRenderable(tileId, value.Image, value.TileSubregion, newToCache);
				this.tileRenderables.Add(tileId, tileRenderable);
			}
		}
	}

		private TileRenderable CreateTileRenderable(TileId tileId, BitmapSource bitmapSource, Rect tileSubregion, bool newToCache)
		{
			FrameworkElement element;
			if (tileSubregion.X == 1.0 && tileSubregion.Y == 1.0 && tileSubregion.Width == (double)(bitmapSource.PixelWidth - 2) && tileSubregion.Height == (double)(bitmapSource.PixelHeight - 2))
			{
				Image image = RasterTileSource.imagePool.Get(new Size((double)bitmapSource.PixelWidth, (double)bitmapSource.PixelHeight));
				if (image == null)
				{
					image = new Image
					{
						Width = (double)bitmapSource.PixelWidth,
						Height = (double)bitmapSource.PixelHeight
					};
				}
				image.Source = bitmapSource;
				image.Stretch = Stretch.None;
				image.IsHitTestVisible = false;
				element = image;
			}
			else
			{
				element = new Rectangle
				{
					Fill = new ImageBrush
					{
						ImageSource = bitmapSource,
						AlignmentX = AlignmentX.Left,
						AlignmentY = AlignmentY.Top,
						Stretch = Stretch.None,
						Transform = new TranslateTransform
						{
							X = -tileSubregion.X + 1.0,
							Y = -tileSubregion.Y + 1.0
						}
					},
					IsHitTestVisible = false,
					Width = tileSubregion.Width + 2.0,
					Height = tileSubregion.Height + 2.0
				};
			}
			bool flag = tileId.LevelOfDetail > this.MinimumLevelOfDetail + 1;
			return new TileRenderable(tileId, element, flag ? this.TileFadeInDuration : null);
		}
	}
}
