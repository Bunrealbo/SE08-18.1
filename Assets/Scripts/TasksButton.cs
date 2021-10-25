using System;
using System.Collections.Generic;
using UnityEngine;

public class TasksButton : MonoBehaviour
{
	public void Show(CarScene scene)
	{
		GGPlayerSettings instance = GGPlayerSettings.instance;
		List<CarModelPart> list = scene.carModel.AvailablePartsAsTasks();
		bool active = false;
		for (int i = 0; i < list.Count; i++)
		{
			CarModelPart carModelPart = list[i];
			SingleCurrencyPrice price = new SingleCurrencyPrice(1, CurrencyType.diamonds);
			if (instance.walletManager.CanBuyItemWithPrice(price))
			{
				active = true;
				break;
			}
		}
		GGUtil.SetActive(this.animationTransform, active);
	}

	public void HideAnimation()
	{
		GGUtil.Hide(this.animationTransform);
	}

	[SerializeField]
	private Transform animationTransform;
}
