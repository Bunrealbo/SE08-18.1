using System;
using UnityEngine;

public class CurrencyPurchaseCurrencyPrefab : MonoBehaviour
{
	public void Init(OffersDB.OfferConfig offer)
	{
		SingleCurrencyPrice price = offer.price;
		this.visualConfig.SetLabel(GGFormat.FormatPrice(price.cost, false));
	}

	[SerializeField]
	private CurrencyPurchaseCurrencyVisualConfig visualConfig;
}
