using Microsoft.Maps.MapControl.WPF.Core;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace Microsoft.Maps.MapControl.WPF.Overlays
{
	public partial class Scale : UserControl
	{
		//private const int MetersPerKm = 1000;

		//private const double YardsPerMeter = 1.0936133;

		//private const int YardsPerMile = 1760;

		//private const int FeetPerYard = 3;

		//private const double FeetPerMeter = 3.2808399;

		//private const int FeetPerMile = 5280;

		private double _ScaleInMetersPerPixel;

		//private RegionInfo regionInfo;

		//private CultureInfo cultureInfo;

		private double _CurrentMetersPerPixel;

		//private double _PreviousMaxWidth;

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.Overlays.Scale.DistanceUnit"></see> dependency property.</summary>
		public readonly static DependencyProperty DistanceUnitProperty;

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.Overlays.Scale.Culture"></see> dependency property.</summary>
		public readonly static DependencyProperty CultureProperty;

		//private Microsoft.Maps.MapControl.WPF.Overlays.OverlayResources overlayResources;

		private readonly static int[] singleDigitValues;

		private readonly static double[] multiDigitValues;

		/// <summary>Gets or sets the culture of the scale bar, which determines the language and default units used by the scale bar.</summary>
		/// <returns>Returns <see cref="T:System.String"></see>.</returns>
		public string Culture
		{
			get
			{
				return (string)base.GetValue(Scale.CultureProperty);
			}
			set
			{
				base.SetValue(Scale.CultureProperty, value);
			}
		}

		/// <summary>Gets or sets the distance unit used by the scale bar.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Overlays.DistanceUnit"></see>.</returns>
		public DistanceUnit DistanceUnit
		{
			get
			{
				return (DistanceUnit)base.GetValue(Scale.DistanceUnitProperty);
			}
			set
			{
				base.SetValue(Scale.DistanceUnitProperty, value);
			}
		}

		/// <summary>Gets or sets the meters per pixel to display on the scale bar.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double MetersPerPixel
		{
			get
			{
				return this._ScaleInMetersPerPixel;
			}
			internal set
			{
				this._ScaleInMetersPerPixel = value;
				this.OnPerPixelChanged();
			}
		}

		public static PropertyChangedCallback OnUnitChanged { get; private set; }

		//private Microsoft.Maps.MapControl.WPF.Overlays.OverlayResources OverlayResources
		//{
		//	get
		//	{
		//		if (this.overlayResources == null)
		//		{
		//			this.overlayResources = ResourceUtility.GetResource<Microsoft.Maps.MapControl.WPF.Overlays.OverlayResources, OverlayResourcesHelper>((!string.IsNullOrEmpty(this.Culture) ? this.Culture : CultureInfo.CurrentUICulture.Name));
		//		}
		//		return this.overlayResources;
		//	}
		//}

		//static Scale()
		//{
		//	Scale.DistanceUnitProperty = DependencyProperty.Register("DistanceUnit", typeof(DistanceUnit), typeof(Scale), new PropertyMetadata(new PropertyChangedCallback(Scale.OnUnitChanged)));
		//	Scale.CultureProperty = DependencyProperty.Register("Culture", typeof(string), typeof(Scale), new PropertyMetadata(new PropertyChangedCallback(Scale.OnCultureChanged)));
		//	Scale.singleDigitValues = new int[] { 5, 2 };
		//	Scale.multiDigitValues = new double[] { 5, 2.5, 2 };
		//}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.Overlays.Scale"></see> class.</summary>
		public Scale()
		{
			this.InitializeComponent();
			base.LayoutUpdated += new EventHandler(this.Scale_LayoutUpdated);
		}

		//private static int GetMultiDigitValue(double value, double exponentOf10)
		//{
		//	double[] numArray = Scale.multiDigitValues;
		//	for (int i = 0; i < (int)numArray.Length; i++)
		//	{
		//		double num = (double)numArray[i];
		//		if (value > num)
		//		{
		//			return (int)(num * exponentOf10);
		//		}
		//	}
		//	return (int)exponentOf10;
		//}

		//private static int GetSingleDigitValue(double value)
		//{
		//	int num = (int)Math.Floor(value);
		//	int[] numArray = Scale.singleDigitValues;
		//	for (int i = 0; i < (int)numArray.Length; i++)
		//	{
		//		int num1 = numArray[i];
		//		if (num > num1)
		//		{
		//			return num1;
		//		}
		//	}
		//	return 1;
		//}

		//private static int LargestNiceNumber(double dIn)
		//{
		//	double num = Math.Log(dIn) / Math.Log(10);
		//	double num1 = Math.Pow(10, Math.Floor(num));
		//	double num2 = dIn / num1;
		//	if (1 == num1)
		//	{
		//		return Scale.GetSingleDigitValue(num2);
		//	}
		//	return Scale.GetMultiDigitValue(num2, num1);
		//}

		private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Scale)d).OnCultureChanged();
		}

		/// <summary>Enables derived classes to provide custom handling when the culture of the scale bar changes.</summary>
		protected virtual void OnCultureChanged()
		{
			//if (!string.IsNullOrEmpty(this.Culture))
			//{
			//	this.regionInfo = ResourceUtility.GetRegionInfo(this.Culture);
			//	this.cultureInfo = ResourceUtility.GetCultureInfo(this.Culture);
			//	this.overlayResources = ResourceUtility.GetResource<Microsoft.Maps.MapControl.WPF.Overlays.OverlayResources, OverlayResourcesHelper>(this.Culture);
			//}
			//else
			//{
			//	this.regionInfo = null;
			//	this.cultureInfo = null;
			//	this.overlayResources = ResourceUtility.GetResource<Microsoft.Maps.MapControl.WPF.Overlays.OverlayResources, OverlayResourcesHelper>(CultureInfo.CurrentUICulture.Name);
			//}
			this.Refresh();
		}

		/// <summary>Enables derived classes to provide custom handling when the <see cref="P:Microsoft.Maps.MapControl.Overlays.WPF.Scale.MetersPerPixel"></see> value of the scale bar changes.</summary>
		protected virtual void OnPerPixelChanged()
		{
			this.SetScaling(this.MetersPerPixel);
		}

		//private static void OnUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//	((Scale)d).OnUnitChanged();
		//}

		///// <summary>Enables derived classes to provide custom handling when the units used by the scale bar changes.</summary>
		//protected virtual void OnUnitChanged()
		//{
		//	this.Refresh();
		//}

		private void Refresh()
		{
			if (this._CurrentMetersPerPixel > 0)
			{
				this.SetScaling(this._CurrentMetersPerPixel);
			}
		}

		private void Scale_LayoutUpdated(object sender, EventArgs e)
		{
		//	if (this._PreviousMaxWidth != base.MaxWidth)
		//	{
		//		this.Refresh();
		//	}
		}

		private void SetScaling(double metersPerPixel)
		{
		//	if (base.Visibility == System.Windows.Visibility.Visible && metersPerPixel > 0)
		//	{
		//		CultureInfo cultureInfo = (this.cultureInfo != null ? this.cultureInfo : CultureInfo.CurrentUICulture);
		//		Microsoft.Maps.MapControl.WPF.Overlays.DistanceUnit distanceUnit = this.DistanceUnit;
		//		if (distanceUnit == Microsoft.Maps.MapControl.WPF.Overlays.DistanceUnit.Default)
		//		{
		//			distanceUnit = (((this.regionInfo != null ? this.regionInfo : RegionInfo.CurrentRegion)).IsMetric ? Microsoft.Maps.MapControl.WPF.Overlays.DistanceUnit.KilometersMeters : Microsoft.Maps.MapControl.WPF.Overlays.DistanceUnit.MilesFeet);
		//		}
		//		double maxWidth = base.MaxWidth;
		//		this._PreviousMaxWidth = maxWidth;
		//		if (Microsoft.Maps.MapControl.WPF.Overlays.DistanceUnit.KilometersMeters != distanceUnit)
		//		{
		//			double num = metersPerPixel * 3.2808399;
		//			double num1 = num * maxWidth;
		//			if (num1 > 5280)
		//			{
		//				int num2 = Scale.LargestNiceNumber(num1 / 5280);
		//				int num3 = (int)((double)(num2 * 5280) / num);
		//				string str = (num2 == 1 ? this.OverlayResources.MilesSingular : this.OverlayResources.MilesPlural);
		//				object[] objArray = new object[] { num2 };
		//				this.SetScaling(num3, string.Format(cultureInfo, str, objArray));
		//			}
		//			else if (Microsoft.Maps.MapControl.WPF.Overlays.DistanceUnit.MilesFeet != distanceUnit)
		//			{
		//				int num4 = Scale.LargestNiceNumber(num1 / 3);
		//				int num5 = (int)((double)(num4 * 3) / num);
		//				string str1 = (num4 == 1 ? this.OverlayResources.YardsSingular : this.OverlayResources.YardsPlural);
		//				object[] objArray1 = new object[] { num4 };
		//				this.SetScaling(num5, string.Format(cultureInfo, str1, objArray1));
		//			}
		//			else
		//			{
		//				int num6 = Scale.LargestNiceNumber(num1);
		//				int num7 = (int)((double)num6 / num);
		//				string str2 = (num6 == 1 ? this.OverlayResources.FeetSingular : this.OverlayResources.FeetPlural);
		//				object[] objArray2 = new object[] { num6 };
		//				this.SetScaling(num7, string.Format(cultureInfo, str2, objArray2));
		//			}
		//		}
		//		else
		//		{
		//			double num8 = metersPerPixel * maxWidth;
		//			if (num8 <= 1000)
		//			{
		//				int num9 = Scale.LargestNiceNumber(num8);
		//				int num10 = (int)((double)num9 / metersPerPixel);
		//				string str3 = (num9 == 1 ? this.OverlayResources.MetersSingular : this.OverlayResources.MetersPlural);
		//				object[] objArray3 = new object[] { num9 };
		//				this.SetScaling(num10, string.Format(cultureInfo, str3, objArray3));
		//			}
		//			else
		//			{
		//				int num11 = Scale.LargestNiceNumber(num8 / 1000);
		//				int num12 = (int)((double)(num11 * 1000) / metersPerPixel);
		//				string str4 = (num11 == 1 ? this.OverlayResources.KilometersSingular : this.OverlayResources.KilometersPlural);
		//				object[] objArray4 = new object[] { num11 };
		//				this.SetScaling(num12, string.Format(cultureInfo, str4, objArray4));
		//			}
		//		}
		//		this._CurrentMetersPerPixel = metersPerPixel;
		//	}
		}

		//private void SetScaling(int pixels, string text)
		//{
		//	double num = (double)pixels;
		//	Thickness margin = this.ScaleRectangle.Margin;
		//	base.Width = num + margin.Left + this.ScaleRectangle.Margin.Right;
		//	this.ScaleString.Text = text;
		//	this.ScaleRectangle.Width = (double)pixels;
		//}
	}
}