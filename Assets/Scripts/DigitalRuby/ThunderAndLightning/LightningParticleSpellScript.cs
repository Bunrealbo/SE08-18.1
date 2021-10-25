using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningParticleSpellScript : LightningSpellScript, ICollisionHandler
	{
		private void PopulateParticleLight(Light src)
		{
			src.bounceIntensity = 0f;
			src.type = LightType.Point;
			src.shadows = LightShadows.None;
			src.color = new Color(UnityEngine.Random.Range(this.ParticleLightColor1.r, this.ParticleLightColor2.r), UnityEngine.Random.Range(this.ParticleLightColor1.g, this.ParticleLightColor2.g), UnityEngine.Random.Range(this.ParticleLightColor1.b, this.ParticleLightColor2.b), 1f);
			src.cullingMask = this.ParticleLightCullingMask;
			src.intensity = UnityEngine.Random.Range(this.ParticleLightIntensity.Minimum, this.ParticleLightIntensity.Maximum);
			src.range = UnityEngine.Random.Range(this.ParticleLightRange.Minimum, this.ParticleLightRange.Maximum);
		}

		private void UpdateParticleLights()
		{
			if (!this.EnableParticleLights)
			{
				return;
			}
			int num = this.ParticleSystem.GetParticles(this.particles);
			while (this.particleLights.Count < num)
			{
				GameObject gameObject = new GameObject("LightningParticleSpellLight");
				gameObject.hideFlags = HideFlags.HideAndDontSave;
				this.PopulateParticleLight(gameObject.AddComponent<Light>());
				this.particleLights.Add(gameObject);
			}
			while (this.particleLights.Count > num)
			{
				UnityEngine.Object.Destroy(this.particleLights[this.particleLights.Count - 1]);
				this.particleLights.RemoveAt(this.particleLights.Count - 1);
			}
			for (int i = 0; i < num; i++)
			{
				this.particleLights[i].transform.position = this.particles[i].position;
			}
		}

		private void UpdateParticleSystems()
		{
			if (this.EmissionParticleSystem != null && this.EmissionParticleSystem.isPlaying)
			{
				this.EmissionParticleSystem.transform.position = this.SpellStart.transform.position;
				this.EmissionParticleSystem.transform.forward = this.Direction;
			}
			if (this.ParticleSystem != null)
			{
				if (this.ParticleSystem.isPlaying)
				{
					this.ParticleSystem.transform.position = this.SpellStart.transform.position;
					this.ParticleSystem.transform.forward = this.Direction;
				}
				this.UpdateParticleLights();
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			foreach (GameObject obj in this.particleLights)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			base.Update();
			this.UpdateParticleSystems();
			this.collisionTimer -= LightningBoltScript.DeltaTime;
		}

		protected override void OnCastSpell()
		{
			if (this.ParticleSystem != null)
			{
				this.ParticleSystem.Play();
				this.UpdateParticleSystems();
			}
		}

		protected override void OnStopSpell()
		{
			if (this.ParticleSystem != null)
			{
				this.ParticleSystem.Stop();
			}
		}

		void ICollisionHandler.HandleCollision(GameObject obj, List<ParticleCollisionEvent> collisions, int collisionCount)
		{
			if (this.collisionTimer <= 0f)
			{
				this.collisionTimer = this.CollisionInterval;
				base.PlayCollisionSound(collisions[0].intersection);
				base.ApplyCollisionForce(collisions[0].intersection);
				if (this.CollisionCallback != null)
				{
					this.CollisionCallback(obj, collisions, collisionCount);
				}
			}
		}

		public ParticleSystem ParticleSystem;

		public float CollisionInterval;

		protected float collisionTimer;

		public Action<GameObject, List<ParticleCollisionEvent>, int> CollisionCallback;

		public bool EnableParticleLights = true;

		public RangeOfFloats ParticleLightRange = new RangeOfFloats
		{
			Minimum = 2f,
			Maximum = 5f
		};

		public RangeOfFloats ParticleLightIntensity = new RangeOfFloats
		{
			Minimum = 0.2f,
			Maximum = 0.3f
		};

		public Color ParticleLightColor1 = Color.white;

		public Color ParticleLightColor2 = Color.white;

		public LayerMask ParticleLightCullingMask = -1;

		private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[512];

		private readonly List<GameObject> particleLights = new List<GameObject>();
	}
}
