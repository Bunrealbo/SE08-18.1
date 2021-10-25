using System;
using System.Collections.Generic;
using UnityEngine;

public class DestroyUtil
{
	public static void DontDestroyOnLoad(GameObject gameObject)
	{
		if (gameObject == null)
		{
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		DestroyUtil.allObjects.Add(gameObject);
	}

	public static List<GameObject> allObjects = new List<GameObject>();
}
