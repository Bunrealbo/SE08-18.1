using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGCloth
{
	public class PointWorld
	{
		public List<PointMass> Points
		{
			get
			{
				return this.points;
			}
		}

		public void SetGravity(Vector3 gravity)
		{
			this.gravityMS = gravity / 1000000f;
		}

		public void Clear()
		{
			this.points.Clear();
			this.constraints.Clear();
		}

		public PointMass GetPoint(int index)
		{
			return this.points[index];
		}

		public void AddPoint(PointMass point)
		{
			this.points.Add(point);
		}

		public void Prepend(Constraint constraint)
		{
			this.constraints.Insert(0, constraint);
		}

		public void AddConstraint(Constraint constraint)
		{
			this.constraints.Add(constraint);
		}

		public void Step(float deltaTime)
		{
			float num = deltaTime * 1000f + this.leftOverTimeMS;
			int num2 = Mathf.FloorToInt(num / this.fixedTimeStepMilliseconds);
			this.leftOverTimeMS = num - (float)num2 * this.fixedTimeStepMilliseconds;
			for (int i = 0; i < num2; i++)
			{
				this.VerletIntegrate();
				this.SatisfyConstraints();
			}
		}

		private void VerletIntegrate()
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				this.points[i].VerletIntegrate(this.fixedTimeStepMilliseconds, this);
			}
		}

		private void SatisfyConstraints()
		{
			for (int i = 0; i < this.constraintRelaxationSteps; i++)
			{
				for (int j = 0; j < this.constraints.Count; j++)
				{
					this.constraints[j].Solve(this);
				}
			}
		}

		private List<PointMass> points = new List<PointMass>();

		private List<Constraint> constraints = new List<Constraint>();

		public float fixedTimeStepMilliseconds = 10f;

		private float leftOverTimeMS;

		public int constraintRelaxationSteps = 1;

		public Vector3 gravityMS;
	}
}
