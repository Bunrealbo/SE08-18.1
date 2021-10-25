using System;
using UnityEngine;

namespace GGCloth
{
	public class SpringLink : Constraint
	{
		public void InitWithPointsAtRest(PointMass p1, PointMass p2, float stiffness)
		{
			this.p1 = p1;
			this.p2 = p2;
			this.restingDistance = Vector3.Distance(p1.currentPosition, p2.currentPosition);
			this.stiffness = stiffness;
		}

		public override void Solve(PointWorld fieldWorld)
		{
			Vector3 a = this.p2.currentPosition - this.p1.currentPosition;
			float magnitude = a.magnitude;
			float num = (magnitude - this.restingDistance) / magnitude;
			Vector3 b = this.stiffness * num * 0.5f * a;
			this.p1.currentPosition += b;
			this.p2.currentPosition -= b;
		}

		public PointMass p1;

		public PointMass p2;

		public float stiffness = 0.5f;

		public float restingDistance = 1f;
	}
}
