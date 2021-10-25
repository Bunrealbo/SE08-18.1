using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public class PipeSettings
	{
		public Color GetColor(int index)
		{
			if (this.colorSettings.Count == 0)
			{
				return Color.white;
			}
			return this.colorSettings[index % this.colorSettings.Count].color;
		}

		public float gravity = 10f;

		public float maxVelocity;

		public float minVelocity;

		public float maxContinueVelocity;

		public float scale = 0.9f;

		public float orthoScale = 1f;

		public float pipeScale = 1.05f;

		public Vector3 offsetPosition;

		public AnimationCurve offsetCurve;

		public List<PipeSettings.ColorSettings> colorSettings = new List<PipeSettings.ColorSettings>();

		[Serializable]
		public class ColorSettings
		{
			public Color color;
		}
	}
}
