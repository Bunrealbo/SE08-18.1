using System;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityHelper
{
	public void Clear()
	{
		this.visibilities.Clear();
	}

	public void SaveIsVisible(List<RectTransform> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			RectTransform rectTransform = list[i];
			if (!(rectTransform == null))
			{
				this.SaveIsVisible(rectTransform.gameObject);
			}
		}
	}

	public void SaveIsVisible(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		this.SetActive(go, go.activeSelf);
	}

	public void SetActive(List<Transform> list, bool isVisible)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = list[i];
			if (!(transform == null))
			{
				this.SetActive(transform.gameObject, isVisible);
			}
		}
	}

	public void SetActive(Transform transform, bool isVisible)
	{
		if (transform == null)
		{
			return;
		}
		this.SetActive(transform.gameObject, isVisible);
	}

	public void SetActive(GameObject go, bool isVisible)
	{
		for (int i = 0; i < this.visibilities.Count; i++)
		{
			VisibilityHelper.VisibilitySetting visibilitySetting = this.visibilities[i];
			if (visibilitySetting.go == go)
			{
				visibilitySetting.isVisible = isVisible;
				this.visibilities[i] = visibilitySetting;
				return;
			}
		}
		VisibilityHelper.VisibilitySetting item = default(VisibilityHelper.VisibilitySetting);
		item.go = go;
		item.isVisible = isVisible;
		this.visibilities.Add(item);
	}

	public void Complete()
	{
		for (int i = 0; i < this.visibilities.Count; i++)
		{
			VisibilityHelper.VisibilitySetting visibilitySetting = this.visibilities[i];
			GGUtil.SetActive(visibilitySetting.go, visibilitySetting.isVisible);
		}
	}

	private List<VisibilityHelper.VisibilitySetting> visibilities = new List<VisibilityHelper.VisibilitySetting>();

	private struct VisibilitySetting
	{
		public GameObject go;

		public bool isVisible;
	}
}
