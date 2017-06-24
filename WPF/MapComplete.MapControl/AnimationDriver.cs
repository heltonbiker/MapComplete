using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace Microsoft.Maps.MapControl.WPF
{
	internal class AnimationDriver : DependencyObject
	{
		public static readonly DependencyProperty AnimationProgressProperty = DependencyProperty.Register("AnimationProgress", typeof(double), typeof(AnimationDriver));

		private Storyboard storyboard;

		private bool isAnimating;

		public event EventHandler AnimationProgressChanged;

		public event EventHandler AnimationStopped;

		public event EventHandler AnimationCompleted;

		public double AnimationProgress
		{
			get
			{
				return (double)base.GetValue(AnimationDriver.AnimationProgressProperty);
			}
		}

		public bool IsAnimating
		{
			get
			{
				return this.isAnimating;
			}
		}

		public AnimationDriver()
		{
			DependencyPropertyDescriptor.FromProperty(AnimationDriver.AnimationProgressProperty, typeof(AnimationDriver)).AddValueChanged(this, new EventHandler(this.OnAnimationProgressChanged));
			DoubleAnimation doubleAnimation = new DoubleAnimation
			{
				From = new double?(0.0),
				To = new double?(1.0)
			};
			Storyboard.SetTarget(doubleAnimation, this);
			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(AnimationProgress)", new object[0]));
			this.storyboard = new Storyboard
			{
				Children =
				{
					doubleAnimation
				}
			};
			this.storyboard.Completed += new EventHandler(this.StoryboardCompleted);
		}

		public void Start(Duration duration)
		{
			this.storyboard.Stop();
			this.storyboard.Duration = duration;
			this.storyboard.Children[0].Duration = duration;
			this.storyboard.Begin();
			this.isAnimating = true;
		}

		public void Stop()
		{
			this.storyboard.Stop();
			this.isAnimating = false;
			if (this.AnimationStopped != null)
			{
				this.AnimationStopped(this, EventArgs.Empty);
			}
		}

		private void OnAnimationProgressChanged(object sender, EventArgs e)
		{
			if (this.isAnimating && this.AnimationProgressChanged != null)
			{
				this.AnimationProgressChanged(this, EventArgs.Empty);
			}
		}

		private void StoryboardCompleted(object sender, EventArgs e)
		{
			base.SetValue(AnimationDriver.AnimationProgressProperty, 1.0);
			if (this.AnimationProgressChanged != null)
			{
				this.AnimationProgressChanged(this, EventArgs.Empty);
			}
			if (this.AnimationCompleted != null)
			{
				this.AnimationCompleted(this, EventArgs.Empty);
			}
			this.isAnimating = false;
		}
	}
}
