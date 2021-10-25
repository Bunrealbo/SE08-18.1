using System;
using System.Collections.Generic;
using UnityEngine;

namespace JSONData
{
	[Serializable]
	public class ShapeGraphShape
	{
		public ShapeGraphShape.Orientation GetOrientation()
		{
			if (GGUtil.SignedArea(this.points) < 0f)
			{
				return ShapeGraphShape.Orientation.CCW;
			}
			return ShapeGraphShape.Orientation.CW;
		}

		public bool IsInside(Vector2 pos)
		{
			int num = 0;
			for (int i = 0; i < this.points.Count; i++)
			{
				Vector2 b = this.points[i];
				Vector2 normalized = (((i == this.points.Count - 1) ? this.points[0] : this.points[i + 1]) - b).normalized;
				Vector2 normalized2 = (pos - b).normalized;
				int num2 = Mathf.RoundToInt(Mathf.Sign(Vector3.Cross(normalized, normalized2).z));
				if (num != 0 && num != num2)
				{
					return false;
				}
				num = num2;
			}
			return true;
		}

		public List<Vector2> points = new List<Vector2>();

		public enum Orientation
		{
			CCW,
			CW
		}
	}
}
