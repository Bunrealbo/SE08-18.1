using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGCloth
{
	public class MultiPointAttachConstraint : Constraint
	{
		public void Init(Vector3 centralPosition)
		{
			this.points.Clear();
			this.centralPosition = centralPosition;
		}

		public void FixPoint(PointMass point)
		{
			MultiPointAttachConstraint.PointPosition pointPosition = new MultiPointAttachConstraint.PointPosition();
			pointPosition.point = point;
			pointPosition.localPosition = point.currentPosition - this.centralPosition;
			this.points.Add(pointPosition);
		}

		public override void Solve(PointWorld fieldWorld)
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				MultiPointAttachConstraint.PointPosition pointPosition = this.points[i];
				Vector3 restingPostion = this.centralPosition + pointPosition.localPosition;
				pointPosition.point.SetRestingPostion(restingPostion);
			}
		}

		public Vector3 centralPosition;

		public List<MultiPointAttachConstraint.PointPosition> points = new List<MultiPointAttachConstraint.PointPosition>();

		public class PointPosition
		{
			public PointMass point;

			public Vector3 localPosition;
		}
	}
}
