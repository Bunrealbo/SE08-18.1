using System;

public class SingletonInit<T> : InitClass where T : InitClass, new()
{
	public static T Instance
	{
		get
		{
			return SingletonInit<T>.instance;
		}
	}

	public static T instance
	{
		get
		{
			if (SingletonInit<T>._instance == null)
			{
				SingletonInit<T>._instance = Activator.CreateInstance<T>();
				SingletonInit<T>._instance.Init();
			}
			return SingletonInit<T>._instance;
		}
	}

	private static T _instance;
}
