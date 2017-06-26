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

		MapTileLayer _googleBack = new MapTileLayer();
		MapTileLayer _googleFront = new MapTileLayer();

		// CONSTRUCTOR
		public MapCompleteViewModel()
		{
			UpdateTileLayer();

			Layers.Add(_googleBack);
			Layers.Add(_googleFront);
		}		

		public string BackUri
		{
			get { return _backUri; }
			set
			{
				_backUri = value;
				RaisePropertyChanged(() => BackUri);
				UpdateTileLayer();
			}
		}
		private string _backUri = "https://mts{i}.google.com/vt?hl=pt-BR&lyrs=y&x={x}&y={y}&z={z}";

		public string FrontUri
		{
			get { return _frontUri; }
			set
			{
				_frontUri = value;
				RaisePropertyChanged(() => FrontUri);
				UpdateTileLayer();
			}
		}
		private string _frontUri = "https://mts{i}.google.com/vt?hl=pt-BR&lyrs=h&x={x}&y={y}&z={z}";



		void UpdateTileLayer()
		{
			_googleBack.TileSource = new ImageTileSource()
			{
				UriFormat = BackUri
			};

			_googleFront.TileSource = new ImageTileSource()
			{
				UriFormat = FrontUri
			};
		}
	}
}
