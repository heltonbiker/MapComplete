using System;
using System.Globalization;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Microsoft.Maps.MapExtras
{
	internal class TileRenderable
	{
		private Canvas layerCanvas;

		private FrameworkElement element;

		private Rect clip;

		private TimeSpan? fadeInDuration;

		private bool configurationDirty;

		private bool allowHardwareAcceleration;

		private bool useHardwareAcceleration;

		private int zIndex;

		private double targetOpacity;

		private DateTime lastFrameTime;

		private bool registeredForCompositionTargetRendering;

		private Canvas elementCanvas;

		public event EventHandler BecameFullyOpaque;

		public TileId TileId
		{
			get;
			protected set;
		}

		public bool AllowHardwareAcceleration
		{
			get
			{
				return this.allowHardwareAcceleration;
			}
			set
			{
				if (this.allowHardwareAcceleration != value)
				{
					this.allowHardwareAcceleration = value;
					this.useHardwareAcceleration = this.allowHardwareAcceleration;
					this.configurationDirty = true;
				}
			}
		}

		public bool IsVisible
		{
			get
			{
				return this.layerCanvas != null;
			}
		}

		public Canvas LayerCanvas
		{
			get
			{
				return this.layerCanvas;
			}
			internal set
			{
				if (this.layerCanvas != value)
				{
					FrameworkElement frameworkElement = (this.elementCanvas != null) ? this.elementCanvas : this.element;
					if (this.layerCanvas == null && value != null)
					{
						this.EnsureRegisteredForCompositionTargetRendering(true);
					}
					else if (this.layerCanvas != null && value == null)
					{
						this.EnsureRegisteredForCompositionTargetRendering(false);
					}
					if (this.layerCanvas != null)
					{
						this.layerCanvas.Children.Remove(frameworkElement);
					}
					this.layerCanvas = value;
					if (this.layerCanvas != null)
					{
						this.layerCanvas.Children.Add(frameworkElement);
					}
					if (this.elementCanvas != null)
					{
						if (this.layerCanvas == null)
						{
							this.elementCanvas.Children.Remove(this.element);
							return;
						}
						if (this.element.Parent == null)
						{
							this.elementCanvas.Children.Add(this.element);
						}
					}
				}
			}
		}

		public double TargetOpacity
		{
			get
			{
				return this.targetOpacity;
			}
			set
			{
				if (value < 0.0 || value > 1.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.targetOpacity != value)
				{
					this.targetOpacity = value;
					if (!this.fadeInDuration.HasValue)
					{
						this.Opacity = this.targetOpacity;
					}
					if (this.Opacity != this.targetOpacity && this.LayerCanvas != null)
					{
						this.EnsureRegisteredForCompositionTargetRendering(true);
					}
				}
			}
		}

		public double Opacity
		{
			get
			{
				return this.element.Opacity;
			}
			set
			{
				if (value < 0.0 || value > 1.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.element.Opacity != this.targetOpacity && !this.fadeInDuration.HasValue)
				{
					this.element.Opacity = this.targetOpacity;
				}
				else
				{
					this.element.Opacity = value;
				}
				if (this.element.Opacity == 1.0 && this.BecameFullyOpaque != null)
				{
					this.BecameFullyOpaque(this, EventArgs.Empty);
				}
			}
		}

		public int ZIndex
		{
			get
			{
				return this.zIndex;
			}
			set
			{
				if (this.zIndex != value)
				{
					this.zIndex = value;
					this.configurationDirty = true;
				}
			}
		}

		public Rect Clip
		{
			get
			{
				return this.clip;
			}
			set
			{
				this.clip = value;
				this.configurationDirty = true;
			}
		}

		public FrameworkElement Element
		{
			get
			{
				return this.element;
			}
			protected set
			{
				this.element = value;
				AutomationProperties.SetAutomationId(this.element, string.Format(CultureInfo.InvariantCulture, "{0}_{1}x{2}", new object[]
				{
					this.TileId.LevelOfDetail,
					this.TileId.X,
					this.TileId.Y
				}));
			}
		}

		public TileRenderable(TileId tileId, FrameworkElement element, TimeSpan? fadeInDuration)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (element.Parent != null)
			{
				throw new ArgumentException("element.Parent not null");
			}
			this.TileId = tileId;
			this.Element = element;
			Canvas.SetLeft(element, 0.0);
			Canvas.SetTop(element, 0.0);
			this.fadeInDuration = fadeInDuration;
			this.Opacity = (this.TargetOpacity = 0.0);
			this.configurationDirty = true;
		}

		public void DetachFromElement()
		{
			this.EnsureRegisteredForCompositionTargetRendering(false);
			this.element = null;
		}

		public virtual void NoLongerRendering()
		{
		}

		public virtual void Render(Point2D viewportSize, ref Matrix3D tileToViewport, double preciseRenderLod)
		{
			this.EnsureConfiguration();
			double num = 0.01;
			double num2 = Math.Max(viewportSize.X, viewportSize.Y);
			double num3 = 1.0 - num2 / (num2 + num);
			double num4 = num3 / Math.Max(this.element.ActualWidth, this.element.ActualHeight);
			if (Math.Abs(tileToViewport.M14 / tileToViewport.M44) + Math.Abs(tileToViewport.M24 / tileToViewport.M44) < num4)
			{
				MatrixTransform matrixTransform = (MatrixTransform)this.element.RenderTransform;
				if (matrixTransform == null || matrixTransform.Matrix.IsIdentity)
				{
					matrixTransform = new MatrixTransform();
					this.element.RenderTransform = matrixTransform;
				}
				Matrix matrix = new Matrix(tileToViewport.M11 / tileToViewport.M44, tileToViewport.M12 / tileToViewport.M44, tileToViewport.M21 / tileToViewport.M44, tileToViewport.M22 / tileToViewport.M44, tileToViewport.OffsetX / tileToViewport.M44, tileToViewport.OffsetY / tileToViewport.M44);
				if ((Math.Abs(Math.Abs(matrix.M11) - 1.0) < 0.0001 && Math.Abs(matrix.M21) < 0.0001 && Math.Abs(Math.Abs(matrix.M22) - 1.0) < 0.0001 && Math.Abs(matrix.M12) < 0.0001) || (Math.Abs(Math.Abs(matrix.M21) - 1.0) < 0.0001 && Math.Abs(matrix.M11) < 0.0001 && Math.Abs(Math.Abs(matrix.M12) - 1.0) < 0.0001 && Math.Abs(matrix.M22) < 0.0001))
				{
					matrix.M11 = Math.Round(matrix.M11);
					matrix.M12 = Math.Round(matrix.M12);
					matrix.M21 = Math.Round(matrix.M21);
					matrix.M22 = Math.Round(matrix.M22);
					matrix.OffsetX = Math.Round(matrix.OffsetX);
					matrix.OffsetY = Math.Round(matrix.OffsetY);
				}
				matrixTransform.Matrix = matrix;
				if (!this.useHardwareAcceleration)
				{
					this.SetPostProjectionClip(viewportSize, ref tileToViewport);
				}
				return;
			}
			throw new InvalidOperationException("WPF implementation doesn't support 3D.");
		}

		private void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan timeSpan = utcNow - this.lastFrameTime;
			this.lastFrameTime = utcNow;
			if (this.element != null)
			{
				if (this.Opacity != this.targetOpacity && timeSpan.Ticks > 0L)
				{
					double num = this.targetOpacity - this.Opacity;
					double num2 = (double)timeSpan.Ticks / (double)this.fadeInDuration.Value.Ticks;
					if (Math.Abs(num) < num2)
					{
						this.Opacity = this.targetOpacity;
					}
					else
					{
						this.Opacity += ((num > 0.0) ? num2 : (-num2));
					}
				}
				if (this.Opacity == this.targetOpacity)
				{
					this.EnsureRegisteredForCompositionTargetRendering(false);
				}
			}
		}

		private void EnsureRegisteredForCompositionTargetRendering(bool registered)
		{
			if (registered)
			{
				if (!this.registeredForCompositionTargetRendering)
				{
					this.lastFrameTime = DateTime.UtcNow;
					CompositionTarget.Rendering += new EventHandler(this.CompositionTarget_Rendering);
					this.registeredForCompositionTargetRendering = true;
					return;
				}
			}
			else if (this.registeredForCompositionTargetRendering)
			{
				CompositionTarget.Rendering -= new EventHandler(this.CompositionTarget_Rendering);
				this.registeredForCompositionTargetRendering = false;
			}
		}

		private void EnsureConfiguration()
		{
			if (this.configurationDirty)
			{
				if (this.useHardwareAcceleration)
				{
					if (this.elementCanvas != null)
					{
						Canvas canvas = this.layerCanvas;
						this.LayerCanvas = null;
						this.elementCanvas = null;
						this.LayerCanvas = canvas;
					}
				}
				else if (this.elementCanvas == null)
				{
					Canvas canvas2 = this.layerCanvas;
					this.LayerCanvas = null;
					this.elementCanvas = new Canvas
					{
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch,
						IsHitTestVisible = false,
						Tag = "ElementCanvas"
					};
					Canvas.SetLeft(this.elementCanvas, 0.0);
					Canvas.SetTop(this.elementCanvas, 0.0);
					this.LayerCanvas = canvas2;
				}
				if (this.useHardwareAcceleration)
				{
					if (this.element.Width <= 0.0 || double.IsNaN(this.element.Width) || this.element.Height <= 0.0 || double.IsNaN(this.element.Height))
					{
						throw new ArgumentException("Element size must be set and positive.");
					}
					RectangleGeometry rectangleGeometry = this.element.Clip as RectangleGeometry;
					if (Math.Abs(this.clip.Left) > 1E-06 || Math.Abs(this.clip.Top) > 1E-06 || Math.Abs(this.clip.Width - this.element.Width) > 1E-06 || Math.Abs(this.clip.Height - this.element.Height) > 1E-06)
					{
						if (rectangleGeometry == null || rectangleGeometry.Rect != this.clip)
						{
							this.element.Clip = new RectangleGeometry
							{
								Rect = this.clip
							};
						}
					}
					else if (this.element.Clip != null)
					{
						this.element.Clip = null;
					}
				}
				else if (this.element.Clip != null)
				{
					this.element.Clip = null;
				}
				if (this.useHardwareAcceleration)
				{
					if (this.element.CacheMode == null)
					{
						this.element.CacheMode = new BitmapCache();
					}
				}
				else if (this.element.CacheMode != null)
				{
					this.element.CacheMode = null;
				}
				if (this.useHardwareAcceleration)
				{
					if (Panel.GetZIndex(this.element) != this.zIndex)
					{
						Panel.SetZIndex(this.element, this.zIndex);
					}
				}
				else if (Panel.GetZIndex(this.elementCanvas) != this.zIndex)
				{
					Panel.SetZIndex(this.elementCanvas, this.zIndex);
				}
				this.configurationDirty = false;
			}
		}

		private void SetPostProjectionClip(Point2D viewportSize, ref Matrix3D tileToViewportTransform)
		{
			//Point4D[] array = new Point4D[]
			//{
			//	VectorMath.Transform(tileToViewportTransform, new Point4D(this.clip.Left, this.clip.Top, 0.0, 1.0)),
			//	VectorMath.Transform(tileToViewportTransform, new Point4D(this.clip.Right, this.clip.Top, 0.0, 1.0)),
			//	VectorMath.Transform(tileToViewportTransform, new Point4D(this.clip.Right, this.clip.Bottom, 0.0, 1.0)),
			//	VectorMath.Transform(tileToViewportTransform, new Point4D(this.clip.Left, this.clip.Bottom, 0.0, 1.0))
			//};
			//RectangularSolid clipBounds = new RectangularSolid(0.0, 0.0, 0.0, viewportSize.X, viewportSize.Y, 1.0);
			//Point4D[] array2 = new Point4D[array.Length + 6];
			//Point4D[] tempVertexBuffer = new Point4D[array.Length + 6];
			//int num;
			//VectorMath.ClipConvexPolygon(clipBounds, array, null, 4, array2, null, out num, tempVertexBuffer, null);
			//if (num > 0)
			//{
			//	for (int i = 0; i < num; i++)
			//	{
			//		Point4D[] expr_193_cp_0 = array2;
			//		int expr_193_cp_1 = i;
			//		expr_193_cp_0[expr_193_cp_1].X = expr_193_cp_0[expr_193_cp_1].X / array2[i].W;
			//		Point4D[] expr_1B4_cp_0 = array2;
			//		int expr_1B4_cp_1 = i;
			//		expr_1B4_cp_0[expr_1B4_cp_1].Y = expr_1B4_cp_0[expr_1B4_cp_1].Y / array2[i].W;
			//	}
			//	PathGeometry pathGeometry = new PathGeometry();
			//	PathFigure pathFigure = new PathFigure
			//	{
			//		StartPoint = new Point(array2[0].X, array2[0].Y),
			//		Segments = new PathSegmentCollection()
			//	};
			//	PolyLineSegment polyLineSegment = new PolyLineSegment();
			//	for (int j = 1; j < num; j++)
			//	{
			//		polyLineSegment.Points.Add(new Point(array2[j].X, array2[j].Y));
			//	}
			//	polyLineSegment.Points.Add(new Point(array2[0].X, array2[0].Y));
			//	pathFigure.Segments.Add(polyLineSegment);
			//	pathGeometry.Figures.Add(pathFigure);
			//	this.elementCanvas.Clip = pathGeometry;
			//}
		}
	}
}
