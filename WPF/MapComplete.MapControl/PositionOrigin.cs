using Microsoft.Maps.MapControl.WPF.Design;
using System;
using System.ComponentModel;

namespace Microsoft.Maps.MapControl.WPF
{
	/// <summary>Represents the anchor point of UIElement, such as an image, at a <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see> on a <see cref="T:Microsoft.Maps.MapControl.WPF.MapLayer"></see>.</summary>
	[TypeConverter(typeof(PositionOriginConverter))]
	public struct PositionOrigin
	{
		private double x;

		private double y;

		/// <summary>Specifies the top left of the position.</summary>
		public readonly static PositionOrigin TopLeft;

		/// <summary>Specifies the top center of the position.</summary>
		public readonly static PositionOrigin TopCenter;

		/// <summary>Specifies the top right of the position.</summary>
		public readonly static PositionOrigin TopRight;

		/// <summary>Specifies the center left of the position.</summary>
		public readonly static PositionOrigin CenterLeft;

		/// <summary>Specifies the center of the position.</summary>
		public readonly static PositionOrigin Center;

		/// <summary>Specifies the center right of the position.</summary>
		public readonly static PositionOrigin CenterRight;

		/// <summary>Specifies the bottom left of the position.</summary>
		public readonly static PositionOrigin BottomLeft;

		/// <summary>Specifies the bottom center of the position.</summary>
		public readonly static PositionOrigin BottomCenter;

		/// <summary>Specifies the bottom right of the position.</summary>
		public readonly static PositionOrigin BottomRight;

		/// <summary>Gets or sets the x-axis position of the position origin.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		/// <summary>Gets or sets the y-axis position of the position origin.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		static PositionOrigin()
		{
			PositionOrigin.TopLeft = new PositionOrigin(0, 0);
			PositionOrigin.TopCenter = new PositionOrigin(0.5, 0);
			PositionOrigin.TopRight = new PositionOrigin(1, 0);
			PositionOrigin.CenterLeft = new PositionOrigin(0, 0.5);
			PositionOrigin.Center = new PositionOrigin(0.5, 0.5);
			PositionOrigin.CenterRight = new PositionOrigin(1, 0.5);
			PositionOrigin.BottomLeft = new PositionOrigin(0, 1);
			PositionOrigin.BottomCenter = new PositionOrigin(0.5, 1);
			PositionOrigin.BottomRight = new PositionOrigin(1, 1);
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.PositionOrigin"></see> class.</summary>
		/// <param name="horizontalOrigin">The position of the origin along the x-axis.</param>
		/// <param name="verticalOrigin">The position of the origin along the y-axis.</param>
		public PositionOrigin(double horizontalOrigin, double verticalOrigin)
		{
			this.x = horizontalOrigin;
			this.y = verticalOrigin;
		}

		/// <summary>Determines whether the specified object is equal to this position origin.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="obj">The object to compare to.</param>
		public override bool Equals(object obj)
		{
			if (!(obj is PositionOrigin))
			{
				return false;
			}
			return this.Equals((PositionOrigin)obj);
		}

		/// <summary>Determines whether the specified position origin is equal to this position origin.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="origin">The position origin to compare to.</param>
		public bool Equals(PositionOrigin origin)
		{
			if (this.x != origin.x)
			{
				return false;
			}
			return this.y == origin.y;
		}

		/// <summary>Retrieves the hash code for this position origin.</summary>
		/// <returns>Returns <see cref="T:System.Int32"></see>.</returns>
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode();
		}

		/// <summary>Determines whether two position origin objects are equal.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="origin2">The second position origin to compare.</param>
		/// <param name="origin1">The first position origin to compare.</param>
		public static bool operator ==(PositionOrigin origin1, PositionOrigin origin2)
		{
			return origin1.Equals(origin2);
		}

		/// <summary>Determines whether two position origins are not equal.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		/// <param name="origin2">Gets or sets the y-axis position of the position origin.</param>
		/// <param name="origin1">Gets or sets the x-axis position of the position origin.</param>
		public static bool operator !=(PositionOrigin origin1, PositionOrigin origin2)
		{
			return !origin1.Equals(origin2);
		}
	}
}