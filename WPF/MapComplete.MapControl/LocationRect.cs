using Microsoft.Maps.MapControl.WPF.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Maps.MapControl.WPF
{
	/// <summary>Represents a rectangle on the map.</summary>
	[TypeConverter(typeof(LocationRectConverter))]
	public class LocationRect : IFormattable
	{
		private Location center;

		private double halfWidth;

		private double halfHeight;

		/// <summary>Gets the location of the center of the rectangle.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		public Location Center
		{
			get
			{
				return this.center;
			}
		}

		/// <summary>Gets or sets the longitude of the eastern border of the rectangle.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double East
		{
			get
			{
				if (this.halfWidth == 180)
				{
					return 180;
				}
				return Location.NormalizeLongitude(this.center.Longitude + this.halfWidth);
			}
			set
			{
				this.Init(this.North, this.West, this.South, value);
			}
		}

		/// <summary>Gets the height of the location rectangle.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double Height
		{
			get
			{
				return this.halfHeight * 2;
			}
		}

		/// <summary>Gets or sets the latitude of the northern border of the rectangle.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double North
		{
			get
			{
				return this.center.Latitude + this.halfHeight;
			}
			set
			{
				this.Init(value, this.West, this.South, this.East);
			}
		}

		/// <summary>Gets or sets the location of the northeast corner of the rectangle.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		public Location Northeast
		{
			get
			{
				return new Location(this.North, this.East);
			}
			set
			{
				if (this.center == null)
				{
					this.Init(value.Latitude, value.Longitude, value.Latitude, value.Longitude);
					return;
				}
				this.Init(value.Latitude, this.West, this.South, value.Longitude);
			}
		}

		/// <summary>Gets the location or the northwest corner of the rectangle.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		public Location Northwest
		{
			get
			{
				return new Location(this.North, this.West);
			}
		}

		/// <summary>Gets or sets the latitude of the southern border of the rectangle.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double South
		{
			get
			{
				return this.center.Latitude - this.halfHeight;
			}
			set
			{
				this.Init(this.North, this.West, value, this.East);
			}
		}

		/// <summary>Gets the location of the southeast corner of the rectangle.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		public Location Southeast
		{
			get
			{
				return new Location(this.South, this.East);
			}
		}

		/// <summary>Gets or sets the location of the southwest corner of the rectangle.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		public Location Southwest
		{
			get
			{
				return new Location(this.South, this.West);
			}
			set
			{
				if (this.center == null)
				{
					this.Init(value.Latitude, value.Longitude, value.Latitude, value.Longitude);
					return;
				}
				this.Init(this.North, value.Longitude, value.Latitude, this.East);
			}
		}

		/// <summary>Gets or sets the longitude of the western border of the rectangle.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double West
		{
			get
			{
				if (this.halfWidth == 180)
				{
					return -180;
				}
				return Location.NormalizeLongitude(this.center.Longitude - this.halfWidth);
			}
			set
			{
				this.Init(this.North, value, this.South, this.East);
			}
		}

		/// <summary>Gets or sets the width of the rectangle.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double Width
		{
			get
			{
				return this.halfWidth * 2;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see> class.</summary>
		public LocationRect()
		{
			this.center = new Location(0, 0);
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see> class, centered on the specified location.</summary>
		/// <param name="width">The width of the rectangle.</param>
		/// <param name="height">The height of the rectangle.</param>
		/// <param name="center">The location of the center of the rectangle.</param>
		public LocationRect(Location center, double width, double height)
		{
			this.center = center;
			this.halfWidth = width / 2;
			this.halfHeight = height / 2;
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see> class using the specified borders.</summary>
		/// <param name="east">The latitude of the eastern border of the rectangle.</param>
		/// <param name="west">The latitude of the western border of the rectangle.</param>
		/// <param name="south">The latitude of the southern border of the rectangle.</param>
		/// <param name="north">The latitude of the northern border of the rectangle.</param>
		public LocationRect(double north, double west, double south, double east) : this()
		{
			this.Init(north, west, south, east);
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see> class using the specified locations as northwest and southeast corners of the rectangle.</summary>
		/// <param name="corner1">The location of the northwest corner of the rectangle.</param>
		/// <param name="corner2">The location of the southeast corner of the rectangle.</param>
		public LocationRect(Location corner1, Location corner2) : this(new Location[] { corner1, corner2 })
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see> class using the specified collection of locations.</summary>
		/// <param name="locations">A location collection containing the corners of the rectangle.</param>
		public LocationRect(IList<Location> locations) : this()
		{
			double num = -90;
			double num1 = 90;
			double num2 = 180;
			double num3 = -180;
			foreach (Location location in locations)
			{
				num = Math.Max(num, location.Latitude);
				num1 = Math.Min(num1, location.Latitude);
				num2 = Math.Min(num2, location.Longitude);
				num3 = Math.Max(num3, location.Longitude);
			}
			this.Init(num, num2, num1, num3);
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see> class.</summary>
		/// <param name="rect">The location rectangle to use.</param>
		public LocationRect(LocationRect rect)
		{
			this.center = new Location(rect.center);
			this.halfHeight = rect.halfHeight;
			this.halfWidth = rect.halfWidth;
		}

		/// <summary>Determines whether this location rectangle is equal to the specified object.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="obj">The object to use.</param>
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is LocationRect))
			{
				return false;
			}
			return this == (LocationRect)obj;
		}

		/// <summary>Retrieves the hash code for this location rectangle.</summary>
		/// <returns>Returns <see cref="T:System.Int32"></see>.</returns>
		public override int GetHashCode()
		{
			return this.center.GetHashCode() ^ this.halfWidth.GetHashCode() ^ this.halfHeight.GetHashCode();
		}

		private void Init(double north, double west, double south, double east)
		{
			if (west > east)
			{
				east = east + 360;
			}
			this.center = new Location((south + north) / 2, (west + east) / 2);
			this.halfHeight = (north - south) / 2;
			this.halfWidth = Math.Abs(east - west) / 2;
		}

		/// <summary>Retrieves the intersection rectangle of this location rectangle and the specified location rectangle.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see>.</returns>
		/// <param name="rect">The location rectangle to use.</param>
		public LocationRect Intersection(LocationRect rect)
		{
			LocationRect locationRect = new LocationRect();
			if (this.Intersects(rect))
			{
				double longitude = this.center.Longitude - this.halfWidth;
				double num = rect.center.Longitude - rect.halfWidth;
				double longitude1 = this.center.Longitude + this.halfWidth;
				double num1 = rect.center.Longitude + rect.halfWidth;
				if (Math.Abs(this.center.Longitude - rect.center.Longitude) > 180)
				{
					if (this.center.Longitude >= rect.center.Longitude)
					{
						num = num + 360;
						num1 = num1 + 360;
					}
					else
					{
						longitude = longitude + 360;
						longitude1 = longitude1 + 360;
					}
				}
				double num2 = Math.Max(longitude, num);
				double num3 = Math.Min(longitude1, num1);
				double num4 = Math.Min(this.North, rect.North);
				double num5 = Math.Max(this.South, rect.South);
				locationRect = new LocationRect(new Location((num4 + num5) / 2, Location.NormalizeLongitude((num2 + num3) / 2)), num3 - num2, num4 - num5);
			}
			return locationRect;
		}

		/// <summary>Determines whether this location rectangle intersects with the specified location rectangle.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="rect">The location rectangle to use.</param>
		public bool Intersects(LocationRect rect)
		{
			double num = Math.Abs(this.center.Latitude - rect.center.Latitude);
			double num1 = Math.Abs(this.center.Longitude - rect.center.Longitude);
			if (num1 > 180)
			{
				num1 = 360 - num1;
			}
			if (num > this.halfHeight + rect.halfHeight)
			{
				return false;
			}
			return num1 <= this.halfWidth + rect.halfWidth;
		}

		/// <summary>Determines whether two location rectangle instances are equal.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="rect2">The second location rectangle to compare.</param>
		/// <param name="rect1">The first location rectangle to compare.</param>
		public static bool operator ==(LocationRect rect1, LocationRect rect2)
		{
			if (object.ReferenceEquals(rect1, rect2))
			{
				return true;
			}
			if (rect1 == null || rect2 == null)
			{
				return false;
			}
			if (!(rect1.center == rect2.center) || rect1.halfWidth != rect2.halfWidth)
			{
				return false;
			}
			return rect1.halfHeight == rect2.halfHeight;
		}

		/// <summary>Determines whether two location rectangle instances are not equal.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="rect2">The second location rectangle to compare.</param>
		/// <param name="rect1">The first location rectangle to compare.</param>
		public static bool operator !=(LocationRect rect1, LocationRect rect2)
		{
			return !(rect1 == rect2);
		}

		string System.IFormattable.ToString(string format, IFormatProvider provider)
		{
			string[] strArrays = new string[] { "{0:", format, "} {1:", format, "}" };
			string str = string.Concat(strArrays);
			object[] northwest = new object[] { this.Northwest, this.Southeast };
			return string.Format(provider, str, northwest);
		}

		/// <summary>Converts the location rectangle to a formatted string containing the latitude, longitude, and altitude values of its corners.</summary>
		/// <returns>Returns <see cref="T:System.String"></see>.</returns>
		public override string ToString()
		{
			return ((IFormattable)this).ToString(null, null);
		}

		/// <summary>Converts the location rectangle to a formatted string containing the latitude, longitude, and altitude values of its corners using a given format provider.</summary>
		/// <returns>Returns <see cref="T:System.String"></see>.</returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"></see> implementation that supplies culture-specific formatting information.</param>
		public string ToString(IFormatProvider provider)
		{
			return ((IFormattable)this).ToString(null, provider);
		}
	}
}