using System;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	public struct QuadKey
	{
		public int ZoomLevel
		{
			get;
			set;
		}

		public int X
		{
			get;
			set;
		}

		public int Y
		{
			get;
			set;
		}

		public string Key
		{
			get
			{
				return QuadKey.QuadPixelToQuadKey(this.X, this.Y, this.ZoomLevel);
			}
		}

		public QuadKey(int x, int y, int zoomLevel)
		{
			this = default(QuadKey);
			this.ZoomLevel = zoomLevel;
			this.X = x;
			this.Y = y;
		}

		public QuadKey(string quadKey)
		{
			this = default(QuadKey);
			int x;
			int y;
			int zoomLevel;
			QuadKey.QuadKeyToQuadPixel(quadKey, out x, out y, out zoomLevel);
			this.ZoomLevel = zoomLevel;
			this.X = x;
			this.Y = y;
		}

		private static string QuadPixelToQuadKey(int x, int y, int zoomLevel)
		{
			uint num = 1u << zoomLevel;
			string text = string.Empty;
			if (x < 0 || (long)x >= (long)((ulong)num) || y < 0 || (long)y >= (long)((ulong)num))
			{
				return null;
			}
			for (int i = 1; i <= zoomLevel; i++)
			{
				switch (2 * (y % 2) + x % 2)
				{
					case 0:
						text = "0" + text;
						break;
					case 1:
						text = "1" + text;
						break;
					case 2:
						text = "2" + text;
						break;
					case 3:
						text = "3" + text;
						break;
				}
				x /= 2;
				y /= 2;
			}
			return text;
		}

		private static void QuadKeyToQuadPixel(string quadKey, out int x, out int y, out int zoomLevel)
		{
			x = 0;
			y = 0;
			zoomLevel = 0;
			if (!string.IsNullOrEmpty(quadKey))
			{
				zoomLevel = quadKey.Length;
				for (int i = 0; i < quadKey.Length; i++)
				{
					switch (quadKey[i])
					{
						case '0':
							x *= 2;
							y *= 2;
							break;
						case '1':
							x = x * 2 + 1;
							y *= 2;
							break;
						case '2':
							x *= 2;
							y = y * 2 + 1;
							break;
						case '3':
							x = x * 2 + 1;
							y = y * 2 + 1;
							break;
					}
				}
			}
		}

		public static bool operator ==(QuadKey tile1, QuadKey tile2)
		{
			return tile1.X == tile2.X && tile1.Y == tile2.Y && tile1.ZoomLevel == tile2.ZoomLevel;
		}

		public static bool operator !=(QuadKey tile1, QuadKey tile2)
		{
			return !(tile1 == tile2);
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is QuadKey && this == (QuadKey)obj;
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.ZoomLevel.GetHashCode();
		}
	}
}
