using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Maps.MapControl.WPF
{
	/// <summary>Represents a pushpin on the map.</summary>
	public class Pushpin : ContentControl
	{
		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.Pushpin.Location"></see> dependency property.</summary>
		public readonly static DependencyProperty LocationDependencyProperty;

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl,WPF.Pushpin.PositionLocation"></see> dependency property.</summary>
		public readonly static DependencyProperty PositionOriginDependencyProperty;

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.Pushpin.Heading"></see> dependency property.</summary>
		public readonly static DependencyProperty HeadingProperty;

		/// <summary>Gets or sets the heading of the pushpin on the map.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double Heading
		{
			get
			{
				return (double)base.GetValue(Pushpin.HeadingProperty);
			}
			set
			{
				base.SetValue(Pushpin.HeadingProperty, value);
			}
		}

		/// <summary>Gets or sets the location of the pushpin on the map.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		public Microsoft.Maps.MapControl.WPF.Location Location
		{
			get
			{
				return (Microsoft.Maps.MapControl.WPF.Location)base.GetValue(Pushpin.LocationDependencyProperty);
			}
			set
			{
				base.SetValue(Pushpin.LocationDependencyProperty, value);
			}
		}

		/// <summary>Gets or sets the position origin of the pushpin, which defines the position on the pushpin to anchor to the map.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.PositionOrigin"></see>.</returns>
		public Microsoft.Maps.MapControl.WPF.PositionOrigin PositionOrigin
		{
			get
			{
				return (Microsoft.Maps.MapControl.WPF.PositionOrigin)base.GetValue(Pushpin.PositionOriginDependencyProperty);
			}
			set
			{
				base.SetValue(Pushpin.PositionOriginDependencyProperty, value);
			}
		}

		static Pushpin()
		{
			Pushpin.LocationDependencyProperty = DependencyProperty.Register("Location", typeof(Microsoft.Maps.MapControl.WPF.Location), typeof(Pushpin), new PropertyMetadata(new PropertyChangedCallback(Pushpin.OnLocationChangedCallback)));
			Pushpin.PositionOriginDependencyProperty = DependencyProperty.Register("PositionOrigin", typeof(Microsoft.Maps.MapControl.WPF.PositionOrigin), typeof(Pushpin), new PropertyMetadata(new PropertyChangedCallback(Pushpin.OnPositionOriginChangedCallback)));
			Pushpin.HeadingProperty = DependencyProperty.Register("Heading", typeof(double), typeof(Pushpin), new PropertyMetadata(new PropertyChangedCallback(Pushpin.OnHeadingChangedCallback)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.Pushpin"></see> class.</summary>
		public Pushpin()
		{
			base.DefaultStyleKey = typeof(Pushpin);
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
		}

		private static void OnHeadingChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
		{
			((Pushpin)d).UpdateRenderTransform();
		}

		private static void OnLocationChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
		{
			MapLayer.SetPosition(d, (Microsoft.Maps.MapControl.WPF.Location)eventArgs.NewValue);
		}

		private static void OnPositionOriginChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
		{
			MapLayer.SetPositionOrigin(d, (Microsoft.Maps.MapControl.WPF.PositionOrigin)eventArgs.NewValue);
			((Pushpin)d).UpdateRenderTransform();
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateRenderTransform();
		}

		/// <summary>???</summary>
		protected void UpdateRenderTransform()
		{
			Microsoft.Maps.MapControl.WPF.PositionOrigin positionOrigin = this.PositionOrigin;
			RotateTransform renderTransform = base.RenderTransform as RotateTransform;
			if (renderTransform == null && this.Heading != 0)
			{
				renderTransform = new RotateTransform();
				base.RenderTransform = renderTransform;
			}
			if (renderTransform != null)
			{
				renderTransform.Angle = this.Heading;
				renderTransform.CenterX = positionOrigin.X * base.ActualWidth;
				renderTransform.CenterY = positionOrigin.Y * base.ActualHeight;
			}
		}
	}
}