using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangeVariation : MonoBehaviour
{
	public string groupName
	{
		get
		{
			if (GGUtil.HasText(this.config.groupName))
			{
				return this.config.groupName;
			}
			VariantGroupSetup componentInParent = base.GetComponentInParent<VariantGroupSetup>();
			if (componentInParent != null)
			{
				return componentInParent.settings.name;
			}
			return this.config.groupName;
		}
	}

	public void Apply()
	{
		CarModelSubpart component = base.GetComponent<CarModelSubpart>();
		if (component == null)
		{
			UnityEngine.Debug.Log("NEED TO BE ON THE SAME OBJECT AS SUBPART");
			return;
		}
		List<VariantModification> list = new List<VariantModification>();
		for (int i = 0; i < component.variantModifications.Count; i++)
		{
			VariantModification variantModification = component.variantModifications[i];
			if (variantModification.variantIndex == this.config.variantIndex)
			{
				list.Add(variantModification);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			VariantModification item = list[j];
			component.variantModifications.Remove(item);
		}
		string groupName = this.groupName;
		foreach (MeshRenderer meshRenderer in base.transform.GetComponentsInChildren<MeshRenderer>(true))
		{
			Material[] sharedMaterials = meshRenderer.sharedMaterials;
			for (int l = 0; l < sharedMaterials.Length; l++)
			{
				Material materialToApply = sharedMaterials[l];
				VariantModification variantModification2 = new VariantModification();
				variantModification2.groupName = groupName;
				variantModification2.materialIndex = l;
				if (this.config.materialToApply == null)
				{
					variantModification2.materialToApply = materialToApply;
				}
				else
				{
					variantModification2.materialToApply = this.config.materialToApply;
				}
				variantModification2.rendererToApplyTo = meshRenderer;
				variantModification2.variantIndex = this.config.variantIndex;
				component.variantModifications.Add(variantModification2);
			}
		}
	}

	public MaterialChangeVariation.Config config = new MaterialChangeVariation.Config();

	[Serializable]
	public class Config
	{
		public int variantIndex;

		public Material materialToApply;

		public string groupName;
	}
}
