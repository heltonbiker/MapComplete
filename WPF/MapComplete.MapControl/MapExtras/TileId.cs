using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Microsoft.Maps.MapExtras
{
	internal struct TileId : IComparable<TileId>
	{
		private int levelOfDetail;

		private long x;

		private long y;

		public int LevelOfDetail
		{
			get
			{
				return this.levelOfDetail;
			}
		}

		public long X
		{
			get
			{
				return this.x;
			}
		}

		public long Y
		{
			get
			{
				return this.y;
			}
		}

		public bool HasParent
		{
			get
			{
				return this.levelOfDetail > 0;
			}
		}

		public TileId(int levelOfDetail, long x, long y)
		{
			this.levelOfDetail = levelOfDetail;
			this.x = x;
			this.y = y;
		}

		public TileId GetParent()
		{
			if (!this.HasParent)
			{
				throw new InvalidOperationException("Level 0 tile has no parent");
			}
			return new TileId(this.levelOfDetail - 1, this.x >> 1, this.y >> 1);
		}

		public static TileId operator -(TileId left, TileId right)
		{
			if (left.LevelOfDetail > right.LevelOfDetail)
			{
				throw new InvalidOperationException("Cannot subtract these tiles. Left must be higher level of detail than right.");
			}
			int num = right.levelOfDetail - left.levelOfDetail;
			return new TileId(num, right.x - (left.x << num), right.y - (left.y << num));
		}

		public bool IsChildOf(TileId other)
		{
			if (this.levelOfDetail < other.LevelOfDetail)
			{
				return false;
			}
			int num = this.levelOfDetail - other.levelOfDetail;
			return this.x >> num == other.x && this.y >> num == other.y;
		}

		public int GetChildIndex(TileId child)
		{
			if (child.GetParent() != this)
			{
				throw new InvalidOperationException("Must be parent of child.");
			}
			return 2 * (int)(child.Y - this.Y * 2L) + (int)(child.X - this.X * 2L);
		}

		public IEnumerable<TileId> GetChildren()
		{
			long num = this.x << 1;
			long num2 = this.y << 1;
			return new TileId[]
			{
				new TileId(this.levelOfDetail + 1, num, num2),
				new TileId(this.levelOfDetail + 1, num + 1L, num2),
				new TileId(this.levelOfDetail + 1, num, num2 + 1L),
				new TileId(this.levelOfDetail + 1, num + 1L, num2 + 1L)
			};
		}

		public string GetRequestCode()
		{
			if (this.levelOfDetail < 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(this.levelOfDetail);
			stringBuilder.Length = this.levelOfDetail;
			long num = this.x;
			long num2 = this.y;
			for (int i = 0; i < this.levelOfDetail; i++)
			{
				stringBuilder[this.levelOfDetail - 1 - i] = "0123"[(int)((num & 1L) + 2L * (num2 & 1L))];
				num >>= 1;
				num2 >>= 1;
			}
			return stringBuilder.ToString();
		}

		public override int GetHashCode()
		{
			return this.levelOfDetail << 26 + (int)this.x << 13 + (int)this.y;
		}

		public override bool Equals(object obj)
		{
			return obj is TileId && (TileId)obj == this;
		}

		public static bool operator ==(TileId left, TileId right)
		{
			return left.levelOfDetail == right.levelOfDetail && left.x == right.x && left.y == right.y;
		}

		public static bool operator !=(TileId left, TileId right)
		{
			return !(left == right);
		}

		public static bool operator <(TileId left, TileId right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator >(TileId left, TileId right)
		{
			return left.CompareTo(right) > 0;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "LOD: {0} Tile ({1},{2}) {3}", new object[]
			{
				this.levelOfDetail,
				this.x,
				this.y,
				this.GetRequestCode()
			});
		}

		public int CompareTo(TileId other)
		{
			int num = this.levelOfDetail.CompareTo(other.levelOfDetail);
			if (num == 0)
			{
				num = this.x.CompareTo(other.x);
				if (num == 0)
				{
					num = this.y.CompareTo(other.y);
				}
			}
			return num;
		}
	}
}
