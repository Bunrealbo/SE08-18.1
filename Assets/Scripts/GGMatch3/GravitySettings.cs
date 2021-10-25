using System;

namespace GGMatch3
{
	[Serializable]
	public class GravitySettings
	{
		public float gravity;

		public float maxVelocity;

		public FloatRange gravityRange;

		public FloatRange minVelocityRange;

		public float minTimeStoppedBeforeMatch = 0.1f;
	}
}
