using System;
using UnityEngine;

namespace GGCloth
{
	public class SquareCloth
	{
		public int GetPointIndex(int column, int row)
		{
			return column + row * (this.columnCount + 1);
		}

		public void Init(int columnCount, int rowCount, Vector3 size, float damping, float stiffness, Vector3 globalOffset)
		{
			this.rowCount = rowCount;
			this.columnCount = columnCount;
			this.damping = damping;
			this.stiffness = stiffness;
			Vector3 a = Vector3.right * size.x / (float)columnCount;
			Vector3 a2 = Vector3.up * size.y / (float)rowCount;
			Vector3 a3 = new Vector3(-size.x * 0.5f, -size.y * 0.5f, 0f);
			this.pointWorld.Clear();
			for (int i = 0; i <= rowCount; i++)
			{
				for (int j = 0; j <= columnCount; j++)
				{
					PointMass pointMass = new PointMass();
					Vector3 restingPostion = a3 + a * (float)j + a2 * (float)i + globalOffset;
					pointMass.SetRestingPostion(restingPostion);
					pointMass.dampingFactor = damping;
					this.pointWorld.AddPoint(pointMass);
				}
			}
			for (int k = 0; k <= rowCount; k++)
			{
				for (int l = 0; l <= columnCount; l++)
				{
					if (l < columnCount)
					{
						this.AddSpring(l, k, l + 1, k);
					}
					if (k < rowCount)
					{
						this.AddSpring(l, k, l, k + 1);
					}
					if (l < columnCount && k < rowCount)
					{
						this.AddSpring(l, k, l + 1, k + 1);
					}
					if (l > 0 && k < rowCount)
					{
						this.AddSpring(l, k, l - 1, k + 1);
					}
					if (l < columnCount - 1)
					{
						this.AddSpring(l, k, l + 2, k);
					}
					if (k < rowCount - 1)
					{
						this.AddSpring(l, k, l, k + 2);
					}
				}
			}
			for (int m = 0; m <= rowCount; m++)
			{
				for (int n = 0; n <= columnCount; n++)
				{
					if (n < columnCount)
					{
						this.AddPositionConstraint(n, m, n + 1, m, PointPositionConstraint.Direction.X);
					}
					if (m < rowCount)
					{
						this.AddPositionConstraint(n, m, n, m + 1, PointPositionConstraint.Direction.Y);
					}
					if (n < columnCount && m < rowCount)
					{
						this.AddPositionConstraint(n, m, n + 1, m + 1, PointPositionConstraint.Direction.XY);
					}
					if (n < columnCount && m > 0)
					{
						this.AddPositionConstraint(n, m, n + 1, m - 1, PointPositionConstraint.Direction.XY);
					}
				}
			}
		}

		private void AddSpring(int column1, int row1, int column2, int row2)
		{
			PointMass point = this.pointWorld.GetPoint(this.GetPointIndex(column1, row1));
			PointMass point2 = this.pointWorld.GetPoint(this.GetPointIndex(column2, row2));
			SpringLink springLink = new SpringLink();
			springLink.InitWithPointsAtRest(point, point2, this.stiffness + this.stiffness * AnimRandom.Range(-this.stiffnessRandom, this.stiffnessRandom));
			this.pointWorld.AddConstraint(springLink);
		}

		private void AddPositionConstraint(int column1, int row1, int column2, int row2, PointPositionConstraint.Direction direction)
		{
			PointMass point = this.pointWorld.GetPoint(this.GetPointIndex(column1, row1));
			PointMass point2 = this.pointWorld.GetPoint(this.GetPointIndex(column2, row2));
			PointPositionConstraint pointPositionConstraint = new PointPositionConstraint();
			pointPositionConstraint.Init(point2, point, direction);
			this.pointWorld.AddConstraint(pointPositionConstraint);
		}

		public Transform localPositionTransform;

		public bool isWorldPosition;

		public float stiffnessRandom;

		public PointWorld pointWorld = new PointWorld();

		public int rowCount;

		public int columnCount;

		public float damping = 0.5f;

		public float stiffness = 0.5f;
	}
}
