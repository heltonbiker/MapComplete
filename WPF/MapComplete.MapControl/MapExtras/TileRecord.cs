using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal class TileRecord
	{
		private Canvas tilePyramidCanvas;

		private TilePyramidDescriptor tilePyramidDescriptor;

		private Rect? tilePyramidClip;

		private TileId tileId;

		private TileRenderable tileRenderable;

		private bool allowHardwareAcceleration;

		private double tileOverlap;

		private bool overlapPyramidEdges;

		private bool visible;

		private bool willNeverBeAvailable;

		private double targetOpacity;

		public event EventHandler NeedsRender;

		public int LastTouched
		{
			get;
			set;
		}

		public bool AllowHardwareAcceleration
		{
			get
			{
				return this.allowHardwareAcceleration;
			}
			set
			{
				this.allowHardwareAcceleration = value;
				this.EnsureTileRenderableConfiguration();
			}
		}

		public Rect? TilePyramidClip
		{
			get
			{
				return this.tilePyramidClip;
			}
			set
			{
				this.tilePyramidClip = value;
				this.EnsureTileRenderableConfiguration();
			}
		}

		public double TileOverlap
		{
			get
			{
				return this.tileOverlap;
			}
			set
			{
				this.tileOverlap = value;
				this.EnsureTileRenderableConfiguration();
			}
		}

		public bool OverlapPyramidEdges
		{
			get
			{
				return this.overlapPyramidEdges;
			}
			set
			{
				this.overlapPyramidEdges = value;
				this.EnsureTileRenderableConfiguration();
			}
		}

		public TileRenderable TileRenderable
		{
			get
			{
				return this.tileRenderable;
			}
			set
			{
				if (this.tileRenderable != null)
				{
					this.tileRenderable.BecameFullyOpaque -= new EventHandler(this.TileRenderable_BecameFullyOpaque);
				}
				this.tileRenderable = value;
				if (this.tileRenderable != null)
				{
					this.tileRenderable.TargetOpacity = this.targetOpacity;
					this.tileRenderable.BecameFullyOpaque += new EventHandler(this.TileRenderable_BecameFullyOpaque);
				}
				this.EnsureTileRenderableConfiguration();
			}
		}

		public bool WillNeverBeAvailable
		{
			get
			{
				return this.willNeverBeAvailable;
			}
			set
			{
				this.willNeverBeAvailable = value;
			}
		}

		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				this.visible = value;
				if (this.tileRenderable != null)
				{
					if (this.visible && this.tileRenderable.LayerCanvas == null)
					{
						this.tileRenderable.LayerCanvas = this.tilePyramidCanvas;
					}
					if (!this.visible && this.tileRenderable.LayerCanvas != null)
					{
						this.tileRenderable.LayerCanvas = null;
					}
				}
			}
		}

		public double TargetOpacity
		{
			get
			{
				return this.targetOpacity;
			}
			set
			{
				this.targetOpacity = value;
				if (this.tileRenderable != null)
				{
					this.tileRenderable.TargetOpacity = value;
				}
			}
		}

		public bool FullyOpaque
		{
			get
			{
				return this.tileRenderable != null && this.tileRenderable.Opacity > 0.99;
			}
		}

		private Point2D LodToFinestLodScaleFactor
		{
			get
			{
				Point2D result = new Point2D((double)this.tilePyramidDescriptor.FinestLevelWidth / (double)this.tilePyramidDescriptor.GetLevelOfDetailWidth(this.tileId.LevelOfDetail), (double)this.tilePyramidDescriptor.FinestLevelHeight / (double)this.tilePyramidDescriptor.GetLevelOfDetailHeight(this.tileId.LevelOfDetail));
				return result;
			}
		}

		public TileRecord(Canvas layerCanvas, TilePyramidDescriptor tilePyramidDescriptor, TileId tileId)
		{
			this.tilePyramidCanvas = layerCanvas;
			this.tilePyramidDescriptor = tilePyramidDescriptor;
			this.tileId = tileId;
			this.visible = false;
			this.willNeverBeAvailable = false;
		}

		private void TileRenderable_BecameFullyOpaque(object sender, EventArgs e)
		{
			if (this.NeedsRender != null)
			{
				this.NeedsRender(this, EventArgs.Empty);
			}
		}

		public void NoLongerRendering()
		{
			if (this.tileRenderable != null)
			{
				this.tileRenderable.NoLongerRendering();
			}
		}

		public void Render(Point2D viewportSize, ref Matrix3D tilePyramidToViewport, double renderLod)
		{
			if (this.tileRenderable != null)
			{
				Point2D point2D = new Point2D((double)((long)this.tilePyramidDescriptor.TileWidth * this.tileId.X - 1L), (double)((long)this.tilePyramidDescriptor.TileHeight * this.tileId.Y - 1L));
				Point2D lodToFinestLodScaleFactor = this.LodToFinestLodScaleFactor;
				Matrix3D matrix3D = VectorMath.TranslationMatrix3D(point2D.X, point2D.Y, 0.0) * VectorMath.ScalingMatrix3D(lodToFinestLodScaleFactor.X, lodToFinestLodScaleFactor.Y, 1.0) * tilePyramidToViewport;
				this.tileRenderable.Render(viewportSize, ref matrix3D, renderLod);
			}
		}

		private void EnsureTileRenderableConfiguration()
		{
			if (this.tileRenderable != null)
			{
				this.tileRenderable.AllowHardwareAcceleration = this.allowHardwareAcceleration;
				this.tileRenderable.ZIndex = this.tileId.LevelOfDetail;
				Rect rect = new Rect((double)(this.tileId.X * (long)this.tilePyramidDescriptor.TileWidth - 1L), (double)(this.tileId.Y * (long)this.tilePyramidDescriptor.TileHeight - 1L), (double)(this.tilePyramidDescriptor.TileWidth + 2), (double)(this.tilePyramidDescriptor.TileHeight + 2));
				rect.Intersect(new Rect(-1.0, -1.0, (double)(this.tilePyramidDescriptor.GetLevelOfDetailWidth(this.tileId.LevelOfDetail) + 2L), (double)(this.tilePyramidDescriptor.GetLevelOfDetailHeight(this.tileId.LevelOfDetail) + 2L)));
				TileEdgeFlags tileEdgeFlags = this.tilePyramidDescriptor.GetTileEdgeFlags(this.tileId);
				Rect rect2 = rect;
				double num = (this.overlapPyramidEdges || !tileEdgeFlags.IsLeftEdge) ? (1.0 - this.tileOverlap) : 1.0;
				rect2.X += num;
				rect2.Width -= num;
				double num2 = (this.overlapPyramidEdges || !tileEdgeFlags.IsTopEdge) ? (1.0 - this.tileOverlap) : 1.0;
				rect2.Y += num2;
				rect2.Height -= num2;
				rect2.Width -= ((this.overlapPyramidEdges || !tileEdgeFlags.IsRightEdge) ? (1.0 - this.tileOverlap) : 1.0);
				rect2.Height -= ((this.overlapPyramidEdges || !tileEdgeFlags.IsBottomEdge) ? (1.0 - this.tileOverlap) : 1.0);
				if (this.tilePyramidClip.HasValue)
				{
					Point2D lodToFinestLodScaleFactor = this.LodToFinestLodScaleFactor;
					Rect rect3 = new Rect(this.tilePyramidClip.Value.X / lodToFinestLodScaleFactor.X, this.tilePyramidClip.Value.Y / lodToFinestLodScaleFactor.Y, this.tilePyramidClip.Value.Width / lodToFinestLodScaleFactor.X, this.tilePyramidClip.Value.Height / lodToFinestLodScaleFactor.Y);
					rect2.Intersect(rect3);
				}
				this.tileRenderable.Clip = new Rect(rect2.X - (double)(this.tileId.X * (long)this.tilePyramidDescriptor.TileWidth) + 1.0, rect2.Y - (double)(this.tileId.Y * (long)this.tilePyramidDescriptor.TileHeight) + 1.0, rect2.Width, rect2.Height);
			}
		}
	}
}
