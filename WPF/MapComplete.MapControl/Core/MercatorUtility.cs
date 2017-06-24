using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	internal static class MercatorUtility
	{
		public const double MercatorLatitudeLimit = 85.051128;

		public const double EarthRadiusInMeters = 6378137;

		public const double EarthCircumferenceInMeters = 40075016.6855785;

		public static double DegreesToRadians(double deg)
		{
			return deg * 3.14159265358979 / 180;
		}

		public static Point LocationToLogicalPoint(Location location)
		{
			double num;
			if (location.Latitude > 85.051128)
			{
				num = 0;
			}
			else if (location.Latitude >= -85.051128)
			{
				double num1 = Math.Sin(location.Latitude * 3.14159265358979 / 180);
				num = 0.5 - Math.Log((1 + num1) / (1 - num1)) / 12.5663706143592;
			}
			else
			{
				num = 1;
			}
			return new Point((location.Longitude + 180) / 360, num);
		}

		public static Location LogicalPointToLocation(Point logicalPoint)
		{
			return new Location(90 - 360 * Math.Atan(Math.Exp((logicalPoint.Y * 2 - 1) * 3.14159265358979)) / 3.14159265358979, logicalPoint.X * 360 - 180);
		}

		public static double ScaleToZoom(Size logicalAreaSizeInScreenSpaceAtLevel1, double scale, Location location)
		{
			double num = scale / Math.Cos(MercatorUtility.DegreesToRadians(location.Latitude));
			return Math.Log(40075016.6855785 / (num * logicalAreaSizeInScreenSpaceAtLevel1.Width), 2) + 1;
		}

		public static double ZoomToScale(Size logicalAreaSizeInScreenSpaceAtLevel1, double zoomLevel, Location location)
		{
			double num = 40075016.6855785 / (Math.Pow(2, zoomLevel - 1) * logicalAreaSizeInScreenSpaceAtLevel1.Width);
			return Math.Cos(MercatorUtility.DegreesToRadians(location.Latitude)) * num;
		}
	}
}