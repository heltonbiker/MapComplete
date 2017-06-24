using Microsoft.Maps.MapControl.WPF.Core;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Microsoft.Maps.MapControl.WPF.Design
{
	internal class ShadowBrushConverter : IValueConverter
	{
		public ShadowBrushConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				ModeBackground modeBackground = (ModeBackground)value;
				Brush brush = null;
				bool upper = parameter.ToString().ToUpper(CultureInfo.InvariantCulture) == "TOP";
				if (modeBackground != ModeBackground.Dark)
				{
					brush = (upper ? Brushes.Black : Brushes.White);
				}
				else
				{
					brush = (upper ? Brushes.White : Brushes.Black);
				}
				obj = brush;
			}
			catch
			{
				obj = null;
			}
			return obj;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}