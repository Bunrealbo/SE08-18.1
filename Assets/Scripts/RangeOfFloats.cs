using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[Serializable]
	public struct RangeOfFloats
	{
		public float Random()
		{
			return UnityEngine.Random.Range(this.Minimum, this.Maximum);
		}

		public float Random(System.Random r)
		{
			return this.Minimum + (float)r.NextDouble() * (this.Maximum - this.Minimum);
		}

		public float Minimum;

		public float Maximum;
	}
}
