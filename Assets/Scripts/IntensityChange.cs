using System;

namespace GGMatch3
{
	[Serializable]
	public struct IntensityChange
	{
		public IntensityChange IntensityRange(float min, float max)
		{
			this.intensityRange.min = min;
			this.intensityRange.max = max;
			return this;
		}

		public float Intensity(float t)
		{
			return this.intensityRange.Lerp(this.easeCurve.Eval(t));
		}

		public IntensityChange Duration(float duration)
		{
			this.duration = duration;
			return this;
		}

		public IntensityChange Delay(float delay)
		{
			this.delay = delay;
			return this;
		}

		public IntensityChange EaseCurve(GGMath.CssCubicBezier easeCurve)
		{
			this.easeCurve = easeCurve;
			return this;
		}

		public float delay;

		public FloatRange intensityRange;

		public GGMath.CssCubicBezier easeCurve;

		public float duration;
	}
}
