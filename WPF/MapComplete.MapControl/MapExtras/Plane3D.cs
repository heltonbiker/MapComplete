using System;
using System.Globalization;

namespace Microsoft.Maps.MapExtras
{
	internal struct Plane3D
	{
		private double _a;

		private double _b;

		private double _c;

		private double _d;

		public double A
		{
			get
			{
				return this._a;
			}
		}

		public double B
		{
			get
			{
				return this._b;
			}
		}

		public double C
		{
			get
			{
				return this._c;
			}
		}

		public double D
		{
			get
			{
				return this._d;
			}
		}

		public Plane3D(double a, double b, double c, double d)
		{
			this._a = a;
			this._b = b;
			this._c = c;
			this._d = d;
		}

		public Plane3D(Point3D point1, Point3D point2, Point3D point3)
		{
			Point3D left = VectorMath.Subtract(point3, point2);
			Point3D right = VectorMath.Subtract(point2, point1);
			Point3D point3D = VectorMath.Cross(left, right);
			point3D = VectorMath.Normalize(point3D);
			this._a = point3D.X;
			this._b = point3D.Y;
			this._c = point3D.Z;
			this._d = VectorMath.Dot(point3D, point1);
		}

		public Plane3D(Point3D point, Point3D normal)
		{
			normal = VectorMath.Normalize(normal);
			this._a = normal.X;
			this._b = normal.Y;
			this._c = normal.Z;
			this._d = VectorMath.Dot(normal, point);
		}

		public static bool operator ==(Plane3D plane1, Plane3D plane2)
		{
			return plane1._a == plane2._a && plane1._b == plane2._b && plane1._c == plane2._c && plane1._d == plane2._d;
		}

		public static bool operator !=(Plane3D plane1, Plane3D plane2)
		{
			return !(plane1 == plane2);
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is Plane3D && this == (Plane3D)obj;
		}

		public override int GetHashCode()
		{
			return this._a.GetHashCode() ^ this._b.GetHashCode() ^ this._c.GetHashCode() ^ this._d.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", new object[]
			{
				this._a,
				this._b,
				this._c,
				this._d
			});
		}
	}
}
