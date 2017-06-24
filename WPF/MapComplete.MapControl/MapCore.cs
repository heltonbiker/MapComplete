//using Microsoft.Maps.MapControl.WPF.Core;
//using Microsoft.Maps.MapControl.WPF.Design;
//using Microsoft.Maps.MapExtras;
using Microsoft.Maps.MapControl.WPF.Core;
using Microsoft.Maps.MapControl.WPF.Design;
using Microsoft.Maps.MapExtras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Microsoft.Maps.MapControl.WPF
{
    /// <summary>Represents the map.</summary>
    [ContentProperty("Children")]
    public class MapCore : Control //, IDisposable
    {
		private Grid _MapContainer;

		private Grid _MapForegroundContainer;

		private Grid _MapModeContainer;

		private MapLayer _MapUserLayerContainer;

		private List<MapMode> _MapModes;

		private MapMode _CurrentMapMode;

		private Timer _CurrentMapModeTransitionTimeout;

		private MapMode _PendingMapMode;

		//private AnimationDriver _ZoomAndPanAnimationDriver;

		//private ZoomAndPanAnimator _ZoomAndPanAnimator;

		private Rect _ZoomAndPan_FromRect;

		private double _ZoomAndPan_FromZoomLevel;

		private bool _ViewUpdatingInternally;

		private CriticallyDampedSpring _HeadingSpring;

		private Point? _ZoomAndRotateOrigin;

		private CriticallyDampedSpring _ZoomLevelSpring;

		private CriticallyDampedSpring _CenterNormalizedMercatorSpringX;

		private CriticallyDampedSpring _CenterNormalizedMercatorSpringY;

		private bool _ViewIsAnimating;

		private bool _Disposed;

		private AnimationDriver _ModeSwitchAnationDriver;

		private static Matrix3D _NormalizedMercatorToViewport_TranslatePre;

		private static Matrix3D _NormalizedMercatorToViewport_Scale;

		private static Matrix3D _NormalizedMercatorToViewport_Rotate;

		private static Matrix3D _NormalizedMercatorToViewport_TranslatePost;

		private DispatcherTimer _UserInputTimeout;

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.MapCore.ZoomLevel"></see> dependency property.</summary>
		public static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register("ZoomLevel", typeof(double), typeof(MapCore), new PropertyMetadata(1.0, new PropertyChangedCallback(MapCore.OnZoomLevelChanged)));

		//    /// <summary>Identifies the <see cref="T:Microsoft.Maps.MapControl.WPF.Overlays.Scale"></see>dependency</summary>
		//    public static readonly DependencyProperty ScaleVisibilityProperty = DependencyProperty.Register("ScaleVisibility", typeof(Visibility), typeof(MapCore), new PropertyMetadata(new PropertyChangedCallback(MapCore.OnScaleVisibilityChanged)));

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.MapCore.Heading"></see> dependency property.</summary>
		public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register("Heading", typeof(double), typeof(MapCore), new PropertyMetadata(0.0, new PropertyChangedCallback(MapCore.OnHeadingChanged)));

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.MapCore.Center"></see> dependency property.</summary>
		public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Location), typeof(MapCore), new PropertyMetadata(new Location(), new PropertyChangedCallback(MapCore.OnCenterChanged)));

		//    /// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.MapCore.CredentialsProvider"></see> dependency property.</summary>
		//    public static readonly DependencyProperty CredentialsProviderProperty = DependencyProperty.Register("CredentialsProvider", typeof(CredentialsProvider), typeof(MapCore), new PropertyMetadata(new PropertyChangedCallback(MapCore.OnCredentialsProviderChangedCallback)));

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.MapCore.Culture"></see> dependency property.</summary>
		public static readonly DependencyProperty CultureProperty = DependencyProperty.Register("Culture", typeof(string), typeof(MapCore), new PropertyMetadata(delegate (DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			(sender as MapCore).UpdateCulture();
		}));

		/// <summary>Identifies the <see cref="P:Microsoft.Maps.MapControl.WPF.MapCore.Mode"></see> dependency property.</summary>
		public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(MapMode), typeof(MapCore), new PropertyMetadata(null, delegate (DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			(sender as MapCore).UpdateMapMode();
		}));

		//    /// <summary>Occurs for every frame of a view change.</summary>
		//    public event EventHandler<MapEventArgs> ViewChangeOnFrame;

		//    /// <summary>Occurs when the view towards which the map is animating changes.</summary>
		//    public event EventHandler<MapEventArgs> TargetViewChanged;

		/// <summary>Occurs when the view starts changing.</summary>
		public event EventHandler<MapEventArgs> ViewChangeStart;

		/// <summary>Occurs when the view is done changing.</summary>
		public event EventHandler<MapEventArgs> ViewChangeEnd;

		public event EventHandler<MapEventArgs> ModeChanged;

		/// <summary>Gets or sets the zoom level.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double ZoomLevel
		{
			get
			{
				return (double)base.GetValue(MapCore.ZoomLevelProperty);
			}
			set
			{
				base.SetValue(MapCore.ZoomLevelProperty, value);
			}
		}

		//    /// <summary>Gets or sets the visibility of all <see cref="T:Microsoft.Maps.MapControl.WPF.Overlays.Scale"></see> objects on the map. </summary>
		//    /// <returns>Returns <see cref="T:System.Windows.Visibility"></see>.</returns>
		//    public Visibility ScaleVisibility
		//    {
		//        get
		//        {
		//            return (Visibility)base.GetValue(MapCore.ScaleVisibilityProperty);
		//        }
		//        set
		//        {
		//            base.SetValue(MapCore.ScaleVisibilityProperty, value);
		//        }
		//    }

		/// <summary>Gets or sets the directional heading of the map.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double Heading
		{
			get
			{
				return (double)base.GetValue(MapCore.HeadingProperty);
			}
			set
			{
				base.SetValue(MapCore.HeadingProperty, value);
			}
		}

		/// <summary>Gets the center location of the map view.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		public Location Center
		{
			get
			{
				return (Location)base.GetValue(MapCore.CenterProperty);
			}
			set
			{
				base.SetValue(MapCore.CenterProperty, value);
			}
		}

		//    /// <summary>Gets or sets the credentials (maps_ticket).</summary>
		//    /// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Core.CredentialsProvider"></see>.</returns>
		//    [TypeConverter(typeof(ApplicationIdCredentialsProviderConverter))]
		//    public CredentialsProvider CredentialsProvider
		//    {
		//        get
		//        {
		//            return (CredentialsProvider)base.GetValue(MapCore.CredentialsProviderProperty);
		//        }
		//        set
		//        {
		//            base.SetValue(MapCore.CredentialsProviderProperty, value);
		//        }
		//    }

		/// <summary>Gets or sets the culture used by the map.</summary>
		/// <returns>Returns <see cref="T:System.String"></see>.</returns>
		public string Culture
		{
			get
			{
				string text = (string)base.GetValue(MapCore.CultureProperty);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
				return CultureInfo.CurrentUICulture.Name;
			}
			set
			{
				bool flag = true;
				try
				{
					new CultureInfo(value);
				}
				catch (ArgumentException)
				{
					flag = Regex.IsMatch(value, "^[A-Za-z]{2}-[A-Za-z]{2}$");
				}
				if (flag)
				{
					base.SetValue(MapCore.CultureProperty, value);
				}
			}
		}

		/// <summary>Gets the child elements of the map.</summary>
		/// <returns>Returns <see cref="T:System.Windows.Controls.UIElementCollection"></see>.</returns>
		public UIElementCollection Children
		{
			get
			{
				return this._MapUserLayerContainer.Children;
			}
		}

		/// <summary>Gets or sets the map mode.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.MapMode"></see>.</returns>
		[TypeConverter(typeof(MapModeConverter))]
		public MapMode Mode
		{
			get
			{
				return (MapMode)base.GetValue(MapCore.ModeProperty);
			}
			set
			{
				base.SetValue(MapCore.ModeProperty, value);
			}
		}

		/// <summary>Gets or sets the duration in milliseconds that it takes to cross-fade to a new map mode when a new map mode is set. The default value is 500 milliseconds.</summary>
		/// <returns>Returns <see cref="T:System.Windows.Duration"></see>.</returns>
		public Duration ModeCrossFadeDuration
		{
			get;
			set;
		}

		/// <summary>Gets or sets the animation level of the map.</summary>
		/// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.AnimationLevel"></see>.</returns>
		public AnimationLevel AnimationLevel
		{
			get;
			set;
		}

		//    /// <summary>Gets the pitch of the map view towards which the map is animating.</summary>
		//    /// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		//    public Location TargetCenter
		//    {
		//        get
		//        {
		//            Point point = this.ApplyWorldWrap(new Point(this._CenterNormalizedMercatorSpringX.TargetValue, this._CenterNormalizedMercatorSpringY.TargetValue));
		//            return MapMath.NormalizeLocation(MercatorCube.Instance.ToLocation(new Microsoft.Maps.MapExtras.Point3D(point.X, point.Y, 0.0)));
		//        }
		//    }

		//    private Point TargetNormalizedMercatorCenter
		//    {
		//        get
		//        {
		//            return new Point(this._CenterNormalizedMercatorSpringX.TargetValue, this._CenterNormalizedMercatorSpringY.TargetValue);
		//        }
		//    }

		//    /// <summary>Gets the zoom level of the map view towards which the map is animating.</summary>
		//    /// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		//    public double TargetZoomLevel
		//    {
		//        get
		//        {
		//            return this._ZoomLevelSpring.TargetValue;
		//        }
		//    }

		/// <summary>Gets the heading of the map view towards which the map is animating.</summary>
		/// <returns>Returns <see cref="T:System.Double"></see>.</returns>
		public double TargetHeading
		{
			get
			{
				return this._HeadingSpring.TargetValue;
			}
		}

		//    /// <summary>Gets the size of the viewport.</summary>
		//    /// <returns>Returns <see cref="T:System.Windows.Size"></see>.</returns>
		//    public Size ViewportSize
		//    {
		//        get
		//        {
		//            return new Size(base.ActualWidth, base.ActualHeight);
		//        }
		//    }

		//    /// <summary>Gets the rectangle that defines the boundaries of the map view.</summary>
		//    /// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.LocationRect"></see>.</returns>
		//    public LocationRect BoundingRectangle
		//    {
		//        get
		//        {
		//            Size arg_06_0 = this.ViewportSize;
		//            return new LocationRect(new Location[]
		//            {
		//                this.ViewportPointToLocation(new Point(0.0, 0.0)),
		//                this.ViewportPointToLocation(new Point(0.0, this.ViewportSize.Height)),
		//                this.ViewportPointToLocation(new Point(this.ViewportSize.Width, this.ViewportSize.Height)),
		//                this.ViewportPointToLocation(new Point(this.ViewportSize.Width, 0.0))
		//            });
		//        }
		//    }

		//    internal bool ViewBeingSetByUserInput
		//    {
		//        get;
		//        set;
		//    }

		internal Point? ZoomAndRotateOrigin
		{
			get
			{
				return this._ZoomAndRotateOrigin;
			}
			set
			{
				this._ZoomAndRotateOrigin = value;
			}
		}

		internal Grid MapForegroundContainer
		{
			get
			{
				return this._MapForegroundContainer;
			}
		}

		//    private bool ViewIsAnimating
		//    {
		//        get
		//        {
		//            return this._ViewIsAnimating;
		//        }
		//        set
		//        {
		//            if (this._ViewIsAnimating != value)
		//            {
		//                if (!this._ViewIsAnimating && value)
		//                {
		//                    CompositionTarget.Rendering += new EventHandler(this.CompositionTarget_Rendering);
		//                }
		//                else
		//                {
		//                    CompositionTarget.Rendering -= new EventHandler(this.CompositionTarget_Rendering);
		//                }
		//                this._ViewIsAnimating = value;
		//            }
		//        }
		//    }

		    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WPF.Core.MapCore"></see> class.</summary>
		    public MapCore()
		    {
		        this._MapForegroundContainer = new Grid();
				this._MapModeContainer = new Grid();
				this._MapUserLayerContainer = new MapLayer();
				this._MapModes = new List<MapMode>();

			//        this.AnimationLevel = AnimationLevel.UserInput;
			//        this._ZoomAndPanAnimator = new ZoomAndPanAnimator();
			//        this._ZoomAndPanAnimationDriver = new AnimationDriver();
			//        this._ZoomAndPanAnimationDriver.AnimationProgressChanged += new EventHandler(this._ZoomAndPanAnimationDriver_AnimationProgressChanged);
			//        this._ZoomAndPanAnimationDriver.AnimationStopped += new EventHandler(this._ZoomAndPanAnimationDriver_AnimationStopped);
			//        this._ZoomAndPanAnimationDriver.AnimationCompleted += new EventHandler(this._ZoomAndPanAnimationDriver_AnimationCompleted);
			//        this._ZoomAndRotateOrigin = new Point?(default(Point));

			this._HeadingSpring = new CriticallyDampedSpring();
			this._HeadingSpring.SnapToValue(this.Heading);

			this._ZoomLevelSpring = new CriticallyDampedSpring();
			this._ZoomLevelSpring.SnapToValue(this.ZoomLevel);

			Point point = this.ConvertLocationToNormalizedMercator(this.Center);

		    this._CenterNormalizedMercatorSpringX = new CriticallyDampedSpring();
		    this._CenterNormalizedMercatorSpringX.SnapToValue(point.X);
		    this._CenterNormalizedMercatorSpringY = new CriticallyDampedSpring();
		    this._CenterNormalizedMercatorSpringY.SnapToValue(point.Y);

			//        MapCore._NormalizedMercatorToViewport_TranslatePre = Matrix3D.Identity;
			//        MapCore._NormalizedMercatorToViewport_Scale = Matrix3D.Identity;
			//        MapCore._NormalizedMercatorToViewport_Rotate = Matrix3D.Identity;
			//        MapCore._NormalizedMercatorToViewport_TranslatePost = Matrix3D.Identity;

			//        this.ModeCrossFadeDuration = new Duration(TimeSpan.FromMilliseconds(500.0));
			//        this._CurrentMapModeTransitionTimeout = new Timer(4000.0);
			//        this._CurrentMapModeTransitionTimeout.AutoReset = false;
			//        this._CurrentMapModeTransitionTimeout.Elapsed += new ElapsedEventHandler(this._CurrentMapModeTransitionTimeout_Elapsed);

			//        this._ModeSwitchAnationDriver = new AnimationDriver();
			//        this._ModeSwitchAnationDriver.AnimationProgressChanged += new EventHandler(this._ModeSwitchAnationDriver_AnimationProgressChanged);
			//        this._ModeSwitchAnationDriver.AnimationCompleted += new EventHandler(this._ModeSwitchAnationDriver_AnimationCompleted);

			base.Loaded += new RoutedEventHandler(this.MapCore_Loaded);

			//        this._UserInputTimeout = new DispatcherTimer();
			//        this._UserInputTimeout.Interval = new TimeSpan(0, 0, 0, 0, 100);

			this.UpdateView();
		}

		/// <summary>When overridden in a derived class, is invoked whenever application code or internal processes call ApplyTemplatehttp://msdn.microsoft.com/en-us/library/system.windows.frameworkelement.applytemplate.aspx</summary>
		public override void OnApplyTemplate()
		{
			if (this._MapContainer != null)
			{
				this._MapContainer.SizeChanged -= new SizeChangedEventHandler(this._MapContainer_SizeChanged);
				this._MapContainer.Children.Clear();
			}
			this._MapContainer = (Grid)base.GetTemplateChild("MapContainer");
			this._MapContainer.Clip = new RectangleGeometry();
			this._MapContainer.SizeChanged += new SizeChangedEventHandler(this._MapContainer_SizeChanged);
			this._MapContainer.Children.Add(this._MapModeContainer);
			this._MapContainer.Children.Add(this._MapUserLayerContainer);
			this._MapContainer.Children.Add(this._MapForegroundContainer);
		}

		//    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.  Additionally, halts any pending downloads of map imagery and allows existing map imagery resources to be freed.  This method requires that the control is no longer in the visual tree and therefore has no parent. Once disposed, the control may not be added to the visual tree again.</summary>
		//    public void Dispose()
		//    {
		//        if (base.Parent != null)
		//        {
		//            throw new InvalidOperationException("Cannot be disposed while still in the visual tree.");
		//        }
		//        this.Dispose(true);
		//        GC.SuppressFinalize(this);
		//    }

		//    /// <summary>Releases unmanaged and managed resources</summary>
		//    /// <param name="disposing">Set to true to release both managed and unmanaged resources. Set to false to release only unmanaged resources.</param>
		//    protected virtual void Dispose(bool disposing)
		//    {
		//        if (!this._Disposed)
		//        {
		//            if (disposing)
		//            {
		//                this._CurrentMapModeTransitionTimeout.Dispose();
		//                foreach (MapMode current in this._MapModes)
		//                {
		//                    current.Detach();
		//                }
		//                this._MapModes.Clear();
		//                if (this._PendingMapMode != null)
		//                {
		//                    this._PendingMapMode.Detach();
		//                    this._PendingMapMode = null;
		//                }
		//                this.ViewIsAnimating = false;
		//                this._ZoomAndPanAnimationDriver.Stop();
		//                this._ModeSwitchAnationDriver.Stop();
		//                this._MapContainer.SizeChanged -= new SizeChangedEventHandler(this._MapContainer_SizeChanged);
		//            }
		//            this._Disposed = true;
		//        }
		//    }

		private static void OnZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MapCore)d).OnZoomLevelChanged(e);
		}

		private void OnZoomLevelChanged(DependencyPropertyChangedEventArgs e)
		{
			if (!this._ViewUpdatingInternally)
			{
				AnimationLevel animationLevel = this.AnimationLevel;
				this.AnimationLevel = AnimationLevel.None;
				this.SetView((double)e.NewValue, this._HeadingSpring.TargetValue);
				this.AnimationLevel = animationLevel;
			}
		}

		//    private static void OnScaleVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//    {
		//        ((MapCore)d).OnScaleVisibilityChanged(e);
		//    }

		//    /// <summary>Occurs when the ScaleVisibility property is changed.</summary>
		//    /// <param name="eventArgs">The event data to use.</param>
		//    protected virtual void OnScaleVisibilityChanged(DependencyPropertyChangedEventArgs eventArgs)
		//    {
		//    }

		private static void OnHeadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MapCore)d).OnHeadingChanged(e);
		}

		private void OnHeadingChanged(DependencyPropertyChangedEventArgs e)
		{
			if (!this._ViewUpdatingInternally)
			{
				AnimationLevel animationLevel = this.AnimationLevel;
				this.AnimationLevel = AnimationLevel.None;
				this.SetView(this._ZoomLevelSpring.CurrentValue, (double)e.NewValue);
				this.AnimationLevel = animationLevel;
			}
		}

		private static void OnCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MapCore)d).OnCenterChanged(e);
		}

		private void OnCenterChanged(DependencyPropertyChangedEventArgs e)
		{
			//if (!this._ViewUpdatingInternally)
			//{
			//	AnimationLevel animationLevel = this.AnimationLevel;
			//	this.AnimationLevel = AnimationLevel.None;
			//	this.SetView((Location)e.NewValue, this._ZoomLevelSpring.TargetValue);
			//	this.AnimationLevel = animationLevel;
			//}
		}

		//    private static void OnCredentialsProviderChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
		//    {
		//        ((MapCore)d).OnCredentialsProviderChanged(eventArgs);
		//    }

		//    /// <summary>Occurs when the user presses and holds down a key or keys on the keyboard.</summary>
		//    /// <param name="eventArgs">The event data to use.</param>
		//    protected virtual void OnCredentialsProviderChanged(DependencyPropertyChangedEventArgs eventArgs)
		//    {
		//    }

		private void UpdateCulture()
		{
			MapConfiguration.GetSection("v1", "Services", this.Culture, null, new MapConfigurationCallback(this.AsynchronousConfigurationLoaded), true);
		}

		private void AsynchronousConfigurationLoaded(MapConfigurationSection config, object userState)
		{
			if (this.Mode != null)
			{
				this.Mode.Culture = this.Culture;
				this.Mode.SessionId = this.Mode.SessionId;
				this.UpdateView();
			}
		}

		private void UpdateMapMode()
		{
			if (this.Mode == null)
			{
				throw new InvalidOperationException("map mode must be non-null");
			}
			//if (this._ModeSwitchAnationDriver.IsAnimating)
			//{
			//	this._PendingMapMode = this.Mode;
			//}
			else
			{
				foreach (MapMode current in this._MapModes)
				{
					current.ChooseLevelOfDetailSettings = TilePyramidRenderable.ChooseLevelOfDetailSettingsDownloadNothing;
				}
				if (this._MapModes.Count > 1)
				{
					this._MapModes[1].Detach();
					this._MapModeContainer.Children.Remove(this._MapModes[1]);
					this._MapModes.RemoveAt(1);
				}
				this._CurrentMapMode = this.Mode;
				this._CurrentMapMode.TileWrap = TileWrap.Horizontal;
				this._CurrentMapMode.Culture = this.Culture;

				//ApplicationIdCredentialsProvider applicationIdCredentialsProvider = this.CredentialsProvider as ApplicationIdCredentialsProvider;
				//if (applicationIdCredentialsProvider != null)
				//{
				//	this._CurrentMapMode.SessionId = applicationIdCredentialsProvider.SessionId;
				//}

				this._MapModes.Add(this._CurrentMapMode);
				this._MapModeContainer.Children.Insert(0, this._CurrentMapMode);
				if (this._MapModes.Count > 1)
				{
					this._CurrentMapMode.Opacity = 0.0;
					this._CurrentMapMode.Rendered += new EventHandler(this._CurrentMapMode_Rendered);
					this._CurrentMapModeTransitionTimeout.Enabled = true;
					this._CurrentMapModeTransitionTimeout.Start();
				}
				this._PendingMapMode = null;
				if (this.ModeChanged != null)
				{
					this.ModeChanged(this, new MapEventArgs());
				}
			}
			this.UpdateView();
		}

		/// <summary>Sets the map view to the specified center location and zoom level.</summary>
		/// <param name="zoomLevel">The zoom level of the map view.</param>
		/// <param name="center">The location of the center of the map view.</param>
		public void SetView(Location center, double zoomLevel)
		{
			this.ZoomAndRotateOrigin = null;
			if (this.ZoomAndRotateOrigin.HasValue)
			{
				throw new InvalidOperationException("Cannot set map center if ZoomAndRotateOrigin is set.");
			}
			this.SetView(center, zoomLevel, this.TargetHeading);
		}

		/// <summary>Sets the map view to the specified center location, zoom level, and directional heading.</summary>
		/// <param name="zoomLevel">The zoom level of the map view.</param>
		/// <param name="center">The location of the center of the map view.</param>
		/// <param name="heading">The directional heading of the map view.</param>
		public void SetView(Location center, double zoomLevel, double heading)
		{
			this.ZoomAndRotateOrigin = null;
			if (this.ZoomAndRotateOrigin.HasValue)
			{
				throw new InvalidOperationException("Cannot set map center if ZoomAndRotateOrigin is set.");
			}
			Point centerNormalizedMercator = this.ConvertLocationToNormalizedMercator(center);
			this.SetViewInternal(centerNormalizedMercator, zoomLevel, heading);
		}

		/// <summary>Sets the map view to the specified zoom level and heading.</summary>
		/// <param name="zoomLevel">The zoom level of the map view.</param>
		/// <param name="heading">The heading of the map view.</param>
		public void SetView(double zoomLevel, double heading)
		{
			//this.SetViewInternal(this.TargetNormalizedMercatorCenter, zoomLevel, heading);
		}

		///// <summary>Sets the map view using the specified location rectangle.</summary>
		///// <param name="boundingRectangle">The rectangle that defines the boundaries of the map view.</param>
		//public void SetView(LocationRect boundingRectangle)
		//{
		//	Size viewportSize = new Size(base.ActualWidth, base.ActualHeight);
		//	if (viewportSize.Width <= 0.0 || double.IsInfinity(viewportSize.Width) || double.IsNaN(viewportSize.Width) || viewportSize.Height <= 0.0 || double.IsInfinity(viewportSize.Height) || double.IsNaN(viewportSize.Height))
		//	{
		//		throw new InvalidOperationException("The actual size of the control must be positive and finite in order to set the view using a bounding rectangle.");
		//	}
		//	this.ZoomAndRotateOrigin = null;
		//	Point centerNormalizedMercator = this.ConvertLocationToNormalizedMercator(MapMath.GetMercatorCenter(boundingRectangle));
		//	this.SetViewInternal(centerNormalizedMercator, MapMath.LocationRectToMercatorZoomLevel(viewportSize, boundingRectangle), 0.0);
		//}

		///// <summary>Sets the map view to the specified center location, margin, and heading.</summary>
		///// <param name="margin">The margin in viewport pixels. This value must be non-negative.</param>
		///// <param name="locations">The location of the center of the map view.</param>
		///// <param name="heading">The heading of the map view.</param>
		//public void SetView(IEnumerable<Location> locations, Thickness margin, double heading)
		//{
		//	if (margin.Left < 0.0 || margin.Right < 0.0 || margin.Top < 0.0 || margin.Bottom < 0.0)
		//	{
		//		throw new ArgumentOutOfRangeException("margin");
		//	}
		//	Size viewportSize = new Size(base.ActualWidth, base.ActualHeight);
		//	if (viewportSize.Width <= 0.0 || double.IsInfinity(viewportSize.Width) || double.IsNaN(viewportSize.Width) || viewportSize.Height <= 0.0 || double.IsInfinity(viewportSize.Height) || double.IsNaN(viewportSize.Height))
		//	{
		//		throw new InvalidOperationException("The actual size of the control must be positive and finite in order to set the view using a bounding rectangle.");
		//	}
		//	this.ZoomAndRotateOrigin = null;
		//	Point centerNormalizedMercator;
		//	double zoomLevel;
		//	MapMath.CalculateViewFromLocations(locations, viewportSize, this.TargetHeading, margin, out centerNormalizedMercator, out zoomLevel);
		//	this.SetViewInternal(centerNormalizedMercator, zoomLevel, heading);
		//}

		///// <summary>Sets the map view to the specified center location, margin, heading, and zoom level.</summary>
		///// <param name="maxZoomLevel">The maximum zoom level of the map view.</param>
		///// <param name="margin">The margin in viewport pixels. This value must be non-negative.</param>
		///// <param name="locations">The location of the center of the map view.</param>
		///// <param name="heading">The heading of the map view.</param>
		//public void SetView(IEnumerable<Location> locations, Thickness margin, double heading, double maxZoomLevel)
		//{
		//	if (margin.Left < 0.0 || margin.Right < 0.0 || margin.Top < 0.0 || margin.Bottom < 0.0)
		//	{
		//		throw new ArgumentOutOfRangeException("margin");
		//	}
		//	Size viewportSize = new Size(base.ActualWidth, base.ActualHeight);
		//	if (viewportSize.Width <= 0.0 || double.IsInfinity(viewportSize.Width) || double.IsNaN(viewportSize.Width) || viewportSize.Height <= 0.0 || double.IsInfinity(viewportSize.Height) || double.IsNaN(viewportSize.Height))
		//	{
		//		throw new InvalidOperationException("The actual size of the control must be positive and finite in order to set the view using a bounding rectangle.");
		//	}
		//	this.ZoomAndRotateOrigin = null;
		//	Point centerNormalizedMercator;
		//	double num;
		//	MapMath.CalculateViewFromLocations(locations, viewportSize, this.TargetHeading, margin, out centerNormalizedMercator, out num);
		//	num = Math.Min(num, maxZoomLevel);
		//	this.SetViewInternal(centerNormalizedMercator, num, heading);
		//}

		internal void SetView(Point centerNormalizedMercator, double zoomLevel, double heading)
		{
			this.ZoomAndRotateOrigin = null;
			this.SetViewInternal(centerNormalizedMercator, zoomLevel, heading);
		}

		//    /// <summary>Attempts to convert a viewport point to a location on a map.</summary>
		//    /// <returns>Returns <see cref="T:System.Boolean"></see> if the point was successfully converted to a point on the map.</returns>
		//    /// <param name="viewportPoint">A viewport coordinate.</param>
		//    /// <param name="location">Location on the map the corresponds to the viewport point.</param>
		//    public bool TryViewportPointToLocation(Point viewportPoint, out Location location)
		//    {
		//        location = this.ViewportPointToLocation(viewportPoint);
		//        return true;
		//    }

		//    /// <summary>Determines the location associated with the viewport point.</summary>
		//    /// <returns>Returns <see cref="T:Microsoft.Maps.MapControl.WPF.Location"></see>.</returns>
		//    /// <param name="viewportPoint">The viewport point.</param>
		//    public Location ViewportPointToLocation(Point viewportPoint)
		//    {
		//        return this.ConvertNormalizedMercatorToLocation(this.TransformViewportToNormalizedMercator_Current(viewportPoint));
		//    }

		//    /// <summary>Determines the location on the map associated with a point on the viewport.</summary>
		//    /// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		//    /// <param name="viewportPoint">The viewport point.</param>
		//    /// <param name="location">The location on the map.</param>
		//    public bool TryLocationToViewportPoint(Location location, out Point viewportPoint)
		//    {
		//        Matrix3D identity = Matrix3D.Identity;
		//        Matrix3D identity2 = Matrix3D.Identity;
		//        this.CalculateNormalizedMercatorToViewportMapping(this.ViewportSize, new Point(this._CenterNormalizedMercatorSpringX.CurrentValue, this._CenterNormalizedMercatorSpringY.CurrentValue), this._HeadingSpring.CurrentValue, this._ZoomLevelSpring.CurrentValue, true, ref identity, ref identity2);
		//        return MapMath.TryLocationToViewportPoint(ref identity, location, out viewportPoint);
		//    }

		//    /// <summary>Converts a location to a viewport point.</summary>
		//    /// <returns>Returns <see cref="T:System.Windows.Point"></see>.</returns>
		//    /// <param name="location">The location to convert.</param>
		//    public Point LocationToViewportPoint(Location location)
		//    {
		//        Point result;
		//        this.TryLocationToViewportPoint(location, out result);
		//        return result;
		//    }

		//    /// <summary>Invoked when the parent of this element in the visual tree is changed. Overrides System.Windows.UIElement.OnVisualParentChangedhttp://msdn.microsoft.com/en-us/library/system.windows.frameworkelement.onvisualparentchanged.aspx.</summary>
		//    /// <param name="oldParent">The old parent element. May be nullNothingnullptra null reference (Nothing in Visual Basic) to indicate that the element did not have a visual parent previously. (Type: System.Windows.DependencyObjecthttp://msdn.microsoft.com/en-us/library/system.windows.dependencyobject.aspx)</param>
		//    protected override void OnVisualParentChanged(DependencyObject oldParent)
		//    {
		//        if (this._Disposed)
		//        {
		//            throw new InvalidOperationException("The control cannot be added to the visual tree if it has been diposed.");
		//        }
		//        base.OnVisualParentChanged(oldParent);
		//    }

		//    private Point TransformNormalizedMercatorToViewport_Current(Point normalizedMercatorPoint)
		//    {
		//        Size viewportSize = new Size(base.ActualWidth, base.ActualHeight);
		//        Matrix3D identity = Matrix3D.Identity;
		//        Matrix3D identity2 = Matrix3D.Identity;
		//        this.CalculateNormalizedMercatorToViewportMapping(viewportSize, new Point(this._CenterNormalizedMercatorSpringX.CurrentValue, this._CenterNormalizedMercatorSpringY.CurrentValue), this._HeadingSpring.CurrentValue, this._ZoomLevelSpring.CurrentValue, false, ref identity, ref identity2);
		//        return this.Transform(identity, normalizedMercatorPoint);
		//    }

		//    internal Point TransformViewportToNormalizedMercator_Current(Point viewportPoint)
		//    {
		//        Size viewportSize = new Size(base.ActualWidth, base.ActualHeight);
		//        Matrix3D identity = Matrix3D.Identity;
		//        Matrix3D identity2 = Matrix3D.Identity;
		//        this.CalculateNormalizedMercatorToViewportMapping(viewportSize, new Point(this._CenterNormalizedMercatorSpringX.CurrentValue, this._CenterNormalizedMercatorSpringY.CurrentValue), this._HeadingSpring.CurrentValue, this._ZoomLevelSpring.CurrentValue, false, ref identity, ref identity2);
		//        return this.Transform(identity2, viewportPoint);
		//    }

		//    internal Point TransformViewportToNormalizedMercator_Target(Point viewportPoint)
		//    {
		//        Size viewportSize = new Size(base.ActualWidth, base.ActualHeight);
		//        Matrix3D identity = Matrix3D.Identity;
		//        Matrix3D identity2 = Matrix3D.Identity;
		//        this.CalculateNormalizedMercatorToViewportMapping(viewportSize, new Point(this._CenterNormalizedMercatorSpringX.TargetValue, this._CenterNormalizedMercatorSpringY.TargetValue), this._HeadingSpring.TargetValue, this._ZoomLevelSpring.TargetValue, false, ref identity, ref identity2);
		//        return this.Transform(identity2, viewportPoint);
		//    }

		//    private void AnimateViewUsingZoomAndPan(double zoomLevel, Point centerNormalizedMercator)
		//    {
		//        this._ZoomAndPan_FromZoomLevel = this._ZoomLevelSpring.CurrentValue;
		//        this._ZoomAndPan_FromRect = new Rect(this.TransformViewportToNormalizedMercator_Current(new Point(0.0, 0.0)), this.TransformViewportToNormalizedMercator_Current(new Point(base.ActualWidth, base.ActualHeight)));
		//        double num = Math.Pow(2.0, this._ZoomAndPan_FromZoomLevel - zoomLevel);
		//        Rect toRect = new Rect(0.0, 0.0, this._ZoomAndPan_FromRect.Width * num, this._ZoomAndPan_FromRect.Height * num);
		//        toRect.X = centerNormalizedMercator.X - toRect.Width / 2.0;
		//        toRect.Y = centerNormalizedMercator.Y - toRect.Height / 2.0;
		//        toRect.X += Math.Floor(this._CenterNormalizedMercatorSpringX.CurrentValue);
		//        toRect.Y += Math.Floor(this._CenterNormalizedMercatorSpringY.CurrentValue);
		//        double value;
		//        this._ZoomAndPanAnimator.Begin(this._ZoomAndPan_FromRect, toRect, out value);
		//        this._ZoomAndPanAnimationDriver.Start(new Duration(TimeSpan.FromSeconds(value)));
		//        this._CurrentMapMode.ChooseLevelOfDetailSettings = TilePyramidRenderable.ChooseLevelOfDetailSettingsDownloadInMotion;
		//    }

		//    private void _ZoomAndPanAnimationDriver_AnimationProgressChanged(object sender, EventArgs e)
		//    {
		//        double num;
		//        Point point;
		//        this._ZoomAndPanAnimator.Tick(this._ZoomAndPanAnimationDriver.AnimationProgress, out num, out point);
		//        this._ZoomLevelSpring.SnapToValue(Math.Log(this._ZoomAndPan_FromRect.Width / num) / Math.Log(2.0) + this._ZoomAndPan_FromZoomLevel);
		//        this._CenterNormalizedMercatorSpringX.SnapToValue(point.X);
		//        this._CenterNormalizedMercatorSpringY.SnapToValue(point.Y);
		//        this.UpdateView();
		//        if (this.ViewChangeOnFrame != null)
		//        {
		//            this.ViewChangeOnFrame(this, new MapEventArgs());
		//        }
		//    }

		//    private void _ZoomAndPanAnimationDriver_AnimationStopped(object sender, EventArgs e)
		//    {
		//        this._CurrentMapMode.ChooseLevelOfDetailSettings = TilePyramidRenderable.ChooseLevelOfDetailSettingsDownloadNormal;
		//    }

		//    private void _ZoomAndPanAnimationDriver_AnimationCompleted(object sender, EventArgs e)
		//    {
		//        this.ViewIsAnimating = false;
		//        if (this.ViewChangeEnd != null && !this._UserInputTimeout.IsEnabled)
		//        {
		//            this.ViewChangeEnd(this, new MapEventArgs());
		//        }
		//        this._CurrentMapMode.ChooseLevelOfDetailSettings = TilePyramidRenderable.ChooseLevelOfDetailSettingsDownloadNormal;
		//    }

		private void CalculateNormalizedMercatorToViewportMapping(Size viewportSize, Point centerNormalizedMercator, double heading, double zoomLevel, bool applyWorldWrap, ref Matrix3D normalizedMercatorToViewport, ref Matrix3D viewportToNormalizedMercator)
		    {
		        Point point = applyWorldWrap ? this.ApplyWorldWrap(centerNormalizedMercator) : centerNormalizedMercator;
		        MapCore._NormalizedMercatorToViewport_TranslatePre.OffsetX = -point.X;
		        MapCore._NormalizedMercatorToViewport_TranslatePre.OffsetY = -point.Y;
		        double num = 256.0 * Math.Pow(2.0, zoomLevel);
		        MapCore._NormalizedMercatorToViewport_Scale.M11 = num;
		        MapCore._NormalizedMercatorToViewport_Scale.M22 = num;
		        MapCore._NormalizedMercatorToViewport_Rotate = VectorMath.RotationMatrix3DZ(3.1415926535897931 * heading / 180.0);
		        MapCore._NormalizedMercatorToViewport_TranslatePost.OffsetX = viewportSize.Width / 2.0;
		        MapCore._NormalizedMercatorToViewport_TranslatePost.OffsetY = viewportSize.Height / 2.0;
		        normalizedMercatorToViewport = MapCore._NormalizedMercatorToViewport_TranslatePre * MapCore._NormalizedMercatorToViewport_Scale * MapCore._NormalizedMercatorToViewport_Rotate * MapCore._NormalizedMercatorToViewport_TranslatePost;
		        viewportToNormalizedMercator = normalizedMercatorToViewport;
		        viewportToNormalizedMercator.Invert();
		    }

		private Point ApplyWorldWrap(Point normalizedMercator)
		{
			return new Point(normalizedMercator.X - Math.Floor(normalizedMercator.X), normalizedMercator.Y);
		}

		//    private Point ApplyWorldWrapInverse(Point normalizedMercator, Point normalizedMercatorTarget)
		//    {
		//        Point point = this.ApplyWorldWrap(normalizedMercator);
		//        return new Point(Math.Floor(normalizedMercatorTarget.X) + point.X, normalizedMercatorTarget.Y + point.Y);
		//    }

		private void UpdateView()
		{
		    Size viewportSize = new Size(base.ActualWidth, base.ActualHeight);
		    Matrix3D identity = Matrix3D.Identity;
		    Matrix3D identity2 = Matrix3D.Identity;
		    if (this._CenterNormalizedMercatorSpringY.CurrentValue < 0.0)
		    {
		        this._CenterNormalizedMercatorSpringY.SnapToValue(0.0);
		    }
		    else if (this._CenterNormalizedMercatorSpringY.CurrentValue > 1.0)
		    {
		        this._CenterNormalizedMercatorSpringY.SnapToValue(1.0);
		    }
		    this.CalculateNormalizedMercatorToViewportMapping(viewportSize, new Point(this._CenterNormalizedMercatorSpringX.CurrentValue, this._CenterNormalizedMercatorSpringY.CurrentValue), this._HeadingSpring.CurrentValue, this._ZoomLevelSpring.CurrentValue, true, ref identity, ref identity2);
		    foreach (MapMode current in this._MapModes)
		    {
		        current.CurrentMapCopyInstance = new Point(Math.Floor(this._CenterNormalizedMercatorSpringX.CurrentValue), Math.Floor(this._CenterNormalizedMercatorSpringY.CurrentValue));
		        ((IProjectable)current).SetView(viewportSize, identity, identity2);
		    }
			((IProjectable)this._MapUserLayerContainer).SetView(viewportSize, identity, identity2);
			foreach (object current2 in this.Children)
			{
				IProjectable projectable = current2 as IProjectable;
				if (projectable != null)
				{
					projectable.SetView(viewportSize, identity, identity2);
				}
			}
			this.UpdateViewDependencyProperties();
		}

		private void UpdateViewDependencyProperties()
		{
			this._ViewUpdatingInternally = true;
			this.ZoomLevel = this._ZoomLevelSpring.CurrentValue;
			this.Heading = this._HeadingSpring.CurrentValue;
			this.Center = this.ConvertNormalizedMercatorToLocation(new Point(this._CenterNormalizedMercatorSpringX.CurrentValue, this._CenterNormalizedMercatorSpringY.CurrentValue));
			this._ViewUpdatingInternally = false;
		}

		private void _MapContainer_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((RectangleGeometry)this._MapContainer.Clip).Rect = new Rect(0.0, 0.0, e.NewSize.Width, e.NewSize.Height);
			this.UpdateView();
		}

		private Location ConvertNormalizedMercatorToLocation(Point normalizedMercatorPoint)
		{
			Point point = this.ApplyWorldWrap(normalizedMercatorPoint);
			return MapMath.NormalizeLocation(MercatorCube.Instance.ToLocation(new Microsoft.Maps.MapExtras.Point3D(point.X, point.Y, 0.0)));
		}

		private Point ConvertLocationToNormalizedMercator(Location location)
		{
			return MercatorCube.Instance.FromLocation(location).ToPoint();
		}

		private void MapCore_Loaded(object sender, RoutedEventArgs e)
		{
			if (this._MapModes.Count == 0)
			{
				this.Mode = new AerialMode(true); //new RoadMode();
				//this.Mode.SessionId = this.CredentialsProvider.SessionId;
			}
		}

		private void SetViewInternal(Point centerNormalizedMercator, double zoomLevel, double heading)
		{
			//zoomLevel = Math.Min(21.0, Math.Max(0.75, zoomLevel));
			//if (!this.ViewBeingSetByUserInput)
			//{
			//	this.ZoomAndRotateOrigin = null;
			//}
			//bool viewIsAnimating = this.ViewIsAnimating;
			//this.ViewIsAnimating = false;
			//bool flag = this._CenterNormalizedMercatorSpringX.TargetValue != centerNormalizedMercator.X || this._CenterNormalizedMercatorSpringY.TargetValue != centerNormalizedMercator.Y || this._ZoomLevelSpring.TargetValue != zoomLevel || this._HeadingSpring.TargetValue != heading;
			//bool flag2 = this.AnimationLevel == AnimationLevel.None || (this.AnimationLevel == AnimationLevel.UserInput && !this.ViewBeingSetByUserInput);
			//if (flag)
			//{
			//	if (this._ZoomAndPanAnimationDriver.IsAnimating)
			//	{
			//		this._ZoomAndPanAnimationDriver.Stop();
			//	}
			//	if (!flag2 && !this.ViewBeingSetByUserInput)
			//	{
			//		this._ZoomLevelSpring.SnapToValue(this._ZoomLevelSpring.CurrentValue);
			//		this._CenterNormalizedMercatorSpringX.SnapToValue(this._CenterNormalizedMercatorSpringX.CurrentValue);
			//		this._CenterNormalizedMercatorSpringY.SnapToValue(this._CenterNormalizedMercatorSpringY.CurrentValue);
			//		this._HeadingSpring.TargetValue = heading;
			//		this.AnimateViewUsingZoomAndPan(zoomLevel, centerNormalizedMercator);
			//		this.ViewIsAnimating = true;
			//	}
			//	else
			//	{
			//		this.AnimateViewUsingSprings(centerNormalizedMercator, zoomLevel, heading, flag2);
			//		this.ViewIsAnimating = !flag2;
			//	}
			//}
			//else if (viewIsAnimating && flag2)
			//{
			//	this.AnimateViewUsingSprings(centerNormalizedMercator, zoomLevel, heading, true);
			//}
			//if (flag)
			//{
			//	if (this.TargetViewChanged != null)
			//	{
			//		this.TargetViewChanged(this, new MapEventArgs());
			//	}
			//	if (!viewIsAnimating && this.ViewChangeStart != null)
			//	{
			//		if (this.ViewBeingSetByUserInput)
			//		{
			//			if (this._UserInputTimeout.IsEnabled)
			//			{
			//				this._UserInputTimeout.Stop();
			//			}
			//			else
			//			{
			//				this.ViewChangeStart(this, new MapEventArgs());
			//				this._UserInputTimeout.Tick += new EventHandler(this.UserInputCompleted);
			//			}
			//			this._UserInputTimeout.Start();
			//		}
			//		else
			//		{
			//			this.ViewChangeStart(this, new MapEventArgs());
			//		}
			//	}
			//	if (!this.ViewIsAnimating)
			//	{
			//		if (this.ViewChangeOnFrame != null)
			//		{
			//			this.ViewChangeOnFrame(this, new MapEventArgs());
			//		}
			//		if (this.ViewChangeEnd != null && !this._UserInputTimeout.IsEnabled)
			//		{
			//			this.ViewChangeEnd(this, new MapEventArgs());
			//		}
			//	}
			//}
		}

		//    private void UserInputCompleted(object sender, EventArgs e)
		//    {
		//        if (!this.ViewIsAnimating && this.ViewChangeEnd != null)
		//        {
		//            this.ViewChangeEnd(this, new MapEventArgs());
		//        }
		//        this._UserInputTimeout.Tick -= new EventHandler(this.UserInputCompleted);
		//        this._UserInputTimeout.Stop();
		//    }

		//    private void AnimateViewUsingSprings(Point centerNormalizedMercator, double zoomLevel, double heading, bool setViewImmediately)
		//    {
		//        Point? point = this._ZoomAndRotateOrigin.HasValue ? new Point?(this.TransformViewportToNormalizedMercator_Current(this._ZoomAndRotateOrigin.Value)) : null;
		//        double currentValue = this._HeadingSpring.CurrentValue;
		//        double currentValue2 = this._ZoomLevelSpring.CurrentValue;
		//        if (setViewImmediately)
		//        {
		//            this._ZoomLevelSpring.SnapToValue(zoomLevel);
		//            this._HeadingSpring.SnapToValue(heading);
		//        }
		//        else
		//        {
		//            this._HeadingSpring.TargetValue = heading;
		//            this._ZoomLevelSpring.TargetValue = zoomLevel;
		//        }
		//        if ((currentValue2 != this._ZoomLevelSpring.CurrentValue || currentValue != this._HeadingSpring.CurrentValue) && point.HasValue)
		//        {
		//            this.AdjustCenterAfterTransform(point.Value);
		//        }
		//        else if (setViewImmediately)
		//        {
		//            this._CenterNormalizedMercatorSpringX.SnapToValue(centerNormalizedMercator.X);
		//            this._CenterNormalizedMercatorSpringY.SnapToValue(centerNormalizedMercator.Y);
		//        }
		//        else
		//        {
		//            this._CenterNormalizedMercatorSpringX.TargetValue = centerNormalizedMercator.X;
		//            this._CenterNormalizedMercatorSpringY.TargetValue = centerNormalizedMercator.Y;
		//        }
		//        this.UpdateView();
		//    }

		//    private Point Transform(Matrix3D matrix, Point point)
		//    {
		//        System.Windows.Media.Media3D.Point3D point3D = matrix.Transform(new System.Windows.Media.Media3D.Point3D(point.X, point.Y, 0.0));
		//        return new Point(point3D.X, point3D.Y);
		//    }

		//    private Point2D Transform(Matrix3D matrix, Point2D point)
		//    {
		//        System.Windows.Media.Media3D.Point3D point3D = matrix.Transform(new System.Windows.Media.Media3D.Point3D(point.X, point.Y, 0.0));
		//        return new Point2D(point3D.X, point3D.Y);
		//    }

		//    private void AdjustCenterAfterTransform(Point zoomAndRotateOriginNormalizedMercatorPreUpdate)
		//    {
		//        Point point = this.TransformViewportToNormalizedMercator_Current(this._ZoomAndRotateOrigin.Value);
		//        this._CenterNormalizedMercatorSpringX.SnapToValue(this._CenterNormalizedMercatorSpringX.CurrentValue + zoomAndRotateOriginNormalizedMercatorPreUpdate.X - point.X);
		//        this._CenterNormalizedMercatorSpringY.SnapToValue(this._CenterNormalizedMercatorSpringY.CurrentValue + zoomAndRotateOriginNormalizedMercatorPreUpdate.Y - point.Y);
		//    }

		//    private void CompositionTarget_Rendering(object sender, EventArgs e)
		//    {
		//        bool flag = false;
		//        Point? point = this._ZoomAndRotateOrigin.HasValue ? new Point?(this.TransformViewportToNormalizedMercator_Current(this._ZoomAndRotateOrigin.Value)) : null;
		//        if (this._ZoomLevelSpring.Update() || this._HeadingSpring.Update())
		//        {
		//            if (point.HasValue)
		//            {
		//                this.AdjustCenterAfterTransform(point.Value);
		//            }
		//            flag = true;
		//        }
		//        else
		//        {
		//            flag |= this._CenterNormalizedMercatorSpringX.Update();
		//            flag |= this._CenterNormalizedMercatorSpringY.Update();
		//        }
		//        if (flag)
		//        {
		//            this.UpdateView();
		//            if (this.ViewChangeOnFrame != null)
		//            {
		//                this.ViewChangeOnFrame(this, new MapEventArgs());
		//                return;
		//            }
		//        }
		//        else if (this.ViewIsAnimating && !this._ZoomAndPanAnimationDriver.IsAnimating)
		//        {
		//            this.ViewIsAnimating = false;
		//            if (this.ViewChangeEnd != null && !this._UserInputTimeout.IsEnabled)
		//            {
		//                this.ViewChangeEnd(this, new MapEventArgs());
		//            }
		//        }
		//    }

		//    internal void ArrestZoomAndRotation()
		//    {
		//        this._ZoomLevelSpring.SnapToValue(this._ZoomLevelSpring.CurrentValue);
		//        this._HeadingSpring.SnapToValue(this._HeadingSpring.CurrentValue);
		//        this._ZoomAndRotateOrigin = null;
		//    }

		//    private void _CurrentMapModeTransitionTimeout_Elapsed(object sender, ElapsedEventArgs e)
		//    {
		//        base.Dispatcher.BeginInvoke(new Action(delegate
		//        {
		//            if (this._MapModes.Count > 1 && !this._ModeSwitchAnationDriver.IsAnimating)
		//            {
		//                this._CurrentMapMode.Opacity = 1.0;
		//                this._ModeSwitchAnationDriver.Start(this.ModeCrossFadeDuration);
		//            }
		//        }), new object[0]);
		//    }

		private void _CurrentMapMode_Rendered(object sender, EventArgs e)
		{
			if (this._MapModes.Count > 1 && !this._ModeSwitchAnationDriver.IsAnimating && this._CurrentMapMode.HasSomeTiles)
			{
				this._CurrentMapMode.Opacity = 1.0;
				this._ModeSwitchAnationDriver.Start(this.ModeCrossFadeDuration);
				this._CurrentMapModeTransitionTimeout.Enabled = false;
			}
		}

		//    private void _ModeSwitchAnationDriver_AnimationCompleted(object sender, EventArgs e)
		//    {
		//        this._MapModes[0].Detach();
		//        this._MapModeContainer.Children.Remove(this._MapModes[0]);
		//        this._MapModes.RemoveAt(0);
		//        this._CurrentMapMode.Rendered -= new EventHandler(this._CurrentMapMode_Rendered);
		//        if (this._PendingMapMode != null)
		//        {
		//            base.Dispatcher.BeginInvoke(new Action(delegate
		//            {
		//                this.Mode = this._PendingMapMode;
		//            }), new object[0]);
		//        }
		//    }

		//    private void _ModeSwitchAnationDriver_AnimationProgressChanged(object sender, EventArgs e)
		//    {
		//        this._MapModes[0].Opacity = 1.0 - this._ModeSwitchAnationDriver.AnimationProgress;
		//    }
	}
}
