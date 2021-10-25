using System;
using GGMatch3;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
	public void OnClick()
	{
		GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.coins, (int)this.coins.Random());
	}

	[SerializeField]
	private LoadingPanel panel;

	[SerializeField]
	private FloatRange coins;
}
