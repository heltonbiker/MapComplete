using MapComplete.MapControl;
using Microsoft.Maps.MapControl.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace Microsoft.Maps.MapControl.WPF.Overlays
{
	public class MapForeground : Control
	{
		//private const int CopyrightTimeout = 2000;

		private Map _Map;

		//private Collection<Logo> _Logos;

		//private Collection<Copyright> _Copyrights;

		private Collection<Scale> _Scales;

		private Collection<Compass> _Compasses;

		private bool _TemplateApplied;

		//private DispatcherTimer _UpdateTimer;

		private static readonly Size MercatorModeLogicalAreaSizeInScreenSpaceAtLevel1;

		//private DispatcherTimer _copyrightUpdateTimer;

		static MapForeground()
		{
			MapForeground.MercatorModeLogicalAreaSizeInScreenSpaceAtLevel1 = new Size(512.0, 512.0);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MapForeground), new FrameworkPropertyMetadata(typeof(MapForeground)));
		}

		public MapForeground(Map map)
		{
		//	if (map == null)
		//	{
		//		throw new ArgumentNullException("map");
		//	}
		//	this._Logos = new Collection<Logo>();
		//	this._Copyrights = new Collection<Copyright>();
			this._Scales = new Collection<Scale>();
			this._Compasses = new Collection<Compass>();
			this._Map = map;
			this.AttachProperty();
		//	this._Map.ModeChanged += new EventHandler<MapEventArgs>(this._Map_ModeChanged);
		//	this._Map.ViewChangeStart += new EventHandler<MapEventArgs>(this._Map_ViewChangeStart);
		//	this._Map.ViewChangeEnd += new EventHandler<MapEventArgs>(this._Map_ViewChangeEnd);
		//	this._UpdateTimer = new DispatcherTimer(DispatcherPriority.Normal, base.Dispatcher)
		//	{
		//		Interval = TimeSpan.FromMilliseconds(500.0)
		//	};
		//	this._UpdateTimer.Tick += new EventHandler(this._UpdateTimer_Tick);
		//	this._copyrightUpdateTimer = new DispatcherTimer(DispatcherPriority.Normal, base.Dispatcher)
		//	{
		//		Interval = TimeSpan.FromMilliseconds(2000.0)
		//	};
		//	this._copyrightUpdateTimer.Tick += new EventHandler(this.CopyrightUpdateTimerTick);
		}

		internal void AttachProperty()
		{
			if (this._Scales != null)
			{
				foreach (Scale current in this._Scales)
				{
					current.SetBinding(UIElement.VisibilityProperty, new Binding
					{
						Mode = BindingMode.TwoWay,
						Source = this._Map,
						Path = new PropertyPath("ScaleVisibility", new object[0])
					});
				}
			}
		}

		public override void OnApplyTemplate()
		{
			//this._Logos = new Collection<Logo>(this.GetVisualOfType<Logo>().ToList<Logo>());
			//this._Copyrights = new Collection<Copyright>(this.GetVisualOfType<Copyright>().ToList<Copyright>());
			this._Scales = new Collection<Scale>(this.GetVisualOfType<Scale>().ToList<Scale>());
			foreach (Scale current in this._Scales)
			{
				current.Culture = this._Map.Culture;
			}
			this.AttachProperty();
			this._Compasses = new Collection<Compass>(this.GetVisualOfType<Compass>().ToList<Compass>());
			foreach (Compass current2 in this._Compasses)
			{
				current2.SetBinding(Compass.HeadingProperty, new Binding
				{
					Source = this._Map,
					Path = new PropertyPath(MapCore.HeadingProperty)
				});
			}
			this._TemplateApplied = true;
			this.RefreshMapMode();
		}

		//private void _Map_ModeChanged(object sender, MapEventArgs e)
		//{
		//	this.RefreshMapMode();
		//}

		private void RefreshMapMode()
		{
			if (this._TemplateApplied && this._Map.Mode != null)
			{
				//this._copyrightUpdateTimer.Stop();
				//this._copyrightUpdateTimer.Start();
				this.UpdateScale();
			}
		}

		//private void _UpdateTimer_Tick(object sender, EventArgs e)
		//{
		//	this.UpdateScale();
		//}

		//private void CopyrightUpdateTimerTick(object sender, EventArgs e)
		//{
		//	this._copyrightUpdateTimer.Stop();
		//	this.InvokeCopyrightRequest();
		//}

		//private void InvokeCopyrightRequest()
		//{
		//	if (this._Map.Mode != null && this._Map.Mode.MapStyle.HasValue)
		//	{
		//		CopyrightManager.GetInstance(this._Map.Culture, null).RequestCopyrightString(this._Map.Mode.MapStyle, this._Map.BoundingRectangle, this._Map.ZoomLevel, this._Map.CredentialsProvider, this._Map.Culture, new Action<CopyrightResult>(this.CopyrightCallback));
		//	}
		//}

		//private void CopyrightCallback(CopyrightResult result)
		//{
		//	if (result != null && result.Culture == this._Map.Culture && result.BoundingRectangle == this._Map.BoundingRectangle && result.ZoomLevel == this._Map.ZoomLevel)
		//	{
		//		using (IEnumerator<Copyright> enumerator = this._Copyrights.GetEnumerator())
		//		{
		//			while (enumerator.MoveNext())
		//			{
		//				Copyright copyright = enumerator.Current;
		//				List<AttributionInfo> list = new List<AttributionInfo>();
		//				foreach (string current in result.CopyrightStrings)
		//				{
		//					AttributionInfo item = new AttributionInfo(current);
		//					if (!copyright.Attributions.Contains(item))
		//					{
		//						list.Add(item);
		//					}
		//				}
		//				List<AttributionInfo> list2 = new List<AttributionInfo>();
		//				foreach (AttributionInfo current2 in copyright.Attributions)
		//				{
		//					if (!result.CopyrightStrings.Contains(current2.Text))
		//					{
		//						list2.Add(current2);
		//					}
		//				}
		//				list2.ForEach(delegate (AttributionInfo attribInfo)
		//				{
		//					copyright.Attributions.Remove(attribInfo);
		//				});
		//				list.ForEach(delegate (AttributionInfo attribInfo)
		//				{
		//					copyright.Attributions.Add(attribInfo);
		//				});
		//			}
		//		}
		//	}
		//}

		//private void _Map_ViewChangeStart(object sender, MapEventArgs e)
		//{
		//	if (this._TemplateApplied)
		//	{
		//		this._UpdateTimer.IsEnabled = true;
		//		this._copyrightUpdateTimer.Stop();
		//	}
		//}

		//private void _Map_ViewChangeEnd(object sender, MapEventArgs e)
		//{
		//	this._UpdateTimer.IsEnabled = false;
		//	this._copyrightUpdateTimer.Stop();
		//	this._copyrightUpdateTimer.Start();
		//	if (this._TemplateApplied)
		//	{
		//		this.UpdateScale();
		//	}
		//}

		private void UpdateScale()
		{
			foreach (Scale current in this._Scales)
			{
				current.MetersPerPixel = MercatorUtility.ZoomToScale(MapForeground.MercatorModeLogicalAreaSizeInScreenSpaceAtLevel1, this._Map.ZoomLevel, this._Map.Center);
			}
		}

		//private static IEnumerable<DependencyObject> GetDescendents(DependencyObject root)
		//{
		//	int childrenCount = VisualTreeHelper.GetChildrenCount(root);
		//	for (int i = 0; i < childrenCount; i++)
		//	{
		//		DependencyObject child = VisualTreeHelper.GetChild(root, i);
		//		yield return child;
		//		foreach (DependencyObject current in MapForeground.GetDescendents(child))
		//		{
		//			yield return current;
		//		}
		//	}
		//	yield break;
		//}
	}
}
