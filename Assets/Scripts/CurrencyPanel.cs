using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPanel : MonoBehaviour, InAppBackend.Listener
{
	public CurrencyDisplay DisplayForCurrency(CurrencyType currencyType)
	{
		for (int i = 0; i < this.currencyDisplays.Count; i++)
		{
			CurrencyDisplay currencyDisplay = this.currencyDisplays[i];
			if (currencyDisplay.currencyType == currencyType)
			{
				return currencyDisplay;
			}
		}
		return null;
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	public void OnInitialized(InAppBackend.InitializeArguments initializeArguments)
	{
	}

	public void OnPurchase(InAppBackend.PurchaseEventArguments purchaseParams)
	{
		this.SetLabels();
	}

	public void OnEnable()
	{
		this.SetLabels();
		BehaviourSingletonInit<InAppBackend>.instance.AddListener(this);
	}

	public void SetLabels()
	{
		WalletManager walletManager = GGPlayerSettings.instance.walletManager;
		for (int i = 0; i < this.currencyDisplays.Count; i++)
		{
			CurrencyDisplay currencyDisplay = this.currencyDisplays[i];
			currencyDisplay.Init(walletManager.CurrencyCount(currencyDisplay.currencyType));
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	[SerializeField]
	private List<CurrencyDisplay> currencyDisplays = new List<CurrencyDisplay>();
}
