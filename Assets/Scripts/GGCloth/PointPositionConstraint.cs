using System;
using UnityEngine;

namespace GGCloth
{
	public class PointPositionConstraint : Constraint
	{
		public void Init(PointMass point, PointMass pointToConstrainBy, PointPositionConstraint.Direction constrainDirection)
		{
			this.point = point;
			this.pointToConstrainBy = pointToConstrainBy;
			this.constrainDirection = constrainDirection;
			this.initialXSign = (int)Mathf.Sign(point.currentPosition.x - pointToConstrainBy.currentPosition.x);
			this.initialYSign = (int)Mathf.Sign(point.currentPosition.y - pointToConstrainBy.currentPosition.y);
		}

		public override void Solve(PointWorld fieldWorld)
		{
			Vector3 currentPosition = this.point.currentPosition;
			Vector3 vector = currentPosition;
			if (this.constrainDirection == PointPositionConstraint.Direction.X || this.constrainDirection == PointPositionConstraint.Direction.XY)
			{
				if (this.initialXSign < 0)
				{
					vector.x = Mathf.Min(this.pointToConstrainBy.currentPosition.x - this.minDistance, this.point.currentPosition.x);
				}
				else
				{
					vector.x = Mathf.Max(this.pointToConstrainBy.currentPosition.x + this.minDistance, this.point.currentPosition.x);
				}
			}
			if (this.constrainDirection == PointPositionConstraint.Direction.Y || this.constrainDirection == PointPositionConstraint.Direction.XY)
			{
				if (this.initialYSign < 0)
				{
					vector.y = Mathf.Min(this.pointToConstrainBy.currentPosition.y - this.minDistance, this.point.currentPosition.y);
				}
				else
				{
					vector.y = Mathf.Max(this.pointToConstrainBy.currentPosition.y + this.minDistance, this.point.currentPosition.y);
				}
			}
			if (vector != currentPosition)
			{
				this.point.SetRestingPostion(vector);
			}
		}

		public PointMass point;

		public PointMass pointToConstrainBy;

		public float minDistance = 0.1f;

		private int initialXSign;

		private int initialYSign;

		public PointPositionConstraint.Direction constrainDirection;

		public enum Direction
		{
			X,
			Y,
			XY
		}
	}
}
