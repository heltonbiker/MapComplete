using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Microsoft.Maps.MapControl.WPF.Overlays
{
	public partial class Compass : UserControl
	{
		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.Overlays.Compass.Heading"></see>  dependency property.</summary>
		public readonly static DependencyProperty HeadingProperty;

		/// <summary>Gets or sets the compass heading.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double Heading
		{
			get
			{
				return (double)base.GetValue(Compass.HeadingProperty);
			}
			set
			{
				base.SetValue(Compass.HeadingProperty, value);
			}
		}

		static Compass()
		{
			Compass.HeadingProperty = DependencyProperty.Register("Heading", typeof(double), typeof(Compass), new PropertyMetadata((object)0));
		}

		/// <summary>Initializes a new instance of the <see cref="T: Microsoft.Maps.MapControl.WPF.Overlays.Compass"></see> class.</summary>
		public Compass()
		{
			this.InitializeComponent();
		}
	}
}