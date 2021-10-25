using System;
using TMPro;
using UnityEngine;

public class CurrencyCountLabel : MonoBehaviour
{
	public void Reinit()
	{
		long price = GGPlayerSettings.instance.walletManager.CurrencyCount(CurrencyType.coins);
		GGUtil.ChangeText(this.coinsLabel, GGFormat.FormatPrice(price, false));
	}

	private void OnEnable()
	{
		this.Reinit();
	}

	[SerializeField]
	private CurrencyType currencyType;

	[SerializeField]
	private TextMeshProUGUI coinsLabel;
}
