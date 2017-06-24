using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	internal abstract class MapConfigurationProvider
	{
		private const string RootNodeName = "Response";

		private const string ResourceSets = "ResourceSets";

		private const string ResourceSet = "ResourceSet";

		private const string Resources = "Resources";

		private const string Resource = "Resource";

		private const string Services = "Services";

		private const string ServiceInfo = "ServiceInfo";

		private const string ServiceName = "ServiceName";

		private const string EndpointName = "Endpoint";

		protected string _version = string.Empty;

		protected string _culture = string.Empty;

		public abstract event EventHandler<MapConfigurationLoadedEventArgs> Loaded;

		protected Dictionary<string, MapConfigurationSection> Sections
		{
			get;
			set;
		}

		public string Culture
		{
			get
			{
				return this._culture;
			}
		}

		public abstract void LoadConfiguration();

		public abstract void Cancel();

		public abstract void GetConfigurationSection(string version, string sectionName, string culture, MapConfigurationCallback callback, bool reExecuteCallback, object userState);

		protected static string GetConfigurationKey(string version, string sectionName, string culture)
		{
			if (string.IsNullOrEmpty(culture))
			{
				culture = string.Empty;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", new object[]
			{
				version,
				sectionName,
				culture
			}).ToUpper(CultureInfo.InvariantCulture);
		}

		protected MapConfigurationSection GetSection(string version, string sectionName, string culture)
		{
			MapConfigurationSection result = null;
			if (this.Sections != null)
			{
				string configurationKey = MapConfigurationProvider.GetConfigurationKey(version, sectionName, culture);
				if (this.Sections.ContainsKey(configurationKey))
				{
					result = this.Sections[configurationKey];
				}
				else if (!string.IsNullOrEmpty(culture))
				{
					configurationKey = MapConfigurationProvider.GetConfigurationKey(version, sectionName, string.Empty);
					if (this.Sections.ContainsKey(configurationKey))
					{
						result = this.Sections[configurationKey];
					}
				}
			}
			return result;
		}

		protected bool ContainConfigurationSection(string version, string sectionName, string culture)
		{
			return this.Sections != null && this.Sections.ContainsKey(MapConfigurationProvider.GetConfigurationKey(version, sectionName, culture));
		}

		protected Dictionary<string, MapConfigurationSection> ParseConfiguration(XmlReader sectionReader)
		{
			if (sectionReader == null)
			{
				//throw new ConfigurationNotLoadedException(ExceptionStrings.ConfigurationException_NullXml);
			}
			Dictionary<string, MapConfigurationSection> dictionary = new Dictionary<string, MapConfigurationSection>();
			if (sectionReader.Read() && sectionReader.IsStartElement() && sectionReader.LocalName == "Response")
			{
				while (sectionReader.Read())
				{
					if (!sectionReader.IsStartElement())
					{
						break;
					}
					if (sectionReader.LocalName != "ResourceSets")
					{
						string localName = sectionReader.LocalName;
						string configurationKey = MapConfigurationProvider.GetConfigurationKey(this._version, localName, this._culture);
						sectionReader.Read();
						string value = sectionReader.Value;
						dictionary[configurationKey] = new MapConfigurationSection(new Dictionary<string, string>
						{
							{
								localName,
								value
							}
						});
						sectionReader.Read();
					}
					else if (sectionReader.Read() && sectionReader.IsStartElement() && sectionReader.LocalName == "ResourceSet")
					{
						while (sectionReader.Read())
						{
							if (sectionReader.IsStartElement() && sectionReader.LocalName == "Resources" && sectionReader.Read() && sectionReader.IsStartElement() && sectionReader.LocalName == "Resource")
							{
								while (sectionReader.Read())
								{
									if (sectionReader.IsStartElement())
									{
										string localName2 = sectionReader.LocalName;
										if (string.IsNullOrEmpty(localName2))
										{
											throw new XmlException(ExceptionStrings.MapConfiguration_ParseConfiguration_InvalidSection_NoVersion);
										}
										string configurationKey2 = MapConfigurationProvider.GetConfigurationKey(this._version, localName2, this._culture);
										if (dictionary.ContainsKey(configurationKey2))
										{
											throw new XmlException(string.Format(CultureInfo.InvariantCulture, ExceptionStrings.MapConfiguration_ParseConfiguration_DuplicateSection, new object[]
											{
												localName2,
												this._version,
												this._culture
											}));
										}
										dictionary[configurationKey2] = this.ParseConfigurationSection(sectionReader.ReadSubtree());
									}
								}
							}
						}
					}
				}
				return dictionary;
			}
			throw new XmlException(string.Format(CultureInfo.InvariantCulture, ExceptionStrings.MapConfiguration_ParseConfiguration_InvalidRoot, new object[]
			{
				sectionReader.LocalName
			}));
		}

		private MapConfigurationSection ParseConfigurationSection(XmlReader sectionReader)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (sectionReader.Read() && !sectionReader.IsEmptyElement)
			{
				string localName = sectionReader.LocalName;
				while (sectionReader.Read())
				{
					if (sectionReader.IsStartElement())
					{
						if (!(sectionReader.LocalName == "ServiceInfo"))
						{
							throw new XmlException(string.Format(CultureInfo.InvariantCulture, ExceptionStrings.MapConfiguration_ParseConfiguration_InvalidTag, new object[]
							{
								sectionReader.LocalName
							}));
						}
						string text = string.Empty;
						string value = string.Empty;
						while (sectionReader.Read())
						{
							if (sectionReader.IsStartElement() && sectionReader.LocalName == "ServiceName")
							{
								sectionReader.Read();
								text = sectionReader.Value.ToUpper(CultureInfo.InvariantCulture);
							}
							if (sectionReader.IsStartElement() && sectionReader.LocalName == "Endpoint")
							{
								sectionReader.Read();
								value = "{UriScheme}://" + sectionReader.Value;
							}
							if (sectionReader.LocalName == "ServiceInfo")
							{
								break;
							}
						}
						if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(value))
						{
							if (dictionary.ContainsKey(text))
							{
								throw new XmlException(string.Format(CultureInfo.InvariantCulture, ExceptionStrings.MapConfiguration_ParseConfiguration_DuplicateNodeKey, new object[]
								{
									text
								}));
							}
							dictionary.Add(text, value);
						}
					}
					else if (sectionReader.Value != string.Empty)
					{
						dictionary.Add(localName, sectionReader.Value);
					}
				}
			}
			return new MapConfigurationSection(dictionary);
		}
	}
}
