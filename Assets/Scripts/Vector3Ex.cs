using System;
using UnityEngine;

public static class Vector3Ex
{
	public static Vector3 CatmullRomLerp(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
	{
		t = Mathf.Clamp01(t);
		float num = t * t;
		float num2 = t * t * t;
		float d = -1f * num2 + 2f * num - t;
		float d2 = 3f * num2 - 5f * num + 2f;
		float d3 = -3f * num2 + 4f * num + t;
		float d4 = num2 - num;
		return 0.5f * (d * p1 + d2 * p2 + d3 * p3 + d4 * p4);
	}

	public static Vector3 OnGround(Vector3 vector, float y = 0f)
	{
		Vector3 result = vector;
		result.y = y;
		return result;
	}

	public static Vector3 down = new Vector3(0f, -1f, 0f);

	public static Vector3 up = new Vector3(0f, 1f, 0f);

	public static Vector3 zero = new Vector3(0f, 1f, 0f);
}
