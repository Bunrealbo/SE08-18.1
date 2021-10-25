using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class CurrencyPurchaseDialogPowerupPrefab : MonoBehaviour
{
	public void Init(OffersDB.OfferConfig config)
	{
		for (int i = 0; i < this.visualConfigs.Count; i++)
		{
			CurrencyPurchaseDialogPowerupPrefab.NamedVisualConfigs namedVisualConfigs = this.visualConfigs[i];
			if (namedVisualConfigs.IsMatching(config))
			{
				namedVisualConfigs.SetLabel(string.Format("x {0}", config.count));
				namedVisualConfigs.SetActive(true);
			}
			else
			{
				namedVisualConfigs.SetActive(false);
			}
		}
	}

	[SerializeField]
	private List<CurrencyPurchaseDialogPowerupPrefab.NamedVisualConfigs> visualConfigs = new List<CurrencyPurchaseDialogPowerupPrefab.NamedVisualConfigs>();

	[Serializable]
	public class NamedVisualConfigs
	{
		public bool IsMatching(OffersDB.OfferConfig config)
		{
			return config.boosterType == this.type;
		}

		public void SetLabel(string text)
		{
			this.visualConfig.SetLabel(text);
		}

		public void SetActive(bool flag)
		{
			GGUtil.SetActive(this.visualConfig.gameObject, flag);
		}

		public CurrencyPurchaseDialogPowerupVisualConfig visualConfig;

		public BoosterType type;
	}
}
