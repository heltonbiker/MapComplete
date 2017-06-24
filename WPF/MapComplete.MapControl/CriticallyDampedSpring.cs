using System;

namespace Microsoft.Maps.MapControl.WPF
{
	internal class CriticallyDampedSpring
	{
		private const double Epsilon = 1E-06;

		private double omegaNought;

		private double currentValue;

		private double currentVelocity;

		private double targetValue;

		private double targetSetTime;

		private double A;

		private double B;

		public double CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		public double TargetValue
		{
			get
			{
				return this.targetValue;
			}
			set
			{
				if (Math.Abs(this.targetValue - value) > 1E-06)
				{
					this.Update();
					double num = this.currentValue - value;
					this.A = num;
					this.B = this.currentVelocity + num * this.omegaNought;
					this.targetValue = value;
					this.targetSetTime = this.CurrentTime;
				}
			}
		}

		private double CurrentTime
		{
			get
			{
				return (double)DateTime.Now.Ticks / 10000000.0;
			}
		}

		public CriticallyDampedSpring()
		{
			this.omegaNought = 20.0;
			this.Initialize();
		}

		public CriticallyDampedSpring(double springConstant, double mass)
		{
			if (springConstant <= 0.0 || mass <= 0.0)
			{
				throw new ArgumentException("mass and spring constant must be positive");
			}
			this.omegaNought = Math.Sqrt(springConstant / mass);
			this.Initialize();
		}

		public void SnapToValue(double value)
		{
			this.currentValue = value;
			this.currentVelocity = 0.0;
			this.targetValue = value;
			this.targetSetTime = this.CurrentTime;
		}

		public bool Update()
		{
			if (this.currentValue != this.targetValue)
			{
				double relativeTime = this.CurrentTime - this.targetSetTime;
				double num = this.CalculateCurrentValue(relativeTime);
				double num2 = this.CalculateCurrentVelocity(relativeTime);
				if (Math.Abs(num - this.targetValue) < 1E-06 && Math.Abs(this.currentVelocity) < 1E-06)
				{
					this.currentValue = this.targetValue;
					this.currentVelocity = 0.0;
				}
				else
				{
					this.currentValue = num;
					this.currentVelocity = num2;
				}
				return true;
			}
			return false;
		}

		private double CalculateCurrentValue(double relativeTime)
		{
			return (this.A + this.B * relativeTime) * Math.Exp(-this.omegaNought * relativeTime) + this.targetValue;
		}

		private double CalculateCurrentVelocity(double relativeTime)
		{
			return (this.B - (this.A + this.B * relativeTime) * this.omegaNought) * Math.Exp(-this.omegaNought * relativeTime);
		}

		private void Initialize()
		{
			this.currentValue = 0.0;
			this.currentVelocity = 0.0;
			this.targetValue = 0.0;
			this.targetSetTime = this.CurrentTime;
		}
	}
}
