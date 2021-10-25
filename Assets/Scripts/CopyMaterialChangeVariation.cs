using System;
using UnityEngine;

public class CopyMaterialChangeVariation : MonoBehaviour
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

	[SerializeField]
	public CopyMaterialChangeVariation.Config config = new CopyMaterialChangeVariation.Config();

	[Serializable]
	public class Config
	{
		public int variantIndex;

		public string replace;

		public string replaceWith;

		public string groupName;
	}
}
