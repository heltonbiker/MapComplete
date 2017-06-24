using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Maps.MapControl.WPF.Design
{
	/// <summary>Contains methods that parse a string into a <see cref="T:Microsoft.Maps.MapControl.WPF.PositionOrigin"></see>.</summary>
	public class PositionOriginConverter : TypeConverter
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.Design.PositionOriginConverter"></see> class.</summary>
		public PositionOriginConverter()
		{
		}

		/// <summary>Determines whether the specified object can be converted to a <see cref="T:Microsoft.Maps.MapControl.WPF.PositionOrigin"></see>.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="sourceType">The type of the object to convert.</param>
		/// <param name="context">The context to use in the conversion.</param>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		/// <summary>Converts the specified object to a <see cref="T:Microsoft.Maps.MapControl.PositionOrigin"></see>.</summary>
		/// <returns>Returns <see cref="T:System.Object"></see>.</returns>
		/// <param name="context">The context to use in the conversion.</param>
		/// <param name="value">The object to convert.</param>
		/// <param name="culture">The culture to use in the conversion.</param>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string str = value as string;
			if (str == null)
			{
				throw new NotSupportedException(ExceptionStrings.TypeConverter_InvalidPositionOriginFormat);
			}
			string[] strArrays = str.Split(new char[] { ',' });
			if ((int)strArrays.Length == 2)
			{
				return new PositionOrigin(double.Parse(strArrays[0], CultureInfo.InvariantCulture), double.Parse(strArrays[1], CultureInfo.InvariantCulture));
			}
			FieldInfo field = typeof(PositionOrigin).GetField(str, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
			if (field == null)
			{
				throw new FormatException(ExceptionStrings.TypeConverter_InvalidPositionOriginFormat);
			}
			return field.GetValue(null);
		}
	}
}