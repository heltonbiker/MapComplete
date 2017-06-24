using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Microsoft.Maps.MapControl.WPF.Overlays
{
	public partial class ShadowText : UserControl
	{
		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF,Overlays.ShadowText.Text"></see> dependency property.</summary>
		public readonly static DependencyProperty TextProperty;

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.Overlays.ShadowText.ForegroundTop"></see> dependency property.</summary>
		public readonly static DependencyProperty ForegroundTopProperty;

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.Overlays.WPF.ShadowText.ForegroundBottom"></see> dependency property.</summary>
		public readonly static DependencyProperty ForegroundBottomProperty;

		/// <summary>Gets or sets the color of the shadow behind the text.</summary>
		/// <returns>Returns <see cref="T:System.Windows.Media.Brush"></see>.</returns>
		public Brush ForegroundBottom
		{
			get
			{
				return (Brush)base.GetValue(ShadowText.ForegroundBottomProperty);
			}
			set
			{
				base.SetValue(ShadowText.ForegroundBottomProperty, value);
			}
		}

		/// <summary>Gets or sets the color of the text.</summary>
		/// <returns>Returns <see cref="T:System.Windows.Media.Brush"></see>.</returns>
		public Brush ForegroundTop
		{
			get
			{
				return (Brush)base.GetValue(ShadowText.ForegroundTopProperty);
			}
			set
			{
				base.SetValue(ShadowText.ForegroundTopProperty, value);
			}
		}

		/// <summary>Gets or sets the text of the shadow text control on the map.</summary>
		/// <returns>Returns <see cref="T:System.String"></see>.</returns>
		public string Text
		{
			get
			{
				return (string)base.GetValue(ShadowText.TextProperty);
			}
			set
			{
				base.SetValue(ShadowText.TextProperty, value);
			}
		}

		static ShadowText()
		{
			ShadowText.TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ShadowText), new PropertyMetadata(new PropertyChangedCallback(ShadowText.OnTextChanged)));
			ShadowText.ForegroundTopProperty = DependencyProperty.Register("ForegroundTop", typeof(Brush), typeof(ShadowText), new PropertyMetadata(new PropertyChangedCallback(ShadowText.OnTextChanged)));
			ShadowText.ForegroundBottomProperty = DependencyProperty.Register("ForegroundBottom", typeof(Brush), typeof(ShadowText), new PropertyMetadata(new PropertyChangedCallback(ShadowText.OnTextChanged)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.Overlays.WPF.ShadowText"></see> class.</summary>
		public ShadowText()
		{
			this.InitializeComponent();
		}

		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ShadowText)d).OnTextChanged();
		}

		private void OnTextChanged()
		{
			this.Text1.Text = this.Text;
			this.Text2.Text = this.Text;
			if (this.ForegroundTop != null && this.ForegroundBottom != null)
			{
				this.Text1.Foreground = this.ForegroundBottom;
				this.Text2.Foreground = this.ForegroundTop;
			}
		}
	}
}