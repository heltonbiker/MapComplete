using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using Microsoft.Maps.MapControl.WPF.Overlays;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Input.Manipulations;
using System.Windows.Threading;

namespace MapComplete.MapControl
{
    /// <summary>Represents the default map class.</summary>
    public class Map : MapCore
    {
        //private static class LogEntry
        //{
        //    public const string StartSession = "0";

        //    public const string ChangeCredentials = "2";
        //}

        //private class StoredManipulationDelta2D
        //{
        //    public int OriginX
        //    {
        //        get;
        //        set;
        //    }

        //    public int OriginY
        //    {
        //        get;
        //        set;
        //    }

        //    public float Rotation
        //    {
        //        get;
        //        set;
        //    }

        //    public float ScaleX
        //    {
        //        get;
        //        set;
        //    }

        //    public float ScaleY
        //    {
        //        get;
        //        set;
        //    }

        //    public float TranslationX
        //    {
        //        get;
        //        set;
        //    }

        //    public float TranslationY
        //    {
        //        get;
        //        set;
        //    }

        //    public StoredManipulationDelta2D()
        //    {
        //        this.Reset();
        //    }

        //    public bool Accumulate(Manipulation2DDeltaEventArgs additional)
        //    {
        //        bool flag = true;
        //        if (this.OriginY == -2147483648)
        //        {
        //            this.OriginX = (int)Math.Round((double)additional.OriginX);
        //            this.OriginY = (int)Math.Round((double)additional.OriginY);
        //        }
        //        this.ScaleX *= additional.Delta.ScaleX;
        //        this.ScaleY *= additional.Delta.ScaleX;
        //        if (Math.Abs(this.OriginX - (int)Math.Round((double)additional.OriginX)) > 2 || Math.Abs(this.OriginY - (int)Math.Round((double)additional.OriginY)) > 2)
        //        {
        //            flag = false;
        //        }
        //        this.Rotation += additional.Delta.Rotation;
        //        this.TranslationX += additional.Delta.TranslationX;
        //        this.TranslationY += additional.Delta.TranslationY;
        //        return ((double)Math.Abs(this.Rotation) >= 0.1 || (double)Math.Abs(1f - this.ScaleX) >= 0.1 || (double)Math.Abs(1f - this.ScaleY) >= 0.1 || Math.Abs(this.TranslationX) >= 2f || Math.Abs(this.TranslationY) >= 2f) && flag;
        //    }

        //    public void Reset()
        //    {
        //        this.Rotation = 0f;
        //        this.TranslationX = 0f;
        //        this.TranslationY = 0f;
        //        this.ScaleX = 1f;
        //        this.ScaleY = 1f;
        //        this.OriginX = -2147483648;
        //        this.OriginY = -2147483648;
        //    }

        //    public bool HasValuesStored()
        //    {
        //        return this.OriginX != -2147483648;
        //    }
        //}

        //private WeakEventListener<Map, object, PropertyChangedEventArgs> _weakMapCredentials;

        //private bool firstFrameDone;

        private bool getConfigurationDone;

        //private bool settingDefaultCredentials;

        //private bool startedSession;

        //private LoadingErrorMessage loadingErrorMessage;

        //private Exception LoadingException;

        //private string logServiceUriFormat;

        private MapForeground _MapForeground;

        //private Point? _LeftButtonDownNormalizedMercatorPoint = null;

        //private Point _LeftButtonDownViewportPoint = default(Point);

        //private bool _useInertia;

        //private ManipulationProcessor2D _ManipulationProcessor;

        //private InertiaProcessor2D _InertiaProcessor;

        //private Map.StoredManipulationDelta2D storedManipulation = new Map.StoredManipulationDelta2D();

        //private DispatcherTimer _InertiaTimer;

        private static bool _useHttps;

        //private int _doubleTapDelay = 500;

        //private int _doubleTapThreshold = 20;

        //private Point? _touchTapPointPrevious = null;

        //private DateTime? _touchTapTimePrevious = null;

        //private Point? _touchTapPointCurrent = null;

        //private DateTime? _touchTapTimeCurrent = null;

        //private long _lastTouchTick;

        //private int _touchCount;

        //private Point _velocity;

        //private Point _previousMousePoint;

        //private DateTime _lastMouseMoveTime = DateTime.Now;

        //private Timer _logTimer;

        private static string version;

		//private Manipulations2D _SupportedManipulations = Manipulations2D.All;

		//private event EventHandler<LoadingErrorEventArgs> loadingErrorEvent;

		///// <summary>Occurs when the left mouse button is pressed down over the map.</summary>
		//public new event MouseButtonEventHandler MouseLeftButtonDown;

		///// <summary>Occurs when the left mouse button is released.</summary>
		//public new event MouseButtonEventHandler MouseLeftButtonUp;

		///// <summary>Occurs when the mouse is used to double click the map.</summary>
		//public new event MouseButtonEventHandler MouseDoubleClick;

