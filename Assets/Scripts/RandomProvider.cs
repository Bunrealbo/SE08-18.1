using System;
using UnityEngine;

public class RandomProvider
{
	public virtual int seed
	{
		get
		{
			return UnityEngine.Random.seed;
		}
		set
		{
			UnityEngine.Random.seed = value;
			this.internalSeed = value;
		}
	}

	public virtual void Init()
	{
		this.internalSeed = this.seed;
	}

	public virtual float Range(float min, float max)
	{
		return UnityEngine.Random.Range(min, max);
	}

	public virtual int Range(int min, int max)
	{
		return UnityEngine.Random.Range(min, max);
	}

	private int internalSeed;
}
