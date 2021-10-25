using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public static class PathGenerator
	{
		public static float SquareRoot(float x)
		{
			return (float)Math.Sqrt((double)x);
		}

		private static float Distance2D(ref Vector3 point1, ref Vector3 point2)
		{
			float num = point2.x - point1.x;
			float num2 = point2.y - point1.y;
			return PathGenerator.SquareRoot(num * num + num2 * num2);
		}

		private static float Distance3D(ref Vector3 point1, ref Vector3 point2)
		{
			float num = point2.x - point1.x;
			float num2 = point2.y - point1.y;
			float num3 = point2.z - point1.z;
			return PathGenerator.SquareRoot(num * num + num2 * num2 + num3 * num3);
		}

		private static void CalculateNonuniformCatmullRom(float p1, float p2, float p3, float p4, float distance1, float distance2, float distance3, out Vector4 point)
		{
			float num = ((p2 - p1) / distance1 - (p3 - p1) / (distance1 + distance2) + (p3 - p2) / distance2) * distance2;
			float num2 = ((p3 - p2) / distance2 - (p4 - p2) / (distance2 + distance3) + (p4 - p3) / distance3) * distance2;
			point = new Vector4(p2, num, -3f * p2 + 3f * p3 - (num2 + 2f * num), num + num2 + (2f * p2 - 2f * p3));
		}

		private static float CalculatePolynomial(ref Vector4 point, float t)
		{
			float num = t * t;
			float num2 = num * t;
			return point.w * num2 + point.z * num + point.y * t + point.x;
		}

		private static void ClampSplineDistances(ref float distance1, ref float distance2, ref float distance3)
		{
			if (distance2 < 0.0001f)
			{
				distance2 = 1f;
			}
			if (distance1 < 0.0001f)
			{
				distance1 = distance2;
			}
			if (distance3 < 0.0001f)
			{
				distance3 = distance2;
			}
		}

		private static void GetSplinePoint2D(ref Vector3 point1, ref Vector3 point2, ref Vector3 point3, ref Vector3 point4, float t, out Vector3 point)
		{
			float distance = PathGenerator.Distance2D(ref point1, ref point2);
			float distance2 = PathGenerator.Distance2D(ref point2, ref point3);
			float distance3 = PathGenerator.Distance2D(ref point3, ref point4);
			PathGenerator.ClampSplineDistances(ref distance, ref distance2, ref distance3);
			Vector4 vector;
			PathGenerator.CalculateNonuniformCatmullRom(point1.x, point2.x, point3.x, point4.x, distance, distance2, distance3, out vector);
			Vector4 vector2;
			PathGenerator.CalculateNonuniformCatmullRom(point1.y, point2.y, point3.y, point4.y, distance, distance2, distance3, out vector2);
			point = new Vector3(PathGenerator.CalculatePolynomial(ref vector, t), PathGenerator.CalculatePolynomial(ref vector2, t), 0f);
		}

		private static void GetSplinePoint3D(ref Vector3 point1, ref Vector3 point2, ref Vector3 point3, ref Vector3 point4, float t, out Vector3 point)
		{
			float distance = PathGenerator.Distance3D(ref point1, ref point2);
			float distance2 = PathGenerator.Distance3D(ref point2, ref point3);
			float distance3 = PathGenerator.Distance3D(ref point3, ref point4);
			PathGenerator.ClampSplineDistances(ref distance, ref distance2, ref distance3);
			Vector4 vector;
			PathGenerator.CalculateNonuniformCatmullRom(point1.x, point2.x, point3.x, point4.x, distance, distance2, distance3, out vector);
			Vector4 vector2;
			PathGenerator.CalculateNonuniformCatmullRom(point1.y, point2.y, point3.y, point4.y, distance, distance2, distance3, out vector2);
			Vector4 vector3;
			PathGenerator.CalculateNonuniformCatmullRom(point1.z, point2.z, point3.z, point4.z, distance, distance2, distance3, out vector3);
			point = new Vector3(PathGenerator.CalculatePolynomial(ref vector, t), PathGenerator.CalculatePolynomial(ref vector2, t), PathGenerator.CalculatePolynomial(ref vector3, t));
		}

		public static bool CreateSpline(ICollection<Vector3> path, IList<Vector3> points, int numberOfSegments, bool closePath)
		{
			if (points.Count < 4)
			{
				return false;
			}
			numberOfSegments = Math.Min(1024, Math.Max(numberOfSegments, 4));
			int num = closePath ? points.Count : (points.Count - 1);
			int num2 = closePath ? 1 : 0;
			float num3 = 1f / (float)numberOfSegments * (float)num;
			float num4 = 0f;
			for (int i = 0; i < num; i++)
			{
				int index = (i == 0) ? (num - num2) : (i - 1);
				int num5 = i + 1;
				int num6 = i + 2;
				if (closePath && num5 > num - 1)
				{
					num5 -= num;
				}
				if (num6 > num - 1)
				{
					num6 = (closePath ? (num6 - num) : num);
				}
				Vector3 vector = points[index];
				Vector3 vector2 = points[i];
				Vector3 vector3 = points[num5];
				Vector3 vector4 = points[num6];
				float num7;
				if (PathGenerator.Is2D)
				{
					for (num7 = num4; num7 <= 1f; num7 += num3)
					{
						Vector3 item;
						PathGenerator.GetSplinePoint2D(ref vector, ref vector2, ref vector3, ref vector4, num7, out item);
						path.Add(item);
					}
				}
				else
				{
					for (num7 = num4; num7 <= 1f; num7 += num3)
					{
						Vector3 item;
						PathGenerator.GetSplinePoint3D(ref vector, ref vector2, ref vector3, ref vector4, num7, out item);
						path.Add(item);
					}
				}
				num4 = num7 - 1f;
			}
			return true;
		}

		public static bool CreateSplineWithSegmentDistance(ICollection<Vector3> path, IList<Vector3> points, float distancePerSegment, bool closePath)
		{
			if (points.Count < 4 || distancePerSegment <= 0f)
			{
				return false;
			}
			int num = closePath ? points.Count : (points.Count - 1);
			int num2 = closePath ? 1 : 0;
			float num3 = 0f;
			for (int i = 0; i < num; i++)
			{
				int index = (i == 0) ? (num - num2) : (i - 1);
				int num4 = i + 1;
				int num5 = i + 2;
				if (closePath && num4 > num - 1)
				{
					num4 -= num;
				}
				if (num5 > num - 1)
				{
					num5 = (closePath ? (num5 - num) : num);
				}
				Vector3 vector = points[index];
				Vector3 vector2 = points[i];
				Vector3 vector3 = points[num4];
				Vector3 vector4 = points[num5];
				if (PathGenerator.Is2D)
				{
					float num6 = 1f / (PathGenerator.Distance2D(ref vector3, ref vector2) / distancePerSegment);
					num6 = Mathf.Clamp(num6, 0.00390625f, 1f);
					for (float num7 = num3; num7 <= 1f; num7 += num6)
					{
						Vector3 item;
						PathGenerator.GetSplinePoint2D(ref vector, ref vector2, ref vector3, ref vector4, num7, out item);
						path.Add(item);
					}
				}
				else
				{
					float num6 = 1f / (PathGenerator.Distance3D(ref vector3, ref vector2) / distancePerSegment);
					num6 = Mathf.Clamp(num6, 0.00390625f, 1f);
					for (float num7 = num3; num7 <= 1f; num7 += num6)
					{
						Vector3 item;
						PathGenerator.GetSplinePoint3D(ref vector, ref vector2, ref vector3, ref vector4, num7, out item);
						path.Add(item);
					}
				}
				num3 = 0f;
			}
			return true;
		}

		public static bool Is2D;
	}
}
