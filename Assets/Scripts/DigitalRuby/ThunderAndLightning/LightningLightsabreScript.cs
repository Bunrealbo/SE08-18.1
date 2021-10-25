using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningLightsabreScript : LightningBoltPrefabScript
	{
		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			if (this.state == 2 || this.state == 3)
			{
				this.bladeTime += LightningBoltScript.DeltaTime;
				float num = Mathf.Lerp(0.01f, 1f, this.bladeTime / this.ActivationTime);
				Vector3 position = this.bladeStart + this.bladeDir * num * this.BladeHeight;
				this.Destination.transform.position = position;
				this.GlowIntensity = this.bladeIntensity * ((this.state == 3) ? num : (1f - num));
				if (this.bladeTime >= this.ActivationTime)
				{
					this.GlowIntensity = this.bladeIntensity;
					this.bladeTime = 0f;
					if (this.state == 2)
					{
						this.ManualMode = true;
						this.state = 0;
					}
					else
					{
						this.state = 1;
					}
				}
			}
			base.Update();
		}

		public bool TurnOn(bool value)
		{
			if (this.state == 2 || this.state == 3 || (this.state == 1 && value) || (this.state == 0 && !value))
			{
				return false;
			}
			this.bladeStart = this.Destination.transform.position;
			this.ManualMode = false;
			this.bladeIntensity = this.GlowIntensity;
			if (value)
			{
				this.bladeDir = (this.Camera.orthographic ? base.transform.up : base.transform.forward);
				this.state = 3;
				this.StartSound.Play();
				this.StopSound.Stop();
				this.ConstantSound.Play();
			}
			else
			{
				this.bladeDir = -(this.Camera.orthographic ? base.transform.up : base.transform.forward);
				this.state = 2;
				this.StartSound.Stop();
				this.StopSound.Play();
				this.ConstantSound.Stop();
			}
			return true;
		}

		public void TurnOnGUI(bool value)
		{
			this.TurnOn(value);
		}

		public float BladeHeight = 19f;

		public float ActivationTime = 0.5f;

		public AudioSource StartSound;

		public AudioSource StopSound;

		public AudioSource ConstantSound;

		private int state;

		private Vector3 bladeStart;

		private Vector3 bladeDir;

		private float bladeTime;

		private float bladeIntensity;
	}
}
