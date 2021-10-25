using System;
using UnityEngine;

public class BehaviourSingletonInit<T> : MonoBehaviourInit where T : MonoBehaviourInit
{
	public static T instance
	{
		get
		{
			return BehaviourSingletonInit<T>.Instance;
		}
	}

	public static T Instance
	{
		get
		{
			T result;
			if (!Application.isPlaying)
			{
				result = default(T);
				return result;
			}
			if (BehaviourSingletonInit<T>.applicationIsQuitting)
			{
				UnityEngine.Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
				result = default(T);
				return result;
			}
			object @lock = BehaviourSingletonInit<T>._lock;
			lock (@lock)
			{
				if (BehaviourSingletonInit<T>._instance == null)
				{
					BehaviourSingletonInit<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
					if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						UnityEngine.Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
						return BehaviourSingletonInit<T>._instance;
					}
					if (BehaviourSingletonInit<T>._instance == null)
					{
						GameObject gameObject = new GameObject();
						BehaviourSingletonInit<T>._instance = gameObject.AddComponent<T>();
						BehaviourSingletonInit<T>._instance.Init();
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
						UnityEngine.Debug.Log("[Singleton] Using instance already created: " + BehaviourSingletonInit<T>._instance.gameObject.name);
					}
				}
				result = BehaviourSingletonInit<T>._instance;
			}
			return result;
		}
	}

	public void OnDestroy()
	{
		BehaviourSingletonInit<T>.applicationIsQuitting = true;
	}

	private static T _instance;

	private static object _lock = new object();

	private static bool applicationIsQuitting = false;
}
