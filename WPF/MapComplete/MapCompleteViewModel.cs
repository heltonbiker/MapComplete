using GalaSoft.MvvmLight;
using MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapComplete
{
	public class MapCompleteViewModel : ViewModelBase
	{
		public ObservableCollection<IMapLayer> Layers { get; set; }
			= new ObservableCollection<IMapLayer>();

		MapTileLayer _baseLayer;
		string _googleHybridUri = "https://mts{i}.google.com/vt?hl=pt-BR&lyrs=y&x={x}&y={y}&z={z}";


		// CONSTRUCTOR
		public MapCompleteViewModel()
		{
			_baseLayer = new MapTileLayer()
			{
				TileSource = new ImageTileSource()
				{
					UriFormat = _googleHybridUri
				}
			};
			Layers.Add(_baseLayer);
		}		
	}
}
