using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[RequireComponent(typeof(ParticleSystem))]
	public class LightningParticleCollisionForwarder : MonoBehaviour
	{
		private void Start()
		{
			this._particleSystem = base.GetComponent<ParticleSystem>();
		}

		private void OnParticleCollision(GameObject other)
		{
			ICollisionHandler collisionHandler = this.CollisionHandler as ICollisionHandler;
			if (collisionHandler != null)
			{
				int num = ParticlePhysicsExtensions.GetCollisionEvents(this._particleSystem, other, this.collisionEvents);
				if (num != 0)
				{
					collisionHandler.HandleCollision(other, this.collisionEvents, num);
				}
			}
		}

		public MonoBehaviour CollisionHandler;

		private ParticleSystem _particleSystem;

		private readonly List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
	}
}
