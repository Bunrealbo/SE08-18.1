using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPurchaseDialogEconomyPrefab : MonoBehaviour
{
	public void Init(OffersDB.OfferConfig config)
	{
		for (int i = 0; i < this.visualConfigs.Count; i++)
		{
			CurrencyPurchaseDialogEconomyPrefab.NamedVisualConfig namedVisualConfig = this.visualConfigs[i];
			if (namedVisualConfig.IsMatching(config))
			{
				namedVisualConfig.SetLabel(GGFormat.FormatPrice(config.price.cost, false));
				namedVisualConfig.SetActive(true);
			}
			else
			{
				namedVisualConfig.SetActive(false);
			}
		}
	}

	[SerializeField]
	private List<CurrencyPurchaseDialogEconomyPrefab.NamedVisualConfig> visualConfigs = new List<CurrencyPurchaseDialogEconomyPrefab.NamedVisualConfig>();

	[Serializable]
	public class NamedVisualConfig
	{
		public bool IsMatching(OffersDB.OfferConfig config)
		{
			return config.price.currency == this.currency;
		}

		public void SetLabel(string text)
		{
			this.visualConfig.SetLabel(text);
		}

		public void SetActive(bool flag)
		{
			GGUtil.SetActive(this.visualConfig, flag);
		}

		public CurrencyPurchaseDialogEconomyVisualConfig visualConfig;

		public CurrencyType currency;
	}
}
