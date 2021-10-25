using System;
using UnityEngine;

namespace GGMatch3
{
	public class WobbleAnimation
	{
		public bool isActive
		{
			get
			{
				return this.settings != null && this.time < this.settings.duration;
			}
		}

		public Vector3 scale
		{
			get
			{
				if (this.settings == null)
				{
					return Vector3.one;
				}
				float num = Mathf.InverseLerp(0f, this.settings.duration, this.time);
				num = this.settings.scaleCurve.Evaluate(num);
				float num2 = Mathf.LerpUnclamped(this.settings.startScale, this.settings.endScale, num);
				if (this.settings.directDriveScaleCurve)
				{
					num2 = num;
				}
				return new Vector3(num2, num2, 1f);
			}
		}

		public void Init(WobbleAnimation.Settings settings, TransformBehaviour transform)
		{
			this.settings = settings;
			this.time = 0f;
			this.transform = transform;
		}

		public void Update(float deltaTime)
		{
			if (this.settings == null)
			{
				return;
			}
			if (this.time >= this.settings.duration)
			{
				return;
			}
			this.time += deltaTime;
			if (this.transform != null)
			{
				this.transform.wobbleLocalScale = this.scale;
			}
		}

		private float time;

		private WobbleAnimation.Settings settings;

		private TransformBehaviour transform;

		[Serializable]
		public class Settings
		{
			public float startScale = 0.8f;

			public float endScale = 1f;

			public bool directDriveScaleCurve;

			public AnimationCurve scaleCurve;

			public float duration;
		}
	}
}
