using MapComplete.MapControl;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Xml;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	public delegate void MapConfigurationCallback(MapConfigurationSection config, object userState);

	public static class MapConfiguration
	{
		private static MapConfigurationProvider configuration;

		private static string defaultServiceUriFormat = "{UriScheme}://dev.virtualearth.net/REST/{UriVersion}/GeospatialEndpoint/{UriCulture}/{UriRegion}/{RegionLocation}?o=xml&key={UriKey}";

		public static event EventHandler<MapConfigurationLoadedEventArgs> Loaded;

		private static bool IsInDesignMode
		{
			get
			{
				return (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
			}
		}

		public static void GetSection(string version, string sectionName, string culture, string key, MapConfigurationCallback callback, bool reExecuteCallback)
		{
			MapConfiguration.GetSection(version, sectionName, culture, key, callback, reExecuteCallback, null);
		}

		public static void GetSection(string version, string sectionName, string culture, string key, MapConfigurationCallback callback, bool reExecuteCallback, object userState)
		{
			if (key == null)
			{
				key = "AnzLvLWGD8KU7wavNObJJdFVQcEGKMuBfBZ08b_Fo2cLb2HANvULeuewDmPDYExL";
			}
			if (MapConfiguration.configuration == null || MapConfiguration.configuration.Culture != culture)
			{
				if (!MapConfiguration.IsInDesignMode)
				{
					string text = MapConfiguration.defaultServiceUriFormat;
					text = text.Replace("{UriScheme}", Map.UriScheme);
					text = text.Replace("{UriKey}", key);
					text = text.Replace("{UriVersion}", version);
					text = text.Replace("{UriCulture}", culture);
					text = text.Replace("{UriRegion}", culture.Substring(culture.Length - 2));
					text = text.Replace("{RegionLocation}", "");
					if (Map.UseHttps)
					{
						text += "&uriScheme=https";
					}
					Uri configurationUri = new Uri(text, UriKind.Absolute);
					MapConfiguration.LoadConfiguration(new MapConfigurationFromWeb(configurationUri));
				}
				else
				{
					try
					{
						typeof(MapConfiguration).Assembly.GetManifestResourceNames();
						Stream manifestResourceStream = typeof(MapConfiguration).Assembly.GetManifestResourceStream("Microsoft.Maps.MapControl.WPF.Data.DesignConfig.xml");
						if (manifestResourceStream != null)
						{
							XmlReader config = XmlReader.Create(manifestResourceStream);
							MapConfiguration.LoadConfiguration(new MapConfigurationFromFile(config, culture, version));
						}
					}
					catch (XmlException)
					{
					}
				}
			}
			if (string.IsNullOrEmpty(version))
			{
				throw new ArgumentException(ExceptionStrings.MapConfiguration_GetSection_NonNull, "version");
			}
			if (string.IsNullOrEmpty(sectionName))
			{
				throw new ArgumentException(ExceptionStrings.MapConfiguration_GetSection_NonNull, "sectionName");
			}
			MapConfiguration.configuration.GetConfigurationSection(version, sectionName, culture, callback, reExecuteCallback, userState);
		}

		//internal static void Reset()
		//{
		//	MapConfiguration.Loaded = null;
		//	if (MapConfiguration.configuration != null)
		//	{
		//		MapConfiguration.configuration.Loaded -= new EventHandler<MapConfigurationLoadedEventArgs>(MapConfiguration.provider_Loaded);
		//		MapConfiguration.configuration = null;
		//	}
		//}

		private static void LoadConfiguration(MapConfigurationProvider provider)
		{
			if (MapConfiguration.configuration != null)
			{
				MapConfiguration.configuration.Cancel();
			}
			MapConfiguration.configuration = provider;
			provider.Loaded += new EventHandler<MapConfigurationLoadedEventArgs>(MapConfiguration.provider_Loaded);
			provider.LoadConfiguration();
		}

		private static void provider_Loaded(object sender, MapConfigurationLoadedEventArgs e)
		{
			EventHandler<MapConfigurationLoadedEventArgs> loaded = MapConfiguration.Loaded;
			if (loaded != null)
			{
				loaded(null, e);
			}
		}
	}
}
