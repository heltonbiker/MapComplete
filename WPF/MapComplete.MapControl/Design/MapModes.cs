using System;

namespace Microsoft.Maps.MapControl.WPF.Design
{
	internal static class MapModes
	{
		//public static AerialMode Aerial
		//{
		//	get
		//	{
		//		return new AerialMode(false);
		//	}
		//}

		public static AerialMode AerialWithLabels
		{
			get
			{
				return new AerialMode(true);
			}
		}

		//public static RoadMode Road
		//{
		//	get
		//	{
		//		return new RoadMode();
		//	}
		//}
	}
}
