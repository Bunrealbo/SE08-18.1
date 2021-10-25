using System;
using UnityEngine;

namespace GGCloth
{
	public class PointMass
	{
		public void SetRestingPostion(Vector3 position)
		{
			this.previosPosition = position;
			this.currentPosition = position;
		}

		public void VerletIntegrate(float deltaTimeMilliseconds, PointWorld world)
		{
			Vector3 a = this.currentPosition - this.previosPosition;
			this.previosPosition = this.currentPosition;
			Vector3 a2 = this.acceleration + world.gravityMS;
			this.currentPosition += a * (1f - this.dampingFactor) + a2 * (deltaTimeMilliseconds * deltaTimeMilliseconds);
		}

		public Vector3 currentPosition;

		public Vector3 previosPosition;

		public float mass;

		public float dampingFactor;

		public Vector3 acceleration;
	}
}
