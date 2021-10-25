using System;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public struct FloatRange
	{
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float Random()
		{
			return UnityEngine.Random.Range(this.min, this.max);
		}

		public float Lerp(float t)
		{
			return Mathf.Lerp(this.min, this.max, t);
		}

		public float InverseLerp(float value)
		{
			return Mathf.InverseLerp(this.min, this.max, value);
		}

		public float min;

		public float max;
	}
}
