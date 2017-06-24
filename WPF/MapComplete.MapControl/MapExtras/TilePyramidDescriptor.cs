using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal class TilePyramidDescriptor
	{
		private int finestLod;

		private int coarsestLod;

		private long finestLodWidth;

		private long finestLodHeight;

		private int tileWidth;

		private int tileHeight;

		private Point2D[] vertices = new Point2D[0];

		private double polygonCrossProductZ;

		private Point4D[] verticesSS;

		private Point2D[] verticesSSTexCoords;

		private Point4D[] clippedVerticesSS;

		private Point2D[] clippedVerticesSSTexCoords;

		private Point4D[] tempVertexBuffer;

		private Point2D[] tempTextureCoordBuffer;

		//public Point2D[] DefaultClipPoly
		//{
		//	get
		//	{
		//		return new Point2D[]
		//		{
		//			new Point2D(0.0, 0.0),
		//			new Point2D(0.0, (double)this.finestLodHeight),
		//			new Point2D((double)this.finestLodWidth, (double)this.finestLodHeight),
		//			new Point2D((double)this.finestLodWidth, 0.0)
		//		};
		//	}
		//}

		//public Point2D[] ClipPoly
		//{
		//	get
		//	{
		//		return (Point2D[])this.vertices.Clone();
		//	}
		//	set
		//	{
		//		if (value == null)
		//		{
		//			throw new ArgumentNullException("value");
		//		}
		//		if (value.Length < 3)
		//		{
		//			throw new ArgumentException("value must contain at least 3 elements");
		//		}
		//		this.vertices = value;
		//		this.polygonCrossProductZ = (this.vertices[2].X - this.vertices[1].X) * (this.vertices[0].Y - this.vertices[1].Y) - (this.vertices[2].Y - this.vertices[1].Y) * (this.vertices[0].X - this.vertices[1].X);
		//		this.verticesSS = new Point4D[this.vertices.Length];
		//		this.verticesSSTexCoords = new Point2D[this.vertices.Length];
		//		this.clippedVerticesSS = new Point4D[this.vertices.Length + 6];
		//		this.clippedVerticesSSTexCoords = new Point2D[this.vertices.Length + 6];
		//		this.tempVertexBuffer = new Point4D[this.vertices.Length + 6];
		//		this.tempTextureCoordBuffer = new Point2D[this.vertices.Length + 6];
		//	}
		//}

		public int TileWidth
		{
			get
			{
				return this.tileWidth;
			}
		}

		public int TileHeight
		{
			get
			{
				return this.tileHeight;
			}
		}

		//public int FinestLevelOfDetail
		//{
		//	get
		//	{
		//		return this.finestLod;
		//	}
		//}

		public long FinestLevelWidth
		{
			get
			{
				return this.finestLodWidth;
			}
		}

		public long FinestLevelHeight
		{
			get
			{
				return this.finestLodHeight;
			}
		}

		public double LevelOfDetailBias
		{
			get;
			set;
		}

		public TilePyramidDescriptor(long finestLevelOfDetailWidth, long finestLevelOfDetailHeight, int coarsestLevelOfDetail, int tileWidth, int tileHeight)
		{
		//	if (finestLevelOfDetailHeight < 1L)
		//	{
		//		throw new ArgumentOutOfRangeException("finestLevelOfDetailHeight");
		//	}
		//	if (finestLevelOfDetailWidth < 1L)
		//	{
		//		throw new ArgumentOutOfRangeException("finestLevelOfDetailWidth");
		//	}
		//	if (tileWidth < 1)
		//	{
		//		throw new ArgumentOutOfRangeException("tileWidth");
		//	}
		//	if (tileHeight < 1)
		//	{
		//		throw new ArgumentOutOfRangeException("tileHeight");
		//	}
		//	this.finestLodWidth = finestLevelOfDetailWidth;
		//	this.finestLodHeight = finestLevelOfDetailHeight;
		//	this.finestLod = VectorMath.CeilLog2(Math.Max(this.finestLodWidth, this.finestLodHeight));
		//	if (coarsestLevelOfDetail < 0 || coarsestLevelOfDetail > this.finestLod)
		//	{
		//		throw new InvalidOperationException("coarsest level of detail must be positive and <= the finest level of detail");
		//	}
		//	this.coarsestLod = coarsestLevelOfDetail;
		//	this.tileWidth = tileWidth;
		//	this.tileHeight = tileHeight;
		//	this.ClipPoly = this.DefaultClipPoly;
		}

		public long GetLevelOfDetailWidth(int lod)
		{
			return VectorMath.DivPow2RoundUp(this.finestLodWidth, this.finestLod - lod);
		}

		public long GetLevelOfDetailHeight(int lod)
		{
			return VectorMath.DivPow2RoundUp(this.finestLodHeight, this.finestLod - lod);
		}

		public long GetLevelOfDetailWidthInTiles(int lod)
		{
			return VectorMath.DivRoundUp(VectorMath.DivPow2RoundUp(this.finestLodWidth, this.finestLod - lod), (long)((ulong)this.tileWidth));
		}

		public long GetLevelOfDetailHeightInTiles(int lod)
		{
			return VectorMath.DivRoundUp(VectorMath.DivPow2RoundUp(this.finestLodHeight, this.finestLod - lod), (long)((ulong)this.tileHeight));
		}

		public TileEdgeFlags GetTileEdgeFlags(TileId tileId)
		{
			return new TileEdgeFlags(tileId.X == 0L, tileId.X == this.GetLevelOfDetailWidthInTiles(tileId.LevelOfDetail) - 1L, tileId.Y == 0L, tileId.Y == this.GetLevelOfDetailHeightInTiles(tileId.LevelOfDetail) - 1L);
		}

		public void GetVisibleTiles(Point2D viewportSize, ref Matrix3D modelToViewportTransform, bool showBackFace, IList<TileId> tiles, Point2D[] visiblePolyAtFinestLod, Point2D[] screenSpacePoly, out int screenSpacePolyVertexCount, out double preciseRenderLod, out int renderLod, out double finestLodNeeded)
		{
			if (screenSpacePoly.Length < this.vertices.Length + 6 || visiblePolyAtFinestLod.Length < this.vertices.Length + 6)
			{
				throw new ArgumentException("Screen space poly and visible poly at finest LOD must have room for the number of vertices in the clip poly plus 6.");
			}
			tiles.Clear();
			preciseRenderLod = -1.7976931348623157E+308;
			renderLod = -2147483648;
			finestLodNeeded = -1.7976931348623157E+308;
			for (int i = 0; i < this.vertices.Length; i++)
			{
				this.verticesSS[i] = VectorMath.Transform(modelToViewportTransform, new Point4D(this.vertices[i].X, this.vertices[i].Y, 0.0, 1.0));
				this.verticesSSTexCoords[i].X = this.vertices[i].X;
				this.verticesSSTexCoords[i].Y = this.vertices[i].Y;
			}
			RectangularSolid clipBounds = new RectangularSolid(0.0, 0.0, 0.0, viewportSize.X, viewportSize.Y, 1.0);
			VectorMath.ClipConvexPolygon(clipBounds, this.verticesSS, this.verticesSSTexCoords, this.vertices.Length, this.clippedVerticesSS, this.clippedVerticesSSTexCoords, out screenSpacePolyVertexCount, this.tempVertexBuffer, this.tempTextureCoordBuffer);
			if (screenSpacePolyVertexCount > 0)
			{
				for (int j = 0; j < screenSpacePolyVertexCount; j++)
				{
					int arg_1B7_0_cp_1 = j;
					Point4D[] expr_195_cp_0 = this.clippedVerticesSS;
					int expr_195_cp_1 = j;
					screenSpacePoly[arg_1B7_0_cp_1].X = (expr_195_cp_0[expr_195_cp_1].X = expr_195_cp_0[expr_195_cp_1].X / this.clippedVerticesSS[j].W);
					int arg_1F2_0_cp_1 = j;
					Point4D[] expr_1D0_cp_0 = this.clippedVerticesSS;
					int expr_1D0_cp_1 = j;
					screenSpacePoly[arg_1F2_0_cp_1].Y = (expr_1D0_cp_0[expr_1D0_cp_1].Y = expr_1D0_cp_0[expr_1D0_cp_1].Y / this.clippedVerticesSS[j].W);
					visiblePolyAtFinestLod[j].X = this.clippedVerticesSSTexCoords[j].X;
					visiblePolyAtFinestLod[j].Y = this.clippedVerticesSSTexCoords[j].Y;
				}
				if (!showBackFace)
				{
					double num = (this.clippedVerticesSS[2].X - this.clippedVerticesSS[1].X) * (this.clippedVerticesSS[0].Y - this.clippedVerticesSS[1].Y) - (this.clippedVerticesSS[2].Y - this.clippedVerticesSS[1].Y) * (this.clippedVerticesSS[0].X - this.clippedVerticesSS[1].X);
					bool flag = (num < 0.0 && this.polygonCrossProductZ >= 0.0) || (num >= 0.0 && this.polygonCrossProductZ < 0.0);
					if (flag)
					{
						return;
					}
				}
				this.CalculateRenderLod(screenSpacePolyVertexCount, out preciseRenderLod, out finestLodNeeded);
				renderLod = this.CalculateFinestLevelOfDetailToUse(preciseRenderLod);
				long levelOfDetailWidthInTiles = this.GetLevelOfDetailWidthInTiles(renderLod);
				long levelOfDetailHeightInTiles = this.GetLevelOfDetailHeightInTiles(renderLod);
				if (levelOfDetailWidthInTiles == 1L && levelOfDetailHeightInTiles == 1L)
				{
					tiles.Add(new TileId(renderLod, 0L, 0L));
					return;
				}
				TilePyramidDescriptor.IntersectClippedPolyWithTileGrid(tiles, this.clippedVerticesSSTexCoords, screenSpacePolyVertexCount, this.finestLod, renderLod, levelOfDetailWidthInTiles, levelOfDetailHeightInTiles, (double)this.tileWidth, (double)this.tileHeight);
			}
		}

		private static void IntersectClippedPolyWithTileGrid(IList<TileId> tiles, Point2D[] clippedVerticesSSTexCoords, int clippedVerticesSSCount, int finestLod, int lod, long tileGridWidth, long tileGridHeight, double tileWidth, double tileHeight)
		{
			double num = 1.7976931348623157E+308;
			double num2 = -1.7976931348623157E+308;
			double num3 = 1.7976931348623157E+308;
			double num4 = -1.7976931348623157E+308;
			double num5 = 1.0 / (1 << finestLod - lod) / tileWidth;
			double num6 = 1.0 / (1 << finestLod - lod) / tileHeight;
			for (int i = 0; i < clippedVerticesSSCount; i++)
			{
				int expr_6C_cp_1 = i;
				clippedVerticesSSTexCoords[expr_6C_cp_1].X = clippedVerticesSSTexCoords[expr_6C_cp_1].X * num5;
				int expr_82_cp_1 = i;
				clippedVerticesSSTexCoords[expr_82_cp_1].Y = clippedVerticesSSTexCoords[expr_82_cp_1].Y * num6;
				num = Math.Min(num, clippedVerticesSSTexCoords[i].X);
				num2 = Math.Max(num2, clippedVerticesSSTexCoords[i].X);
				num3 = Math.Min(num3, clippedVerticesSSTexCoords[i].Y);
				num4 = Math.Max(num4, clippedVerticesSSTexCoords[i].Y);
			}
			double num7 = 0.01 / Math.Min(tileWidth, tileHeight);
			long num8 = VectorMath.Clamp((long)Math.Floor(num - num7), 0L, tileGridWidth);
			long num9 = VectorMath.Clamp((long)Math.Ceiling(num2 + num7), 0L, tileGridWidth);
			long num10 = VectorMath.Clamp((long)Math.Floor(num3 - num7), 0L, tileGridHeight);
			long num11 = VectorMath.Clamp((long)Math.Ceiling(num4 + num7), 0L, tileGridHeight);
			long num12 = num9 - num8;
			long num13 = num11 - num10;
			if (num12 <= 2L && num13 <= 2L)
			{
				for (long num14 = num10; num14 < num11; num14 += 1L)
				{
					for (long num15 = num8; num15 < num9; num15 += 1L)
					{
						tiles.Add(new TileId(lod, num15, num14));
					}
				}
				return;
			}
			bool[,] array = checked(new bool[(int)((IntPtr)num13), (int)((IntPtr)num12)]);
			double num16 = 2.0 * (0.5 + num7) * (0.5 + num7);
			int num17 = clippedVerticesSSCount - 1;
			for (int j = 0; j < clippedVerticesSSCount; j++)
			{
				Point2D line = clippedVerticesSSTexCoords[num17];
				Point2D line2 = clippedVerticesSSTexCoords[j];
				long num18 = (long)Math.Floor(line.X - num7);
				long num19 = (long)Math.Ceiling(line.X + num7);
				long num20 = (long)Math.Floor(line.Y - num7);
				long num21 = (long)Math.Ceiling(line.Y + num7);
				for (long num22 = num20; num22 < num21; num22 += 1L)
				{
					for (long num23 = num18; num23 < num19; num23 += 1L)
					{
						if (num23 >= 0L && num23 < tileGridWidth && num22 >= 0L && num22 < tileGridHeight)
						{
							checked(array[(int)((IntPtr)(unchecked(num22 - num10))), (int)((IntPtr)(unchecked(num23 - num8)))]) = true;
						}
					}
				}
				long num24 = VectorMath.Clamp((long)Math.Floor(Math.Min(line.X, line2.X) - num7), 0L, tileGridWidth);
				long num25 = VectorMath.Clamp((long)Math.Ceiling(Math.Max(line.X, line2.X) + num7), 0L, tileGridWidth);
				long num26 = VectorMath.Clamp((long)Math.Floor(Math.Min(line.Y, line2.Y) - num7), 0L, tileGridHeight);
				long num27 = VectorMath.Clamp((long)Math.Ceiling(Math.Max(line.Y, line2.Y) + num7), 0L, tileGridHeight);
				for (long num28 = num26; num28 < num27; num28 += 1L)
				{
					for (long num29 = num24; num29 < num25; num29 += 1L)
					{
						if (!checked(array[(int)((IntPtr)(unchecked(num28 - num10))), (int)((IntPtr)(unchecked(num29 - num8)))]))
						{
							Point2D point = new Point2D((double)num29 + 0.5, (double)num28 + 0.5);
							bool flag;
							double num30 = VectorMath.LinePointDistanceSquared(line, line2, point, out flag);
							if (num30 <= num16)
							{
								if (flag)
								{
									checked(array[(int)((IntPtr)(unchecked(num28 - num10))), (int)((IntPtr)(unchecked(num29 - num8)))]) = true;
								}
								else if (VectorMath.OrientedBoundingBoxIntersectsAxisAlignedBoundingBox(new Point2D(line.X, line.Y), new Point2D(line2.X, line2.Y), 2.0 * num7, new Rect((double)num29, (double)num28, 1.0, 1.0)))
								{
									checked(array[(int)((IntPtr)(unchecked(num28 - num10))), (int)((IntPtr)(unchecked(num29 - num8)))]) = true;
								}
							}
						}
					}
				}
				num17 = j;
			}
			int num31 = 0;
			while ((long)num31 < num13)
			{
				int num32 = 2147483647;
				int num33 = -2147483648;
				int num34 = 0;
				while ((long)num34 < num12)
				{
					if (array[num31, num34])
					{
						num32 = Math.Min(num32, num34);
						num33 = Math.Max(num33, num34 + 1);
					}
					num34++;
				}
				if (num32 < num33)
				{
					for (int k = num32; k < num33; k++)
					{
						array[num31, k] = true;
					}
				}
				num31++;
			}
			for (long num35 = num10; num35 < num11; num35 += 1L)
			{
				for (long num36 = num8; num36 < num9; num36 += 1L)
				{
					if (checked(array[(int)((IntPtr)(unchecked(num35 - num10))), (int)((IntPtr)(unchecked(num36 - num8)))]))
					{
						tiles.Add(new TileId(lod, num36, num35));
					}
				}
			}
		}

		private int CalculateFinestLevelOfDetailToUse(double renderLod)
		{
			renderLod = ((renderLod - Math.Floor(renderLod) < 0.5849625) ? Math.Floor(renderLod) : Math.Ceiling(renderLod));
			return VectorMath.Clamp((int)renderLod, this.coarsestLod, this.finestLod);
		}

		private void CalculateRenderLod(int numClippedVerticesSS, out double renderLod, out double finestLodNeeded)
		{
			int num = numClippedVerticesSS - 1;
			double num2 = 0.0;
			double num3 = 1.7976931348623157E+308;
			int num4 = 0;
			for (int i = 0; i < numClippedVerticesSS; i++)
			{
				double num5 = this.clippedVerticesSS[i].X - this.clippedVerticesSS[num].X;
				double num6 = this.clippedVerticesSS[i].Y - this.clippedVerticesSS[num].Y;
				double num7 = this.clippedVerticesSSTexCoords[i].X - this.clippedVerticesSSTexCoords[num].X;
				double num8 = this.clippedVerticesSSTexCoords[i].Y - this.clippedVerticesSSTexCoords[num].Y;
				if (num5 != 0.0 || num6 != 0.0)
				{
					double num9 = Math.Sqrt((num7 * num7 + num8 * num8) / (num5 * num5 + num6 * num6));
					num2 += num9;
					num4++;
					num3 = Math.Min(num3, num9);
				}
				num = i;
			}
			double texelToPixelRatio = num2 / (double)num4;
			renderLod = this.CalculateRenderLodFromTexelToPixelRatio(texelToPixelRatio);
			finestLodNeeded = this.CalculateRenderLodFromTexelToPixelRatio(num3);
		}

		private double CalculateRenderLodFromTexelToPixelRatio(double texelToPixelRatio)
		{
			return (double)this.finestLod - Math.Log(texelToPixelRatio, 2.0) + this.LevelOfDetailBias;
		}
	}
}
