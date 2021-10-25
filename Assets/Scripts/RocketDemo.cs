using System;
using UnityEngine;

public class RocketDemo : MonoBehaviour
{
	public static RocketDemo.Tangents FindOuterTangents(RocketDemo.Circle c1, RocketDemo.Circle c2)
	{
		RocketDemo.Tangents tangents = default(RocketDemo.Tangents);
		bool flag = false;
		if (c1.radius > c2.radius)
		{
			RocketDemo.Circle circle = c1;
			c1 = c2;
			c2 = circle;
			flag = true;
		}
		tangents.c1 = c1;
		tangents.c2 = c2;
		Vector3 vector = c2.position - c1.position;
		vector.z = 0f;
		float magnitude = vector.magnitude;
		tangents.gamaRad = Mathf.Atan2(c2.position.y - c1.position.y, c2.position.x - c1.position.x);
		tangents.betaRad = Mathf.Asin((c2.radius - c1.radius) / magnitude);
		tangents.alphaRad = tangents.gamaRad + tangents.betaRad;
		tangents.tan1AngleRad = tangents.alphaRad;
		tangents.tan2AngleRad = 3.14159274f + (tangents.gamaRad - tangents.betaRad);
		Vector3 a = Quaternion.AngleAxis(tangents.tan1AngleRad * 57.29578f, Vector3.forward) * Vector3.up;
		Vector3 a2 = Quaternion.AngleAxis(tangents.tan2AngleRad * 57.29578f, Vector3.forward) * Vector3.up;
		if (flag)
		{
			tangents.c2Tan1 = c1.position + a * c1.radius;
			tangents.c1Tan1 = c2.position + a * c2.radius;
			tangents.c2Tan2 = c1.position + a2 * c1.radius;
			tangents.c1Tan2 = c2.position + a2 * c2.radius;
		}
		else
		{
			tangents.c1Tan1 = c1.position + a * c1.radius;
			tangents.c2Tan1 = c2.position + a * c2.radius;
			tangents.c1Tan2 = c1.position + a2 * c1.radius;
			tangents.c2Tan2 = c2.position + a2 * c2.radius;
		}
		return tangents;
	}

	public RocketDemo.Circle c1 = new RocketDemo.Circle();

	public RocketDemo.Circle c2 = new RocketDemo.Circle();

	public float angleOffset;

	[SerializeField]
	public RocketDemo.TangentDistanceFactors tangentFactors = new RocketDemo.TangentDistanceFactors();

	public bool controlPoint2LookingToStart;

	public RocketDemo.DrawMode drawMode;

	[Serializable]
	public class Circle
	{
		public Vector3 position;

		public float radius;
	}

	[Serializable]
	public class TangentDistanceFactors
	{
		public float tangentMultDistance = 0.5f;

		public float radiusTangentMultDistance = 0.5f;
	}

	public enum DrawMode
	{
		DrawModeCircles,
		DrawModePath
	}

	public struct Tangents
	{
		public float gamaRad;

		public float betaRad;

		public float alphaRad;

		public float tan1AngleRad;

		public float tan2AngleRad;

		public RocketDemo.Circle c1;

		public RocketDemo.Circle c2;

		public Vector3 c1Tan1;

		public Vector3 c2Tan1;

		public Vector3 c1Tan2;

		public Vector3 c2Tan2;
	}
}
