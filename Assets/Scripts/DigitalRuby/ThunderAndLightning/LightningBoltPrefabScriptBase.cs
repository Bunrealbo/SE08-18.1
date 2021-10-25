using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public abstract class LightningBoltPrefabScriptBase : LightningBoltScript
	{
		public System.Random RandomOverride
		{
			get
			{
				return this._003CRandomOverride_003Ek__BackingField;
			}
			set
			{
				this._003CRandomOverride_003Ek__BackingField = value;
			}
		}

		private void CalculateNextLightningTimestamp(float offset)
		{
			this.nextLightningTimestamp = ((this.IntervalRange.Minimum == this.IntervalRange.Maximum) ? this.IntervalRange.Minimum : (offset + this.IntervalRange.Random()));
		}

		private void CustomTransform(LightningCustomTransformStateInfo state)
		{
			if (this.CustomTransformHandler != null)
			{
				this.CustomTransformHandler.Invoke(state);
			}
		}

		private void CallLightning()
		{
			this.CallLightning(null, null);
		}

		private void CallLightning(Vector3? start, Vector3? end)
		{
			System.Random r = this.RandomOverride ?? this.random;
			int num = this.CountRange.Random(r);
			for (int i = 0; i < num; i++)
			{
				LightningBoltParameters lightningBoltParameters = base.CreateParameters();
				if (this.CountProbabilityModifier >= 0.9999f || i == 0 || (float)lightningBoltParameters.Random.NextDouble() <= this.CountProbabilityModifier)
				{
					lightningBoltParameters.CustomTransform = ((this.CustomTransformHandler == null) ? null : new Action<LightningCustomTransformStateInfo>(this.CustomTransform));
					this.CreateLightningBolt(lightningBoltParameters);
					if (start != null)
					{
						lightningBoltParameters.Start = start.Value;
					}
					if (end != null)
					{
						lightningBoltParameters.End = end.Value;
					}
				}
				else
				{
					LightningBoltParameters.ReturnParametersToCache(lightningBoltParameters);
				}
			}
			this.CreateLightningBoltsNow();
		}

		protected void CreateLightningBoltsNow()
		{
			int maximumLightsPerBatch = LightningBolt.MaximumLightsPerBatch;
			LightningBolt.MaximumLightsPerBatch = this.MaximumLightsPerBatch;
			base.CreateLightningBolts(this.batchParameters);
			LightningBolt.MaximumLightsPerBatch = maximumLightsPerBatch;
			this.batchParameters.Clear();
		}

		protected override void PopulateParameters(LightningBoltParameters p)
		{
			base.PopulateParameters(p);
			p.RandomOverride = this.RandomOverride;
			float lifeTime = this.DurationRange.Random(p.Random);
			float trunkWidth = this.TrunkWidthRange.Random(p.Random);
			p.Generations = this.Generations;
			p.LifeTime = lifeTime;
			p.ChaosFactor = this.ChaosFactor;
			p.ChaosFactorForks = this.ChaosFactorForks;
			p.TrunkWidth = trunkWidth;
			p.Intensity = this.Intensity;
			p.GlowIntensity = this.GlowIntensity;
			p.GlowWidthMultiplier = this.GlowWidthMultiplier;
			p.Forkedness = this.Forkedness;
			p.ForkLengthMultiplier = this.ForkLengthMultiplier;
			p.ForkLengthVariance = this.ForkLengthVariance;
			p.FadePercent = this.FadePercent;
			p.FadeInMultiplier = this.FadeInMultiplier;
			p.FadeOutMultiplier = this.FadeOutMultiplier;
			p.FadeFullyLitMultiplier = this.FadeFullyLitMultiplier;
			p.GrowthMultiplier = this.GrowthMultiplier;
			p.EndWidthMultiplier = this.EndWidthMultiplier;
			p.ForkEndWidthMultiplier = this.ForkEndWidthMultiplier;
			p.DelayRange = this.DelayRange;
			p.LightParameters = this.LightParameters;
		}

		protected override void Start()
		{
			base.Start();
			this.CalculateNextLightningTimestamp(0f);
			this.lifeTimeRemaining = ((this.LifeTime <= 0f) ? float.MaxValue : this.LifeTime);
		}

		protected override void Update()
		{
			base.Update();
			if ((this.lifeTimeRemaining -= LightningBoltScript.DeltaTime) < 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if ((this.nextLightningTimestamp -= LightningBoltScript.DeltaTime) <= 0f)
			{
				this.CalculateNextLightningTimestamp(this.nextLightningTimestamp);
				if (!this.ManualMode)
				{
					this.CallLightning();
				}
			}
			if (this.AutomaticModeSeconds > 0f)
			{
				this.AutomaticModeSeconds = Mathf.Max(0f, this.AutomaticModeSeconds - LightningBoltScript.DeltaTime);
				this.ManualMode = (this.AutomaticModeSeconds == 0f);
			}
		}

		public override void CreateLightningBolt(LightningBoltParameters p)
		{
			this.batchParameters.Add(p);
		}

		public void Trigger()
		{
			this.Trigger(-1f);
		}

		public void Trigger(float seconds)
		{
			this.CallLightning();
			if (seconds >= 0f)
			{
				this.AutomaticModeSeconds = Mathf.Max(0f, seconds);
			}
		}

		public void Trigger(Vector3? start, Vector3? end)
		{
			this.CallLightning(start, end);
		}

		private readonly List<LightningBoltParameters> batchParameters = new List<LightningBoltParameters>();

		private readonly System.Random random = new System.Random();

		public RangeOfFloats IntervalRange = new RangeOfFloats
		{
			Minimum = 0.05f,
			Maximum = 0.1f
		};

		public RangeOfIntegers CountRange = new RangeOfIntegers
		{
			Minimum = 1,
			Maximum = 1
		};

		public float CountProbabilityModifier = 1f;

		public RangeOfFloats DelayRange = new RangeOfFloats
		{
			Minimum = 0f,
			Maximum = 0f
		};

		public RangeOfFloats DurationRange = new RangeOfFloats
		{
			Minimum = 0.06f,
			Maximum = 0.12f
		};

		public RangeOfFloats TrunkWidthRange = new RangeOfFloats
		{
			Minimum = 0.1f,
			Maximum = 0.2f
		};

		public float LifeTime;

		public int Generations = 6;

		public float ChaosFactor = 0.075f;

		public float ChaosFactorForks = 0.095f;

		public float Intensity = 1f;

		public float GlowIntensity = 0.1f;

		public float GlowWidthMultiplier = 4f;

		public float FadePercent = 0.15f;

		public float FadeInMultiplier = 1f;

		public float FadeFullyLitMultiplier = 1f;

		public float FadeOutMultiplier = 1f;

		public float GrowthMultiplier;

		public float EndWidthMultiplier = 0.5f;

		public float Forkedness = 0.25f;

		public float ForkLengthMultiplier = 0.6f;

		public float ForkLengthVariance = 0.2f;

		public float ForkEndWidthMultiplier = 1f;

		public LightningLightParameters LightParameters;

		public int MaximumLightsPerBatch = 8;

		public bool ManualMode;

		public float AutomaticModeSeconds;

		public LightningCustomTransformDelegate CustomTransformHandler;

		private System.Random _003CRandomOverride_003Ek__BackingField;

		private float nextLightningTimestamp;

		private float lifeTimeRemaining;
	}
}
