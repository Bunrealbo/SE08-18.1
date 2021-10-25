using System;

public static class MathEx
{
	public static long Min(long a, long b)
	{
		if (a >= b)
		{
			return b;
		}
		return a;
	}

	public static float Max(float a, float b)
	{
		if (a <= b)
		{
			return b;
		}
		return a;
	}

	public static double Max(double a, double b)
	{
		if (a <= b)
		{
			return b;
		}
		return a;
	}

	public static long Max(long a, long b)
	{
		if (a <= b)
		{
			return b;
		}
		return a;
	}
}
