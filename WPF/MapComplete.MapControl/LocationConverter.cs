using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Maps.MapControl.WPF.Design
{
	public class LocationConverter : TypeConverter
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
				string[] array = text.Split(new char[]
				{
					','
				});
				switch (array.Length)
				{
					case 2:
						{
							double latitude;
							double longitude;
							if (double.TryParse(array[0], NumberStyles.Float, CultureInfo.InvariantCulture, out latitude) && double.TryParse(array[1], NumberStyles.Float, CultureInfo.InvariantCulture, out longitude))
							{
								return new Location(latitude, longitude);
							}
							break;
						}
					case 3:
						{
							double latitude2;
							double longitude2;
							double altitude;
							if (double.TryParse(array[0], NumberStyles.Float, CultureInfo.InvariantCulture, out latitude2) && double.TryParse(array[1], NumberStyles.Float, CultureInfo.InvariantCulture, out longitude2) && double.TryParse(array[2], NumberStyles.Float, CultureInfo.InvariantCulture, out altitude))
							{
								return new Location(latitude2, longitude2, altitude);
							}
							break;
						}
					case 4:
						{
							double latitude3;
							double longitude3;
							double altitude2;
							if (double.TryParse(array[0], NumberStyles.Float, CultureInfo.InvariantCulture, out latitude3) && double.TryParse(array[1], NumberStyles.Float, CultureInfo.InvariantCulture, out longitude3) && double.TryParse(array[2], NumberStyles.Float, CultureInfo.InvariantCulture, out altitude2))
							{
								try
								{
									AltitudeReference altitudeReference = (AltitudeReference)Enum.Parse(typeof(AltitudeReference), array[3], true);
									string name = Enum.GetName(typeof(AltitudeReference), altitudeReference);
									if (!string.IsNullOrEmpty(name))
									{
										return new Location(latitude3, longitude3, altitude2, altitudeReference);
									}
								}
								catch (ArgumentException)
								{
								}
								break;
							}
							break;
						}
				}
				throw new FormatException(ExceptionStrings.TypeConverter_InvalidLocationFormat);
			}
			throw new NotSupportedException(ExceptionStrings.TypeConverter_InvalidLocationFormat);
		}
	}
}
