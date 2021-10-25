using System;
using UnityEngine;

public class GGMath
{
	public static Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
	{
		t = Mathf.Clamp01(t);
		float num = 1f - t;
		float num2 = num * num;
		float d = num2 * num;
		float num3 = t * t;
		float d2 = num3 * t;
		return d * p0 + 3f * num2 * t * p1 + 3f * num * num3 * p2 + d2 * p3;
	}

	[Serializable]
	public struct FloatRange
	{
		public float Clamp(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		public float min;

		public float max;
	}

	[Serializable]
	public struct CssCubicBezier
	{
		public CssCubicBezier(float x1, float y1, float x2, float y2)
		{
			this.p1 = new Vector2(x1, y1);
			this.p2 = new Vector2(x2, y2);
		}

		public float Eval(float t)
		{
			return GGMath.CubicBezier(Vector2.zero, this.p1, this.p2, Vector2.one, t).y;
		}

		public Vector2 p1;

		public Vector2 p2;
	}

	public static class Ease
	{
		public static GGMath.CssCubicBezier Linear = new GGMath.CssCubicBezier(0.5f, 0.5f, 0.5f, 0.5f);

		public static GGMath.CssCubicBezier EaseInSine = new GGMath.CssCubicBezier(0.47f, 0f, 0.745f, 0.715f);

		public static GGMath.CssCubicBezier EaseOutSine = new GGMath.CssCubicBezier(0.39f, 0.575f, 0.565f, 1f);

		public static GGMath.CssCubicBezier EaseInOutSine = new GGMath.CssCubicBezier(0.445f, 0.05f, 0.55f, 0.95f);

		public static GGMath.CssCubicBezier EaseInCubic = new GGMath.CssCubicBezier(0.55f, 0.055f, 0.675f, 0.19f);

		public static GGMath.CssCubicBezier EaseOutCubic = new GGMath.CssCubicBezier(0.215f, 0.61f, 0.355f, 1f);

		public static GGMath.CssCubicBezier EaseInOutCubic = new GGMath.CssCubicBezier(0.645f, 0.045f, 0.355f, 1f);
	}
}
