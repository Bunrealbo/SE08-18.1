using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[Serializable]
	public class LightningLightParameters
	{
		public bool HasLight
		{
			get
			{
				return this.LightColor.a > 0f && this.LightIntensity >= 0.01f && this.LightPercent >= 1E-07f && this.LightRange > 0.01f;
			}
		}

		public LightRenderMode RenderMode;

		public Color LightColor = Color.white;

		public float LightPercent = 1E-06f;

		public float LightShadowPercent;

		public float LightIntensity = 0.5f;

		public float BounceIntensity;

		public float ShadowStrength = 1f;

		public float ShadowBias = 0.05f;

		public float ShadowNormalBias = 0.4f;

		public float LightRange;

		public LayerMask CullingMask = -1;

		public float OrthographicOffset;

		public float FadeInMultiplier = 1f;

		public float FadeFullyLitMultiplier = 1f;

		public float FadeOutMultiplier = 1f;
	}
}
