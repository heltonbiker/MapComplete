using System;

namespace Microsoft.Maps.MapExtras
{
	internal struct Point2D
	{
		public double X;

		public double Y;

		public Point2D(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public static Point2D operator +(Point2D point0, Point2D point1)
		{
			return new Point2D(point0.X + point1.X, point0.Y + point1.Y);
		}

		public static Point2D operator -(Point2D point0, Point2D point1)
		{
			return new Point2D(point0.X - point1.X, point0.Y - point1.Y);
		}

		public static Point2D operator *(double scalar, Point2D point)
		{
			return new Point2D(point.X * scalar, point.Y * scalar);
		}

		public static double Dot(Point2D left, Point2D right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		public static Point2D Normalize(Point2D point)
		{
			double num = point.Length();
			if (num == 0.0)
			{
				return point;
			}
			double num2 = 1.0 / num;
			return new Point2D(point.X * num2, point.Y * num2);
		}

		public double Length()
		{
			return Math.Sqrt(this.X * this.X + this.Y * this.Y);
		}

		public double LengthSquared()
		{
			return this.X * this.X + this.Y * this.Y;
		}

		public static double DistanceSquared(Point2D pointA, Point2D pointB)
		{
			double num = pointA.X - pointB.X;
			double num2 = pointA.Y - pointB.Y;
			return num * num + num2 * num2;
		}

		public static double Cross(Point2D left, Point2D right)
		{
			return left.X * right.Y - left.Y * right.X;
		}

		public static Point2D Lerp(Point2D point0, Point2D point1, double alpha)
		{
			return point0 + alpha * (point1 - point0);
		}

		public static bool operator ==(Point2D point0, Point2D point1)
		{
			return point0.X == point1.X && point0.Y == point1.Y;
		}

		public static bool operator !=(Point2D point0, Point2D point1)
		{
			return !(point0 == point1);
		}

		public override bool Equals(object obj)
		{
			return base.GetType() == obj.GetType() && this == (Point2D)obj;
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}
	}
}
