using MapComplete.MapControl;
using Microsoft.Maps.MapControl.WPF.Core;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Microsoft.Maps.MapControl.WPF
{
	public class TileSource : INotifyPropertyChanged
	{
		//public const string QuadKeyUriFragment = "{quadkey}";

		//public const string UriSchemeUriFragment = "{UriScheme}";

		//public const string UriCulture = "{UriCulture}";

		//public const string UriRegion = "{UriRegion}";

		//public const string UriVersion = "{UriVersion}";

		//public const string UriKey = "{UriKey}";

		//public const string SubdomainUriFragment = "{subdomain}";

		//public const string UriRegionLoc = "{RegionLocation}";

		//private const string InternalQuadKeyUriFragment = "{QUADKEY}";

		//private const string InternalSubdomainUriFragment = "{SUBDOMAIN}";

		private string uriFormat;

		private string convertedUriFormat;

		private Visibility visibility;

		private string[][] subdomainsList;

		private int maxX;

		private int maxY;

		//private ImageCallback directImage;

		public event PropertyChangedEventHandler PropertyChanged;

		public string UriFormat
		{
			get
			{
				return this.uriFormat;
			}
			set
			{
				if (this.uriFormat != value)
				{
					this.uriFormat = value;
					this.convertedUriFormat = TileSource.ReplaceString(this.uriFormat, "{UriScheme}", Map.UriScheme);
					this.convertedUriFormat = TileSource.ReplaceString(this.convertedUriFormat, "{quadkey}", "{QUADKEY}");
					this.convertedUriFormat = TileSource.ReplaceString(this.convertedUriFormat, "{subdomain}", "{SUBDOMAIN}");
					this.OnPropertyChanged("UriFormat");
				}
			}
		}

		public Visibility Visibility
		{
			get
			{
				return this.visibility;
			}
			set
			{
				this.visibility = value;
				this.OnPropertyChanged("Visibility");
			}
		}

		//public virtual bool SuppliesImagesDirectly
		//{
		//	get
		//	{
		//		return this.directImage != null;
		//	}
		//}

		//public ImageCallback DirectImage
		//{
		//	get
		//	{
		//		return this.directImage;
		//	}
		//	set
		//	{
		//		this.directImage = value;
		//	}
		//}

		public TileSource()
		{
			this.subdomainsList = new string[][]
			{
				new string[]
				{
					"0",
					"2",
					"4",
					"6"
				},
				new string[]
				{
					"1",
					"3",
					"5",
					"7"
				}
			};
			this.maxX = 2;
			this.maxY = 4;
		}

		public TileSource(string uriFormat) : this()
		{
			this.UriFormat = uriFormat;
		}

		public virtual Uri GetUri(int x, int y, int zoomLevel)
		{
			Uri result = null;
			QuadKey quadKey = new QuadKey(x, y, zoomLevel);
			if (!string.IsNullOrEmpty(this.convertedUriFormat) && !string.IsNullOrEmpty(quadKey.Key) && this.Visibility == Visibility.Visible)
			{
				result = new Uri(this.convertedUriFormat.Replace("{QUADKEY}", quadKey.Key).Replace("{SUBDOMAIN}", this.GetSubdomain(quadKey)));
			}
			return result;
		}

		public virtual string GetSubdomain(QuadKey quadKey)
		{
			if (this.subdomainsList == null)
			{
				return string.Empty;
			}
			return this.subdomainsList[quadKey.X % this.maxX][quadKey.Y % this.maxY];
		}

		public void SetSubdomains(string[][] subdomains)
		{
			if (subdomains == null)
			{
				this.subdomainsList = null;
				return;
			}
			if (subdomains.Length == 0 || subdomains[0].Length == 0)
			{
				throw new ArgumentException(ExceptionStrings.TileSource_InvalidSubdomains_LengthMoreThan0);
			}
			int num = subdomains[0].Length;
			for (int i = 0; i < subdomains.Length; i++)
			{
				string[] array = subdomains[i];
				if (array.Length != num)
				{
					throw new ArgumentException(ExceptionStrings.TileSource_InvalidSubdomains_DifferentLength);
				}
				string[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j] == null)
					{
						throw new ArgumentException(ExceptionStrings.TileSource_InvalidSubdomain_stringNull);
					}
				}
			}
			this.subdomainsList = subdomains;
			this.maxX = subdomains.Length;
			this.maxY = num;
		}

		//public virtual BitmapImage GetImage(long x, long y, int zoomLevel)
		//{
		//	return null;
		//}

		private static string ReplaceString(string input, string pattern, string replacement)
		{
			return Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
				propertyChanged(this, e);
			}
		}
	}
}
