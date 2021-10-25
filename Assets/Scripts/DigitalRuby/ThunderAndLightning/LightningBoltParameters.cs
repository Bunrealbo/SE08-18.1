using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[Serializable]
	public sealed class LightningBoltParameters
	{
		static LightningBoltParameters()
		{
			string[] names = QualitySettings.names;
			for (int i = 0; i < names.Length; i++)
			{
				switch (i)
				{
				case 0:
					LightningBoltParameters.QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 3,
						MaximumLightPercent = 0f,
						MaximumShadowPercent = 0f
					};
					break;
				case 1:
					LightningBoltParameters.QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 4,
						MaximumLightPercent = 0f,
						MaximumShadowPercent = 0f
					};
					break;
				case 2:
					LightningBoltParameters.QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 5,
						MaximumLightPercent = 0.1f,
						MaximumShadowPercent = 0f
					};
					break;
				case 3:
					LightningBoltParameters.QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 5,
						MaximumLightPercent = 0.1f,
						MaximumShadowPercent = 0f
					};
					break;
				case 4:
					LightningBoltParameters.QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 6,
						MaximumLightPercent = 0.05f,
						MaximumShadowPercent = 0.1f
					};
					break;
				case 5:
					LightningBoltParameters.QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 7,
						MaximumLightPercent = 0.025f,
						MaximumShadowPercent = 0.05f
					};
					break;
				default:
					LightningBoltParameters.QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 8,
						MaximumLightPercent = 0.025f,
						MaximumShadowPercent = 0.05f
					};
					break;
				}
			}
		}

		public LightningBoltParameters()
		{
			this.random = (this.currentRandom = new System.Random(LightningBoltParameters.randomSeed++));
			this.Points = new List<Vector3>();
		}

		public int Generations
		{
			get
			{
				return this.generations;
			}
			set
			{
				int b = Mathf.Clamp(value, 1, 8);
				if (this.quality == LightningBoltQualitySetting.UseScript)
				{
					this.generations = b;
					return;
				}
				int qualityLevel = QualitySettings.GetQualityLevel();
				LightningQualityMaximum lightningQualityMaximum;
				if (LightningBoltParameters.QualityMaximums.TryGetValue(qualityLevel, out lightningQualityMaximum))
				{
					this.generations = Mathf.Min(lightningQualityMaximum.MaximumGenerations, b);
					return;
				}
				this.generations = b;
				UnityEngine.Debug.LogError("Unable to read lightning quality settings from level " + qualityLevel.ToString());
			}
		}

		public System.Random Random
		{
			get
			{
				return this.currentRandom;
			}
		}

		public System.Random RandomOverride
		{
			set
			{
				this.randomOverride = value;
				this.currentRandom = (this.randomOverride ?? this.random);
			}
		}

		public float GrowthMultiplier
		{
			get
			{
				return this.growthMultiplier;
			}
			set
			{
				this.growthMultiplier = Mathf.Clamp(value, 0f, 0.999f);
			}
		}

		public List<Vector3> Points
		{
			get
			{
				return this._003CPoints_003Ek__BackingField;
			}
			set
			{
				this._003CPoints_003Ek__BackingField = value;
			}
		}

		public float ForkMultiplier()
		{
			return (float)this.Random.NextDouble() * this.ForkLengthVariance + this.ForkLengthMultiplier;
		}

		public Vector3 ApplyVariance(Vector3 pos, Vector3 variance)
		{
			return new Vector3(pos.x + ((float)this.Random.NextDouble() * 2f - 1f) * variance.x, pos.y + ((float)this.Random.NextDouble() * 2f - 1f) * variance.y, pos.z + ((float)this.Random.NextDouble() * 2f - 1f) * variance.z);
		}

		public void Reset()
		{
			this.Start = (this.End = Vector3.zero);
			this.Generator = null;
			this.SmoothingFactor = 0;
			this.RandomOverride = null;
			this.CustomTransform = null;
			if (this.Points != null)
			{
				this.Points.Clear();
			}
		}

		public static LightningBoltParameters GetOrCreateParameters()
		{
			LightningBoltParameters result;
			if (LightningBoltParameters.cache.Count == 0)
			{
				result = new LightningBoltParameters();
			}
			else
			{
				int index = LightningBoltParameters.cache.Count - 1;
				result = LightningBoltParameters.cache[index];
				LightningBoltParameters.cache.RemoveAt(index);
			}
			return result;
		}

		public static void ReturnParametersToCache(LightningBoltParameters p)
		{
			if (!LightningBoltParameters.cache.Contains(p))
			{
				p.Reset();
				LightningBoltParameters.cache.Add(p);
			}
		}

		private static int randomSeed = Environment.TickCount;

		private static readonly List<LightningBoltParameters> cache = new List<LightningBoltParameters>();

		internal int generationWhereForksStop;

		internal int forkednessCalculated;

		internal LightningBoltQualitySetting quality;

		internal float delaySeconds;

		internal int maxLights;

		public static float Scale = 1f;

		public static readonly Dictionary<int, LightningQualityMaximum> QualityMaximums = new Dictionary<int, LightningQualityMaximum>();

		public LightningGenerator Generator;

		public Vector3 Start;

		public Vector3 End;

		public Vector3 StartVariance;

		public Vector3 EndVariance;

		public Action<LightningCustomTransformStateInfo> CustomTransform;

		private int generations;

		public float LifeTime;

		public float Delay;

		public RangeOfFloats DelayRange;

		public float ChaosFactor;

		public float ChaosFactorForks = -1f;

		public float TrunkWidth;

		public float EndWidthMultiplier = 0.5f;

		public float Intensity = 1f;

		public float GlowIntensity;

		public float GlowWidthMultiplier;

		public float Forkedness;

		public int GenerationWhereForksStopSubtractor = 5;

		public Color32 Color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private System.Random random;

		private System.Random currentRandom;

		private System.Random randomOverride;

		public float FadePercent = 0.15f;

		public float FadeInMultiplier = 1f;

		public float FadeFullyLitMultiplier = 1f;

		public float FadeOutMultiplier = 1f;

		private float growthMultiplier;

		public float ForkLengthMultiplier = 0.6f;

		public float ForkLengthVariance = 0.2f;

		public float ForkEndWidthMultiplier = 1f;

		public LightningLightParameters LightParameters;

		private List<Vector3> _003CPoints_003Ek__BackingField;

		public int SmoothingFactor;
	}
}
