using System;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal class TilePriorityCalculator
	{
		//public const int NumFoveationPriorityBuckets = 5;

		//private TileId VisibleTileId;

		//private Point2D viewportCenter;

		//private double bucketRadiusInterval;

		//private int lod;

		//private double tileWidthAtFinestLod;

		//private double tileHeightAtFinestLod;

		//private Point4D vTL0;

		//private Point4D vR;

		//private Point4D vD;

		public void Initialize(TilePyramidDescriptor tilePyramid, ref Matrix3D renderableToViewportTransform, Point2D viewportSize, TileId visibleTileId)
		{
			//	this.VisibleTileId = visibleTileId;
			//	this.viewportCenter = 0.5 * viewportSize;
			//	this.bucketRadiusInterval = 0.5 * Math.Sqrt(viewportSize.X * viewportSize.X + viewportSize.Y * viewportSize.Y) / 5.0 - 1.0;
			//	this.lod = visibleTileId.LevelOfDetail;
			//	this.tileWidthAtFinestLod = (double)(tilePyramid.TileWidth * (1 << tilePyramid.FinestLevelOfDetail - this.lod));
			//	this.tileHeightAtFinestLod = (double)(tilePyramid.TileHeight * (1 << tilePyramid.FinestLevelOfDetail - this.lod));
			//	this.vTL0 = VectorMath.Transform(renderableToViewportTransform, new Point4D(this.tileWidthAtFinestLod * (double)visibleTileId.X, this.tileHeightAtFinestLod * (double)visibleTileId.Y, 0.0, 1.0));
			//	Point4D point = VectorMath.Transform(renderableToViewportTransform, new Point4D(this.tileWidthAtFinestLod * (double)(visibleTileId.X + 1L), this.tileHeightAtFinestLod * (double)visibleTileId.Y, 0.0, 1.0));
			//	Point4D point2 = VectorMath.Transform(renderableToViewportTransform, new Point4D(this.tileWidthAtFinestLod * (double)visibleTileId.X, this.tileHeightAtFinestLod * (double)(visibleTileId.Y + 1L), 0.0, 1.0));
			//	this.vR = point - this.vTL0;
			//	this.vD = point2 - this.vTL0;
		}

			//public int GetPriority(TileId tileId)
			//{
			//	Point4D point4D = this.vTL0 + (double)(tileId.X - this.VisibleTileId.X) * this.vR + (double)(tileId.Y - this.VisibleTileId.Y) * this.vD;
			//	Point4D point4D2 = this.vTL0 + (double)(tileId.X + 1L - this.VisibleTileId.X) * this.vR + (double)(tileId.Y - this.VisibleTileId.Y) * this.vD;
			//	Point4D point4D3 = this.vTL0 + (double)(tileId.X + 1L - this.VisibleTileId.X) * this.vR + (double)(tileId.Y + 1L - this.VisibleTileId.Y) * this.vD;
			//	Point4D point4D4 = this.vTL0 + (double)(tileId.X - this.VisibleTileId.X) * this.vR + (double)(tileId.Y + 1L - this.VisibleTileId.Y) * this.vD;
			//	double num = 1.7976931348623157E+308;
			//	if (point4D.W > 0.0)
			//	{
			//		num = Math.Min(num, Point2D.DistanceSquared(new Point2D(point4D.X / point4D.W, point4D.Y / point4D.W), this.viewportCenter));
			//	}
			//	if (point4D2.W > 0.0)
			//	{
			//		num = Math.Min(num, Point2D.DistanceSquared(new Point2D(point4D2.X / point4D2.W, point4D2.Y / point4D2.W), this.viewportCenter));
			//	}
			//	if (point4D3.W > 0.0)
			//	{
			//		num = Math.Min(num, Point2D.DistanceSquared(new Point2D(point4D3.X / point4D3.W, point4D3.Y / point4D3.W), this.viewportCenter));
			//	}
			//	if (point4D4.W > 0.0)
			//	{
			//		num = Math.Min(num, Point2D.DistanceSquared(new Point2D(point4D4.X / point4D4.W, point4D4.Y / point4D4.W), this.viewportCenter));
			//	}
			//	double num2 = Math.Sqrt(num);
			//	return 4 - VectorMath.Clamp((int)Math.Floor(num2 / this.bucketRadiusInterval), 0, 4);
			//}
		}
}
