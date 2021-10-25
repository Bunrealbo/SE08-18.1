using System;
using UnityEngine;

public class SubpartVariant : MonoBehaviour
{
	public CarModelInfo.VariantGroup variantGroup
	{
		get
		{
			if (this.variantGroup_ == null)
			{
				this.variantGroup_ = this.subpart.part.model.modelInfo.GetVariantGroup(this.info.groupName);
			}
			return this.variantGroup_;
		}
	}

	public void ShowIfInActiveVariant()
	{
		CarModelInfo.VariantGroup variantGroup = this.variantGroup;
		int num = 0;
		if (variantGroup != null)
		{
			num = variantGroup.selectedVariationIndex;
		}
		GGUtil.SetActive(this, num == this.info.index);
	}

	public void Init(CarModelSubpart subpart)
	{
		this.subpart = subpart;
	}

	[SerializeField]
	public CarSubPartVariantInfo info = new CarSubPartVariantInfo();

	[SerializeField]
	private CarModelSubpart subpart;

	[NonSerialized]
	private CarModelInfo.VariantGroup variantGroup_;
}
