using System;
using System.ComponentModel;

namespace Microsoft.Maps.MapControl.WPF
{
	//[TypeConverter(typeof(LocationConverter))]
	public class Location : IFormattable
	{
		//public const double MaxLatitude = 90.0;

		//public const double MinLatitude = -90.0;

		//public const double MaxLongitude = 180.0;

		//public const double MinLongitude = -180.0;

		private double latitude;

		private double longitude;

		private double altitude;

		private AltitudeReference altitudeReference;

		public double Latitude
		{
			get
			{
				return this.latitude;
			}
			set
			{
				this.latitude = value;
			}
		}

		public double Longitude
		{
			get
			{
				return this.longitude;
			}
			set
			{
				this.longitude = value;
			}
		}

		public double Altitude
		{
			get
			{
				return this.altitude;
			}
			set
			{
				this.altitude = value;
			}
		}

		public AltitudeReference AltitudeReference
		{
			get
			{
				return this.altitudeReference;
			}
			set
			{
				this.altitudeReference = value;
			}
		}

		public Location() : this(0.0, 0.0, 0.0, AltitudeReference.Ground)
		{
		}

		public Location(double latitude, double longitude) : this(latitude, longitude, 0.0, AltitudeReference.Ground)
		{
		}

		public Location(double latitude, double longitude, double altitude) : this(latitude, longitude, altitude, AltitudeReference.Ground)
		{
		}

		public Location(double latitude, double longitude, double altitude, AltitudeReference altitudeReference)
		{
			this.latitude = latitude;
			this.longitude = longitude;
			this.altitude = altitude;
			this.altitudeReference = altitudeReference;
		}

		public Location(Location location)
		{
			this.Latitude = location.latitude;
			this.Longitude = location.Longitude;
			this.Altitude = location.Altitude;
			this.AltitudeReference = location.AltitudeReference;
		}

		public static double NormalizeLongitude(double longitude)
		{
			if (longitude < -180.0 || longitude > 180.0)
			{
				return longitude - Math.Floor((longitude + 180.0) / 360.0) * 360.0;
			}
			return longitude;
		}

		//public static bool operator ==(Location location1, Location location2)
		//{
		//	return object.ReferenceEquals(location1, location2) || (location1 != null && !(location2 == null) && (Location.IsEqual(location1.Latitude, location2.Latitude) && Location.IsEqual(location1.Longitude, location2.Longitude) && Location.IsEqual(location1.Altitude, location2.Altitude)) && location1.AltitudeReference == location2.AltitudeReference);
		//}

		//public static bool operator !=(Location location1, Location location2)
		//{
		//	return !(location1 == location2);
		//}

		//public override bool Equals(object obj)
		//{
		//	return obj != null && obj is Location && this == (Location)obj;
		//}

		//public override int GetHashCode()
		//{
		//	return this.Latitude.GetHashCode() ^ this.Longitude.GetHashCode() ^ this.Altitude.GetHashCode() ^ this.AltitudeReference.GetHashCode();
		//}

		//public override string ToString()
		//{
		//	return ((IFormattable)this).ToString(null, null);
		//}

		//public string ToString(IFormatProvider provider)
		//{
		//	return ((IFormattable)this).ToString(null, provider);
		//}

		string IFormattable.ToString(string format, IFormatProvider provider)
		{
			return string.Format(provider, string.Concat(new string[]
			{
				"{0:",
				format,
				"},{1:",
				format,
				"},{2:",
				format,
				"}"
			}), new object[]
			{
				this.latitude,
				this.longitude,
				this.altitude
			});
		}

		//private static bool IsEqual(double value1, double value2)
		//{
		//	return Math.Abs(value1 - value2) <= 4.94065645841247E-324;
		//}
	}
}
