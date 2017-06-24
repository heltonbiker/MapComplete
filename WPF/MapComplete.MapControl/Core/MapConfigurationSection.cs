using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Maps.MapControl.WPF.Core
{
	public class MapConfigurationSection
	{
		private Dictionary<string, string> values;

		public string this[string key]
		{
			get
			{
				key = key.ToUpper(CultureInfo.InvariantCulture);
				if (this.values.ContainsKey(key))
				{
					return this.values[key];
				}
				return string.Empty;
			}
		}

		internal MapConfigurationSection(Dictionary<string, string> values)
		{
			this.values = values;
		}

		public bool Contains(string key)
		{
			key = key.ToUpper(CultureInfo.InvariantCulture);
			return this.values.ContainsKey(key);
		}
	}
}
