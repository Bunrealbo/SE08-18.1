using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningFieldScript : LightningBoltPrefabScriptBase
	{
		private Vector3 RandomPointInBounds()
		{
			float x = UnityEngine.Random.Range(this.FieldBounds.min.x, this.FieldBounds.max.x);
			float y = UnityEngine.Random.Range(this.FieldBounds.min.y, this.FieldBounds.max.y);
			float z = UnityEngine.Random.Range(this.FieldBounds.min.z, this.FieldBounds.max.z);
			return new Vector3(x, y, z);
		}

		protected override void Start()
		{
			base.Start();
			if (this.Light != null)
			{
				this.Light.enabled = false;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (this.Light != null)
			{
				this.Light.transform.position = this.FieldBounds.center;
				this.Light.intensity = UnityEngine.Random.Range(2.8f, 3.2f);
			}
		}

		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			this.minimumLengthSquared = this.MinimumLength * this.MinimumLength;
			for (int i = 0; i < 16; i++)
			{
				parameters.Start = this.RandomPointInBounds();
				parameters.End = this.RandomPointInBounds();
				if ((parameters.End - parameters.Start).sqrMagnitude >= this.minimumLengthSquared)
				{
					break;
				}
			}
			if (this.Light != null)
			{
				this.Light.enabled = true;
			}
			base.CreateLightningBolt(parameters);
		}

		public float MinimumLength = 0.01f;

		private float minimumLengthSquared;

		public Bounds FieldBounds;

		public Light Light;
	}
}