		///// <summary>Occurs when the mouse moves over the map.(</summary>
		//public new event MouseEventHandler MouseMove;

		///// <summary>Occurs when the mouse wheel is used.</summary>
		//public new event MouseWheelEventHandler MouseWheel;

		///// <summary>Occurs when a key is pressed while focus is on the map.</summary>
		//public new event KeyEventHandler KeyDown;

		///// <summary>Occurs when the touch of the map on the screen finishes.</summary>
		//public new event EventHandler<TouchEventArgs> TouchUp;

		///// <summary>Occurs when the screen is touched over the map.</summary>
		//public new event EventHandler<TouchEventArgs> TouchDown;

		///// <summary>Occurs when the screen is touched and dragged to move the map.</summary>
		//public new event EventHandler<TouchEventArgs> TouchMove;

		///// <summary>Occurs when there is an error loading the map.</summary>
		//public event EventHandler<LoadingErrorEventArgs> LoadingError
		//{
		//    add
		//    {
		//        if (this.LoadingException != null && value != null)
		//        {
		//            value(this, new LoadingErrorEventArgs(this.LoadingException));
		//        }
		//        this.loadingErrorEvent += value;
		//    }
		//    remove
		//    {
		//        this.loadingErrorEvent -= value;
		//    }
		//}

		/// <summary>Gets or sets whether to use HTTPS protocol when requesting a map.</summary>
		/// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		public static bool UseHttps
		{
			get
			{
				return Map._useHttps;
			}
			set
			{
				Map._useHttps = value;
			}
		}

		/// <summary>Gets the protocol (http or https) that is used to request the map.</summary>
		/// <returns>Returns <see cref="T:System.String"></see>.</returns>
		public static string UriScheme
		{
			get
			{
				if (!Map._useHttps)
				{
					return Uri.UriSchemeHttp;
				}
				return Uri.UriSchemeHttps;
			}
		}

		//public static int LoggingDelay
		//{
		//    get;
		//    set;
		//}

		///// <summary>Gets or sets the map foreground style.</summary>
		///// <returns>Returns <see cref="T:System.Windows.Style"></see>.</returns>
		//public Style MapForegroundStyle
		//{
		//    get
		//    {
		//        return this._MapForeground.Style;
		//    }
		//    set
		//    {
		//        this._MapForeground.Style = value;
		//        this._MapForeground.Visibility = ((value == null) ? Visibility.Hidden : Visibility.Visible);
		//    }
		//}

		///// <summary>Gets or sets the current set of supported two-dimensional (2-D) manipulationshttp://msdn.microsoft.com/en-us/library/system.windows.input.manipulations.manipulations2d.aspx.</summary>
		///// <returns>Returns System.Windows.Input.Manipulations.Manipulations2Dhttp://msdn.microsoft.com/en-us/library/system.windows.input.manipulations.manipulations2d.aspx.</returns>
		//public Manipulations2D SupportedManipulations
		//{
		//    get
		//    {
		//        return this._SupportedManipulations;
		//    }
		//    set
		//    {
		//        this.StopInertia();
		//        this._SupportedManipulations = value;
		//        this._ManipulationProcessor.SupportedManipulations = this._SupportedManipulations;
		//    }
		//}

		///// <summary>Gets or sets whether to use the inertial animation effect during map navigation.</summary>
		///// <returns>Returns <see cref="T:System.Boolean"></see>.</returns>
		//public bool UseInertia
		//{
		//    get
		//    {
		//        return this._useInertia;
		//    }
		//    set
		//    {
		//        this._useInertia = value;
		//    }
		//}

		//private static bool IsInDesignMode
		//{
		//    get
		//    {
		//        return (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
		//    }
		//}

		private static string GetVersion()
        {
            string result = string.Empty;
            string[] array = Assembly.GetExecutingAssembly().FullName.Split(new char[]
            {
                ','
            });
            if (array.Length > 1)
            {
                result = array[1].Replace("Version=", string.Empty).Trim();
            }
            return result;
        }

