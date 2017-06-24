using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal class TilePyramid
	{
		//private bool allowHardwareAcceleration;

		//private double tileOverlap = 0.5;

		//private bool overlapPyramidEdges;

		//private Rect? clip;

		//private TilePyramidDescriptor tilePyramidDescriptor;

		//private TileSource tileSource;

		//private List<Tuple<TileId, int?>> explicitRelevantTiles = new List<Tuple<TileId, int?>>();

		//private Canvas parentCanvas;

		//private Canvas tilePyramidCanvas;

		//private int frameCount;

		//private List<LevelOfDetailSettings?> levelOfDetailSettings;

		private Dictionary<TileId, TileRecord> tiles = new Dictionary<TileId, TileRecord>();

		//private TilePyramidCoverageMap coverageMap;

		//private List<KeyValuePair<TileId, TileRecord>> tilesToRemove = new List<KeyValuePair<TileId, TileRecord>>();

		//private Dictionary<TileId, int?> relevantTilesByTileId = new Dictionary<TileId, int?>();

		//private List<Tuple<TileId, int?>> relevantTiles = new List<Tuple<TileId, int?>>();

		public event EventHandler NeedsRender;

		//public TilePyramidDescriptor TilePyramidDescriptor
		//{
		//	get
		//	{
		//		return this.tilePyramidDescriptor;
		//	}
		//}

		//public TileSource TileSource
		//{
		//	get
		//	{
		//		return this.tileSource;
		//	}
		//}

		//public IList<Tuple<TileId, int?>> ExplicitRelevantTiles
		//{
		//	get
		//	{
		//		return new List<Tuple<TileId, int?>>(this.explicitRelevantTiles);
		//	}
		//	set
		//	{
		//		if (value == null)
		//		{
		//			throw new ArgumentNullException("value");
		//		}
		//		this.explicitRelevantTiles.Clear();
		//		this.explicitRelevantTiles.AddRange(value);
		//		if (this.NeedsRender != null)
		//		{
		//			this.NeedsRender(this, EventArgs.Empty);
		//		}
		//	}
		//}

		public ChooseLevelOfDetailSettings ChooseLevelOfDetailSettings
		{
			get;
			set;
		}

		//public double Opacity
		//{
		//	get
		//	{
		//		return this.parentCanvas.Opacity;
		//	}
		//	set
		//	{
		//		this.parentCanvas.Opacity = value;
		//	}
		//}

		//public double TileOverlap
		//{
		//	get
		//	{
		//		return this.tileOverlap;
		//	}
		//	set
		//	{
		//		if (this.tileOverlap != value)
		//		{
		//			this.tileOverlap = value;
		//			foreach (KeyValuePair<TileId, TileRecord> current in this.tiles)
		//			{
		//				current.Value.TileOverlap = this.tileOverlap;
		//			}
		//		}
		//	}
		//}

		//public bool OverlapPyramidEdges
		//{
		//	get
		//	{
		//		return this.overlapPyramidEdges;
		//	}
		//	set
		//	{
		//		this.overlapPyramidEdges = value;
		//		foreach (KeyValuePair<TileId, TileRecord> current in this.tiles)
		//		{
		//			current.Value.OverlapPyramidEdges = this.overlapPyramidEdges;
		//		}
		//	}
		//}

		//public bool AllowHardwareAcceleration
		//{
		//	get
		//	{
		//		return this.allowHardwareAcceleration;
		//	}
		//	set
		//	{
		//		this.allowHardwareAcceleration = value;
		//		foreach (KeyValuePair<TileId, TileRecord> current in this.tiles)
		//		{
		//			current.Value.AllowHardwareAcceleration = this.allowHardwareAcceleration;
		//		}
		//	}
		//}

		public TilePyramid(TilePyramidDescriptor tilePyramidDescriptor, TileSource tileSource, Canvas parentCanvas)
		{
		//	if (tilePyramidDescriptor == null)
		//	{
		//		throw new ArgumentNullException("tilePyramidDescriptor");
		//	}
		//	if (tileSource == null)
		//	{
		//		throw new ArgumentNullException("tileSource");
		//	}
		//	if (parentCanvas == null)
		//	{
		//		throw new ArgumentNullException("parentCanvas");
		//	}
		//	if (tilePyramidDescriptor.FinestLevelWidth != tileSource.FinestLodWidth || tilePyramidDescriptor.FinestLevelHeight != tileSource.FinestLodHeight)
		//	{
		//		throw new ArgumentException("Tile pyramid descriptor and tile source sizes must match.");
		//	}
		//	this.tilePyramidDescriptor = tilePyramidDescriptor;
		//	this.parentCanvas = parentCanvas;
		//	this.tileSource = tileSource;
		//	this.tileSource.NewTilesAvailable += new EventHandler(this.TileSource_NewTilesAvailable);
		//	this.tilePyramidCanvas = new Canvas
		//	{
		//		HorizontalAlignment = HorizontalAlignment.Stretch,
		//		VerticalAlignment = VerticalAlignment.Stretch,
		//		UseLayoutRounding = false,
		//		IsHitTestVisible = false,
		//		Tag = "TilePyramidCanvas"
		//	};
		//	Canvas.SetLeft(this.tilePyramidCanvas, 0.0);
		//	Canvas.SetTop(this.tilePyramidCanvas, 0.0);
		//	this.parentCanvas.Children.Add(this.tilePyramidCanvas);
		//	this.ChooseLevelOfDetailSettings = TilePyramid.DownloadFinestNLevelsOfDetailNeeded(3, true, true);
		//	this.levelOfDetailSettings = new List<LevelOfDetailSettings?>(tileSource.MaximumLevelOfDetail);
		//	for (int i = 0; i <= tileSource.MaximumLevelOfDetail; i++)
		//	{
		//		this.levelOfDetailSettings.Add(new LevelOfDetailSettings?(new LevelOfDetailSettings(false, 0.0, null)));
		//	}
		//	this.coverageMap = new TilePyramidCoverageMap(tileSource.MinimumLevelOfDetail, tileSource.MaximumLevelOfDetail);
		}

		//public static ChooseLevelOfDetailSettings DownloadFinestNLevelsOfDetailNeeded(int numberOfFinestLevelsOfDetailToDownloadFirst, bool downloadTheRest, bool showTheRest)
		//{
		//	if (numberOfFinestLevelsOfDetailToDownloadFirst <= 0)
		//	{
		//		throw new ArgumentOutOfRangeException("numberOfFinestLevelsOfDetailToDownloadFirst");
		//	}
		//	return delegate (int renderLevelOfDetail, int levelOfDetail, int minimumLevelOfDetail)
		//	{
		//		LevelOfDetailSettings value = new LevelOfDetailSettings(true, 1.0, new int?(renderLevelOfDetail - levelOfDetail));
		//		if (value.DownloadPriority > numberOfFinestLevelsOfDetailToDownloadFirst - 1)
		//		{
		//			value.DownloadPriority = (downloadTheRest ? (new int?(minimumLevelOfDetail) - levelOfDetail - 1) : null);
		//			value.Visible = showTheRest;
		//		}
		//		if (value.DownloadPriority.HasValue)
		//		{
		//			value.DownloadPriority = new int?(value.DownloadPriority.Value * 5);
		//		}
		//		return new LevelOfDetailSettings?(value);
		//	};
		//}

		public void Render(Point2D viewportSize, ref Matrix3D tilePyramidToViewport, IList<TileId> visibleTiles, TilePriorityCalculator tilePriorityCalculator, double preciseRenderLod, int renderLod, bool showBackgroundTiles)
		{
		//	if (this.tileSource == null)
		//	{
		//		throw new InvalidOperationException("Tile source has been detached.");
		//	}
		//	this.frameCount++;
		//	for (int i = renderLod; i >= this.tileSource.MinimumLevelOfDetail; i--)
		//	{
		//		this.levelOfDetailSettings[i] = this.ChooseLevelOfDetailSettings(renderLod, i, this.tileSource.MinimumLevelOfDetail);
		//	}
		//	if (this.clip != this.tileSource.Clip)
		//	{
		//		this.clip = this.tileSource.Clip;
		//		if (this.clip.HasValue)
		//		{
		//			Rect value = this.clip.Value;
		//			this.tilePyramidDescriptor.ClipPoly = new Point2D[]
		//			{
		//				new Point2D(value.Left, value.Top),
		//				new Point2D(value.Left, value.Bottom),
		//				new Point2D(value.Right, value.Bottom),
		//				new Point2D(value.Right, value.Top)
		//			};
		//		}
		//		else
		//		{
		//			this.tilePyramidDescriptor.ClipPoly = this.tilePyramidDescriptor.DefaultClipPoly;
		//		}
		//		foreach (KeyValuePair<TileId, TileRecord> current in this.tiles)
		//		{
		//			current.Value.TilePyramidClip = this.clip;
		//		}
		//	}
		//	if (visibleTiles.Count > 0)
		//	{
		//		long num = 9223372036854775807L;
		//		long num2 = 9223372036854775807L;
		//		long num3 = -9223372036854775808L;
		//		long num4 = -9223372036854775808L;
		//		foreach (TileId current2 in visibleTiles)
		//		{
		//			num = Math.Min(num, current2.X);
		//			num2 = Math.Min(num2, current2.Y);
		//			num3 = Math.Max(num3, current2.X + 1L);
		//			num4 = Math.Max(num4, current2.Y + 1L);
		//		}
		//		this.coverageMap.Intialize(renderLod, num, num2, num3, num4);
		//	}
		//	foreach (TileId current3 in visibleTiles)
		//	{
		//		if (current3.LevelOfDetail >= this.tileSource.MinimumLevelOfDetail)
		//		{
		//			int priority = tilePriorityCalculator.GetPriority(current3);
		//			LevelOfDetailSettings? levelOfDetailSettings = this.levelOfDetailSettings[current3.LevelOfDetail];
		//			if (levelOfDetailSettings.HasValue)
		//			{
		//				TileRecord tileRecord;
		//				if (!this.tiles.TryGetValue(current3, out tileRecord))
		//				{
		//					tileRecord = new TileRecord(this.tilePyramidCanvas, this.tilePyramidDescriptor, current3)
		//					{
		//						AllowHardwareAcceleration = this.allowHardwareAcceleration,
		//						OverlapPyramidEdges = this.overlapPyramidEdges,
		//						TileOverlap = this.tileOverlap,
		//						TilePyramidClip = this.clip
		//					};
		//					tileRecord.NeedsRender += new EventHandler(this.TileRecord_NeedsRender);
		//					this.tiles.Add(current3, tileRecord);
		//				}
		//				if (levelOfDetailSettings.Value.Visible)
		//				{
		//					tileRecord.TargetOpacity = levelOfDetailSettings.Value.TargetOpacity;
		//				}
		//				else
		//				{
		//					tileRecord.TargetOpacity = 0.0;
		//				}
		//				tileRecord.LastTouched = this.frameCount;
		//				this.AddToRelevantTiles(current3, levelOfDetailSettings, priority);
		//			}
		//			this.coverageMap.MarkAsOccluder(current3, false);
		//			if (showBackgroundTiles)
		//			{
		//				TileId tileId = current3;
		//				while (tileId.LevelOfDetail > this.tileSource.MinimumLevelOfDetail)
		//				{
		//					tileId = tileId.GetParent();
		//					LevelOfDetailSettings? levelOfDetailSettings2 = this.levelOfDetailSettings[tileId.LevelOfDetail];
		//					if (levelOfDetailSettings2.HasValue)
		//					{
		//						TileRecord tileRecord2;
		//						if (!this.tiles.TryGetValue(tileId, out tileRecord2))
		//						{
		//							tileRecord2 = new TileRecord(this.tilePyramidCanvas, this.tilePyramidDescriptor, tileId)
		//							{
		//								AllowHardwareAcceleration = this.allowHardwareAcceleration,
		//								OverlapPyramidEdges = this.overlapPyramidEdges,
		//								TileOverlap = this.tileOverlap,
		//								TilePyramidClip = this.clip
		//							};
		//							tileRecord2.NeedsRender += new EventHandler(this.TileRecord_NeedsRender);
		//							this.tiles.Add(tileId, tileRecord2);
		//							this.AddToRelevantTiles(tileId, levelOfDetailSettings2, priority);
		//						}
		//						if (levelOfDetailSettings2.Value.Visible)
		//						{
		//							tileRecord2.TargetOpacity = levelOfDetailSettings2.Value.TargetOpacity;
		//						}
		//						else
		//						{
		//							tileRecord2.TargetOpacity = 0.0;
		//						}
		//						if (tileRecord2.LastTouched != this.frameCount)
		//						{
		//							this.AddToRelevantTiles(tileId, levelOfDetailSettings2, priority);
		//							tileRecord2.LastTouched = this.frameCount;
		//						}
		//					}
		//					this.coverageMap.MarkAsOccluder(tileId, false);
		//				}
		//			}
		//		}
		//	}
		//	foreach (KeyValuePair<TileId, TileRecord> current4 in this.tiles)
		//	{
		//		if (current4.Value.LastTouched != this.frameCount)
		//		{
		//			this.tilesToRemove.Add(current4);
		//		}
		//	}
		//	foreach (KeyValuePair<TileId, TileRecord> current5 in this.tilesToRemove)
		//	{
		//		this.PrepareTileForRemoval(current5.Value);
		//		this.tiles.Remove(current5.Key);
		//	}
		//	this.tilesToRemove.Clear();
		//	foreach (Tuple<TileId, int?> current6 in this.explicitRelevantTiles)
		//	{
		//		this.AddToRelevantTiles(current6.Item1, current6.Item2);
		//	}
		//	foreach (KeyValuePair<TileId, int?> current7 in this.relevantTilesByTileId)
		//	{
		//		this.relevantTiles.Add(Tuple.Create<TileId, int?>(current7.Key, current7.Value));
		//	}
		//	this.relevantTilesByTileId.Clear();
		//	this.tileSource.SetRelevantTiles(new ReadOnlyCollection<Tuple<TileId, int?>>(this.relevantTiles));
		//	this.relevantTiles.Clear();
		//	foreach (KeyValuePair<TileId, TileRecord> current8 in this.tiles)
		//	{
		//		if (current8.Value.TileRenderable == null && !current8.Value.WillNeverBeAvailable)
		//		{
		//			TileRenderable tileRenderable;
		//			bool flag;
		//			this.tileSource.GetTile(current8.Key, out tileRenderable, out flag);
		//			if (flag)
		//			{
		//				current8.Value.WillNeverBeAvailable = true;
		//			}
		//			else if (tileRenderable != null)
		//			{
		//				current8.Value.TileRenderable = tileRenderable;
		//			}
		//		}
		//	}
		//	if (this.tiles.Count > 0)
		//	{
		//		foreach (KeyValuePair<TileId, TileRecord> current9 in this.tiles)
		//		{
		//			if (current9.Value.FullyOpaque)
		//			{
		//				this.coverageMap.MarkAsOccluder(current9.Key, true);
		//			}
		//		}
		//		this.coverageMap.CalculateOcclusions();
		//	}
		//	foreach (KeyValuePair<TileId, TileRecord> current10 in this.tiles)
		//	{
		//		TileRecord value2 = current10.Value;
		//		if (!this.levelOfDetailSettings[current10.Key.LevelOfDetail].Value.Visible || (value2.TileRenderable == null && !value2.WillNeverBeAvailable) || this.coverageMap.IsOccludedByDescendents(current10.Key))
		//		{
		//			if (value2.Visible)
		//			{
		//				value2.NoLongerRendering();
		//			}
		//			value2.Visible = false;
		//		}
		//		else
		//		{
		//			if (!value2.Visible)
		//			{
		//				value2.Visible = true;
		//			}
		//			value2.Render(viewportSize, ref tilePyramidToViewport, preciseRenderLod);
		//		}
		//	}
		}

		public ICollection<TileStatus> GetTiles()
		{
			List<TileStatus> list = new List<TileStatus>();
			foreach (KeyValuePair<TileId, TileRecord> current in this.tiles)
			{
				list.Add(new TileStatus
				{
					TileId = current.Key,
					Visible = current.Value.Visible,
					Available = (current.Value.TileRenderable != null),
					WillNeverBeAvailable = current.Value.WillNeverBeAvailable,
					FullyOpaque = current.Value.FullyOpaque
				});
			}
			return list;
		}

		//public void ClearAllTiles()
		//{
		//	foreach (KeyValuePair<TileId, TileRecord> current in this.tiles)
		//	{
		//		this.PrepareTileForRemoval(current.Value);
		//	}
		//	this.tiles.Clear();
		//	this.tileSource.SetRelevantTiles(this.relevantTiles);
		//}

		public void DetachTileSource()
		{
			//this.ClearAllTiles();
			//this.tileSource.NewTilesAvailable -= new EventHandler(this.TileSource_NewTilesAvailable);
			//this.tileSource = null;
		}

		//private static int? MaxPriority(int? priority0, int? priority1)
		//{
		//	if (priority0.HasValue && priority1.HasValue)
		//	{
		//		return new int?(Math.Max(priority0.Value, priority1.Value));
		//	}
		//	if (priority0.HasValue)
		//	{
		//		return priority0;
		//	}
		//	return priority1;
		//}

		//private void AddToRelevantTiles(TileId tileId, LevelOfDetailSettings? levelOfDetailSettings, int intraLodTilePriority)
		//{
		//	int? priority = null;
		//	if (levelOfDetailSettings.HasValue)
		//	{
		//		priority = levelOfDetailSettings.Value.DownloadPriority;
		//		if (priority.HasValue)
		//		{
		//			priority = new int?(priority.Value + intraLodTilePriority);
		//		}
		//	}
		//	this.AddToRelevantTiles(tileId, priority);
		//}

		//private void AddToRelevantTiles(TileId tileId, int? priority)
		//{
		//	int? num;
		//	if (this.relevantTilesByTileId.TryGetValue(tileId, out num))
		//	{
		//		priority = TilePyramid.MaxPriority(num, priority);
		//		if (priority != num)
		//		{
		//			this.relevantTilesByTileId[tileId] = priority;
		//			return;
		//		}
		//	}
		//	else
		//	{
		//		this.relevantTilesByTileId[tileId] = priority;
		//	}
		//}

		//private void PrepareTileForRemoval(TileRecord tr)
		//{
		//	if (tr.Visible)
		//	{
		//		tr.NoLongerRendering();
		//	}
		//	tr.Visible = false;
		//	tr.TileRenderable = null;
		//	tr.NeedsRender -= new EventHandler(this.TileRecord_NeedsRender);
		//}

		//private void TileRecord_NeedsRender(object sender, EventArgs e)
		//{
		//	if (this.NeedsRender != null)
		//	{
		//		this.NeedsRender(this, EventArgs.Empty);
		//	}
		//}

		//private void TileSource_NewTilesAvailable(object sender, EventArgs e)
		//{
		//	if (this.NeedsRender != null)
		//	{
		//		this.NeedsRender(this, EventArgs.Empty);
		//	}
		//}
	}
}
