using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Maps.MapControl.WPF
{
	internal static class VisualEnumerable
	{
		internal static IEnumerable<T> GetVisualOfType<T>(this DependencyObject element)
		{
			return (
				from t in element.GetVisualTree()
				where t.GetType() == typeof(T)
				select t).Cast<T>();
		}

		internal static IEnumerable<DependencyObject> GetVisualTree(this DependencyObject element)
		{
			int num = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < num; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(element, i);
				yield return child;
				foreach (DependencyObject visualTree in child.GetVisualTree())
				{
					yield return visualTree;
				}
			}
		}
	}
}