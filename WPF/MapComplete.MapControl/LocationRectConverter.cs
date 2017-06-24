using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Maps.MapControl.WPF.Design
{
	/// <summary>Contains methods that parse a string into a <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see>.</summary>
	public class LocationRectConverter : TypeConverter
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.Design.LocationRectConverter"></see> class.</summary>
		public LocationRectConverter()
		{
		}

		/// <summary>Determines whether the given type can be converted to a <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see>.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="sourceType">The object type from which to convert.</param>
		/// <param name="context">The format context provider of the type.</param>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		/// <summary>Converts the given object to a <see cref="T:Microsoft.Maps.MapControl.LocationRect"></see>.</summary>
		/// <returns>Returns <see cref="T:System.Object"></see>.</returns>
		/// <param name="context">The format context provider of the type.</param>
		/// <param name="value">The object to convert.</param>
		/// <param name="culture">The culture to use in the conversion.</param>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string str = value as string;
			if (str == null)
			{
				throw new NotSupportedException(ExceptionStrings.TypeConverter_InvalidLocationRectFormat);
			}
			string[] strArrays = str.Split(new char[] { ',' });
			if ((int)strArrays.Length != 4)
			{
				throw new FormatException(ExceptionStrings.TypeConverter_InvalidLocationRectFormat);
			}
			return new LocationRect(double.Parse(strArrays[0], CultureInfo.InvariantCulture), double.Parse(strArrays[1], CultureInfo.InvariantCulture), double.Parse(strArrays[2], CultureInfo.InvariantCulture), double.Parse(strArrays[3], CultureInfo.InvariantCulture));
		}
	}
}