using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Maps.MapControl.WPF
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class ExceptionStrings
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(ExceptionStrings.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.Maps.MapControl.WPF.MapControl_Core.ExceptionStrings", typeof(ExceptionStrings).Assembly);
					ExceptionStrings.resourceMan = resourceManager;
				}
				return ExceptionStrings.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return ExceptionStrings.resourceCulture;
			}
			set
			{
				ExceptionStrings.resourceCulture = value;
			}
		}

		internal static string ConfigurationException_InvalidLoad
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("ConfigurationException_InvalidLoad", ExceptionStrings.resourceCulture);
			}
		}

		internal static string ConfigurationException_NullXml
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("ConfigurationException_NullXml", ExceptionStrings.resourceCulture);
			}
		}

		internal static string IProjectable
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("IProjectable", ExceptionStrings.resourceCulture);
			}
		}

		internal static string LocationToViewportPoint_DefaultException
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("LocationToViewportPoint_DefaultException", ExceptionStrings.resourceCulture);
			}
		}

		internal static string MapConfiguration_GetSection_NonNull
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("MapConfiguration_GetSection_NonNull", ExceptionStrings.resourceCulture);
			}
		}

		internal static string MapConfiguration_ParseConfiguration_DuplicateNodeKey
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("MapConfiguration_ParseConfiguration_DuplicateNodeKey", ExceptionStrings.resourceCulture);
			}
		}

		internal static string MapConfiguration_ParseConfiguration_DuplicateSection
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("MapConfiguration_ParseConfiguration_DuplicateSection", ExceptionStrings.resourceCulture);
			}
		}

		internal static string MapConfiguration_ParseConfiguration_InvalidRoot
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("MapConfiguration_ParseConfiguration_InvalidRoot", ExceptionStrings.resourceCulture);
			}
		}

		internal static string MapConfiguration_ParseConfiguration_InvalidSection_NoVersion
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("MapConfiguration_ParseConfiguration_InvalidSection_NoVersion", ExceptionStrings.resourceCulture);
			}
		}

		internal static string MapConfiguration_ParseConfiguration_InvalidTag
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("MapConfiguration_ParseConfiguration_InvalidTag", ExceptionStrings.resourceCulture);
			}
		}

		internal static string MapConfiguration_WebService_InvalidResult
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("MapConfiguration_WebService_InvalidResult", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TypeConverter_InvalidApplicationIdCredentialsProvider
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TypeConverter_InvalidApplicationIdCredentialsProvider", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TypeConverter_InvalidMapMode
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TypeConverter_InvalidMapMode", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TypeConverter_InvalidRangeFormat
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TypeConverter_InvalidRangeFormat", ExceptionStrings.resourceCulture);
			}
		}

		internal static string ViewportPointToLocation_DefaultException
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("ViewportPointToLocation_DefaultException", ExceptionStrings.resourceCulture);
			}
		}

		internal static string InvalidMode
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("InvalidMode", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TileSource_InvalidSubdomain_stringNull
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TileSource_InvalidSubdomain_stringNull", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TileSource_InvalidSubdomains_DifferentLength
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TileSource_InvalidSubdomains_DifferentLength", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TileSource_InvalidSubdomains_LengthMoreThan0
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TileSource_InvalidSubdomains_LengthMoreThan0", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TypeConverter_InvalidLocationCollection
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TypeConverter_InvalidLocationCollection", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TypeConverter_InvalidLocationFormat
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TypeConverter_InvalidLocationFormat", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TypeConverter_InvalidLocationRectFormat
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TypeConverter_InvalidLocationRectFormat", ExceptionStrings.resourceCulture);
			}
		}

		internal static string TypeConverter_InvalidPositionOriginFormat
		{
			get
			{
				return ExceptionStrings.ResourceManager.GetString("TypeConverter_InvalidPositionOriginFormat", ExceptionStrings.resourceCulture);
			}
		}

		internal ExceptionStrings()
		{
		}
	}
}
