using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal static class MapMath
	{
		public const int DefaultTileSize = 256;

		public const int NorthUpHeading = 0;

		public const int EastUpHeading = 90;

		public const int SouthUpHeading = 180;

		public const int WestUpHeading = 270;

		public readonly static double EnhancedBirdseyePitch;

		static MapMath()
		{
			MapMath.EnhancedBirdseyePitch = -57.2957795130823 * Math.Asin(0.703125);
		}

		public static void CalculateViewFromLocations(IEnumerable<Location> locations, Size viewportSize, double heading, Thickness margin, out Point centerNormalizedMercator, out double zoomLevel)
		{
			Point point = new Point(double.MaxValue, double.MaxValue);
			Point point1 = new Point(double.MinValue, double.MinValue);
			RotateTransform rotateTransform = new RotateTransform(heading)
			{
				CenterX = 0.5,
				CenterY = 0.5
			};
			RotateTransform rotateTransform1 = rotateTransform;
			foreach (Location location in locations)
			{
				Microsoft.Maps.MapExtras.Point3D point3D = MercatorCube.Instance.FromLocation(location);
				Point point2 = rotateTransform1.Transform(point3D.ToPoint());
				point.X = Math.Min(point.X, point2.X);
				point.Y = Math.Min(point.Y, point2.Y);
				point1.X = Math.Max(point1.X, point2.X);
				point1.Y = Math.Max(point1.Y, point2.Y);
			}
			if (point1.X <= point.X || point1.Y <= point.Y)
			{
				if (point.X != point1.X)
				{
					throw new InvalidOperationException("Must provide at least one location.");
				}
				zoomLevel = 1;
				centerNormalizedMercator = point;
				return;
			}
			double num = Math.Min((viewportSize.Width - margin.Left - margin.Right) / (256 * (point1.X - point.X)), (viewportSize.Height - margin.Top - margin.Bottom) / (256 * (point1.Y - point.Y)));
			zoomLevel = Math.Log(num, 2);
			double num1 = 1 / (256 * Math.Pow(2, zoomLevel));
			centerNormalizedMercator = rotateTransform1.Inverse.Transform(new Point((point1.X + num1 * margin.Right + point.X - num1 * margin.Left) / 2, (point1.Y + num1 * margin.Bottom + point.Y - num1 * margin.Top) / 2));
		}

		public static Location GetMercatorCenter(LocationRect boundingRectangle)
		{
			Microsoft.Maps.MapExtras.Point3D point3D = MercatorCube.Instance.FromLocation(boundingRectangle.Northwest);
			Point point = point3D.ToPoint();
			Microsoft.Maps.MapExtras.Point3D point3D1 = MercatorCube.Instance.FromLocation(boundingRectangle.Southeast);
			Point point1 = point3D1.ToPoint();
			Point point2 = VectorMath.Multiply(VectorMath.Add(point, point1), 0.5);
			Location location = MercatorCube.Instance.ToLocation(new Microsoft.Maps.MapExtras.Point3D(point2, 0));
			location = new Location(location.Latitude, location.Longitude, boundingRectangle.Center.Altitude, boundingRectangle.Center.AltitudeReference);
			return location;
		}

		public static double LocationRectToMercatorZoomLevel(Size viewportSize, LocationRect boundingRectangle)
		{
			Microsoft.Maps.MapExtras.Point3D point3D = MercatorCube.Instance.FromLocation(boundingRectangle.Northwest);
			Point point = point3D.ToPoint();
			Microsoft.Maps.MapExtras.Point3D point3D1 = MercatorCube.Instance.FromLocation(boundingRectangle.Southeast);
			Rect rect = new Rect(point, point3D1.ToPoint());
			double num = Math.Min(viewportSize.Width / (256 * rect.Width), viewportSize.Height / (256 * rect.Height));
			return Math.Log(num, 2);
		}

		internal static Rect LocationToViewportPoint(ref Matrix3D normalizedMercatorToViewport, LocationRect boundingRectangle)
		{
			Microsoft.Maps.MapExtras.Point3D point3D = MercatorCube.Instance.FromLocation(boundingRectangle.Northwest);
			System.Windows.Media.Media3D.Point3D point3D1 = normalizedMercatorToViewport.Transform(new System.Windows.Media.Media3D.Point3D(point3D.X, point3D.Y, 0));
			Microsoft.Maps.MapExtras.Point3D point3D2 = MercatorCube.Instance.FromLocation(boundingRectangle.Southeast);
			System.Windows.Media.Media3D.Point3D point3D3 = normalizedMercatorToViewport.Transform(new System.Windows.Media.Media3D.Point3D(point3D2.X, point3D2.Y, 0));
			return new Rect(new Point(point3D1.X, point3D1.Y), new Point(point3D3.X, point3D3.Y));
		}

		internal static bool LocationToViewportPoint(ref Matrix3D normalizedMercatorToViewport, Location location, out Point viewportPosition)
		{
			Microsoft.Maps.MapExtras.Point3D point3D = MercatorCube.Instance.FromLocation(location);
			System.Windows.Media.Media3D.Point3D point3D1 = normalizedMercatorToViewport.Transform(new System.Windows.Media.Media3D.Point3D(point3D.X, point3D.Y, 0));
			viewportPosition = new Point(point3D1.X, point3D1.Y);
			return true;
		}

		public static double MercatorZoomLevelToScale(double zoomLevel, double latitude)
		{
			double num = 40075016.6855785 / (Math.Pow(2, zoomLevel) * 256);
			return Math.Cos(latitude * 57.2957795130823) * num;
		}

		public static Location NormalizeLocation(Location location)
		{
			return new Location(Math.Min(Math.Max(location.Latitude, -90), 90), Location.NormalizeLongitude(location.Longitude), location.Altitude, location.AltitudeReference);
		}

		public static LocationRect NormalizeLocationRect(LocationRect locaitonRect)
		{
			Location location = MapMath.NormalizeLocation(locaitonRect.Center);
			double width = locaitonRect.Width;
			double height = locaitonRect.Height;
			if (width >= 360)
			{
				width = 360;
				location.Longitude = 0;
			}
			if (height >= 180)
			{
				height = 180;
				location.Latitude = 0;
			}
			return new LocationRect(location, width, height);
		}

		public static double ScaleToMercatorZoomLevel(double scale, double latitude)
		{
			double num = scale / Math.Cos(latitude * 57.2957795130823);
			return Math.Log(40075016.6855785 / (num * 256), 2);
		}

		public static int SnapToCardinalHeading(double heading)
		{
			int num = (int)Math.Round(heading / 90);
			return (num % 4 + 4) % 4 * 90;
		}

		internal static bool TryLocationToViewportPoint(ref Matrix3D normalizedMercatorToViewport, Location location, out Point viewportPosition)
		{
			Microsoft.Maps.MapExtras.Point3D point3D = MercatorCube.Instance.FromLocation(location);
			System.Windows.Media.Media3D.Point3D point3D1 = normalizedMercatorToViewport.Transform(new System.Windows.Media.Media3D.Point3D(point3D.X, point3D.Y, 0));
			viewportPosition = new Point(point3D1.X, point3D1.Y);
			return true;
		}
	}
}