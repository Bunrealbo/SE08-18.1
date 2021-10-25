using System;

public class Singleton<T> where T : class, new()
{
	public static T instance
	{
		get
		{
			return Singleton<T>.Instance;
		}
	}

	public static T Instance
	{
		get
		{
			if (Singleton<T>._instance == null)
			{
				Singleton<T>._instance = Activator.CreateInstance<T>();
			}
			return Singleton<T>._instance;
		}
	}

	private static T _instance;
}
