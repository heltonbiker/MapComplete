using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MapComplete
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var window = new MapCompleteWindow()
			{
				DataContext = new MapCompleteViewModel()
			};

			MainWindow = window;

			MainWindow.Show();
		}
	}
}
