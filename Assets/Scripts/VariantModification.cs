using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VariantModification
{
	public bool IsApplicable(int index)
	{
		return this.variantIndex == index;
	}

	public void Apply(bool useSharedMaterial)
	{
		if (this.materialToApply == null || this.rendererToApplyTo == null)
		{
			return;
		}
		if (!useSharedMaterial)
		{
			if (this.materialIndex > 0)
			{
				this.materialsHelper.Clear();
				this.rendererToApplyTo.GetMaterials(this.materialsHelper);
				if (this.materialsHelper[this.materialIndex] != this.materialToApply)
				{
					this.materialsHelper[this.materialIndex] = this.materialToApply;
					this.rendererToApplyTo.materials = this.materialsHelper.ToArray();
					return;
				}
			}
			else
			{
				this.rendererToApplyTo.material = this.materialToApply;
			}
			return;
		}
		if (this.materialIndex > 0)
		{
			Material[] sharedMaterials = this.rendererToApplyTo.sharedMaterials;
			sharedMaterials[this.materialIndex] = this.materialToApply;
			this.rendererToApplyTo.sharedMaterials = sharedMaterials;
			return;
		}
		this.rendererToApplyTo.sharedMaterial = this.materialToApply;
	}

	[SerializeField]
	public int variantIndex;

	[SerializeField]
	public string groupName;

	[SerializeField]
	public Material materialToApply;

	[SerializeField]
	public int materialIndex;

	[SerializeField]
	public MeshRenderer rendererToApplyTo;

	private List<Material> materialsHelper = new List<Material>();
}
