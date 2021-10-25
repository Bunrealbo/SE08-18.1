using System;
using UnityEngine;

public class VariantGroupSetup : MonoBehaviour
{
	public void Apply()
	{
		CarModel componentInParent = base.transform.GetComponentInParent<CarModel>();
		if (string.IsNullOrWhiteSpace(this.settings.name))
		{
			UnityEngine.Debug.Log("VARIANT GROUP NAME EMPTY " + base.transform.name);
			return;
		}
		componentInParent.modelInfo.RemoveVariantGroup(this.settings.name);
		componentInParent.modelInfo.AddGroup(this.settings);
	}

	[SerializeField]
	public CarModelInfo.VariantGroup settings = new CarModelInfo.VariantGroup();
}
