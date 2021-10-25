using System;
using UnityEngine;

public class AnimRandom
{
	public static float Range(float min, float max)
	{
		return Mathf.Lerp(min, max, (float)AnimRandom.random.NextDouble());
	}

	public static int Range(int min, int max)
	{
		return Mathf.FloorToInt(Mathf.Lerp((float)min, (float)max, (float)AnimRandom.random.NextDouble()));
	}

	public static System.Random random = new System.Random();
}
