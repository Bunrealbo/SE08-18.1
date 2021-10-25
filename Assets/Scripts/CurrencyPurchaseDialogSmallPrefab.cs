using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyPurchaseDialogSmallPrefab : MonoBehaviour, InAppBackend.Listener
{
	private void OnEnable()
	{
		if (this.onlyForShow)
		{
			return;
		}
		BehaviourSingletonInit<InAppBackend>.instance.AddListener(this);
	}

	private void OnDisable()
	{
		if (this.onlyForShow)
		{
			return;
		}
		BehaviourSingletonInit<InAppBackend>.instance.RemoveListener(this);
	}

	public void OnInitialized(InAppBackend.InitializeArguments initializeArguments)
	{
		if (this.onlyForShow)
		{
			return;
		}
		if (initializeArguments.isSuccess)
		{
			this.UpdatePrice();
		}
	}

	public void OnPurchase(InAppBackend.PurchaseEventArguments purchaseParams)
	{
	}

	private void UpdatePrice()
	{
		string productId = null;
		if (this.product != null)
		{
			productId = this.product.productID;
		}
		string text = BehaviourSingletonInit<InAppBackend>.instance.LocalisedPriceString(productId);
		if (Application.isEditor && !string.IsNullOrEmpty(this.product.mocupPrice))
		{
			text = this.product.mocupPrice;
		}
		GGUtil.ChangeText(this.label, text);
	}

	public void Init(OffersDB.ProductDefinition product)
	{
		this.product = product;
		base.transform.localScale = Vector3.one;
		OffersDB.OfferConfig offerConfig = product.offer.config[0];
		for (int i = 0; i < this.visualConfigs.Count; i++)
		{
			CurrencyPurchaseDialogEconomyPrefab.NamedVisualConfig namedVisualConfig = this.visualConfigs[i];
			if (namedVisualConfig.IsMatching(offerConfig))
			{
				namedVisualConfig.SetLabel(GGFormat.FormatPrice(offerConfig.price.cost, false));
				namedVisualConfig.SetActive(true);
			}
			else
			{
				namedVisualConfig.SetActive(false);
			}
		}
		this.UpdatePrice();
		if (this.onlyForShow)
		{
			GGUtil.ChangeText(this.label, "Purchased");
			CanvasGroup component = base.GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.interactable = false;
				component.blocksRaycasts = false;
			}
		}
	}

	public void ButtonCallback_OnBuyButtonPressed()
	{
		this.animationClick.Play(0f, new Action(this.NotifyScreenButtonPress));
	}

	public void NotifyScreenButtonPress()
	{
		if (this.screen != null)
		{
			this.screen.OnButtonPressed(this);
		}
	}

	public void AnimateIn(float delay)
	{
		this.animationIn.Init();
		this.animationIn.Play(delay, null);
	}

	public void AnimateOut(float delay)
	{
		this.animationOut.Play(delay, null);
	}

	public IEnumerator DoAnimateIn(float delay)
	{
		return this.animationIn.DoPlay(delay, null);
	}

	public IEnumerator DoAnimateOut(float delay)
	{
		return this.animationOut.DoPlay(delay, null);
	}

	[SerializeField]
	private bool onlyForShow;

	[SerializeField]
	private List<CurrencyPurchaseDialogEconomyPrefab.NamedVisualConfig> visualConfigs = new List<CurrencyPurchaseDialogEconomyPrefab.NamedVisualConfig>();

	[SerializeField]
	private CurrencyPurchaseDialog screen;

	public OffersDB.ProductDefinition product;

	[SerializeField]
	public TextMeshProUGUI label;

	[SerializeField]
	private CurrencyPrefabAnimation animationIn;

	[SerializeField]
	private CurrencyPrefabAnimation animationOut;

	[SerializeField]
	private CurrencyPrefabAnimation animationClick;
}
