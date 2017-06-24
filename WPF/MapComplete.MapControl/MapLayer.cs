using Microsoft.Maps.MapExtras;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapControl.WPF
{
	public class MapLayer : Panel, IProjectable
	{
		private Size _ViewportSize;

		private Matrix3D _NormalizedMercatorToViewport;

		private Matrix3D _ViewportToNormalizedMercator;

		public static readonly DependencyProperty PositionProperty = DependencyProperty.RegisterAttached("Position", typeof(Location), typeof(MapLayer), new PropertyMetadata(new PropertyChangedCallback(MapLayer.OnPositionChanged)));

		//public static readonly DependencyProperty PositionRectangleProperty = DependencyProperty.RegisterAttached("PositionRectangle", typeof(LocationRect), typeof(MapLayer), new PropertyMetadata(new PropertyChangedCallback(MapLayer.OnPositionRectangleChanged)));

		public static readonly DependencyProperty PositionOriginProperty = DependencyProperty.RegisterAttached("PositionOrigin", typeof(PositionOrigin), typeof(MapLayer), new PropertyMetadata(new PropertyChangedCallback(MapLayer.OnPositionOriginChanged)));

		//public static readonly DependencyProperty PositionOffsetProperty = DependencyProperty.RegisterAttached("PositionOffset", typeof(Point), typeof(MapLayer), new PropertyMetadata(new PropertyChangedCallback(MapLayer.OnPositionOffsetChanged)));

		//private static readonly DependencyProperty ProjectionUpdatedTag = DependencyProperty.RegisterAttached("ProjectionUpdatedTagProperty", typeof(Guid), typeof(MapLayer), null);

		//public void AddChild(UIElement element, Location location)
		//{
		//	base.Children.Add(element);
		//	MapLayer.SetPosition(element, location);
		//}

		//public void AddChild(UIElement element, Location location, Point offset)
		//{
		//	base.Children.Add(element);
		//	MapLayer.SetPosition(element, location);
		//	MapLayer.SetPositionOffset(element, offset);
		//}

		//public void AddChild(UIElement element, Location location, PositionOrigin origin)
		//{
		//	base.Children.Add(element);
		//	MapLayer.SetPosition(element, location);
		//	MapLayer.SetPositionOrigin(element, origin);
		//}

		//public void AddChild(UIElement element, LocationRect locationRect)
		//{
		//	base.Children.Add(element);
		//	MapLayer.SetPositionRectangle(element, locationRect);
		//}

		//public static Location GetPosition(DependencyObject dependencyObject)
		//{
		//	Location location = (Location)dependencyObject.GetValue(MapLayer.PositionProperty);
		//	if (location == null && dependencyObject is ContentPresenter && VisualTreeHelper.GetChildrenCount(dependencyObject) > 0)
		//	{
		//		DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, 0);
		//		if (child != null)
		//		{
		//			location = MapLayer.GetPosition(child);
		//		}
		//	}
		//	return location;
		//}

		public static void SetPosition(DependencyObject dependencyObject, Location location)
		{
			dependencyObject.SetValue(MapLayer.PositionProperty, location);
		}

		public static void OnPositionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
		{
			MapLayer.InvalidateParentLayout(dependencyObject);
		}

		//public static LocationRect GetPositionRectangle(DependencyObject dependencyObject)
		//{
		//	LocationRect locationRect = (LocationRect)dependencyObject.GetValue(MapLayer.PositionRectangleProperty);
		//	if (locationRect == null && dependencyObject is ContentPresenter && VisualTreeHelper.GetChildrenCount(dependencyObject) > 0)
		//	{
		//		DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, 0);
		//		if (child != null)
		//		{
		//			locationRect = MapLayer.GetPositionRectangle(child);
		//		}
		//	}
		//	return locationRect;
		//}

		//public static void SetPositionRectangle(DependencyObject dependencyObject, LocationRect rect)
		//{
		//	dependencyObject.SetValue(MapLayer.PositionRectangleProperty, rect);
		//}

		//public static void OnPositionRectangleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
		//{
		//	MapLayer.InvalidateParentLayout(dependencyObject);
		//}

		//public static PositionOrigin GetPositionOrigin(DependencyObject dependencyObject)
		//{
		//	PositionOrigin result = (PositionOrigin)dependencyObject.GetValue(MapLayer.PositionOriginProperty);
		//	if (dependencyObject is ContentPresenter && VisualTreeHelper.GetChildrenCount(dependencyObject) > 0)
		//	{
		//		DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, 0);
		//		if (child != null)
		//		{
		//			result = MapLayer.GetPositionOrigin(child);
		//		}
		//	}
		//	return result;
		//}

		public static void SetPositionOrigin(DependencyObject dependencyObject, PositionOrigin origin)
		{
			dependencyObject.SetValue(MapLayer.PositionOriginProperty, origin);
		}

		public static void OnPositionOriginChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
		{
			MapLayer.InvalidateParentLayout(dependencyObject);
		}

		//public static Point GetPositionOffset(DependencyObject dependencyObject)
		//{
		//	Point result = (Point)dependencyObject.GetValue(MapLayer.PositionOffsetProperty);
		//	if (dependencyObject is ContentPresenter && VisualTreeHelper.GetChildrenCount(dependencyObject) > 0)
		//	{
		//		DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, 0);
		//		if (child != null)
		//		{
		//			result = MapLayer.GetPositionOffset(child);
		//		}
		//	}
		//	return result;
		//}

		//public static void SetPositionOffset(DependencyObject dependencyObject, Point point)
		//{
		//	dependencyObject.SetValue(MapLayer.PositionOffsetProperty, point);
		//}

		//public static void OnPositionOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
		//{
		//	MapLayer.InvalidateParentLayout(dependencyObject);
		//}

		void IProjectable.SetView(Size viewportSize, Matrix3D normalizedMercatorToViewport, Matrix3D viewportToNormalizedMercator)
		{
			this._ViewportSize = viewportSize;
			this._NormalizedMercatorToViewport = normalizedMercatorToViewport;
			this._ViewportToNormalizedMercator = viewportToNormalizedMercator;
			base.InvalidateMeasure();
		}

		//protected override Size MeasureOverride(Size availableSize)
		//{
		//	foreach (UIElement uIElement in base.Children)
		//	{
		//		LocationRect positionRectangle = MapLayer.GetPositionRectangle(uIElement);
		//		if (positionRectangle != null)
		//		{
		//			Rect rect = MapMath.LocationToViewportPoint(ref this._NormalizedMercatorToViewport, positionRectangle);
		//			uIElement.Measure(new Size(rect.Width, rect.Height));
		//		}
		//		else
		//		{
		//			if (uIElement is ContentPresenter && VisualTreeHelper.GetChildrenCount(uIElement) > 0)
		//			{
		//				IProjectable projectable = VisualTreeHelper.GetChild(uIElement, 0) as IProjectable;
		//				if (projectable != null)
		//				{
		//					projectable.SetView(this._ViewportSize, this._NormalizedMercatorToViewport, this._ViewportToNormalizedMercator);
		//					UIElement uIElement2 = projectable as UIElement;
		//					if (uIElement2 != null)
		//					{
		//						uIElement2.InvalidateMeasure();
		//					}
		//				}
		//			}
		//			IProjectable projectable2 = uIElement as IProjectable;
		//			if (projectable2 != null)
		//			{
		//				projectable2.SetView(this._ViewportSize, this._NormalizedMercatorToViewport, this._ViewportToNormalizedMercator);
		//			}
		//			uIElement.Measure(this._ViewportSize);
		//		}
		//	}
		//	return this._ViewportSize;
		//}

		//protected override Size ArrangeOverride(Size finalSize)
		//{
		//	foreach (UIElement uIElement in base.Children)
		//	{
		//		Rect finalRect = new Rect(0.0, 0.0, this._ViewportSize.Width, this._ViewportSize.Height);
		//		LocationRect positionRectangle = MapLayer.GetPositionRectangle(uIElement);
		//		if (positionRectangle != null)
		//		{
		//			finalRect = MapMath.LocationToViewportPoint(ref this._NormalizedMercatorToViewport, positionRectangle);
		//		}
		//		else
		//		{
		//			Location position = MapLayer.GetPosition(uIElement);
		//			Point point;
		//			if (position != null && MapMath.TryLocationToViewportPoint(ref this._NormalizedMercatorToViewport, position, out point))
		//			{
		//				PositionOrigin positionOrigin = MapLayer.GetPositionOrigin(uIElement);
		//				point.X -= positionOrigin.X * uIElement.DesiredSize.Width;
		//				point.Y -= positionOrigin.Y * uIElement.DesiredSize.Height;
		//				finalRect = new Rect(point.X, point.Y, uIElement.DesiredSize.Width, uIElement.DesiredSize.Height);
		//			}
		//		}
		//		Point positionOffset = MapLayer.GetPositionOffset(uIElement);
		//		finalRect.X += positionOffset.X;
		//		finalRect.Y += positionOffset.Y;
		//		uIElement.Arrange(finalRect);
		//	}
		//	return this._ViewportSize;
		//}

		//protected override void OnVisualChildrenChanged(DependencyObject childAdded, DependencyObject childRemoved)
		//{
		//	if (childAdded != null)
		//	{
		//		IAttachable attachable = childAdded as IAttachable;
		//		if (attachable != null)
		//		{
		//			attachable.Attach();
		//		}
		//	}
		//	if (childRemoved != null)
		//	{
		//		IAttachable attachable2 = childRemoved as IAttachable;
		//		if (attachable2 != null)
		//		{
		//			attachable2.Detach();
		//		}
		//	}
		//	base.OnVisualChildrenChanged(childAdded, childRemoved);
		//}

		private static void InvalidateParentLayout(DependencyObject dependencyObject)
		{
			FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
			if (frameworkElement != null)
			{
				MapLayer mapLayer = frameworkElement.Parent as MapLayer;
				if (mapLayer == null)
				{
					ContentPresenter contentPresenter = frameworkElement.Parent as ContentPresenter;
					if (contentPresenter != null)
					{
						mapLayer = (contentPresenter.Parent as MapLayer);
					}
				}
				if (mapLayer != null)
				{
					mapLayer.InvalidateMeasure();
				}
			}
		}
	}
}
