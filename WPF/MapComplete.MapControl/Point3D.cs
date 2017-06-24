using System;
using System.Globalization;
using System.Windows;

namespace Microsoft.Maps.MapExtras
{
	internal struct Point3D
	{
		public static readonly Point3D Empty = new Point3D(0.0, 0.0, 0.0);

		public double X;

		public double Y;

		public double Z;

		public Point3D(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public Point3D(Point point, double z)
		{
			this.X = point.X;
			this.Y = point.Y;
			this.Z = z;
		}

		public Point ToPoint()
		{
			return new Point(this.X, this.Y);
		}

		public static bool operator ==(Point3D point1, Point3D point2)
		{
			return point1.X == point2.X && point1.Y == point2.Y && point1.Z == point2.Z;
		}

		public static Point3D operator /(Point3D val, double div)
		{
			Point3D result = new Point3D(val.X / div, val.Y / div, val.Z / div);
			return result;
		}

		public static bool operator !=(Point3D point1, Point3D point2)
		{
			return !(point1 == point2);
		}

		public bool Equals(Point3D other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		public static Point3D Add(ref Point3D a, ref Point3D b)
		{
			return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public void Add(Point3D val)
		{
			this.X += val.X;
			this.Y += val.Y;
			this.Z += val.Z;
		}

		public static Point3D Subtract(ref Point3D a, ref Point3D b)
		{
			return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is Point3D && this.Equals((Point3D)obj);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", new object[]
			{
				this.X,
				this.Y,
				this.Z
			});
		}
	}
}
