using System;

namespace Microsoft.Maps.MapExtras
{
	internal struct RectangularSolid
	{
		private double x;

		private double y;

		private double z;

		private double sizeX;

		private double sizeY;

		private double sizeZ;

		public double X
		{
			get
			{
				return this.x;
			}
		}

		public double Y
		{
			get
			{
				return this.y;
			}
		}

		public double Z
		{
			get
			{
				return this.z;
			}
		}

		public double SizeX
		{
			get
			{
				return this.sizeX;
			}
		}

		public double SizeY
		{
			get
			{
				return this.sizeY;
			}
		}

		public double SizeZ
		{
			get
			{
				return this.sizeZ;
			}
		}

		public RectangularSolid(double x, double y, double z, double sizeX, double sizeY, double sizeZ)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			if (sizeX < 0.0 || sizeY < 0.0 || sizeZ < 0.0)
			{
				throw new ArgumentException("It is not valid to set a size dimension to less than 0 in a Rect3D structure");
			}
			this.sizeX = sizeX;
			this.sizeY = sizeY;
			this.sizeZ = sizeZ;
		}

		public static bool operator ==(RectangularSolid left, RectangularSolid right)
		{
			return left.x == right.x && left.y == right.y && left.z == right.z && left.sizeX == right.sizeX && left.sizeY == right.sizeY && left.sizeZ == right.sizeZ;
		}

		public static bool operator !=(RectangularSolid left, RectangularSolid right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return base.GetType() == obj.GetType() && this == (RectangularSolid)obj;
		}

		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode() ^ this.sizeX.GetHashCode() ^ this.sizeY.GetHashCode() ^ this.sizeZ.GetHashCode();
		}
	}
}
