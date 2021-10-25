using System;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectActivator : MonoBehaviour
{
	public void OnEnable()
	{
		bool @bool = GGAB.GetBool(this.propertyName, this.defaultValue);
		for (int i = 0; i < this.objectsToEnable.Count; i++)
		{
			GGUtil.SetActive(this.objectsToEnable[i], @bool);
		}
	}

	public string propertyName;

	public bool defaultValue;

	public List<Transform> objectsToEnable = new List<Transform>();
}
