using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Maps.MapExtras
{
	internal class RelevantTileSet
	{
		private List<Tuple<TileId, int?>> relevantTiles = new List<Tuple<TileId, int?>>();

		private List<Tuple<TileId, int?>> previousRelevantTiles = new List<Tuple<TileId, int?>>();

		private HashSet<TileId> relevantTilesSet = new HashSet<TileId>();

		private List<TileId> removedTiles = new List<TileId>();

		public IList<Tuple<TileId, int?>> RelevantTiles
		{
			get
			{
				return new ReadOnlyCollection<Tuple<TileId, int?>>(this.relevantTiles);
			}
		}

		public IList<TileId> RemovedTiles
		{
			get
			{
				return new ReadOnlyCollection<TileId>(this.removedTiles);
			}
		}

		public void SetRelevantTiles(IList<Tuple<TileId, int?>> relevantTilesList)
		{
			VectorMath.Swap<List<Tuple<TileId, int?>>>(ref this.relevantTiles, ref this.previousRelevantTiles);
			this.relevantTiles.Clear();
			this.relevantTilesSet.Clear();
			this.removedTiles.Clear();
			foreach (Tuple<TileId, int?> current in relevantTilesList)
			{
				this.relevantTiles.Add(current);
				this.relevantTilesSet.Add(current.Item1);
			}
			this.relevantTiles.Sort(delegate (Tuple<TileId, int?> left, Tuple<TileId, int?> right)
			{
				int num = left.Item2 ?? -2147483648;
				int num2 = right.Item2 ?? -2147483648;
				if (num == num2)
				{
					return left.Item1.CompareTo(right.Item1);
				}
				return -num.CompareTo(num2);
			});
			foreach (Tuple<TileId, int?> current2 in this.previousRelevantTiles)
			{
				if (!this.relevantTilesSet.Contains(current2.Item1))
				{
					this.removedTiles.Add(current2.Item1);
				}
			}
		}

		public bool Contains(TileId tileId)
		{
			return this.relevantTilesSet.Contains(tileId);
		}
	}
}
