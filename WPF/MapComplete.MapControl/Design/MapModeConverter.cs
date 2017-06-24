using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Maps.MapControl.WPF.Design
{
	public class MapModeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				PropertyInfo[] properties = typeof(MapModes).GetProperties();
				for (int i = 0; i < properties.Length; i++)
				{
					PropertyInfo propertyInfo = properties[i];
					if (string.Equals(propertyInfo.Name, text, StringComparison.OrdinalIgnoreCase) || string.Equals(propertyInfo.PropertyType.FullName, text, StringComparison.OrdinalIgnoreCase))
					{
						return propertyInfo.GetValue(null, null);
					}
				}
				throw new FormatException(ExceptionStrings.TypeConverter_InvalidMapMode);
			}
			throw new NotSupportedException(ExceptionStrings.TypeConverter_InvalidMapMode);
		}
	}
}
