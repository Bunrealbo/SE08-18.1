using System;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DistortedSprite : MonoBehaviour
{
	public Vector3 bl
	{
		set
		{
			this.distortedImageRenderer.bl = value;
		}
	}

	public Vector3 tl
	{
		set
		{
			this.distortedImageRenderer.tl = value;
		}
	}

	public Vector3 br
	{
		set
		{
			this.distortedImageRenderer.br = value;
		}
	}

	public Vector3 tr
	{
		set
		{
			this.distortedImageRenderer.tr = value;
		}
	}

	public MeshFilter meshFilter
	{
		get
		{
			if (!this._meshFilter)
			{
				this._meshFilter = base.GetComponent<MeshFilter>();
			}
			return this._meshFilter;
		}
	}

	public MeshRenderer meshRenderer
	{
		get
		{
			if (!this._meshRenderer)
			{
				this._meshRenderer = base.GetComponent<MeshRenderer>();
			}
			return this._meshRenderer;
		}
	}

	public void CreateGeometry()
	{
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.name = base.name + "_BRMesh";
		}
		this.distortedImageRenderer.PopulateMesh(this.vertexHelper);
		this.vertexHelper.FillMesh(this.mesh);
		this.meshFilter.mesh = this.mesh;
		this.meshRenderer.sortingLayerID = this.sortingSettings.sortingLayerId;
		this.meshRenderer.sortingOrder = this.sortingSettings.sortingOrder;
	}

	[SerializeField]
	private DistortedImageRenderer distortedImageRenderer = new DistortedImageRenderer();

	[SerializeField]
	public SpriteSortingSettings sortingSettings = new SpriteSortingSettings();

	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	private Mesh mesh;

	private VertexHelper vertexHelper = new VertexHelper();
}
