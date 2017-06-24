using Microsoft.Maps.MapControl.WPF.Core;
using Microsoft.Maps.MapControl.WPF.PlatformServices;
using Microsoft.Maps.MapExtras;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapControl.WPF
{
	public abstract class MapMode : Grid, IProjectable
	{
		private class SetViewParams
		{
			public Size ViewportSize;

			public Matrix3D NormalizedMercatorToViewport;

			public Matrix3D ViewportToNormalizedMercator;
		}

		//private const int Log2DuplicatePyramidCount = 4;

		//internal const string configurationVersion = "v1";

		//internal const string configurationSection = "Services";

		private string _Culture;

		private string _SessionId;

		private Canvas _TilePyramidRenderableCanvas;

		private MapExtras.TileSource _TileSource;

		private TilePyramidRenderable _TilePyramidRenderable;

		private Point? _CurrentMapInstance;

		private TileWrap _TileWrap;

		internal event EventHandler Rendered;

		internal TileWrap TileWrap
		{
			get
			{
				return this._TileWrap;
			}
			set
			{
				if (this._TileWrap != value && this._TileSource != null)
				{
					throw new InvalidOperationException("Must be set immediately after map mode construction. You can't switch the tile wrap of an initialized map mode.");
				}
				this._TileWrap = value;
			}
		}

		internal bool HasSomeTiles
		{
			get
			{
				return this._TilePyramidRenderable.HasSomeTiles;
			}
		}

		//public virtual ModeBackground ModeBackground
		//{
		//	get
		//	{
		//		return ModeBackground.Dark;
		//	}
		//}

		internal abstract MapStyle? MapStyle
		{
			get;
		}

		internal string Culture
		{
			get
			{
				if (!string.IsNullOrEmpty(this._Culture))
				{
					return this._Culture;
				}
				return CultureInfo.CurrentUICulture.Name;
			}
			set
			{
				this._Culture = value;
			}
		}

		internal string SessionId
		{
			get
			{
				return this._SessionId;
			}
			set
			{
				this._SessionId = value;
				MapConfiguration.GetSection("v1", "Services", this.Culture, this.SessionId, new MapConfigurationCallback(this.AsynchronousConfigurationLoaded), true);
			}
		}

		internal abstract string TileUriFormat
		{
			get;
		}

		internal abstract string Subdomains
		{
			get;
		}

		internal ChooseLevelOfDetailSettings ChooseLevelOfDetailSettings
		{
			get
			{
				return this._TilePyramidRenderable.ChooseLevelOfDetailSettings;
			}
			set
			{
				if (this._TilePyramidRenderable == null)
				{
					return;
				}
				this._TilePyramidRenderable.ChooseLevelOfDetailSettings = value;
			}
		}

		internal Point CurrentMapCopyInstance
		{
			set
			{
				this._CurrentMapInstance = new Point?(value);
			}
		}

		protected MapMode()
		{
			this._TileWrap = TileWrap.None;
			base.SizeChanged += new SizeChangedEventHandler(this.MapMode_SizeChanged);
			this._TilePyramidRenderableCanvas = new Canvas();
			base.Children.Add(this._TilePyramidRenderableCanvas);
			this._TilePyramidRenderable = new TilePyramidRenderable(this._TilePyramidRenderableCanvas);
			this._TilePyramidRenderable.NeedsRender += new EventHandler(this._TilePyramidRenderable_NeedsRender);
		}

		void IProjectable.SetView(Size viewportSize, Matrix3D normalizedMercatorToViewport, Matrix3D viewportToNormalizedMercator)
		{
			if (this._TileSource != null)
			{
				this.SetViewImpl(viewportSize, normalizedMercatorToViewport, viewportToNormalizedMercator);
				return;
			}
			MapMode.SetViewParams setViewParams = new MapMode.SetViewParams();
			setViewParams.ViewportSize = viewportSize;
			setViewParams.NormalizedMercatorToViewport = normalizedMercatorToViewport;
			setViewParams.ViewportToNormalizedMercator = viewportToNormalizedMercator;
			MapConfiguration.GetSection("v1", "Services", this.Culture, this.SessionId, new MapConfigurationCallback(this.AsynchronousConfigurationLoadedSetViewCallback), false, setViewParams);
		}

		private void AsynchronousConfigurationLoadedSetViewCallback(MapConfigurationSection config, object userState)
		{
			this.EnsureTileSource();
			MapMode.SetViewParams setViewParams = (MapMode.SetViewParams)userState;
			if (setViewParams != null)
			{
				this.SetViewImpl(setViewParams.ViewportSize, setViewParams.NormalizedMercatorToViewport, setViewParams.ViewportToNormalizedMercator);
			}
		}

		private void SetViewImpl(Size viewportSize, Matrix3D normalizedMercatorToViewport, Matrix3D viewportToNormalizedMercator)
		{
			if (this.TileWrap == TileWrap.None)
			{
				this._TilePyramidRenderable.NormalizedTilePyramidToToViewportTransform = normalizedMercatorToViewport;
				return;
			}
			int num = 16;
			bool flag = this.TileWrap == TileWrap.Horizontal || this.TileWrap == TileWrap.Both;
			bool flag2 = this.TileWrap == TileWrap.Vertical || this.TileWrap == TileWrap.Both;
			Matrix3D matrix = VectorMath.ScalingMatrix3D((double)num, (double)num, 1.0) * VectorMath.TranslationMatrix3D((double)(flag ? (-(double)num / 2) : 0), (double)(flag2 ? (-(double)num / 2) : 0), 0.0);
			this._TilePyramidRenderable.NormalizedTilePyramidToToViewportTransform = matrix * normalizedMercatorToViewport;
		}

		internal abstract void AsynchronousConfigurationLoaded(MapConfigurationSection config, object userState);

		internal void Detach()
		{
			this._TilePyramidRenderable.TileSource = null;
			this._TilePyramidRenderable.NeedsRender -= new EventHandler(this._TilePyramidRenderable_NeedsRender);
			this._TilePyramidRenderable = null;
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			Size result = base.ArrangeOverride(arrangeSize);
			this.InternalRender();
			return result;
		}

		private void EnsureTileSource()
		{
			if (this._TileSource == null)
			{
				this.InitializeTileSource();
			}
		}

		protected void RebuildTileSource()
		{
			if (this._TileSource != null)
			{
				this.InitializeTileSource();
			}
			this.InternalRender();
		}

		private void InitializeTileSource()
		{
			if (string.IsNullOrEmpty(this.TileUriFormat))
			{
				MapConfiguration.GetSection("v1", "Services", this.Culture, this.SessionId, new MapConfigurationCallback(this.AsynchronousConfigurationLoaded), true);
			}
			TileSource tileSource = new TileSource(this.TileUriFormat);
			string[][] subdomains;
			if (this.Subdomains != null && MapMode.TryParseSubdomains(this.Subdomains, out subdomains))
			{
				tileSource.SetSubdomains(subdomains);
			}
			RasterTileDownloader rasterTileDownloader = new GenericRasterTileDownloader(tileSource, OverlapBorderPresence.None, base.Dispatcher);

			unchecked
			{
				if (this.TileWrap == TileWrap.None)
				{
					this._TileSource = new RasterTileSource((long)((ulong)-2147483648), (long)((ulong)-2147483648), 256, 256, 9, rasterTileDownloader, false);
				}
				else
				{
					this._TileSource = new RasterTileSource((long)((ulong)-2147483648), (long)((ulong)-2147483648), 256, 256, 9, rasterTileDownloader, this.TileWrap, 4, false);
				}
			}

			this._TilePyramidRenderable.TileSource = this._TileSource;
		}

		private void MapMode_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.InternalRender();
		}

		private void _TilePyramidRenderable_NeedsRender(object sender, EventArgs e)
		{
			base.InvalidateArrange();
		}

		private void InternalRender()
		{
			if (this._TileSource != null && this._TilePyramidRenderable != null && this._TilePyramidRenderable.TileSource != null)
			{
				this._TilePyramidRenderable.Render(new Point2D(base.ActualWidth, base.ActualHeight));
				if (this.Rendered != null)
				{
					this.Rendered(this, EventArgs.Empty);
				}
			}
		}

		private static bool TryParseSubdomains(string subdomainString, out string[][] subdomains)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(subdomainString))
			{
				string[] array = subdomainString.Split(new char[]
				{
					' '
				});
				subdomains = new string[array.Length][];
				for (int i = 0; i < array.Length; i++)
				{
					subdomains[i] = array[i].Split(new char[]
					{
						','
					});
				}
				result = true;
			}
			else
			{
				subdomains = null;
			}
			return result;
		}
	}
}
