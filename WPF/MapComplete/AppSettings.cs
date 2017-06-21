using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapComplete
{
	[Serializable]
	public class AppSettings
	{
		public GridLength LeftPanelWidth { get; set; }
			= new GridLength(1, GridUnitType.Star);

		public GridLength RightPanelWidth { get; set; }
			= new GridLength(3, GridUnitType.Star);
	}
}
