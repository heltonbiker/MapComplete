using System;

namespace Microsoft.Maps.MapControl.WPF.Overlays
{
	/// <summary>Represents the units of distance used by the scale bar.</summary>
	public enum DistanceUnit
	{
		/// <summary>The scale bar uses the default unit of distance of the current country or region.</summary>
		Default,
		/// <summary>The scale bar uses miles for long distances and feet for short distances.</summary>
		MilesFeet,
		/// <summary>The scale bar uses miles for long distances and yards for short distances.</summary>
		MilesYards,
		/// <summary>The scale bar uses kilometers for long distances and meters for shorter distances.</summary>
		KilometersMeters
	}
}