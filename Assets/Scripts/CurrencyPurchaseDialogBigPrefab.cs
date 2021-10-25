using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPurchaseDialogBigPrefab : MonoBehaviour, InAppBackend.Listener
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
		GGUtil.ChangeText(this.priceLabel, text);
	}

	public void Init(OffersDB.ProductDefinition product)
	{
		this.product = product;
		base.transform.localScale = Vector3.one;
		for (int i = 0; i < this.ribbonLabels.Count; i++)
		{
			if(this.ribbonLabels[i] != null)
				this.ribbonLabels[i].text = product.offer.name;
		}
		bool flag = false;
		this.defaulVisualConfig.SetActive(false);
		for (int j = 0; j < this.visualConfigs.Count; j++)
		{
			CurrencyPurchaseDialogBigPrefab.NamedVisualConfig namedVisualConfig = this.visualConfigs[j];
			if (namedVisualConfig.IsMatching(product))
			{
				namedVisualConfig.SetActive(true);
				flag = true;
			}
			else
			{
				namedVisualConfig.SetActive(false);
			}
		}
		if (!flag)
		{
			this.defaulVisualConfig.SetActive(true);
		}
		this.UpdatePrice();
		List<OffersDB.OfferConfig> config = product.offer.config;
		List<OffersDB.OfferConfig> list = new List<OffersDB.OfferConfig>();
		List<OffersDB.OfferConfig> list2 = new List<OffersDB.OfferConfig>();
		for (int k = 0; k < config.Count; k++)
		{
			OffersDB.OfferConfig offerConfig = config[k];
			if (offerConfig.usePrice)
			{
				list2.Add(offerConfig);
			}
			else
			{
				list.Add(offerConfig);
			}
		}
		CurrencyPurchaseDialog.PageItemInfo pageItemInfo = new CurrencyPurchaseDialog.PageItemInfo();
		CurrencyPurchaseDialog.PageItemInfo pageItemInfo2 = new CurrencyPurchaseDialog.PageItemInfo();
		pageItemInfo2.rank = 1;
		pageItemInfo.rank = 0;
		pageItemInfo2.count = Mathf.CeilToInt((float)list.Count / (float)this.powerupsPerRow);
		pageItemInfo.count = list2.Count;
		pageItemInfo2.space = this.powerupGroupPrefabContainer.rect.size;
		pageItemInfo.space = this.economyPrefabContainer.rect.size;
		CurrencyPurchaseDialog.PageSpace.SpacingInfo spacingInfo = new CurrencyPurchaseDialog.PageSpace.SpacingInfo();
		spacingInfo.direction = Vector2.down;
		spacingInfo.offset = this.offset;
		spacingInfo.size = this.infoContainer.rect.size;
		spacingInfo.size.y = float.PositiveInfinity;
		pageItemInfo2.spacingInfo = spacingInfo;
		pageItemInfo.spacingInfo = spacingInfo;
		List<CurrencyPurchaseDialog.PageItemInfo> list3 = new List<CurrencyPurchaseDialog.PageItemInfo>();
		list3.Add(pageItemInfo2);
		list3.Add(pageItemInfo);
		new CurrencyPurchaseDialog.PageConfig().Pack(list3);
		this.economyPrefabsPool.Clear();
		this.economyPrefabsPool.HideNotUsed();
		this.powerupGroupPrefabPool.Clear();
		this.powerupGroupPrefabPool.HideNotUsed();
		this.powerupPrefabsPool.Clear();
		this.powerupPrefabsPool.HideNotUsed();
		List<RectTransform> list4 = new List<RectTransform>();
		List<RectTransform> list5 = new List<RectTransform>();
		for (int l = 0; l < pageItemInfo2.results.Count - 1; l++)
		{
			OffersDB.OfferConfig offerConfig2 = list[l];
			CurrencyPurchaseDialogMultyPrefab currencyPurchaseDialogMultyPrefab = this.powerupGroupPrefabPool.Next<CurrencyPurchaseDialogMultyPrefab>(true);
			List<RectTransform> buttons = this.CreatePowerupPrefabs(list, l * this.powerupsPerRow, this.powerupsPerRow);
			currencyPurchaseDialogMultyPrefab.Init(buttons);
			CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult = pageItemInfo2.results[l];
			GGUtil.uiUtil.PositionRectInsideRect(this.infoContainer, currencyPurchaseDialogMultyPrefab.transform as RectTransform, fittingResult.position);
			list4.Add(currencyPurchaseDialogMultyPrefab.transform as RectTransform);
		}
		int num = Mathf.Min(pageItemInfo2.results.Count * this.powerupsPerRow, list.Count);
		if (pageItemInfo2.results.Count > 0)
		{
			OffersDB.OfferConfig offerConfig3 = list[pageItemInfo2.results.Count - 1];
			CurrencyPurchaseDialogMultyPrefab currencyPurchaseDialogMultyPrefab2 = this.powerupGroupPrefabPool.Next<CurrencyPurchaseDialogMultyPrefab>(true);
			int length = num % this.powerupsPerRow;
			List<RectTransform> buttons2 = this.CreatePowerupPrefabs(list, (pageItemInfo2.results.Count - 1) * this.powerupsPerRow, length);
			currencyPurchaseDialogMultyPrefab2.Init(buttons2);
			CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult2 = pageItemInfo2.results[pageItemInfo2.results.Count - 1];
			GGUtil.uiUtil.PositionRectInsideRect(this.infoContainer, currencyPurchaseDialogMultyPrefab2.transform as RectTransform, fittingResult2.position);
			list4.Add(currencyPurchaseDialogMultyPrefab2.transform as RectTransform);
		}
		for (int m = 0; m < pageItemInfo.results.Count; m++)
		{
			OffersDB.OfferConfig config2 = list2[m];
			CurrencyPurchaseDialogEconomyPrefab currencyPurchaseDialogEconomyPrefab = this.economyPrefabsPool.Next<CurrencyPurchaseDialogEconomyPrefab>(true);
			currencyPurchaseDialogEconomyPrefab.Init(config2);
			CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult3 = pageItemInfo.results[m];
			GGUtil.uiUtil.PositionRectInsideRect(this.infoContainer, currencyPurchaseDialogEconomyPrefab.transform as RectTransform, fittingResult3.position);
			list5.Add(currencyPurchaseDialogEconomyPrefab.transform as RectTransform);
		}
		Pair<Vector2, Vector2> aabb = GGUtil.uiUtil.GetAABB(list5);
		RectTransform trans = this.economyContainer.parent as RectTransform;
		Vector2 worldDimensions = GGUtil.uiUtil.GetWorldDimensions(trans);
		float num2 = aabb.second.y - aabb.first.y;
		this.economyContainer.anchorMin = new Vector2(0f, 1f - num2 / worldDimensions.y);
		this.economyContainer.anchorMax = Vector2.one;
		this.economyContainer.anchoredPosition = Vector2.zero;
		this.economyContainer.offsetMax = Vector2.zero;
		this.economyContainer.offsetMin = Vector2.zero;
		this.powerupContainer.anchorMin = new Vector2(0f, 0f);
		this.powerupContainer.anchorMax = new Vector2(1f, (worldDimensions.y - num2) / worldDimensions.y);
		this.powerupContainer.anchoredPosition = Vector2.zero;
		this.powerupContainer.offsetMax = Vector2.zero;
		this.powerupContainer.offsetMin = Vector2.zero;
		for (int n = 0; n < list4.Count; n++)
		{
			RectTransform rectTransform = list4[n];
			Vector3 localPosition = rectTransform.transform.localPosition;
			localPosition.z = 0f;
			rectTransform.transform.localPosition = localPosition;
		}
		for (int num3 = 0; num3 < list5.Count; num3++)
		{
			RectTransform rectTransform2 = list5[num3];
			Vector3 localPosition2 = rectTransform2.transform.localPosition;
			localPosition2.z = 0f;
			rectTransform2.transform.localPosition = localPosition2;
		}
		if (this.onlyForShow)
		{
			GGUtil.ChangeText(this.priceLabel, "Purchased");
			CanvasGroup component = base.GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.interactable = false;
				component.blocksRaycasts = false;
			}
		}
	}

	public List<RectTransform> CreatePowerupPrefabs(List<OffersDB.OfferConfig> configs, int startIndex, int length)
	{
		List<RectTransform> list = new List<RectTransform>();
		for (int i = startIndex; i < startIndex + length; i++)
		{
			OffersDB.OfferConfig config = configs[i];
			CurrencyPurchaseDialogPowerupPrefab currencyPurchaseDialogPowerupPrefab = this.powerupPrefabsPool.Next<CurrencyPurchaseDialogPowerupPrefab>(true);
			currencyPurchaseDialogPowerupPrefab.Init(config);
			list.Add(currencyPurchaseDialogPowerupPrefab.transform as RectTransform);
		}
		return list;
	}

	public void ButtonCallback_OnBuyButtonClicked()
	{
		this.clickAnimation.Play(0f, new Action(this.NotifyScreenForClick));
	}

	private void NotifyScreenForClick()
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
	private List<CurrencyPurchaseDialogBigPrefab.NamedVisualConfig> visualConfigs = new List<CurrencyPurchaseDialogBigPrefab.NamedVisualConfig>();

	[SerializeField]
	private CurrencyPurchaseDialogBigPrefab.NamedVisualConfig defaulVisualConfig;

	[SerializeField]
	private List<TextMeshProUGUI> ribbonLabels;

	[SerializeField]
	private RectTransform infoContainer;

	[SerializeField]
	private RectTransform economyPrefabContainer;

	[SerializeField]
	private RectTransform powerupGroupPrefabContainer;

	[SerializeField]
	private ComponentPool economyPrefabsPool = new ComponentPool();

	[SerializeField]
	private ComponentPool powerupPrefabsPool = new ComponentPool();

	[SerializeField]
	private ComponentPool powerupGroupPrefabPool = new ComponentPool();

	[SerializeField]
	private RectTransform powerupContainer;

	[SerializeField]
	private RectTransform economyContainer;

	[SerializeField]
	private Vector2 offset;

	[SerializeField]
	private int powerupsPerRow = 2;

	public OffersDB.ProductDefinition product;

	[SerializeField]
	private CurrencyPurchaseDialog screen;

	[SerializeField]
	public TextMeshProUGUI priceLabel;

	[SerializeField]
	private CurrencyPrefabAnimation animationIn;

	[SerializeField]
	private CurrencyPrefabAnimation animationOut;

	[SerializeField]
	private CurrencyPrefabAnimation clickAnimation;

	[Serializable]
	public class NamedVisualConfig
	{
		public bool IsMatching(OffersDB.ProductDefinition product)
		{
			return product.offer.name == this.name;
		}

		public void SetActive(bool flag)
		{
			GGUtil.SetActive(this.image.gameObject, flag);
		}

		public Image image;

		public string name;
	}
}
