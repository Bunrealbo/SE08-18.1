using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class CharacterVisualObjectSceneItem : MonoBehaviour
{
	public void InitForRuntime()
	{
		if (this.meshes.Count == 0)
		{
			return;
		}
		Material material = this.meshes[0].material;
		material.SetInt("_Stencil", this.stencilIndex);
		for (int i = 0; i < this.meshes.Count; i++)
		{
			this.meshes[i].material = material;
		}
	}

	public void SetActive(bool isActive)
	{
		GGUtil.SetActive(this, isActive);
	}

	public void Init(DecoratingScene3DSetup.VisualObject visualObject, Material materialToReplace, SpriteSortingSettings sortingSettings)
	{
		this.meshes.Clear();
		this.InitWithTransform(base.transform, visualObject.collisionRoot.name);
		this.lookAtPosition = Vector3.zero;
		for (int i = 0; i < this.meshes.Count; i++)
		{
			MeshRenderer meshRenderer = this.meshes[i];
			meshRenderer.sharedMaterial = materialToReplace;
			if (sortingSettings != null)
			{
				meshRenderer.gameObject.AddComponent<GGSetSortingLayer>().sortingLayer = sortingSettings;
			}
			this.lookAtPosition += meshRenderer.bounds.center;
		}
		if (this.meshes.Count > 0)
		{
			this.lookAtPosition /= (float)this.meshes.Count;
			this.isLookAtPositionDefined = true;
		}
	}

	private void InitWithTransform(Transform root, string nameToSearchFor)
	{
		string name = root.name;
		if (name == nameToSearchFor)
		{
			this.FillMeshes(root);
			return;
		}
		if (name.ToLower().StartsWith("data") || root == base.transform)
		{
			List<Transform> list = new List<Transform>();
			foreach (object obj in root)
			{
				Transform item = (Transform)obj;
				list.Add(item);
			}
			for (int i = 0; i < list.Count; i++)
			{
				Transform root2 = list[i];
				this.InitWithTransform(root2, nameToSearchFor);
			}
			return;
		}
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(root.gameObject);
			return;
		}
		UnityEngine.Object.DestroyImmediate(root.gameObject);
	}

	private void FillMeshes(Transform root)
	{
		MeshRenderer component = root.GetComponent<MeshRenderer>();
		if (component != null)
		{
			this.meshes.Add(component);
		}
		foreach (object obj in root)
		{
			Transform root2 = (Transform)obj;
			this.FillMeshes(root2);
		}
	}

	[SerializeField]
	private List<MeshRenderer> meshes = new List<MeshRenderer>();

	[SerializeField]
	public Vector3 lookAtPosition;

	[SerializeField]
	public bool isLookAtPositionDefined;

	[SerializeField]
	public int stencilIndex;
}
