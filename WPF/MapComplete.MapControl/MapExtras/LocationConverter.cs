using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal abstract class LocationConverter
	{
		//public const double EarthRadiusInMeters = 6378137.0;

		//public const double EarthCircumferenceInMeters = 40075016.685578488;

		//public abstract CoordinateSystemDirection Direction
		//{
		//	get;
		//}

		public abstract Location ToLocation(Point3D vector);

		public virtual Location ToLocation(Point3D vector, Point3D wrappingCenter)
		{
			return this.ToLocation(vector);
		}

		public abstract Point3D FromLocation(Location location);

		public virtual Point3D FromLocation(Location location, Point3D wrappingCenter)
		{
			return this.FromLocation(location);
		}

		//public abstract Point3D GetUpVector(Point3D vector);

		//public abstract Point3D GetNorthVector(Point3D vector);

		//public abstract Point3D ChangeAltitude(Point3D vector, double altitude);

		//public abstract double VectorDistanceToMeters(Point3D vector, double distance);

		//public abstract double MetersToVectorDistance(Point3D vector, double meters);
	}
}
