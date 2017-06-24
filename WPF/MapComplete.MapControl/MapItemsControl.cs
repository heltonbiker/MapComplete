using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapControl.WPF
{
	/// <summary>Represents the class that uses a <see cref="T:Microsoft.Maps.MapControl.WPF.MapLayer"></see> as an ItemsPanel. This enables data binding using an ItemsSource and an ItemTemplate. This class inherits from the ItemsControl class.</summary>
	public class MapItemsControl : ItemsControl, IProjectable
	{
		private MapLayer _MapLayer;

		private Size _ViewportSize = new Size();

		private Matrix3D _NormalizedMercatorToViewport = Matrix3D.Identity;

		private Matrix3D _ViewportToNormalizedMercator = Matrix3D.Identity;

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.MapItemsControl"></see> class.</summary>
		public MapItemsControl()
		{
			base.DefaultStyleKey = typeof(MapItemsControl);
			base.Loaded += new RoutedEventHandler(this.MapItemsControl_Loaded);
		}

		private void MapItemsControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				ItemsPresenter child = (ItemsPresenter)VisualTreeHelper.GetChild(this, 0);
				this._MapLayer = (MapLayer)VisualTreeHelper.GetChild(child, 0);
				((IProjectable)this._MapLayer).SetView(this._ViewportSize, this._NormalizedMercatorToViewport, this._ViewportToNormalizedMercator);
			}
		}

		void IProjectable.SetView(Size viewportSize, Matrix3D normalizedMercatorToViewport, Matrix3D viewportToNormalizedMercator)
		{
			this._ViewportSize = viewportSize;
			this._NormalizedMercatorToViewport = normalizedMercatorToViewport;
			this._ViewportToNormalizedMercator = viewportToNormalizedMercator;
			if (this._MapLayer != null)
			{
				((IProjectable)this._MapLayer).SetView(viewportSize, normalizedMercatorToViewport, viewportToNormalizedMercator);
			}
			base.InvalidateMeasure();
		}
	}
}