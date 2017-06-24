using System;

namespace Microsoft.Maps.MapExtras
{
	internal struct Point4D
	{
		public double X;

		public double Y;

		public double Z;

		public double W;

		public Point4D(double x, double y, double z, double w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		public static bool operator ==(Point4D point0, Point4D point1)
		{
			return point0.X == point1.X && point0.Y == point1.Y && point0.Z == point1.Z && point0.W == point1.W;
		}

		public override bool Equals(object obj)
		{
			return base.GetType() == obj.GetType() && this == (Point4D)obj;
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode() ^ this.W.GetHashCode();
		}

		public static bool operator !=(Point4D point0, Point4D point1)
		{
			return !(point0 == point1);
		}

		public static Point4D operator +(Point4D point0, Point4D point1)
		{
			return new Point4D(point0.X + point1.X, point0.Y + point1.Y, point0.Z + point1.Z, point0.W + point1.W);
		}

		public static Point4D operator -(Point4D point0, Point4D point1)
		{
			return new Point4D(point0.X - point1.X, point0.Y - point1.Y, point0.Z - point1.Z, point0.W - point1.W);
		}

		public static Point4D operator *(double scalar, Point4D point)
		{
			return new Point4D(point.X * scalar, point.Y * scalar, point.Z * scalar, point.W * scalar);
		}

		public static Point4D Lerp(Point4D point0, Point4D point1, double alpha)
		{
			return point0 + alpha * (point1 - point0);
		}
	}
}
