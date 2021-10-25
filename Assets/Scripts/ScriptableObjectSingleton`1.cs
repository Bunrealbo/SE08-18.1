using System;
using UnityEngine;

public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
	public void OnDestroy()
	{
		ScriptableObjectSingleton<T>.applicationIsQuitting = true;
	}

	protected virtual void UpdateData()
	{
	}

	public static T instance
	{
		get
		{
			if (!ScriptableObjectSingleton<T>.didTryToLoadSingleton_ && ScriptableObjectSingleton<T>.instance_ == null)
			{
				if (ScriptableObjectSingleton<T>.applicationIsQuitting)
				{
					return default(T);
				}
				ScriptableObjectSingleton<T>.didTryToLoadSingleton_ = true;
				UnityEngine.Debug.Log("Loading singleton from " + typeof(T).ToString());
				ScriptableObjectSingleton<T>.instance_ = Resources.Load<T>(typeof(T).ToString());
				if (ScriptableObjectSingleton<T>.instance_ != null)
				{
					(ScriptableObjectSingleton<T>.instance_ as ScriptableObjectSingleton<T>).UpdateData();
				}
			}
			return ScriptableObjectSingleton<T>.instance_;
		}
	}

	private static bool applicationIsQuitting;

	protected static bool didTryToLoadSingleton_;

	protected static T instance_;
}
