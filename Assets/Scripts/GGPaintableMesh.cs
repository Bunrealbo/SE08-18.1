using System;
using UnityEngine;

public class GGPaintableMesh : MonoBehaviour
{
	public int uvIndex
	{
		get
		{
			return (int)this.uvIndex_;
		}
	}

	public Mesh cachedMesh
	{
		get
		{
			if (this.cachedMesh_ == null)
			{
				MeshFilter component = base.GetComponent<MeshFilter>();
				if (component == null)
				{
					return null;
				}
				this.cachedMesh_ = component.sharedMesh;
			}
			return this.cachedMesh_;
		}
	}

	public MeshRenderer meshRenderer
	{
		get
		{
			if (this.meshRenderer_ == null)
			{
				this.meshRenderer_ = base.GetComponent<MeshRenderer>();
			}
			return this.meshRenderer_;
		}
	}

	[SerializeField]
	private GGPaintableMesh.UVIndex uvIndex_;

	[SerializeField]
	public bool ignoreForRendering;

	[NonSerialized]
	private Mesh cachedMesh_;

	[NonSerialized]
	private MeshRenderer meshRenderer_;

	public enum UVIndex
	{
		UV0,
		UV1
	}
}
