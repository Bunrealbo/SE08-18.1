using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class LightSlotComponent : SlotComponent
	{
		private LightSlotComponent.Settings settings
		{
			get
			{
				return Match3Settings.instance.lightSlotSettings;
			}
		}

		public void Init(SlotLightBehaviour lightBehaviour)
		{
			this.lightBehaviour = lightBehaviour;
		}

		public void AddIntensityChange(IntensityChange change)
		{
			LightSlotComponent.IntensityChangeProcessor intensityChangeProcessor = null;
			for (int i = 0; i < this.intensityChanges.Count; i++)
			{
				LightSlotComponent.IntensityChangeProcessor intensityChangeProcessor2 = this.intensityChanges[i];
				if (!intensityChangeProcessor2.isActive)
				{
					intensityChangeProcessor = intensityChangeProcessor2;
					break;
				}
			}
			if (intensityChangeProcessor == null)
			{
				intensityChangeProcessor = new LightSlotComponent.IntensityChangeProcessor();
				this.intensityChanges.Add(intensityChangeProcessor);
			}
			intensityChangeProcessor.Activate(change);
			this.OnUpdate(0f);
		}

		public void AddLight(LightSlotComponent.PermanentLight light)
		{
			if (this.permanentLights.Contains(light))
			{
				return;
			}
			this.permanentLights.Add(light);
			this.OnUpdate(0f);
		}

		public void RemoveLight(LightSlotComponent.PermanentLight light)
		{
			this.permanentLights.Remove(light);
			this.OnUpdate(0f);
		}

		public float maxIntensity
		{
			get
			{
				return this.settings.maxIntensity;
			}
		}

		public void AddLight(float intensity)
		{
			IntensityChange fadeOut = this.settings.fadeOut;
			fadeOut.intensityRange.min = intensity;
			this.AddIntensityChange(fadeOut);
		}

		public void AddLightWithDuration(float intensity, float duration)
		{
			IntensityChange fadeOut = this.settings.fadeOut;
			fadeOut.intensityRange.min = intensity;
			fadeOut.duration = duration;
			this.AddIntensityChange(fadeOut);
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			float num = 0f;
			for (int i = 0; i < this.intensityChanges.Count; i++)
			{
				LightSlotComponent.IntensityChangeProcessor intensityChangeProcessor = this.intensityChanges[i];
				if (intensityChangeProcessor.isActive)
				{
					intensityChangeProcessor.timeActive += deltaTime;
					if (intensityChangeProcessor.timeActive >= intensityChangeProcessor.change.delay + intensityChangeProcessor.change.duration)
					{
						intensityChangeProcessor.isActive = false;
					}
					else
					{
						num += intensityChangeProcessor.currentIntensity;
					}
				}
			}
			for (int j = 0; j < this.permanentLights.Count; j++)
			{
				LightSlotComponent.PermanentLight permanentLight = this.permanentLights[j];
				num += permanentLight.GetCurrentIntensity(this);
			}
			if (this.lightBehaviour != null)
			{
				this.lightBehaviour.SetLight(Mathf.InverseLerp(0f, this.settings.maxIntensity, num));
			}
		}

		private List<LightSlotComponent.IntensityChangeProcessor> intensityChanges = new List<LightSlotComponent.IntensityChangeProcessor>();

		private List<LightSlotComponent.PermanentLight> permanentLights = new List<LightSlotComponent.PermanentLight>();

		private SlotLightBehaviour lightBehaviour;

		public class PermanentLight
		{
			public virtual float GetCurrentIntensity(LightSlotComponent component)
			{
				return this.currentIntensity;
			}

			public virtual float currentIntensity
			{
				get
				{
					return this._003CcurrentIntensity_003Ek__BackingField;
				}
			}

			private float _003CcurrentIntensity_003Ek__BackingField;
		}

		private class IntensityChangeProcessor
		{
			public float currentIntensity
			{
				get
				{
					if (this.timeActive < this.change.delay)
					{
						return 0f;
					}
					float t = Mathf.InverseLerp(this.change.delay, this.change.delay + this.change.duration, this.timeActive);
					return this.change.Intensity(t);
				}
			}

			public void Activate(IntensityChange change)
			{
				this.change = change;
				this.isActive = true;
				this.timeActive = 0f;
			}

			public IntensityChange change;

			public bool isActive;

			public float timeActive;
		}

		[Serializable]
		public class Settings
		{
			public float lightFadeoutSpeed = 2f;

			public float maxIntensity = 2f;

			public IntensityChange fadeOut;
		}
	}
}
