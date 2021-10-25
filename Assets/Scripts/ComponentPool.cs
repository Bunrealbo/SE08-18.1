using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComponentPool
{
	public Vector2 prefabSizeDelta
	{
		get
		{
			RectTransform component = this.prefab.GetComponent<RectTransform>();
			if (component == null)
			{
				return Vector2.zero;
			}
			if (this.resetScale)
			{
				component.localScale = Vector3.one;
			}
			return component.sizeDelta;
		}
	}

	public void Clear()
	{
		if (GGUtil.isPartOfHierarchy(this.prefab))
		{
			GGUtil.Hide(this.prefab);
		}
		for (int i = this.usedObjects.Count - 1; i >= 0; i--)
		{
			GameObject item = this.usedObjects[i];
			this.notUsedObjects.Add(item);
		}
		this.usedObjects.Clear();
	}

	public void HideNotUsed()
	{
		for (int i = 0; i < this.notUsedObjects.Count; i++)
		{
			GGUtil.SetActive(this.notUsedObjects[i], false);
		}
	}

	public GameObject Instantiate(bool activate = false)
	{
		if (this.prefab == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
		gameObject.transform.parent = this.parent.transform;
		GameObject gameObject2 = gameObject;
		if (this.resetScale)
		{
			gameObject2.transform.localScale = Vector3.one;
		}
		if (this.resetRotation)
		{
			gameObject2.transform.localRotation = Quaternion.identity;
		}
		if (activate)
		{
			GGUtil.SetActive(gameObject2, true);
		}
		return gameObject2;
	}

	public GameObject Next(bool activate = false)
	{
		if (this.prefab == null)
		{
			return null;
		}
		GameObject gameObject;
		if (this.notUsedObjects.Count > 0)
		{
			gameObject = this.notUsedObjects[this.notUsedObjects.Count - 1];
			this.notUsedObjects.RemoveAt(this.notUsedObjects.Count - 1);
		}
		else
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			gameObject2.transform.parent = this.parent.transform;
			gameObject = gameObject2;
			if (this.resetScale)
			{
				gameObject.transform.localScale = Vector3.one;
			}
			if (this.resetRotation)
			{
				gameObject.transform.localRotation = Quaternion.identity;
			}
		}
		if (activate)
		{
			GGUtil.SetActive(gameObject, true);
		}
		this.usedObjects.Add(gameObject);
		return gameObject;
	}

	public T Next<T>(bool activate = false) where T : MonoBehaviour
	{
		return this.Next(activate).GetComponent<T>();
	}

	public Transform parent;

	public GameObject prefab;

	[SerializeField]
	private bool resetScale;

	[SerializeField]
	private bool resetRotation;

	[NonSerialized]
	public List<GameObject> usedObjects = new List<GameObject>();

	private List<GameObject> notUsedObjects = new List<GameObject>();
}
