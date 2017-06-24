using MapComplete.MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Timers;
using System.Xml;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	internal class MapConfigurationFromWeb : MapConfigurationProvider
	{
		private Uri configurationUri;

		private bool requestsPending;

		private object configLock = new object();

		private Dictionary<string, Collection<MapConfigurationGetSectionRequest>> requestQueue;

		private Collection<string> requestedSections;

		private Timer requestTimer;

		public override event EventHandler<MapConfigurationLoadedEventArgs> Loaded;

		public MapConfigurationFromWeb(Uri configurationUri)
		{
			this.configurationUri = configurationUri;
			base.Sections = new Dictionary<string, MapConfigurationSection>();
			this.requestQueue = new Dictionary<string, Collection<MapConfigurationGetSectionRequest>>();
			this.requestedSections = new Collection<string>();
		}

		public override void LoadConfiguration()
		{
		}

		public override void Cancel()
		{
			//lock (this.configLock)
			//{
			//	this.requestQueue.Clear();
			//}
		}

		public override void GetConfigurationSection(string version, string sectionName, string culture, MapConfigurationCallback callback, bool reExecuteCallback, object userState)
		{
			this._version = version;
			this._culture = culture;
			bool flag = base.ContainConfigurationSection(version, sectionName, culture);
			string requestKey = MapConfigurationProvider.GetConfigurationKey(version, sectionName, culture);
			if (!flag)
			{
				bool flag3;
				lock (this.configLock)
				{
					flag = (base.ContainConfigurationSection(version, sectionName, culture) || this.requestedSections.Contains(requestKey));
					flag3 = (!flag && !this.requestQueue.ContainsKey(requestKey));
					this.AddRequestToPendingQueue(version, sectionName, culture, callback, userState, requestKey);
					this.requestsPending = true;
				}
				if (flag3)
				{
					try
					{
						XmlReader xmlReader = null;
						using (IsolatedStorageFileStream isolatedStorageFileStream = this.GetIsolatedStorageFileStream(requestKey, FileMode.OpenOrCreate))
						{
							if (isolatedStorageFileStream.Length > 0L)
							{
								xmlReader = XmlReader.Create(new StreamReader(isolatedStorageFileStream));
							}
							else
							{
								Stream manifestResourceStream = typeof(MapConfiguration).Assembly.GetManifestResourceStream("MapComplete.MapControl.DesignConfig.xml");
								if (manifestResourceStream != null)
								{
									xmlReader = XmlReader.Create(manifestResourceStream);
								}
							}
							if (xmlReader != null)
							{
								this.ConfigLoaded(requestKey, base.ParseConfiguration(xmlReader));
							}
						}
					}
					catch (XmlException)
					{
					}
					int num = (Map.LoggingDelay > 0) ? Map.LoggingDelay : 1;
					this.requestTimer = new Timer((double)num);
					this.requestTimer.AutoReset = false;
					this.requestTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
					{
						this.requestTimer.Dispose();
						this.requestTimer = null;
						using (WebClient webClient = new WebClient())
						{
							webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(this.LoadFromServer_OpenReadCompleted);
							webClient.OpenReadAsync(this.configurationUri, requestKey);
						}
					};
					this.requestTimer.Start();
				}
			}
			else
			{
				lock (this.configLock)
				{
					if (this.requestsPending && reExecuteCallback)
					{
						this.AddRequestToPendingQueue(version, sectionName, culture, callback, userState, requestKey);
					}
				}
			}
			if (flag && callback != null)
			{
				callback(base.GetSection(version, sectionName, culture), userState);
			}
		}

		private void AddRequestToPendingQueue(string version, string sectionName, string culture, MapConfigurationCallback callback, object userState, string requestKey)
		{
			if (!this.requestQueue.ContainsKey(requestKey))
			{
				this.requestQueue.Add(requestKey, new Collection<MapConfigurationGetSectionRequest>());
			}
			this.requestQueue[requestKey].Add(new MapConfigurationGetSectionRequest(version, sectionName, culture, callback, userState));
		}

		private void LoadFromServer_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
		//	string text = e.UserState as string;
		//	try
		//	{
		//		if (e.Error != null)
		//		{
		//			throw e.Error;
		//		}
		//		if (e.Result == null)
		//		{
		//			throw new ConfigurationNotLoadedException(ExceptionStrings.MapConfiguration_WebService_InvalidResult);
		//		}
		//		List<byte> list = new List<byte>();
		//		byte[] array = new byte[1000];
		//		int num = e.Result.Read(array, 0, 1000);
		//		while (0 < num)
		//		{
		//			if (num == 1000)
		//			{
		//				list.AddRange(array);
		//			}
		//			else
		//			{
		//				for (int i = 0; i < num; i++)
		//				{
		//					list.Add(array[i]);
		//				}
		//			}
		//			num = e.Result.Read(array, 0, 1000);
		//		}
		//		byte[] buffer = list.ToArray();
		//		using (MemoryStream memoryStream = new MemoryStream(buffer))
		//		{
		//			XmlReader sectionReader = XmlReader.Create(memoryStream);
		//			using (IsolatedStorageFileStream isolatedStorageFileStream = this.GetIsolatedStorageFileStream(text, FileMode.Create))
		//			{
		//				BinaryWriter binaryWriter = new BinaryWriter(isolatedStorageFileStream);
		//				binaryWriter.Write(buffer);
		//			}
		//			this.ConfigLoaded(text, base.ParseConfiguration(sectionReader));
		//		}
		//	}
		//	catch (Exception error)
		//	{
		//		if (this.Loaded != null)
		//		{
		//			this.Loaded(this, new MapConfigurationLoadedEventArgs(error));
		//		}
		//	}
		//	lock (this.configLock)
		//	{
		//		this.requestQueue.Remove(text);
		//	}
		}

		private void ConfigLoaded(string requestKey, Dictionary<string, MapConfigurationSection> sections)
		{
			List<MapConfigurationGetSectionRequest> list = new List<MapConfigurationGetSectionRequest>();
			lock (this.configLock)
			{
				foreach (string current in sections.Keys)
				{
					base.Sections[current] = sections[current];
				}
				if (!this.requestedSections.Contains(requestKey))
				{
					this.requestedSections.Add(requestKey);
				}
				if (this.requestQueue.ContainsKey(requestKey))
				{
					foreach (MapConfigurationGetSectionRequest current2 in this.requestQueue[requestKey])
					{
						list.Add(current2);
					}
				}
			}
			if (this.Loaded != null)
			{
				this.Loaded(this, new MapConfigurationLoadedEventArgs(null));
			}
			foreach (MapConfigurationGetSectionRequest current3 in list)
			{
				if (current3.Callback != null)
				{
					current3.CallbackDispatcher.BeginInvoke(current3.Callback, new object[]
					{
						base.GetSection(current3.Version, current3.SectionName, current3.Culture),
						current3.UserState
					});
				}
			}
		}

		private IsolatedStorageFileStream GetIsolatedStorageFileStream(string requestKey, FileMode mode)
		{
			IsolatedStorageFile userStoreForAssembly = IsolatedStorageFile.GetUserStoreForAssembly();
			string path = string.Format("WPFMapcontrolIS2_{0}", requestKey);
			return new IsolatedStorageFileStream(path, mode, userStoreForAssembly);
		}
	}
}
