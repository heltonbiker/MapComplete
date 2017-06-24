using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Microsoft.Maps.MapExtras
{
	internal delegate void RasterTileAvailableDelegate(BitmapSource image, Rect tileSubregion, Dictionary<string, string> metadata, object token);

	internal class GenericRasterTileDownloader : RasterTileDownloader
	{
		//internal class TileRequest
		//{
		//	public TileId TileId
		//	{
		//		get;
		//		set;
		//	}

		//	public TileEdgeFlags TileEdgeFlags
		//	{
		//		get;
		//		set;
		//	}

		//	public object Token
		//	{
		//		get;
		//		set;
		//	}

		//	public RasterTileAvailableDelegate TileAvailableDelegate
		//	{
		//		get;
		//		set;
		//	}

		//	public bool Cancelled
		//	{
		//		get;
		//		set;
		//	}

		//	public BitmapImageRequest WebRequest
		//	{
		//		get;
		//		set;
		//	}
		//}

		//private const string HeaderMetadataPrefix = "X-VE-";

		//private Dictionary<TileId, GenericRasterTileDownloader.TileRequest> tileRequests = new Dictionary<TileId, GenericRasterTileDownloader.TileRequest>();

		//private Dictionary<BitmapImage, GenericRasterTileDownloader.TileRequest> tileRequestsByBitmapImage = new Dictionary<BitmapImage, GenericRasterTileDownloader.TileRequest>();

		//private TileUriDelegate tileUriDelegate;

		//private TileImageDelegate tileImageDelegate;

		//private Microsoft.Maps.MapControl.WPF.TileSource tileSource;

		//private Dispatcher uiThreadDispatcher;

		//public OverlapBorderPresence OverlapBorderPresence
		//{
		//	get;
		//	protected set;
		//}

		//public Func<TileId, BitmapSource> ProvideMissingTileImage
		//{
		//	get;
		//	set;
		//}

		public GenericRasterTileDownloader(Microsoft.Maps.MapControl.WPF.TileSource tileSource, OverlapBorderPresence overlapBordersPresence, Dispatcher uiThreadDispatcher)
		{
		//	this.tileUriDelegate = ((TileId tileId) => VectorMath.TileSourceGetUriWrapper(tileSource, tileId));
		//	if (tileSource.DirectImage != null)
		//	{
		//		this.tileImageDelegate = ((TileId tileId) => tileSource.DirectImage(tileId.X, tileId.Y, tileId.LevelOfDetail - 8));
		//	}
		//	else
		//	{
		//		this.tileImageDelegate = ((TileId tileId) => tileSource.GetImage(tileId.X, tileId.Y, tileId.LevelOfDetail - 8));
		//	}
		//	this.tileSource = tileSource;
		//	this.OverlapBorderPresence = overlapBordersPresence;
		//	this.uiThreadDispatcher = uiThreadDispatcher;
		}

		public override void DownloadTile(TileId tileId, TileEdgeFlags tileEdgeFlags, object token, RasterTileAvailableDelegate tileAvailableDelegate, int priority)
		{
			//Uri uri = this.tileUriDelegate(tileId);
			//if (uri != null)
			//{
			//	GenericRasterTileDownloader.TileRequest tileRequest;
			//	if (!this.tileRequests.TryGetValue(tileId, out tileRequest))
			//	{
			//		tileRequest = (this.tileRequests[tileId] = new GenericRasterTileDownloader.TileRequest
			//		{
			//			TileId = tileId,
			//			Token = token,
			//			TileEdgeFlags = tileEdgeFlags,
			//			TileAvailableDelegate = tileAvailableDelegate
			//		});
			//		tileRequest.WebRequest = BitmapImageRequestQueue.Instance.CreateRequest(uri, (NetworkPriority)priority, tileRequest, new BitmapImageRequestCompletedHandler(this.TileDownloadCompleted));
			//		return;
			//	}
			//	throw new InvalidOperationException("Multiple concurrent downloads of the same tile is not supported.");
			//}
			//else
			//{
			//	if (!this.tileSource.SuppliesImagesDirectly)
			//	{
			//		tileAvailableDelegate(null, default(Rect), null, token);
			//		return;
			//	}
			//	GenericRasterTileDownloader.TileRequest tileRequest2;
			//	if (!this.tileRequests.TryGetValue(tileId, out tileRequest2))
			//	{
			//		tileRequest2 = (this.tileRequests[tileId] = new GenericRasterTileDownloader.TileRequest
			//		{
			//			TileId = tileId,
			//			Token = token,
			//			TileEdgeFlags = tileEdgeFlags,
			//			TileAvailableDelegate = tileAvailableDelegate
			//		});
			//		tileRequest2.WebRequest = BitmapImageRequestQueue.Instance.CreateRequest(this.tileImageDelegate, (NetworkPriority)priority, tileRequest2, new BitmapImageRequestCompletedHandler(this.TileDownloadCompleted));
			//		return;
			//	}
			//	throw new InvalidOperationException("Multiple concurrent downloads of the same tile is not supported.");
			//}
		}

		public override void UpdateTileDownloadPriority(TileId tileId, int priority)
		{
		//	GenericRasterTileDownloader.TileRequest tileRequest;
		//	if (!this.tileRequests.TryGetValue(tileId, out tileRequest))
		//	{
		//		throw new InvalidOperationException("Tile download must be in progress to update its priority.");
		//	}
		//	tileRequest.WebRequest.NetworkPriority = (NetworkPriority)priority;
		}

		public override void CancelTileDownload(TileId tileId)
		{
		//	GenericRasterTileDownloader.TileRequest tileRequest;
		//	if (!this.tileRequests.TryGetValue(tileId, out tileRequest))
		//	{
		//		throw new InvalidOperationException("Tile download must be in progress to be cancelled.");
		//	}
		//	tileRequest.Cancelled = true;
		//	tileRequest.WebRequest.AbortIfInQueue();
		//	this.tileRequests.Remove(tileId);
		}

		//private void TileDownloadCompleted(object userToken, BitmapImage bitmapImage, Exception error)
		//{
		//	if (bitmapImage != null && error != null)
		//	{
		//		throw new ArgumentException("only one of bitmapImage and error may be null");
		//	}
		//	GenericRasterTileDownloader.TileRequest tileRequest = (GenericRasterTileDownloader.TileRequest)userToken;
		//	if (!tileRequest.Cancelled)
		//	{
		//		BitmapSource bitmapSourceResult = null;
		//		Rect tileSubregionResult = default(Rect);
		//		if (!tileRequest.Cancelled)
		//		{
		//			if (error != null)
		//			{
		//				if (this.ProvideMissingTileImage != null)
		//				{
		//					bitmapSourceResult = this.ProvideMissingTileImage(tileRequest.TileId);
		//					tileSubregionResult = new Rect(1.0, 1.0, (double)(bitmapSourceResult.PixelWidth - 2), (double)(bitmapSourceResult.PixelHeight - 2));
		//				}
		//			}
		//			else if (this.OverlapBorderPresence == OverlapBorderPresence.OnAllEdges)
		//			{
		//				bitmapSourceResult = bitmapImage;
		//				tileSubregionResult = new Rect(1.0, 1.0, (double)(bitmapImage.PixelWidth - 2), (double)(bitmapImage.PixelHeight - 2));
		//			}
		//			else
		//			{
		//				try
		//				{
		//					int num = (bitmapImage.Format.BitsPerPixel + 7) / 8;
		//					int num2 = bitmapImage.PixelWidth * num;
		//					byte[] array = new byte[num2 * bitmapImage.PixelHeight];
		//					bitmapImage.CopyPixels(array, num2, 0);
		//					int pixelWidth = bitmapImage.PixelWidth;
		//					int pixelHeight = bitmapImage.PixelHeight;
		//					int num3 = bitmapImage.PixelWidth;
		//					int num4 = bitmapImage.PixelHeight;
		//					int num5;
		//					int num6;
		//					if (this.OverlapBorderPresence == OverlapBorderPresence.None)
		//					{
		//						num3 += 2;
		//						num4 += 2;
		//						num5 = 1;
		//						num6 = 1;
		//					}
		//					else
		//					{
		//						num3 += (tileRequest.TileEdgeFlags.IsLeftEdge ? 1 : 0) + (tileRequest.TileEdgeFlags.IsRightEdge ? 1 : 0);
		//						num4 += (tileRequest.TileEdgeFlags.IsTopEdge ? 1 : 0) + (tileRequest.TileEdgeFlags.IsBottomEdge ? 1 : 0);
		//						num5 = (tileRequest.TileEdgeFlags.IsLeftEdge ? 1 : 0);
		//						num6 = (tileRequest.TileEdgeFlags.IsTopEdge ? 1 : 0);
		//					}
		//					int num7 = num;
		//					int num8 = num3 * num7;
		//					byte[] array2 = new byte[num4 * num8];
		//					for (int i = 0; i < num4; i++)
		//					{
		//						for (int j = 0; j < num3; j++)
		//						{
		//							int num9 = Math.Max(0, Math.Min(pixelHeight - 1, i - num6));
		//							int num10 = Math.Max(0, Math.Min(pixelWidth - 1, j - num5));
		//							int num11 = num9 * num2 + num10 * num;
		//							int num12 = i * num8 + j * num7;
		//							for (int k = 0; k < num; k++)
		//							{
		//								array2[num12++] = array[num11++];
		//							}
		//						}
		//					}
		//					bitmapSourceResult = BitmapSource.Create(num3, num4, 96.0, 96.0, bitmapImage.Format, bitmapImage.Palette, array2, num8);
		//					bitmapSourceResult.Freeze();
		//					tileSubregionResult = new Rect(1.0, 1.0, (double)(num3 - 2), (double)(num4 - 2));
		//				}
		//				catch (Exception)
		//				{
		//				}
		//			}
		//			this.uiThreadDispatcher.BeginInvoke(new Action(delegate
		//			{
		//				if (!tileRequest.Cancelled)
		//				{
		//					tileRequest.TileAvailableDelegate(bitmapSourceResult, tileSubregionResult, null, tileRequest.Token);
		//					this.tileRequests.Remove(tileRequest.TileId);
		//				}
		//			}), new object[0]);
		//		}
		//	}
		//}
	}
}
