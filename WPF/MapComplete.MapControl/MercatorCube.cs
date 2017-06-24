using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal sealed class MercatorCube : LocationConverter
	{
		//public const double MercatorLatitudeLimit = 85.051128;

		//public const double OneMeterAsVectorDistanceAtEquator = 2.4953202336653371E-08;

		private static MercatorCube _instance = new MercatorCube();

		public static MercatorCube Instance
		{
			get
			{
				return MercatorCube._instance;
			}
		}

		//public override CoordinateSystemDirection Direction
		//{
		//	get
		//	{
		//		return CoordinateSystemDirection.LeftHanded;
		//	}
		//}

		//private MercatorCube()
		//{
		//}

		public override Location ToLocation(Point3D vector)
		{
			double latitude = this.YToLatitude(vector.Y);
			return new Location(latitude, (vector.X - 0.5) * 360.0, this.VectorDistanceToMetersAtLatitude(latitude, vector.Z), AltitudeReference.Ellipsoid);
		}

		public override Location ToLocation(Point3D vector, Point3D wrappingCenter)
		{
			vector.X = MercatorCube.WrapX(vector.X, wrappingCenter.X);
			return this.ToLocation(vector);
		}

		public override Point3D FromLocation(Location location)
		{
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			return new Point3D(location.Longitude / 360.0 + 0.5, this.LatitudeToY(location.Latitude), this.MetersToVectorDistanceAtLatitude(location.Latitude, location.Altitude));
		}

		public override Point3D FromLocation(Location location, Point3D wrappingCenter)
		{
			Point3D result = this.FromLocation(location);
			result.X = MercatorCube.WrapX(result.X, wrappingCenter.X);
			return result;
		}

		//public override Point3D GetUpVector(Point3D vector)
		//{
		//	return new Point3D(0.0, 0.0, 1.0);
		//}

		//public override Point3D GetNorthVector(Point3D vector)
		//{
		//	return new Point3D(0.0, -1.0, 0.0);
		//}

		//public override Point3D ChangeAltitude(Point3D vector, double altitude)
		//{
		//	return new Point3D(vector.X, vector.Y, this.MetersToVectorDistance(vector, altitude));
		//}

		//public override double VectorDistanceToMeters(Point3D vector, double distance)
		//{
		//	double latitude = this.YToLatitude(vector.Y);
		//	return this.VectorDistanceToMetersAtLatitude(latitude, distance);
		//}

		//public override double MetersToVectorDistance(Point3D vector, double meters)
		//{
		//	double latitude = this.YToLatitude(vector.Y);
		//	return this.MetersToVectorDistanceAtLatitude(latitude, meters);
		//}

		public double VectorDistanceToMetersAtLatitude(double latitude, double distance)
		{
			if (Math.Abs(distance) <= 4.94065645841247E-324)
			{
				return 0.0;
			}
			return distance * Math.Cos(latitude * 0.017453292519943295) / 2.4953202336653371E-08;
		}

		public double MetersToVectorDistanceAtLatitude(double latitude, double meters)
		{
			if (Math.Abs(meters) <= 4.94065645841247E-324)
			{
				return 0.0;
			}
			return meters * 2.4953202336653371E-08 / Math.Cos(latitude * 0.017453292519943295);
		}

		public double YToLatitude(double y)
		{
			return 90.0 - 2.0 * Math.Atan(Math.Exp((y * 2.0 - 1.0) * 3.1415926535897931)) * 57.295779513082323;
		}

		public double LatitudeToY(double latitude)
		{
			if (latitude >= 85.051128)
			{
				return 0.0;
			}
			if (latitude <= -85.051128)
			{
				return 1.0;
			}
			double num = Math.Sin(latitude * 0.017453292519943295);
			return 0.5 - Math.Log((1.0 + num) / (1.0 - num)) / 12.566370614359172;
		}

		public static double WrapX(double x, double wrappingCenterX)
		{
			if (x < wrappingCenterX - 0.5 || x > wrappingCenterX + 0.5)
			{
				return x - Math.Floor(x - wrappingCenterX + 0.5);
			}
			return x;
		}
	}
}
