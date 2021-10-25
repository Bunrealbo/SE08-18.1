using System;
using UnityEngine;

public class BehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T instance
	{
		get
		{
			return BehaviourSingleton<T>.Instance;
		}
	}

	public static T Instance
	{
		get
		{
			T result;
			if (BehaviourSingleton<T>.applicationIsQuitting)
			{
				UnityEngine.Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
				result = default(T);
				return result;
			}
			object @lock = BehaviourSingleton<T>._lock;
			lock (@lock)
			{
				if (BehaviourSingleton<T>._instance == null)
				{
					BehaviourSingleton<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
					if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						UnityEngine.Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
						return BehaviourSingleton<T>._instance;
					}
					if (BehaviourSingleton<T>._instance == null)
					{
						GameObject gameObject = new GameObject();
						BehaviourSingleton<T>._instance = gameObject.AddComponent<T>();
						gameObject.name = typeof(T).ToString();
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"[Singleton] An instance of ",
							typeof(T),
							" is needed in the scene, so '",
							gameObject,
							"' was created with DontDestroyOnLoad."
						}));
					}
					else
					{
						UnityEngine.Debug.Log("[Singleton] Using instance already created: " + BehaviourSingleton<T>._instance.gameObject.name);
					}
				}
				result = BehaviourSingleton<T>._instance;
			}
			return result;
		}
	}

	public void OnDestroy()
	{
		if (Application.isEditor)
		{
			BehaviourSingleton<T>.applicationIsQuitting = false;
			return;
		}
		BehaviourSingleton<T>.applicationIsQuitting = true;
	}

	private static T _instance;

	private static object _lock = new object();

	private static bool applicationIsQuitting = false;
}