        static Map()
        {
            Map._useHttps = false;
            Map.version = Map.GetVersion();
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Map), new FrameworkPropertyMetadata(typeof(Map)));
        }

        ///// <summary>Initializes a new instance of the <see cref="T:Microsoft.Maps.MapControl.WFF.Map"></see> class.</summary>
        public Map()
        {
        //    this.settingDefaultCredentials = true;
        //    base.CredentialsProvider = new ApplicationIdCredentialsProvider();
        //    this.settingDefaultCredentials = false;
        
		    MapConfiguration.GetSection("v1", "Services", base.Culture, null, new MapConfigurationCallback(this.AsynchronousConfigurationLoaded), true);
        
		    this._MapForeground = new MapForeground(this);        
		    base.MapForegroundContainer.Children.Add(this._MapForeground);
        
        //    this._ManipulationProcessor = new ManipulationProcessor2D(this._SupportedManipulations);
        //    this._ManipulationProcessor.Started += new EventHandler<Manipulation2DStartedEventArgs>(this.ManipulationProcessor_Started);
        //    this._ManipulationProcessor.Delta += new EventHandler<Manipulation2DDeltaEventArgs>(this.ManipulationProcessor_Delta);
        //    this._ManipulationProcessor.Completed += new EventHandler<Manipulation2DCompletedEventArgs>(this.ManipulationProcessor_Completed);
        
        //    this._InertiaProcessor = new InertiaProcessor2D();
        //    this._InertiaProcessor.TranslationBehavior.DesiredDeceleration = 0.00384f;
        //    this._InertiaProcessor.ExpansionBehavior.DesiredDeceleration = 9.6E-05f;
        //    this._InertiaProcessor.RotationBehavior.DesiredDeceleration = 0.00072f;
        //    this._InertiaProcessor.Delta += new EventHandler<Manipulation2DDeltaEventArgs>(this.ManipulationProcessor_Delta);
        //    this._InertiaProcessor.Completed += delegate (object sender, Manipulation2DCompletedEventArgs e)
        //    {
        //        this.StopInertia();
        //    };
        //    this._InertiaTimer = new DispatcherTimer
        //    {
        //        Interval = TimeSpan.FromMilliseconds(30.0)
        //    };
        //    this._InertiaTimer.Tick += delegate (object sender, EventArgs e)
        //    {
        //        this._InertiaProcessor.Process(DateTime.UtcNow.Ticks);
        //    };

            base.IsTabStop = true;
        }

		//~Map()
		//{
		//    this.Dispose(false);
		//}

		//protected override void Dispose(bool disposing)
		//{
		//    base.Dispose(disposing);
		//    if (this._logTimer != null)
		//    {
		//        this._logTimer.Dispose();
		//        this._logTimer = null;
		//    }
		//    if (this._weakMapCredentials != null)
		//    {
		//        this._weakMapCredentials.Detach();
		//        this._weakMapCredentials = null;
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when a keyboard key is pressed.</summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnKeyDown(KeyEventArgs e)
		//{
		//    KeyEventHandler keyDown = this.KeyDown;
		//    if (keyDown != null)
		//    {
		//        keyDown(this, e);
		//    }
		//    if (!e.Handled)
		//    {
		//        if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
		//        {
		//            Point point = default(Point);
		//            point.X += ((e.Key == Key.Right) ? 100.0 : 0.0);
		//            point.X += ((e.Key == Key.Left) ? -100.0 : 0.0);
		//            point.Y += ((e.Key == Key.Up) ? -100.0 : 0.0);
		//            point.Y += ((e.Key == Key.Down) ? 100.0 : 0.0);
		//            Point centerNormalizedMercator = base.TransformViewportToNormalizedMercator_Target(new Point(base.ActualWidth / 2.0 + point.X, base.ActualHeight / 2.0 + point.Y));
		//            base.ArrestZoomAndRotation();
		//            base.ViewBeingSetByUserInput = true;
		//            base.SetView(centerNormalizedMercator, base.TargetZoomLevel, base.TargetHeading);
		//            base.ViewBeingSetByUserInput = false;
		//        }
		//        else if (e.Key == Key.OemPlus || e.Key == Key.Add)
		//        {
		//            base.ZoomAndRotateOrigin = new Point?(new Point(base.ActualWidth / 2.0, base.ActualHeight / 2.0));
		//            base.ViewBeingSetByUserInput = true;
		//            base.SetView(base.TargetZoomLevel + 0.5, base.TargetHeading);
		//            base.ViewBeingSetByUserInput = false;
		//        }
		//        else if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
		//        {
		//            base.ZoomAndRotateOrigin = new Point?(new Point(base.ActualWidth / 2.0, base.ActualHeight / 2.0));
		//            base.ViewBeingSetByUserInput = true;
		//            base.SetView(base.TargetZoomLevel - 0.5, base.TargetHeading);
		//            base.ViewBeingSetByUserInput = false;
		//        }
		//        base.OnKeyDown(e);
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when the left mouse button is pressed.</summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		//{
		//    if (e.StylusDevice != null && e.StylusDevice.TabletDevice != null && e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
		//    {
		//        return;
		//    }
		//    MouseButtonEventHandler mouseLeftButtonDown = this.MouseLeftButtonDown;
		//    if (mouseLeftButtonDown != null)
		//    {
		//        mouseLeftButtonDown(this, e);
		//    }
		//    if (!e.Handled)
		//    {
		//        this.StopInertia();
		//        if (base.CaptureMouse())
		//        {
		//            this._LeftButtonDownViewportPoint = e.GetPosition(this);
		//            this._LeftButtonDownNormalizedMercatorPoint = new Point?(base.TransformViewportToNormalizedMercator_Current(this._LeftButtonDownViewportPoint));
		//            this._previousMousePoint = this._LeftButtonDownViewportPoint;
		//            this._lastMouseMoveTime = DateTime.Now;
		//        }
		//        else
		//        {
		//            this._LeftButtonDownNormalizedMercatorPoint = null;
		//        }
		//        base.OnMouseLeftButtonDown(e);
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when the mouse is used to double-click the map.</summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
		//{
		//    MouseButtonEventHandler mouseDoubleClick = this.MouseDoubleClick;
		//    if (mouseDoubleClick != null)
		//    {
		//        mouseDoubleClick(this, e);
		//    }
		//    if (!e.Handled)
		//    {
		//        this._LeftButtonDownNormalizedMercatorPoint = null;
		//        double zoomLevelIncrement = 1.0;
		//        this.ZoomAboutViewportPoint(zoomLevelIncrement, e.GetPosition(this));
		//        base.OnMouseDoubleClick(e);
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when the mouse wheel moves at the same time the mouse cursor is over the map.</summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnMouseWheel(MouseWheelEventArgs e)
		//{
		//    MouseWheelEventHandler mouseWheel = this.MouseWheel;
		//    if (mouseWheel != null)
		//    {
		//        mouseWheel(this, e);
		//    }
		//    if (!e.Handled)
		//    {
		//        this.ZoomAboutViewportPoint((double)e.Delta / 100.0, e.GetPosition(this));
		//        base.OnMouseWheel(e);
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when the mouse cursor is over the map.</summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnMouseMove(MouseEventArgs e)
		//{
		//    if (e.StylusDevice != null && e.StylusDevice.TabletDevice != null && e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
		//    {
		//        return;
		//    }
		//    MouseEventHandler mouseMove = this.MouseMove;
		//    if (mouseMove != null)
		//    {
		//        mouseMove(this, e);
		//    }
		//    if (!e.Handled)
		//    {
		//        if (base.IsMouseCaptured)
		//        {
		//            Point position = e.GetPosition(this);
		//            if (this.UseInertia)
		//            {
		//                this.UpdateVelocity(position);
		//            }
		//            this._previousMousePoint = position;
		//            if (this._LeftButtonDownNormalizedMercatorPoint.HasValue && this._LeftButtonDownViewportPoint != position)
		//            {
		//                this._LeftButtonDownViewportPoint = new Point(double.NaN, double.NaN);
		//                Point point = base.TransformViewportToNormalizedMercator_Target(position);
		//                Point point2 = base.TransformViewportToNormalizedMercator_Target(new Point(base.ActualWidth / 2.0, base.ActualHeight / 2.0));
		//                Point centerNormalizedMercator = new Point(this._LeftButtonDownNormalizedMercatorPoint.Value.X - point.X + point2.X, this._LeftButtonDownNormalizedMercatorPoint.Value.Y - point.Y + point2.Y);
		//                AnimationLevel animationLevel = base.AnimationLevel;
		//                base.AnimationLevel = AnimationLevel.None;
		//                base.ArrestZoomAndRotation();
		//                base.ViewBeingSetByUserInput = true;
		//                base.SetView(centerNormalizedMercator, base.TargetZoomLevel, base.TargetHeading);
		//                base.ViewBeingSetByUserInput = false;
		//                base.ZoomAndRotateOrigin = new Point?(position);
		//                base.AnimationLevel = animationLevel;
		//            }
		//        }
		//        base.OnMouseMove(e);
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when the left mouse button is released.</summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		//{
		//    if (e.StylusDevice != null && e.StylusDevice.TabletDevice != null && e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
		//    {
		//        return;
		//    }
		//    MouseButtonEventHandler mouseLeftButtonUp = this.MouseLeftButtonUp;
		//    if (mouseLeftButtonUp != null)
		//    {
		//        mouseLeftButtonUp(this, e);
		//    }
		//    if (!e.Handled)
		//    {
		//        base.ReleaseMouseCapture();
		//        if (!base.IsMouseCaptured && !base.AreAnyTouchesCaptured)
		//        {
		//            this._LeftButtonDownNormalizedMercatorPoint = null;
		//        }
		//        if (this.UseInertia && !this._InertiaTimer.IsEnabled)
		//        {
		//            DateTime now = DateTime.Now;
		//            int milliseconds = (now - this._lastMouseMoveTime).Milliseconds;
		//            this._lastMouseMoveTime = now;
		//            if (milliseconds < 200)
		//            {
		//                this._InertiaProcessor.InitialOriginX = (float)this._previousMousePoint.X;
		//                this._InertiaProcessor.InitialOriginY = (float)this._previousMousePoint.Y;
		//                this._InertiaProcessor.TranslationBehavior.InitialVelocityX = (float)this._velocity.X;
		//                this._InertiaProcessor.TranslationBehavior.InitialVelocityY = (float)this._velocity.Y;
		//                this._InertiaProcessor.Process(DateTime.UtcNow.Ticks);
		//                this._InertiaTimer.Start();
		//            }
		//        }
		//        base.OnMouseLeftButtonUp(e);
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when a finger touches the screen while it is over the map. </summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnTouchDown(TouchEventArgs e)
		//{
		//    EventHandler<TouchEventArgs> touchDown = this.TouchDown;
		//    if (touchDown != null)
		//    {
		//        touchDown(this, e);
		//    }
		//    this._lastTouchTick = 0L;
		//    if (!e.Handled)
		//    {
		//        e.Handled = true;
		//        if (e.TouchDevice.Capture(this))
		//        {
		//            this._LeftButtonDownViewportPoint = e.GetTouchPoint(this).Position;
		//            this._LeftButtonDownNormalizedMercatorPoint = new Point?(base.TransformViewportToNormalizedMercator_Current(this._LeftButtonDownViewportPoint));
		//            this.ProcessManipulators();
		//        }
		//        else
		//        {
		//            this._LeftButtonDownNormalizedMercatorPoint = null;
		//        }
		//        base.OnTouchDown(e);
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when a finger moves on the screen while it is over the map.</summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnTouchMove(TouchEventArgs e)
		//{
		//    EventHandler<TouchEventArgs> touchMove = this.TouchMove;
		//    if (touchMove != null)
		//    {
		//        touchMove(this, e);
		//    }
		//    if (!e.Handled)
		//    {
		//        if (e.TouchDevice.Captured == this)
		//        {
		//            e.Handled = true;
		//            this.ProcessManipulators();
		//        }
		//        base.OnTouchMove(e);
		//    }
		//}

		///// <summary>Enables the ability to provide custom handling when a finger is raised off the screen while it is over the map.</summary>
		///// <param name="e">The event arguments.</param>
		//protected override void OnTouchUp(TouchEventArgs e)
		//{
		//    EventHandler<TouchEventArgs> touchUp = this.TouchUp;
		//    if (touchUp != null)
		//    {
		//        touchUp(this, e);
		//    }
		//    this._lastTouchTick = 0L;
		//    if (!e.Handled)
		//    {
		//        e.Handled = true;
		//        e.TouchDevice.Capture(null);
		//        if (!base.IsMouseCaptured && !base.AreAnyTouchesCaptured)
		//        {
		//            this._LeftButtonDownNormalizedMercatorPoint = null;
		//        }
		//        this.ProcessManipulators();
		//        base.OnTouchUp(e);
		//    }
		//}

		//private void UpdateVelocity(Point position)
		//{
		//    DateTime now = DateTime.Now;
		//    int milliseconds = (now - this._lastMouseMoveTime).Milliseconds;
		//    this._lastMouseMoveTime = now;
		//    if (milliseconds > 0)
		//    {
		//        this._velocity.X = 0.3 * this._velocity.X + 0.7 * (position.X - this._previousMousePoint.X) / (double)milliseconds;
		//        this._velocity.Y = 0.3 * this._velocity.Y + 0.7 * (position.Y - this._previousMousePoint.Y) / (double)milliseconds;
		//    }
		//}

		//private void ManipulationProcessor_Started(object sender, Manipulation2DStartedEventArgs e)
		//{
		//    this.StopInertia();
		//    this._touchTapPointCurrent = new Point?(new Point((double)e.OriginX, (double)e.OriginY));
		//    this._touchTapTimeCurrent = new DateTime?(DateTime.Now);
		//}

		//private void ManipulationProcessor_Delta(object sender, Manipulation2DDeltaEventArgs e)
		//{
		//    if (!this.UseInertia && this._InertiaProcessor.IsRunning)
		//    {
		//        return;
		//    }
		//    if (this.storedManipulation.Accumulate(e))
		//    {
		//        return;
		//    }
		//    this.ProcessStoredManipulation();
		//}

		//private void ProcessStoredManipulation()
		//{
		//    if (!this.storedManipulation.HasValuesStored())
		//    {
		//        return;
		//    }
		//    double heading = base.TargetHeading + (double)this.storedManipulation.Rotation * 57.295779513082323;
		//    AnimationLevel animationLevel = base.AnimationLevel;
		//    base.AnimationLevel = AnimationLevel.None;
		//    if (this._touchCount > 1)
		//    {
		//        base.ZoomAndRotateOrigin = new Point?(new Point((double)this.storedManipulation.OriginX, (double)this.storedManipulation.OriginY));
		//        if (this.storedManipulation.ScaleX != 1f || this.storedManipulation.ScaleY != 1f)
		//        {
		//            this._cancelDoubleTap();
		//            double d = (double)Math.Max(this.storedManipulation.ScaleX, this.storedManipulation.ScaleY);
		//            this.ZoomAboutViewportPoint(Math.Log(d) / Math.Log(2.0), base.ZoomAndRotateOrigin.Value);
		//        }
		//        if (this.storedManipulation.Rotation != 0f)
		//        {
		//            this._cancelDoubleTap();
		//            base.ViewBeingSetByUserInput = true;
		//            base.SetView(base.TargetZoomLevel, heading);
		//            base.ViewBeingSetByUserInput = false;
		//        }
		//    }
		//    if (this.storedManipulation.TranslationX != 0f || this.storedManipulation.TranslationY != 0f)
		//    {
		//        if (Math.Abs(this.storedManipulation.TranslationX) > (float)this._doubleTapThreshold || Math.Abs(this.storedManipulation.TranslationY) > (float)this._doubleTapThreshold)
		//        {
		//            this._cancelDoubleTap();
		//        }
		//        Point centerNormalizedMercator = base.TransformViewportToNormalizedMercator_Target(new Point(base.ActualWidth / 2.0 - (double)this.storedManipulation.TranslationX, base.ActualHeight / 2.0 - (double)this.storedManipulation.TranslationY));
		//        base.ZoomAndRotateOrigin = null;
		//        base.ViewBeingSetByUserInput = true;
		//        base.SetView(centerNormalizedMercator, base.TargetZoomLevel, heading);
		//        base.ViewBeingSetByUserInput = false;
		//    }
		//    base.AnimationLevel = animationLevel;
		//    this.storedManipulation.Reset();
		//}

		//private void ManipulationProcessor_Completed(object sender, Manipulation2DCompletedEventArgs e)
		//{
		//    this.ProcessStoredManipulation();
		//    bool flag = false;
		//    if (this._touchTapPointCurrent.HasValue)
		//    {
		//        if (this._touchTapPointPrevious.HasValue && (DateTime.Now - this._touchTapTimePrevious.Value).Milliseconds < this._doubleTapDelay && Math.Abs(this._touchTapPointCurrent.Value.X - this._touchTapPointPrevious.Value.X) < (double)this._doubleTapThreshold && Math.Abs(this._touchTapPointCurrent.Value.Y - this._touchTapPointPrevious.Value.Y) < (double)this._doubleTapThreshold)
		//        {
		//            this.ZoomAboutViewportPoint(1.0, this._touchTapPointCurrent.Value);
		//            this._cancelDoubleTap();
		//            flag = true;
		//        }
		//        else
		//        {
		//            this._touchTapPointPrevious = this._touchTapPointCurrent;
		//            this._touchTapTimePrevious = this._touchTapTimeCurrent;
		//        }
		//    }
		//    if (this.UseInertia && !flag && !this._InertiaTimer.IsEnabled)
		//    {
		//        this._InertiaProcessor.InitialOriginX = e.OriginX;
		//        this._InertiaProcessor.InitialOriginY = e.OriginY;
		//        this._InertiaProcessor.TranslationBehavior.InitialVelocityX = e.Velocities.LinearVelocityX;
		//        this._InertiaProcessor.TranslationBehavior.InitialVelocityY = e.Velocities.LinearVelocityY;
		//        this._InertiaProcessor.Process(DateTime.UtcNow.Ticks);
		//        this._InertiaTimer.Start();
		//    }
		//}

		//private void _cancelDoubleTap()
		//{
		//    this._touchTapPointCurrent = null;
		//    this._touchTapPointPrevious = null;
		//}

		//private void ProcessManipulators()
		//{
		//    long ticks = DateTime.UtcNow.Ticks;
		//    if (ticks - this._lastTouchTick > 50000L)
		//    {
		//        List<Manipulator2D> list = new List<Manipulator2D>();
		//        this._touchCount = 0;
		//        foreach (TouchDevice current in base.TouchesCaptured)
		//        {
		//            Point position = current.GetTouchPoint(this).Position;
		//            list.Add(new Manipulator2D(current.Id, (float)position.X, (float)position.Y));
		//            this._touchCount++;
		//        }
		//        this._ManipulationProcessor.ProcessManipulators(DateTime.UtcNow.Ticks, list);
		//        this._lastTouchTick = ticks;
		//    }
		//}

		//private void StopInertia()
		//{
		//    if (this._InertiaProcessor.IsRunning)
		//    {
		//        this._InertiaProcessor.Complete(DateTime.UtcNow.Ticks);
		//    }
		//    this._velocity = default(Point);
		//    this._InertiaTimer.Stop();
		//}

		//private void ZoomAboutViewportPoint(double zoomLevelIncrement, Point zoomTargetInViewport)
		//{
		//    base.ZoomAndRotateOrigin = new Point?(zoomTargetInViewport);
		//    base.ViewBeingSetByUserInput = true;
		//    base.SetView(base.TargetZoomLevel + zoomLevelIncrement, base.TargetHeading);
		//    base.ViewBeingSetByUserInput = false;
		//}

		///// <summary>Builds the visual tree for the <see cref="T:Microsoft.Maps.MapControl.WPF.Map"></see> when a new template is applied.</summary>
		//public override void OnApplyTemplate()
		//{
		//    base.OnApplyTemplate();
		//    if (!this.firstFrameDone)
		//    {
		//        this.firstFrameDone = true;
		//        this.OnFirstFrame();
		//    }
		//}

		//protected void OnFirstFrame()
		//{
		//    this.Log(false);
		//}

		///// <summary>Enables the ability to provide custom handling when the credentials provider changes.</summary>
		///// <param name="eventArgs">The event arguments.</param>
		//protected override void OnCredentialsProviderChanged(DependencyPropertyChangedEventArgs eventArgs)
		//{
		//    INotifyPropertyChanged notifyPropertyChanged = eventArgs.OldValue as INotifyPropertyChanged;
		//    if (notifyPropertyChanged != null && this._weakMapCredentials != null)
		//    {
		//        this._weakMapCredentials.Detach();
		//        this._weakMapCredentials = null;
		//    }
		//    INotifyPropertyChanged newCredentials = eventArgs.NewValue as INotifyPropertyChanged;
		//    if (newCredentials != null)
		//    {
		//        this._weakMapCredentials = new WeakEventListener<Map, object, PropertyChangedEventArgs>(this);
		//        this._weakMapCredentials.OnEventAction = delegate (Map instance, object source, PropertyChangedEventArgs leventArgs)
		//        {
		//            instance.Credentials_PropertyChanged(source, leventArgs);
		//        };
		//        this._weakMapCredentials.OnDetachAction = delegate (WeakEventListener<Map, object, PropertyChangedEventArgs> weakEventListener)
		//        {
		//            newCredentials.PropertyChanged -= new PropertyChangedEventHandler(weakEventListener.OnEvent);
		//        };
		//        newCredentials.PropertyChanged += new PropertyChangedEventHandler(this._weakMapCredentials.OnEvent);
		//    }
		//    Map.EndSession(eventArgs.OldValue as CredentialsProvider);
		//    this.Log(true);
		//}

		private void AsynchronousConfigurationLoaded(MapConfigurationSection config, object userState)
		{
			//this.getConfigurationDone = true;
			//if (config == null)
			//{
			//	this.ShowLoadingError(new ConfigurationNotLoadedException());
			//}
			//else
			//{
			//	this.logServiceUriFormat = (Map.IsInDesignMode ? null : config["WPFLOGGING"]);
			//	if (!string.IsNullOrEmpty(this.logServiceUriFormat))
			//	{
			//		this.logServiceUriFormat = this.logServiceUriFormat.Replace("{UriScheme}", Map.UriScheme);
			//		this.logServiceUriFormat = this.logServiceUriFormat.Replace("{entryPoint}", "{0}");
			//		this.logServiceUriFormat = this.logServiceUriFormat.Replace("{authKey}", "{1}");
			//		this.logServiceUriFormat = this.logServiceUriFormat.Replace("{productBuildVersion}", "{2}");
			//		this.logServiceUriFormat = this.logServiceUriFormat.Replace("{session}", "{3}");
			//		this.logServiceUriFormat = this.logServiceUriFormat.Replace("{culture}", "{4}");
			//	}
			//}
			//this.Log(false);
		}

		//private void Log(bool hasCredentialsProviderChanged)
		//{
		//    if (!this.settingDefaultCredentials)
		//    {
		//        if (this.getConfigurationDone && string.IsNullOrEmpty(this.logServiceUriFormat))
		//        {
		//            Map.EndSession(base.CredentialsProvider);
		//            return;
		//        }
		//        if (hasCredentialsProviderChanged)
		//        {
		//            Map.StartSession(base.CredentialsProvider as ApplicationIdCredentialsProvider);
		//        }
		//        if (this.startedSession)
		//        {
		//            if (hasCredentialsProviderChanged)
		//            {
		//                this.Log("2");
		//                return;
		//            }
		//        }
		//        else if (this.getConfigurationDone && this.firstFrameDone)
		//        {
		//            this.startedSession = true;
		//            this.Log("0");
		//        }
		//    }
		//}

		//private void Log(string entry)
		//{
		//    if (base.CredentialsProvider == null)
		//    {
		//        base.Dispatcher.BeginInvoke(new Action(delegate
		//        {
		//            this.OnCredentialsError();
		//        }), new object[0]);
		//        return;
		//    }
		//    ApplicationIdCredentialsProvider applicationIdCredentialsProvider = base.CredentialsProvider as ApplicationIdCredentialsProvider;
		//    if (applicationIdCredentialsProvider != null)
		//    {
		//        Credentials credentials2 = new Credentials();
		//        credentials2.ApplicationId = applicationIdCredentialsProvider.ApplicationId;
		//        this.Log(entry, base.CredentialsProvider, credentials2);
		//        return;
		//    }
		//    base.CredentialsProvider.GetCredentials(delegate (Credentials credentials)
		//    {
		//        this.Log(entry, this.CredentialsProvider, credentials);
		//    });
		//}

		//private void Log(string entry, CredentialsProvider credentialsProvider, Credentials credentials)
		//{
		//    string logRequestString = string.Format(CultureInfo.InvariantCulture, this.logServiceUriFormat, new object[]
		//    {
		//        entry,
		//        credentials.ToString(),
		//        Map.version,
		//        Guid.Empty,
		//        base.Culture
		//    });
		//    if (this._logTimer != null)
		//    {
		//        this._logTimer.Dispose();
		//    }
		//    int num = (Map.LoggingDelay > 0) ? Map.LoggingDelay : 1;
		//    this._logTimer = new Timer((double)num);
		//    this._logTimer.AutoReset = false;
		//    this._logTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
		//    {
		//        this._logTimer.Dispose();
		//        this._logTimer = null;
		//        try
		//        {
		//            using (WebClient webClient = new WebClient())
		//            {
		//                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.LogResponse);
		//                webClient.DownloadStringAsync(new Uri(logRequestString, UriKind.Absolute), credentialsProvider);
		//            }
		//        }
		//        catch (WebException)
		//        {
		//            Map.EndSession(credentialsProvider);
		//            this.OnCredentialsError();
		//        }
		//        catch (NotSupportedException)
		//        {
		//            Map.EndSession(credentialsProvider);
		//        }
		//    };
		//    this._logTimer.Start();
		//}

		//private void LogResponse(object sender, DownloadStringCompletedEventArgs e)
		//{
		//    bool credentialsValid = e.Error == null;
		//    ApplicationIdCredentialsProvider appId = e.UserState as ApplicationIdCredentialsProvider;
		//    string serverSessionId = null;
		//    if (appId != null && credentialsValid)
		//    {
		//        try
		//        {
		//            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Session));
		//            Session session;
		//            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(e.Result)))
		//            {
		//                session = (dataContractJsonSerializer.ReadObject(memoryStream) as Session);
		//            }
		//            if (session != null && !string.IsNullOrEmpty(session.SessionId))
		//            {
		//                serverSessionId = session.SessionId;
		//            }
		//        }
		//        catch (SerializationException)
		//        {
		//        }
		//    }
		//    base.Dispatcher.BeginInvoke(new Action(delegate
		//    {
		//        if (appId != null)
		//        {
		//            if (credentialsValid && serverSessionId != null)
		//            {
		//                appId.SetSessionId(serverSessionId);
		//            }
		//            else
		//            {
		//                appId.EndSession();
		//            }
		//        }
		//        if (credentialsValid)
		//        {
		//            this.OnCredentialsValid();
		//            return;
		//        }
		//        this.OnCredentialsError();
		//    }), new object[0]);
		//}

		//private void OnCredentialsError()
		//{
		//    this.ShowLoadingError(new CredentialsInvalidException());
		//}

		//private void OnCredentialsValid()
		//{
		//    CredentialsInvalidException ex = this.LoadingException as CredentialsInvalidException;
		//    if (ex != null && this.loadingErrorMessage != null)
		//    {
		//        base.Children.Remove(this.loadingErrorMessage);
		//        this.loadingErrorMessage = null;
		//    }
		//}

		//private void Credentials_PropertyChanged(object sender, PropertyChangedEventArgs e)
		//{
		//    this.Log(true);
		//}

		//private void ShowLoadingError(Exception e)
		//{
		//    if (this.LoadingException == null && e != null)
		//    {
		//        this.LoadingException = e;
		//        if (this.loadingErrorEvent != null)
		//        {
		//            this.loadingErrorEvent(this, new LoadingErrorEventArgs(this.LoadingException));
		//        }
		//    }
		//    if (this.loadingErrorMessage == null)
		//    {
		//        this.loadingErrorMessage = new LoadingErrorMessage();
		//        base.Children.Add(this.loadingErrorMessage);
		//    }
		//    if (e is ConfigurationNotLoadedException)
		//    {
		//        this.loadingErrorMessage.SetConfigurationError(base.Culture);
		//        return;
		//    }
		//    if (e is CredentialsInvalidException)
		//    {
		//        this.loadingErrorMessage.SetCredentialsError(base.Culture);
		//    }
		//}

		//private static void StartSession(CredentialsProvider credentialsProvider)
		//{
		//    ApplicationIdCredentialsProvider applicationIdCredentialsProvider = credentialsProvider as ApplicationIdCredentialsProvider;
		//    if (applicationIdCredentialsProvider != null)
		//    {
		//        applicationIdCredentialsProvider.StartSession();
		//    }
		//}

		//private static void EndSession(CredentialsProvider credentialsProvider)
		//{
		//    ApplicationIdCredentialsProvider applicationIdCredentialsProvider = credentialsProvider as ApplicationIdCredentialsProvider;
		//    if (applicationIdCredentialsProvider != null)
		//    {
		//        applicationIdCredentialsProvider.EndSession();
		//    }
		//}
	}
}
