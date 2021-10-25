using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBeamSpellScript : LightningSpellScript
	{
		private void CheckCollision()
		{
			RaycastHit obj;
			if (Physics.Raycast(this.SpellStart.transform.position, this.Direction, out obj, this.MaxDistance, this.CollisionMask))
			{
				this.SpellEnd.transform.position = obj.point;
				this.SpellEnd.transform.position += UnityEngine.Random.insideUnitSphere * this.EndPointRandomization;
				base.PlayCollisionSound(this.SpellEnd.transform.position);
				if (this.CollisionParticleSystem != null)
				{
					this.CollisionParticleSystem.transform.position = obj.point;
					this.CollisionParticleSystem.Play();
				}
				base.ApplyCollisionForce(obj.point);
				if (this.CollisionCallback != null)
				{
					this.CollisionCallback(obj);
					return;
				}
			}
			else
			{
				if (this.CollisionParticleSystem != null)
				{
					this.CollisionParticleSystem.Stop();
				}
				this.SpellEnd.transform.position = this.SpellStart.transform.position + this.Direction * this.MaxDistance;
				this.SpellEnd.transform.position += UnityEngine.Random.insideUnitSphere * this.EndPointRandomization;
			}
		}

		protected override void Start()
		{
			base.Start();
			this.LightningPathScript.ManualMode = true;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (!base.Casting)
			{
				return;
			}
			this.CheckCollision();
		}

		protected override void OnCastSpell()
		{
			this.LightningPathScript.ManualMode = false;
		}

		protected override void OnStopSpell()
		{
			this.LightningPathScript.ManualMode = true;
		}

		public LightningBoltPathScriptBase LightningPathScript;

		public float EndPointRandomization = 1.5f;

		public Action<RaycastHit> CollisionCallback;
	}
}
