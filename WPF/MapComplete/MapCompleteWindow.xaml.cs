using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapComplete
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MapCompleteWindow : Window
	{
		public MapCompleteWindow()
		{
			InitializeComponent();

		//	SourceInitialized += MapCompleteWindow_Initialized;

		//	Closing += MapCompleteWindow_Closing;
		//}


		//private void MapCompleteWindow_Initialized(object sender, EventArgs e)
		//{
		//	LeftPanelColumn.Width = Properties.Settings.Default.LeftPanelWidth;
		//	RightPanelColumn.Width = Properties.Settings.Default.RightPanelWidth;
		//}

		//private void MapCompleteWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		//{
		//	Properties.Settings.Default.LeftPanelWidth = LeftPanelColumn.Width;
		//	Properties.Settings.Default.RightPanelWidth = RightPanelColumn.Width;

		//	Properties.Settings.Default.Save();
		}

	}
}
