using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal static class VectorMath
	{
		public const double DegreesPerRadian = 57.295779513082323;

		public const double RadiansPerDegree = 0.017453292519943295;

		public static void ClipConvexPolygon(RectangularSolid clipBounds, Point4D[] poly, Point2D[] polyTextureCoords, int polyVertexCount, Point4D[] clippedPoly, Point2D[] clippedPolyTextureCoords, out int clippedPolyVertexCount, Point4D[] tempVertexBuffer, Point2D[] tempTextureCoordBuffer)
		{
			if (polyVertexCount < 3 || poly == null || poly.Length < polyVertexCount || clippedPoly == null || clippedPoly.Length < polyVertexCount + 6 || tempVertexBuffer == null || tempVertexBuffer.Length < polyVertexCount + 6)
			{
				throw new ArgumentException("polygon arrays must have sufficient capacity");
			}
			if (polyTextureCoords != null && (polyTextureCoords.Length < polyVertexCount || clippedPolyTextureCoords == null || clippedPolyTextureCoords.Length < polyVertexCount + 6 || tempTextureCoordBuffer == null || tempTextureCoordBuffer.Length < polyVertexCount + 6))
			{
				throw new ArgumentException("polygon arrays must have sufficient capacity");
			}
			VectorMath.Swap<Point4D[]>(ref tempVertexBuffer, ref clippedPoly);
			VectorMath.Swap<Point2D[]>(ref tempTextureCoordBuffer, ref clippedPolyTextureCoords);
			Point4D[] array = tempVertexBuffer;
			Point2D[] clippedPolyTextureCoordsCurrent = tempTextureCoordBuffer;
			clippedPolyVertexCount = 0;
			int num = polyVertexCount - 1;
			for (int i = 0; i < polyVertexCount; i++)
			{
				double bC = poly[num].X - clipBounds.X * poly[num].W;
				double bC2 = poly[i].X - clipBounds.X * poly[i].W;
				VectorMath.GenericClipAgainstPlane(clippedPoly, clippedPolyTextureCoords, ref clippedPolyVertexCount, poly, polyTextureCoords, num, i, bC, bC2);
				num = i;
			}
			if (clippedPolyVertexCount == 0)
			{
				return;
			}
			VectorMath.Swap<Point4D[]>(ref array, ref clippedPoly);
			VectorMath.Swap<Point2D[]>(ref clippedPolyTextureCoordsCurrent, ref clippedPolyTextureCoords);
			int num2 = clippedPolyVertexCount;
			clippedPolyVertexCount = 0;
			int num3 = num2 - 1;
			for (int j = 0; j < num2; j++)
			{
				double bC3 = (clipBounds.X + clipBounds.SizeX) * array[num3].W - array[num3].X;
				double bC4 = (clipBounds.X + clipBounds.SizeX) * array[j].W - array[j].X;
				VectorMath.GenericClipAgainstPlane(clippedPoly, clippedPolyTextureCoords, ref clippedPolyVertexCount, array, clippedPolyTextureCoordsCurrent, num3, j, bC3, bC4);
				num3 = j;
			}
			if (clippedPolyVertexCount == 0)
			{
				return;
			}
			VectorMath.Swap<Point4D[]>(ref array, ref clippedPoly);
			VectorMath.Swap<Point2D[]>(ref clippedPolyTextureCoordsCurrent, ref clippedPolyTextureCoords);
			num2 = clippedPolyVertexCount;
			clippedPolyVertexCount = 0;
			int num4 = num2 - 1;
			for (int k = 0; k < num2; k++)
			{
				double bC5 = array[num4].Y - clipBounds.Y * array[num4].W;
				double bC6 = array[k].Y - clipBounds.Y * array[k].W;
				VectorMath.GenericClipAgainstPlane(clippedPoly, clippedPolyTextureCoords, ref clippedPolyVertexCount, array, clippedPolyTextureCoordsCurrent, num4, k, bC5, bC6);
				num4 = k;
			}
			if (clippedPolyVertexCount == 0)
			{
				return;
			}
			VectorMath.Swap<Point4D[]>(ref array, ref clippedPoly);
			VectorMath.Swap<Point2D[]>(ref clippedPolyTextureCoordsCurrent, ref clippedPolyTextureCoords);
			num2 = clippedPolyVertexCount;
			clippedPolyVertexCount = 0;
			int num5 = num2 - 1;
			for (int l = 0; l < num2; l++)
			{
				double bC7 = (clipBounds.Y + clipBounds.SizeY) * array[num5].W - array[num5].Y;
				double bC8 = (clipBounds.Y + clipBounds.SizeY) * array[l].W - array[l].Y;
				VectorMath.GenericClipAgainstPlane(clippedPoly, clippedPolyTextureCoords, ref clippedPolyVertexCount, array, clippedPolyTextureCoordsCurrent, num5, l, bC7, bC8);
				num5 = l;
			}
			if (clippedPolyVertexCount == 0)
			{
				return;
			}
			VectorMath.Swap<Point4D[]>(ref array, ref clippedPoly);
			VectorMath.Swap<Point2D[]>(ref clippedPolyTextureCoordsCurrent, ref clippedPolyTextureCoords);
			num2 = clippedPolyVertexCount;
			clippedPolyVertexCount = 0;
			int num6 = num2 - 1;
			for (int m = 0; m < num2; m++)
			{
				double bC9 = array[num6].Z - clipBounds.Z * array[num6].W;
				double bC10 = array[m].Z - clipBounds.Z * array[m].W;
				VectorMath.GenericClipAgainstPlane(clippedPoly, clippedPolyTextureCoords, ref clippedPolyVertexCount, array, clippedPolyTextureCoordsCurrent, num6, m, bC9, bC10);
				num6 = m;
			}
			if (clippedPolyVertexCount == 0)
			{
				return;
			}
			VectorMath.Swap<Point4D[]>(ref array, ref clippedPoly);
			VectorMath.Swap<Point2D[]>(ref clippedPolyTextureCoordsCurrent, ref clippedPolyTextureCoords);
			num2 = clippedPolyVertexCount;
			clippedPolyVertexCount = 0;
			int num7 = num2 - 1;
			for (int n = 0; n < num2; n++)
			{
				double bC11 = (clipBounds.Z + clipBounds.SizeZ) * array[num7].W - array[num7].Z;
				double bC12 = (clipBounds.Z + clipBounds.SizeZ) * array[n].W - array[n].Z;
				VectorMath.GenericClipAgainstPlane(clippedPoly, clippedPolyTextureCoords, ref clippedPolyVertexCount, array, clippedPolyTextureCoordsCurrent, num7, n, bC11, bC12);
				num7 = n;
			}
		}

		private static void GenericClipAgainstPlane(Point4D[] clippedPoly, Point2D[] clippedPolyTextureCoords, ref int clippedPolyVertexCount, Point4D[] clippedPolyCurrent, Point2D[] clippedPolyTextureCoordsCurrent, int p0Idx, int p1Idx, double BC0, double BC1)
		{
			if (BC1 >= 0.0)
			{
				if (BC0 < 0.0)
				{
					double alpha = BC0 / (BC0 - BC1);
					clippedPoly[clippedPolyVertexCount] = Point4D.Lerp(clippedPolyCurrent[p0Idx], clippedPolyCurrent[p1Idx], alpha);
					if (clippedPolyTextureCoords != null)
					{
						clippedPolyTextureCoords[clippedPolyVertexCount] = Point2D.Lerp(clippedPolyTextureCoordsCurrent[p0Idx], clippedPolyTextureCoordsCurrent[p1Idx], alpha);
					}
					clippedPolyVertexCount++;
				}
				clippedPoly[clippedPolyVertexCount] = clippedPolyCurrent[p1Idx];
				if (clippedPolyTextureCoords != null)
				{
					clippedPolyTextureCoords[clippedPolyVertexCount] = clippedPolyTextureCoordsCurrent[p1Idx];
				}
				clippedPolyVertexCount++;
				return;
			}
			if (BC0 >= 0.0)
			{
				double alpha2 = BC0 / (BC0 - BC1);
				clippedPoly[clippedPolyVertexCount] = Point4D.Lerp(clippedPolyCurrent[p0Idx], clippedPolyCurrent[p1Idx], alpha2);
				if (clippedPolyTextureCoords != null)
				{
					clippedPolyTextureCoords[clippedPolyVertexCount] = Point2D.Lerp(clippedPolyTextureCoordsCurrent[p0Idx], clippedPolyTextureCoordsCurrent[p1Idx], alpha2);
				}
				clippedPolyVertexCount++;
			}
		}

		public static int Clamp(int value, int minimum, int maximum)
		{
			if (value < minimum)
			{
				return minimum;
			}
			if (value <= maximum)
			{
				return value;
			}
			return maximum;
		}

		public static long Clamp(long value, long minimum, long maximum)
		{
			if (value < minimum)
			{
				return minimum;
			}
			if (value <= maximum)
			{
				return value;
			}
			return maximum;
		}

		public static int CeilLog2(long value)
		{
			long num = value;
			int num2 = 0;
			if (num >= 4294967296L)
			{
				num >>= 32;
				num2 += 32;
			}
			if (num >= 65536L)
			{
				num >>= 16;
				num2 += 16;
			}
			if (num >= 256L)
			{
				num >>= 8;
				num2 += 8;
			}
			if (num >= 16L)
			{
				num >>= 4;
				num2 += 4;
			}
			if (num >= 4L)
			{
				num >>= 2;
				num2 += 2;
			}
			if (num >= 2L)
			{
				num2++;
			}
			return num2 + ((value > 1L << num2) ? 1 : 0);
		}

		public static long DivPow2RoundUp(long value, int power)
		{
			return VectorMath.DivRoundUp(value, 1L << power);
		}

		public static long DivRoundUp(long value, long denominator)
		{
			return (value + denominator - 1L) / denominator;
		}

		public static double Clamp(double d, double min, double max)
		{
			if (double.IsNaN(d))
			{
				return max;
			}
			if (d < min)
			{
				return min;
			}
			if (d > max)
			{
				return max;
			}
			return d;
		}

		public static double NormalizeAngle(double angle)
		{
			return (angle % 360.0 + 360.0) % 360.0;
		}

		public static double AngleDelta(double angle1, double angle2)
		{
			double num = VectorMath.NormalizeAngle(angle1 - angle2);
			return Math.Min(num, 360.0 - num);
		}

		public static Point3D Add(Point3D point1, Point3D point2)
		{
			return new Point3D(point1.X + point2.X, point1.Y + point2.Y, point1.Z + point2.Z);
		}

		public static Point3D Subtract(Point3D point1, Point3D point2)
		{
			return new Point3D(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
		}

		public static Point Add(Point point1, Point point2)
		{
			return new Point(point1.X + point2.X, point1.Y + point2.Y);
		}

		public static Point Subtract(Point point1, Point point2)
		{
			return new Point(point1.X - point2.X, point1.Y - point2.Y);
		}

		public static Point Multiply(Point point, double scalar)
		{
			return new Point(point.X * scalar, point.Y * scalar);
		}

		public static Point3D Multiply(Point3D point, double scalar)
		{
			return new Point3D(point.X * scalar, point.Y * scalar, point.Z * scalar);
		}

		public static double Distance(Point3D from, Point3D to)
		{
			return VectorMath.GetLength(VectorMath.Subtract(to, from));
		}

		public static double Distance(Point from, Point to)
		{
			return VectorMath.GetLength(VectorMath.Subtract(to, from));
		}

		public static double GetLength(Point3D point)
		{
			return Math.Sqrt(point.X * point.X + point.Y * point.Y + point.Z * point.Z);
		}

		public static double GetLength(Point point)
		{
			return Math.Sqrt(point.X * point.X + point.Y * point.Y);
		}

		public static Point3D Lerp(Point3D point1, Point3D point2, double weight)
		{
			return new Point3D(point1.X + (point2.X - point1.X) * weight, point1.Y + (point2.Y - point1.Y) * weight, point1.Z + (point2.Z - point1.Z) * weight);
		}

		public static Point3D Normalize(Point3D point)
		{
			double length = VectorMath.GetLength(point);
			if (length == 0.0)
			{
				return point;
			}
			double num = 1.0 / length;
			return new Point3D(point.X * num, point.Y * num, point.Z * num);
		}

		public static double Dot(Point3D point1, Point3D point2)
		{
			return point1.X * point2.X + point1.Y * point2.Y + point1.Z * point2.Z;
		}

		public static Point3D Cross(Point3D left, Point3D right)
		{
			return new Point3D(left.Y * right.Z - left.Z * right.Y, left.Z * right.X - left.X * right.Z, left.X * right.Y - left.Y * right.X);
		}

		public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
		{
			return new Matrix(matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21, matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22, matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21, matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22, matrix1.OffsetX * matrix2.M11 + matrix1.OffsetY * matrix2.M21 + matrix2.OffsetX, matrix1.OffsetX * matrix2.M12 + matrix1.OffsetY * matrix2.M22 + matrix2.OffsetY);
		}

		public static Matrix Invert(Matrix matrix)
		{
			double num = matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;
			if (num == 0.0)
			{
				throw new InvalidOperationException("Matrix invert failed, determinant is 0.");
			}
			double num2 = 1.0 / num;
			return new Matrix(matrix.M22 * num2, -1.0 * matrix.M12 * num2, -1.0 * matrix.M21 * num2, matrix.M11 * num2, (matrix.OffsetY * matrix.M21 - matrix.OffsetX * matrix.M22) * num2, (matrix.OffsetX * matrix.M12 - matrix.OffsetY * matrix.M11) * num2);
		}

		public static Matrix InferTransform(Rect from, Rect to)
		{
			double num = (to.Right - to.X) / (from.Right - from.X);
			double num2 = (to.Bottom - to.Y) / (from.Bottom - from.Y);
			Matrix identity = Matrix.Identity;
			identity.M11 = num;
			identity.M22 = num2;
			identity.OffsetX = -num * from.X + to.X;
			identity.OffsetY = -num2 * from.Y + to.Y;
			return identity;
		}

		public static Matrix UnitToPoints(Point to1, Point to2, Point to3)
		{
			return new Matrix(to2.X - to1.X, to2.Y - to1.Y, to3.X - to1.X, to3.Y - to1.Y, to1.X, to1.Y);
		}

		public static Matrix Conversion(Point from1, Point from2, Point from3, Point to1, Point to2, Point to3)
		{
			Matrix matrix = VectorMath.Invert(VectorMath.UnitToPoints(from1, from2, from3));
			Matrix matrix2 = VectorMath.UnitToPoints(to1, to2, to3);
			return VectorMath.Multiply(matrix, matrix2);
		}

		public static Point3D Transform(Matrix3D matrix, Point3D point)
		{
			double num = point.X * matrix.M14 + point.Y * matrix.M24 + point.Z * matrix.M34 + matrix.M44;
			double num2 = (num == 1.0) ? 1.0 : (1.0 / num);
			return new Point3D((point.X * matrix.M11 + point.Y * matrix.M21 + point.Z * matrix.M31 + matrix.OffsetX) * num2, (point.X * matrix.M12 + point.Y * matrix.M22 + point.Z * matrix.M32 + matrix.OffsetY) * num2, (point.X * matrix.M13 + point.Y * matrix.M23 + point.Z * matrix.M33 + matrix.OffsetZ) * num2);
		}

		public static Point4D Transform(Matrix3D matrix, Point4D point)
		{
			return new Point4D(point.X * matrix.M11 + point.Y * matrix.M21 + point.Z * matrix.M31 + point.W * matrix.OffsetX, point.X * matrix.M12 + point.Y * matrix.M22 + point.Z * matrix.M32 + point.W * matrix.OffsetY, point.X * matrix.M13 + point.Y * matrix.M23 + point.Z * matrix.M33 + point.W * matrix.OffsetZ, point.X * matrix.M14 + point.Y * matrix.M24 + point.Z * matrix.M34 + point.W * matrix.M44);
		}

		public static Matrix3D TranslationMatrix3D(Point3D offset)
		{
			return new Matrix3D(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, offset.X, offset.Y, offset.Z, 1.0);
		}

		public static Matrix3D TranslationMatrix3D(double offsetX, double offsetY, double offsetZ)
		{
			return new Matrix3D(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, offsetX, offsetY, offsetZ, 1.0);
		}

		public static Matrix3D ScalingMatrix3D(double valueX, double valueY, double valueZ)
		{
			return new Matrix3D(valueX, 0.0, 0.0, 0.0, 0.0, valueY, 0.0, 0.0, 0.0, 0.0, valueZ, 0.0, 0.0, 0.0, 0.0, 1.0);
		}

		public static Matrix3D RotationMatrix3DX(double angle)
		{
			double num = Math.Sin(angle);
			double num2 = Math.Cos(angle);
			return new Matrix3D(1.0, 0.0, 0.0, 0.0, 0.0, num2, num, 0.0, 0.0, -num, num2, 0.0, 0.0, 0.0, 0.0, 1.0);
		}

		public static Matrix3D RotationMatrix3DY(double angle)
		{
			double num = Math.Sin(angle);
			double num2 = Math.Cos(angle);
			return new Matrix3D(num2, 0.0, -num, 0.0, 0.0, 1.0, 0.0, 0.0, num, 0.0, num2, 0.0, 0.0, 0.0, 0.0, 1.0);
		}

		public static Matrix3D RotationMatrix3DZ(double angle)
		{
			double num = Math.Sin(angle);
			double num2 = Math.Cos(angle);
			return new Matrix3D(num2, num, 0.0, 0.0, -num, num2, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
		}

		public static Matrix3D ConvertMatrixToMatrix3D(Matrix matrix)
		{
			return new Matrix3D(matrix.M11, matrix.M12, 0.0, 0.0, matrix.M21, matrix.M22, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, matrix.OffsetX, matrix.OffsetY, 0.0, 1.0);
		}

		public static Matrix3D GetAxisRedefinitionMatrix(Point3D xAxis, Point3D yAxis, Point3D zAxis)
		{
			return new Matrix3D(xAxis.X, yAxis.X, zAxis.X, 0.0, xAxis.Y, yAxis.Y, zAxis.Y, 0.0, xAxis.Z, yAxis.Z, zAxis.Z, 0.0, 0.0, 0.0, 0.0, 1.0);
		}

		public static Matrix3D ProjectOnPlane(Matrix3D projectToWorld, Plane3D plane)
		{
			Matrix3D matrix3D = projectToWorld;
			double num = plane.A * matrix3D.M11 + plane.B * matrix3D.M12 + plane.C * matrix3D.M13 - plane.D * matrix3D.M14;
			double num2 = plane.A * matrix3D.M21 + plane.B * matrix3D.M22 + plane.C * matrix3D.M23 - plane.D * matrix3D.M24;
			double num3 = plane.A * matrix3D.M31 + plane.B * matrix3D.M32 + plane.C * matrix3D.M33 - plane.D * matrix3D.M34;
			double num4 = plane.A * matrix3D.OffsetX + plane.B * matrix3D.OffsetY + plane.C * matrix3D.OffsetZ - plane.D * matrix3D.M44;
			if (num3 == 0.0)
			{
				throw new InvalidOperationException();
			}
			double num5 = -1.0 / num3;
			Matrix3D matrix = new Matrix3D(1.0, 0.0, num * num5, 0.0, 0.0, 1.0, num2 * num5, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, num4 * num5, 1.0);
			return matrix * projectToWorld;
		}

		public static Matrix3D PerspectiveMatrix3D(double fieldOfViewY, double aspectRatio, double zNearPlane, double zFarPlane)
		{
			double num = 1.0 / Math.Tan(fieldOfViewY / 2.0);
			double m = num / aspectRatio;
			double num2 = zFarPlane - zNearPlane;
			return new Matrix3D(m, 0.0, 0.0, 0.0, 0.0, num, 0.0, 0.0, 0.0, 0.0, zFarPlane / num2, 1.0, 0.0, 0.0, -zNearPlane * zFarPlane / num2, 0.0);
		}

		public static Point[] GetCorners(Rect rect)
		{
			return new Point[]
			{
				new Point(rect.X, rect.Y),
				new Point(rect.Right, rect.Y),
				new Point(rect.Right, rect.Bottom),
				new Point(rect.X, rect.Bottom)
			};
		}

		public static Point3D IntersectLineToPlane(Point3D linePoint1, Point3D linePoint2, Plane3D plane)
		{
			Point3D point = VectorMath.Subtract(linePoint2, linePoint1);
			double num = plane.D - plane.A * linePoint1.X - plane.B * linePoint1.Y - plane.C * linePoint1.Z;
			double num2 = plane.A * point.X + plane.B * point.Y + plane.C * point.Z;
			if (num2 == 0.0)
			{
				throw new InvalidOperationException();
			}
			double scalar = num / num2;
			return VectorMath.Add(linePoint1, VectorMath.Multiply(point, scalar));
		}

		public static double LinePointDistanceSquared(Point2D line0, Point2D line1, Point2D point, out bool inLineSegment)
		{
			double num = ((point.X - line0.X) * (line1.X - line0.X) + (point.Y - line0.Y) * (line1.Y - line0.Y)) / Point2D.DistanceSquared(line0, line1);
			inLineSegment = (num >= 0.0 && num <= 1.0);
			Point2D pointA = new Point2D(line0.X + num * (line1.X - line0.X), line0.Y + num * (line1.Y - line0.Y));
			return Point2D.DistanceSquared(pointA, point);
		}

		public static void Swap<T>(ref T left, ref T right)
		{
			T t = left;
			left = right;
			right = t;
		}

		public static Uri TileSourceGetUriWrapper(MapControl.WPF.TileSource tileSource, TileId tileId)
		{
			return tileSource.GetUri((int)tileId.X, (int)tileId.Y, tileId.LevelOfDetail - 8);
		}

		public static bool OrientedBoundingBoxIntersectsAxisAlignedBoundingBox(Point2D orientedBBox0, Point2D orientedBBox1, double orientedBBoxWidth, Rect axisAlignedBBox)
		{
			if (orientedBBoxWidth <= 0.0)
			{
				throw new ArgumentException("box must have positive width");
			}
			Point2D point2D = Point2D.Normalize(orientedBBox1 - orientedBBox0);
			point2D = orientedBBoxWidth * 0.5 * point2D;
			Point2D point = new Point2D(-point2D.Y, point2D.X);
			Point2D[][] array = new Point2D[][]
			{
				new Point2D[]
				{
					orientedBBox0 + point - point2D,
					orientedBBox1 + point + point2D,
					orientedBBox1 - point + point2D,
					orientedBBox0 - point - point2D
				},
				new Point2D[]
				{
					new Point2D(axisAlignedBBox.Left, axisAlignedBBox.Top),
					new Point2D(axisAlignedBBox.Right, axisAlignedBBox.Top),
					new Point2D(axisAlignedBBox.Right, axisAlignedBBox.Bottom),
					new Point2D(axisAlignedBBox.Left, axisAlignedBBox.Bottom)
				}
			};
			Point2D[] array2 = array[0];
			Point2D[] array3 = array[1];
			for (int i = 0; i < 1; i++)
			{
				Point2D point2D2 = array2[1] - array2[0];
				Point2D point2D3 = array2[3] - array2[0];
				point2D2 = 1.0 / point2D2.LengthSquared() * point2D2;
				point2D3 = 1.0 / point2D3.LengthSquared() * point2D3;
				double num = Point2D.Dot(array2[0], point2D2);
				double num2 = Point2D.Dot(array2[0], point2D3);
				for (int j = 0; j < 2; j++)
				{
					Point2D right = (j == 0) ? point2D2 : point2D3;
					double num3 = (j == 0) ? num : num2;
					double num4 = 1.7976931348623157E+308;
					double num5 = -1.7976931348623157E+308;
					double num6 = Point2D.Dot(array3[0], right);
					if (num6 < num4)
					{
						num4 = num6;
					}
					if (num6 > num5)
					{
						num5 = num6;
					}
					num6 = Point2D.Dot(array3[1], right);
					if (num6 < num4)
					{
						num4 = num6;
					}
					if (num6 > num5)
					{
						num5 = num6;
					}
					num6 = Point2D.Dot(array3[2], right);
					if (num6 < num4)
					{
						num4 = num6;
					}
					if (num6 > num5)
					{
						num5 = num6;
					}
					num6 = Point2D.Dot(array3[3], right);
					if (num6 < num4)
					{
						num4 = num6;
					}
					if (num6 > num5)
					{
						num5 = num6;
					}
					if (num4 - num3 > 1.0 || num5 - num3 < 0.0)
					{
						return false;
					}
				}
				VectorMath.Swap<Point2D[]>(ref array2, ref array3);
			}
			return true;
		}
	}
}
