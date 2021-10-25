using System;
using GGMatch3;
using UnityEngine;

public class GGSetSortingLayer : MonoBehaviour
{
	private void TryGetSkinnedMeshRenderer()
	{
		SkinnedMeshRenderer component = base.GetComponent<SkinnedMeshRenderer>();
		if (component == null)
		{
			return;
		}
		this.sortingLayer.Set(component);
	}

	private void OnEnable()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			this.TryGetSkinnedMeshRenderer();
			return;
		}
		this.sortingLayer.Set(component);
	}

	[SerializeField]
	public SpriteSortingSettings sortingLayer = new SpriteSortingSettings();
}
