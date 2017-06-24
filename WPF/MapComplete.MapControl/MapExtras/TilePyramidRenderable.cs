using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal class TilePyramidRenderable
	{
		private const int MaxOversampleLevels = 5;

		private Canvas tilePyramidCanvas;

		private TileSource tileSource;

		private TilePyramidDescriptor tilePyramidDescriptor;

		private TilePyramid tilePyramid;

		private TilePriorityCalculator tilePriorityCalculator = new TilePriorityCalculator();

		private List<TileId> visibleTilesAtRenderLod = new List<TileId>();

		private HashSet<TileId> visibleTilesAtEffectiveRenderLod = new HashSet<TileId>();

		private Point2D[] visibleRenderablePoly = new Point2D[10];

		private Point2D[] screenPoly = new Point2D[10];

		private int _effectiveRenderLod;

		private bool?[] lodAvailabilities;

		private Rect screenRect;
		
		private Matrix3D tilePyramidToViewportTransform;

		private ChooseLevelOfDetailSettings chooseLevelOfDetailSettings;

		private ICollection<TileStatus> _tileStatuses = new List<TileStatus>();

		private bool showBackgroundTiles = true;

		public static readonly ChooseLevelOfDetailSettings ChooseLevelOfDetailSettingsDownloadNormal = delegate (int renderLevelOfDetail, int levelOfDetail, int minimumLevelOfDetail)
		{
			LevelOfDetailSettings value = new LevelOfDetailSettings(levelOfDetail >= renderLevelOfDetail - 5, 1.0, new int?(-2147483648));
			if (levelOfDetail == Math.Max(renderLevelOfDetail - 2, minimumLevelOfDetail))
			{
				value.DownloadPriority = new int?(1000);
			}
			else if (levelOfDetail == renderLevelOfDetail)
			{
				value.DownloadPriority = new int?(0);
			}
			else if (levelOfDetail >= renderLevelOfDetail - 5)
			{
				value.DownloadPriority = new int?(-1000);
			}
			return new LevelOfDetailSettings?(value);
		};

		public static readonly ChooseLevelOfDetailSettings ChooseLevelOfDetailSettingsDownloadInMotion = delegate (int renderLevelOfDetail, int levelOfDetail, int minimumLevelOfDetail)
		{
			LevelOfDetailSettings value = new LevelOfDetailSettings(levelOfDetail >= renderLevelOfDetail - 5, 1.0, null);
			if (levelOfDetail == Math.Max(renderLevelOfDetail - 5, minimumLevelOfDetail))
			{
				value.DownloadPriority = new int?(1000);
			}
			else if (levelOfDetail == renderLevelOfDetail - 3)
			{
				value.DownloadPriority = new int?(0);
			}
			else if (levelOfDetail == renderLevelOfDetail)
			{
				value.DownloadPriority = new int?(-1000);
			}
			return new LevelOfDetailSettings?(value);
		};

		public static readonly ChooseLevelOfDetailSettings ChooseLevelOfDetailSettingsDownloadNothing = (int renderLevelOfDetail, int levelOfDetail, int minimumLevelOfDetail) => new LevelOfDetailSettings?(new LevelOfDetailSettings(levelOfDetail >= renderLevelOfDetail - 5, 1.0, null));

		private Func<TileStatus, bool> VisibleTilePredicate = (TileStatus tile) => tile.Visible;

		//private Func<TileStatus, bool> MissingTilePredicate = (TileStatus tile) => !tile.WillNeverBeAvailable && (!tile.Available || !tile.FullyOpaque);

		public event EventHandler NeedsRender;

		public TileSource TileSource
		{
			get
			{
				return this.tileSource;
			}
			set
			{
				if (this.tileSource != null)
				{
					this.tilePyramid.DetachTileSource();
					this.tilePyramid.NeedsRender -= new EventHandler(this.TilePyramid_NeedsRender);
					this.tilePyramid = null;
					this.tilePyramidDescriptor = null;
					this.tileSource = null;
				}
				this.tileSource = value;
				if (this.tileSource != null)
				{
					this.tilePyramidDescriptor = new TilePyramidDescriptor(this.tileSource.FinestLodWidth, this.tileSource.FinestLodHeight, this.tileSource.MinimumLevelOfDetail, this.tileSource.TileWidth, this.tileSource.TileHeight);
					this.tilePyramid = new TilePyramid(this.tilePyramidDescriptor, this.tileSource, this.tilePyramidCanvas)
					{
						ChooseLevelOfDetailSettings = this.chooseLevelOfDetailSettings
					};
					this.tilePyramid.NeedsRender += new EventHandler(this.TilePyramid_NeedsRender);
					this.lodAvailabilities = new bool?[this.tileSource.MaximumLevelOfDetail + 1];
					return;
				}
				this.tilePyramidDescriptor = null;
				this.tilePyramid = null;
				this.lodAvailabilities = null;
			}
		}

		public bool ShowBackgroundTiles
		{
			get
			{
				return this.showBackgroundTiles;
			}
			set
			{
				this.showBackgroundTiles = value;
			}
		}

		//public Matrix3D NormalizedTilePyramidToToViewportTransform
		//{
		//	set
		//	{
		//		this.tilePyramidToViewportTransform = value;
		//		this.tilePyramidToViewportTransform.ScalePrepend(new Vector3D(1.0 / (double)this.tilePyramidDescriptor.FinestLevelWidth, 1.0 / (double)this.tilePyramidDescriptor.FinestLevelHeight, 1.0));
		//		this.FireNeedsRender();
		//	}
		//}

		//public double Opacity
		//{
		//	get
		//	{
		//		return this.tilePyramid.Opacity;
		//	}
		//	set
		//	{
		//		this.tilePyramid.Opacity = value;
		//	}
		//}

		public ChooseLevelOfDetailSettings ChooseLevelOfDetailSettings
		{
			get
			{
				return this.chooseLevelOfDetailSettings;
			}
			set
			{
				this.chooseLevelOfDetailSettings = value;
				if (this.tilePyramid != null)
				{
					this.tilePyramid.ChooseLevelOfDetailSettings = this.chooseLevelOfDetailSettings;
				}
			}
		}

		//public bool IsIdle
		//{
		//	get
		//	{
		//		if (this.tilePyramid != null)
		//		{
		//			return !this._tileStatuses.Where(this.MissingTilePredicate).Any((TileStatus tile) => tile.TileId.LevelOfDetail == this._effectiveRenderLod);
		//		}
		//		return true;
		//	}
		//}

		public bool HasSomeTiles
		{
			get
			{
				return this.tilePyramid == null || (this.visibleTilesAtEffectiveRenderLod.Count > 0 && this._tileStatuses.Count(this.VisibleTilePredicate) >= this.visibleTilesAtEffectiveRenderLod.Count / 4);
			}
		}

		//public TilePyramidRenderable(Canvas parentCanvas)
		//{
		//	this.tilePyramidCanvas = parentCanvas;
		//	this.tilePyramidToViewportTransform = Matrix3D.Identity;
		//	this.screenRect = new Rect(0.0, 0.0, 0.0, 0.0);
		//	this.chooseLevelOfDetailSettings = TilePyramidRenderable.ChooseLevelOfDetailSettingsDownloadNormal;
		//}

		public void Render(Point2D viewportSize)
		{
			if (this.tileSource != null)
			{
				int num;
				double preciseRenderLod;
				int renderLod;
				double num2;
				this.tilePyramidDescriptor.GetVisibleTiles(viewportSize, ref this.tilePyramidToViewportTransform, false, this.visibleTilesAtRenderLod, this.visibleRenderablePoly, this.screenPoly, out num, out preciseRenderLod, out renderLod, out num2);
				this.UpdateLodAvailabilitiesAndEffectiveRenderLod(renderLod);
				if (this.visibleTilesAtRenderLod.Count > 0)
				{
					this.tilePriorityCalculator.Initialize(this.tilePyramidDescriptor, ref this.tilePyramidToViewportTransform, viewportSize, this.visibleTilesAtRenderLod[0]);
				}
				this.tilePyramid.Render(viewportSize, ref this.tilePyramidToViewportTransform, this.visibleTilesAtRenderLod, this.tilePriorityCalculator, preciseRenderLod, renderLod, this.ShowBackgroundTiles);
				this._tileStatuses = this.tilePyramid.GetTiles();
			}
		}

		private void TilePyramid_NeedsRender(object sender, EventArgs e)
		{
			this.FireNeedsRender();
		}

		private void FireNeedsRender()
		{
			if (this.NeedsRender != null)
			{
				this.NeedsRender(this, EventArgs.Empty);
			}
		}

		private void UpdateLodAvailabilitiesAndEffectiveRenderLod(int renderLod)
		{
			int val = -2147483648;
			int num = 2147483647;
			for (int i = 0; i < this.lodAvailabilities.Length; i++)
			{
				this.lodAvailabilities[i] = null;
			}
			foreach (TileStatus current in this._tileStatuses)
			{
				if (current.Available)
				{
					this.lodAvailabilities[current.TileId.LevelOfDetail] = new bool?(true);
					val = Math.Max(val, current.TileId.LevelOfDetail);
				}
				else if (current.WillNeverBeAvailable)
				{
					if (!this.lodAvailabilities[current.TileId.LevelOfDetail].HasValue)
					{
						this.lodAvailabilities[current.TileId.LevelOfDetail] = new bool?(false);
					}
					num = Math.Min(num, current.TileId.LevelOfDetail);
				}
			}
			this._effectiveRenderLod = Math.Min(renderLod, Math.Max(val, num - 1));
			this.visibleTilesAtEffectiveRenderLod.Clear();
			if (this.visibleTilesAtRenderLod.Count > 0 && this._effectiveRenderLod >= this.tileSource.MinimumLevelOfDetail)
			{
				int num2 = renderLod - this._effectiveRenderLod;
				foreach (TileId current2 in this.visibleTilesAtRenderLod)
				{
					this.visibleTilesAtEffectiveRenderLod.Add(new TileId(this._effectiveRenderLod, current2.X >> num2, current2.Y >> num2));
				}
			}
		}
	}
}
