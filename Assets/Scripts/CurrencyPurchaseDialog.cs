using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPurchaseDialog : UILayer
{
	public void Show(OffersDB offers, Action onHide = null)
	{
		this.onHide = onHide;
		this.offers = offers;
		this.type = CurrencyPurchaseDialog.PageType.FirstPage;
		NavigationManager.instance.Push(base.gameObject, false);
	}

	public void Init()
	{
		this.Init(this.offers, this.onHide, this.type);
	}

	public void Init(OffersDB offers, Action onHide, CurrencyPurchaseDialog.PageType type)
	{
		this.onHide = onHide;
		this.offers = offers;
		this.type = type;
		this.offersContainer.anchorMin = Vector2.zero;
		this.offersContainer.anchorMax = Vector2.one;
		this.offersContainer.anchoredPosition = Vector2.zero;
		this.offersContainer.offsetMin = Vector2.zero;
		this.offersContainer.offsetMax = Vector2.zero;
		this.unamedOffersPool.parent.localScale = Vector3.one;
		if(this.coinsCurrencyLabel != null)
			this.coinsCurrencyLabel.text = GGFormat.FormatPrice(GGPlayerSettings.instance.walletManager.CurrencyCount(CurrencyType.coins), false);
		List<OffersDB.ProductDefinition> products = offers.products;
		List<OffersDB.ProductDefinition> list = new List<OffersDB.ProductDefinition>();
		List<OffersDB.ProductDefinition> list2 = new List<OffersDB.ProductDefinition>();
		for (int i = 0; i < products.Count; i++)
		{
			OffersDB.ProductDefinition productDefinition = products[i];
			if (productDefinition.active)
			{
				if (productDefinition.offer.isNamedOffer)
				{
					list.Add(productDefinition);
				}
				else
				{
					list2.Add(productDefinition);
				}
			}
		}
		Vector2 size = this.offersContainer.rect.size;
		CurrencyPurchaseDialog.PageItemInfo pageItemInfo = new CurrencyPurchaseDialog.PageItemInfo();
		CurrencyPurchaseDialog.PageItemInfo pageItemInfo2 = new CurrencyPurchaseDialog.PageItemInfo();
		pageItemInfo.count = list.Count;
		pageItemInfo.space = this.namedOffersContainer.rect.size;
		pageItemInfo.rank = 0;
		CurrencyPurchaseDialog.PageSpace.SpacingInfo spacingInfo = new CurrencyPurchaseDialog.PageSpace.SpacingInfo();
		spacingInfo.offset = this.spacingBigPrefabs;
		spacingInfo.size = size;
		spacingInfo.direction = Vector2.right;
		spacingInfo.size.x = float.PositiveInfinity;
		spacingInfo.groupOffset = this.groupsSpacing;
		pageItemInfo.spacingInfo = spacingInfo;
		if (type == CurrencyPurchaseDialog.PageType.FirstPage)
		{
			pageItemInfo2.count = list2.Count / this.smallItemsPerGroup + 1;
		}
		else
		{
			pageItemInfo2.count = Mathf.CeilToInt((float)list2.Count / (float)this.smallItemsPerGroup);
		}
		pageItemInfo2.space = this.unamedOffersContainer.rect.size;
		pageItemInfo2.rank = 1;
		CurrencyPurchaseDialog.PageSpace.SpacingInfo spacingInfo2 = new CurrencyPurchaseDialog.PageSpace.SpacingInfo();
		spacingInfo2.offset = this.spacingSmallPrefabs;
		spacingInfo2.size = size;
		spacingInfo2.direction = Vector2.right;
		spacingInfo2.size.x = float.PositiveInfinity;
		spacingInfo2.groupOffset = this.groupsSpacing;
		pageItemInfo2.spacingInfo = spacingInfo2;
		List<CurrencyPurchaseDialog.PageItemInfo> list3 = new List<CurrencyPurchaseDialog.PageItemInfo>();
		list3.Add(pageItemInfo);
		list3.Add(pageItemInfo2);
		if (type == CurrencyPurchaseDialog.PageType.FirstPage)
		{
			this.firstPageConfig.Pack(list3);
		}
		else
		{
			this.defaultPageConfig.Pack(list3);
		}
		this.namedOffersPool.Clear();
		this.namedOffersPool.HideNotUsed();
		this.bigItems.Clear();
		for (int j = 0; j < pageItemInfo.results.Count; j++)
		{
			Vector2 position = pageItemInfo.results[j].position;
			CurrencyPurchaseDialogBigPrefab currencyPurchaseDialogBigPrefab = this.namedOffersPool.Next<CurrencyPurchaseDialogBigPrefab>(true);
			OffersDB.ProductDefinition product = list[j];
			currencyPurchaseDialogBigPrefab.Init(product);
			GGUtil.uiUtil.GetWorldDimensions(this.offersContainer);
			GGUtil.uiUtil.PositionRectInsideRect(this.offersContainer, currencyPurchaseDialogBigPrefab.transform as RectTransform, position);
			this.bigItems.Add(currencyPurchaseDialogBigPrefab);
		}
		this.unamedOffersPool.Clear();
		this.unamedOffersPool.HideNotUsed();
		this.unamedGroupContainerPool.Clear();
		this.unamedGroupContainerPool.HideNotUsed();
		int num = Mathf.Min(this.smallItemsPerGroup * pageItemInfo2.results.Count, list2.Count + 1);
		this.groupSmallItems.Clear();
		this.smallItems.Clear();
		for (int k = 0; k < pageItemInfo2.results.Count - 1; k++)
		{
			List<RectTransform> buttons = this.CreateSmallPrefabs(list2, k * this.smallItemsPerGroup, this.smallItemsPerGroup);
			CurrencyPurchaseDialogMultyPrefab currencyPurchaseDialogMultyPrefab = this.unamedGroupContainerPool.Next<CurrencyPurchaseDialogMultyPrefab>(true);
			currencyPurchaseDialogMultyPrefab.Init(buttons);
			CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult = pageItemInfo2.results[k];
			GGUtil.uiUtil.PositionRectInsideRect(this.offersContainer, currencyPurchaseDialogMultyPrefab.transform as RectTransform, fittingResult.position);
			this.groupSmallItems.Add(currencyPurchaseDialogMultyPrefab);
		}
		if (pageItemInfo2.results.Count > 0)
		{
			CurrencyPurchaseDialogMultyPrefab currencyPurchaseDialogMultyPrefab2 = this.unamedGroupContainerPool.Next<CurrencyPurchaseDialogMultyPrefab>(true);
			if (type == CurrencyPurchaseDialog.PageType.FirstPage)
			{
				int num2 = this.smallItemsPerGroup - num % this.smallItemsPerGroup;
				List<RectTransform> list4 = this.CreateSmallPrefabs(list2, (pageItemInfo2.results.Count - 1) * this.smallItemsPerGroup, num2 - 1);
				list4.Add(this.nextButtonContainer);
				currencyPurchaseDialogMultyPrefab2.Init(list4);
			}
			else
			{
				int length = this.smallItemsPerGroup - num % this.smallItemsPerGroup;
				List<RectTransform> buttons2 = this.CreateSmallPrefabs(list2, (pageItemInfo2.results.Count - 1) * this.smallItemsPerGroup, length);
				currencyPurchaseDialogMultyPrefab2.Init(buttons2);
			}
			CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult2 = pageItemInfo2.results[pageItemInfo2.results.Count - 1];
			GGUtil.uiUtil.PositionRectInsideRect(this.offersContainer, currencyPurchaseDialogMultyPrefab2.transform as RectTransform, fittingResult2.position);
			this.groupSmallItems.Add(currencyPurchaseDialogMultyPrefab2);
		}
		if (type == CurrencyPurchaseDialog.PageType.FirstPage)
		{
			GGUtil.SetActive(this.nextButtonContainer, true);
		}
		else
		{
			GGUtil.SetActive(this.nextButtonContainer, false);
		}
		List<RectTransform> list5 = new List<RectTransform>();
		for (int l = 0; l < this.bigItems.Count; l++)
		{
			RectTransform item = this.bigItems[l].transform as RectTransform;
			list5.Add(item);
		}
		for (int m = 0; m < this.smallItems.Count; m++)
		{
			RectTransform item2 = this.smallItems[m].transform as RectTransform;
			list5.Add(item2);
		}
		for (int n = 0; n < this.groupSmallItems.Count; n++)
		{
			RectTransform item3 = this.groupSmallItems[n].transform as RectTransform;
			list5.Add(item3);
		}
		Pair<Vector2, Vector2> aabb = GGUtil.uiUtil.GetAABB(list5);
		Vector3 b = aabb.first * 0.5f + aabb.second * 0.5f;
		Vector3 vector = this.offersContainer.transform.position - b;
		for (int num3 = 0; num3 < this.bigItems.Count; num3++)
		{
			this.bigItems[num3].transform.position += vector;
		}
		for (int num4 = 0; num4 < this.groupSmallItems.Count; num4++)
		{
			this.groupSmallItems[num4].transform.position += vector;
		}
		Pair<Vector2, Vector2> pair = aabb;
		pair.first.x = pair.first.x + vector.x;
		Pair<Vector2, Vector2> pair2 = aabb;
		pair2.first.y = pair2.first.y + vector.y;
		Pair<Vector2, Vector2> pair3 = aabb;
		pair3.second.x = pair3.second.x + vector.x;
		Pair<Vector2, Vector2> pair4 = aabb;
		pair4.second.y = pair4.second.y + vector.y;
		RectTransform trans = this.offersContainer.parent as RectTransform;
		Vector2 worldDimensions = GGUtil.uiUtil.GetWorldDimensions(trans);
		float num5 = (aabb.second.x - aabb.first.x + this.totalScrollingPaddingHorizontalWorldSpace) / worldDimensions.x;
		this.offersContainer.anchorMin = new Vector2(-num5 * 0.5f, this.offersContainer.anchorMin.y);
		this.offersContainer.anchorMax = new Vector2(num5 * 0.5f, this.offersContainer.anchorMax.y);
		this.offersContainer.anchoredPosition = new Vector2(0f, this.offersContainer.anchoredPosition.y);
		this.offersContainer.offsetMin = new Vector2(0f, this.offersContainer.offsetMin.y);
		this.offersContainer.offsetMax = new Vector2(0f, this.offersContainer.offsetMax.y);
		this.scrollRect.horizontalNormalizedPosition = 0f;
		if (type == CurrencyPurchaseDialog.PageType.FirstPage)
		{
			Camera camera = NavigationManager.instance.GetCamera();
			Vector2 worldDimensions2 = GGUtil.uiUtil.GetWorldDimensions(this.offersContainer);
			GGUtil.uiUtil.AnchorRectInsideScreen(this.offersContainer, camera);
			Vector2 worldDimensions3 = GGUtil.uiUtil.GetWorldDimensions(this.offersContainer);
			float d = Mathf.Min(worldDimensions3.x / worldDimensions2.x, worldDimensions3.y / worldDimensions2.y);
			this.namedOffersPool.parent.localScale = (Vector3.right + Vector3.up) * d + Vector3.forward;
		}
		float num6 = 0f;
		for (int num7 = 0; num7 < this.bigItems.Count; num7++)
		{
			this.bigItems[num7].AnimateIn(num6);
			num6 += this.animationDelayPerColumn;
		}
		for (int num8 = 0; num8 < this.smallItems.Count; num8++)
		{
			this.smallItems[num8].AnimateIn(num6);
			if ((num8 + 1) % this.smallItemsPerGroup == 0)
			{
				num6 += this.animationDelayPerColumn;
			}
		}
		this.nextButton.AnimateIn(num6);
	}

	private List<RectTransform> CreateSmallPrefabs(List<OffersDB.ProductDefinition> products, int startIndex, int length)
	{
		List<RectTransform> list = new List<RectTransform>();
		for (int i = startIndex; i < startIndex + length; i++)
		{
			OffersDB.ProductDefinition product = products[i];
			CurrencyPurchaseDialogSmallPrefab currencyPurchaseDialogSmallPrefab = this.unamedOffersPool.Next<CurrencyPurchaseDialogSmallPrefab>(true);
			currencyPurchaseDialogSmallPrefab.Init(product);
			this.smallItems.Add(currencyPurchaseDialogSmallPrefab);
			list.Add(currencyPurchaseDialogSmallPrefab.transform as RectTransform);
		}
		return list;
	}

	public void ButtonCallback_Next()
	{
		this.type = CurrencyPurchaseDialog.PageType.AllPage;
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
		this.animateOutEnum = this.DoAnimateOut(new Action(this.Init));
	}

	public void ButtonCallback_Close()
	{
		this.Hide();
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public override void OnGoBack(NavigationManager nav)
	{
		this.Hide();
	}

	private void Hide()
	{
		this.animateOutEnum = this.DoAnimateOut(new Action(this.OnHideEnd));
	}

	private void OnHideEnd()
	{
		if (this.onHide != null)
		{
			this.onHide();
		}
		NavigationManager.instance.Pop(true);
	}

	private IEnumerator DoAnimateOut(Action onEnd = null)
	{
		return new CurrencyPurchaseDialog._003CDoAnimateOut_003Ed__35(0)
		{
			_003C_003E4__this = this,
			onEnd = onEnd
		};
	}

	public void Update()
	{
		if (this.animateOutEnum != null)
		{
			this.animateOutEnum.MoveNext();
		}
	}

	public void OnButtonPressed(CurrencyPurchaseDialogSmallPrefab button)
	{
		OffersDB.ProductDefinition product = button.product;
		this.BuyOffer(product);
	}

	public void OnButtonPressed(CurrencyPurchaseDialogBigPrefab button)
	{
		OffersDB.ProductDefinition product = button.product;
		this.BuyOffer(product);
	}

	private void BuyOffer(OffersDB.ProductDefinition product)
	{
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
		NavigationManager instance = NavigationManager.instance;
		instance.Pop(false);
		instance.GetObject<InAppPurchaseConfirmScreen>().Show(new InAppPurchaseConfirmScreen.PurchaseArguments
		{
			productToBuy = product
		});
	}

	private void OnEnable()
	{
		this.Init();
		GGInAppPurchase instance = GGInAppPurchase.instance;
		if (!instance.IsInventoryAvailable())
		{
			instance.QueryInventory();
		}
	}

	private Action onHide;

	[SerializeField]
	private CurrencyPurchaseDialog.LimitTypePageConfig firstPageConfig = new CurrencyPurchaseDialog.LimitTypePageConfig();

	private CurrencyPurchaseDialog.PageConfig defaultPageConfig = new CurrencyPurchaseDialog.PageConfig();

	[SerializeField]
	private ComponentPool namedOffersPool = new ComponentPool();

	[SerializeField]
	private ComponentPool unamedOffersPool = new ComponentPool();

	[SerializeField]
	private ComponentPool unamedGroupContainerPool = new ComponentPool();

	[SerializeField]
	private RectTransform offersContainer;

	[SerializeField]
	private RectTransform namedOffersContainer;

	[SerializeField]
	private RectTransform unamedOffersContainer;

	[SerializeField]
	private RectTransform nextButtonContainer;

	[SerializeField]
	private Vector2 spacingBigPrefabs;

	[SerializeField]
	private Vector2 spacingSmallPrefabs;

	[SerializeField]
	private Vector2 groupsSpacing;

	[SerializeField]
	private int smallItemsPerGroup = 2;

	[SerializeField]
	private TextMeshProUGUI coinsCurrencyLabel;

	[SerializeField]
	private float totalScrollingPaddingHorizontalWorldSpace;

	[SerializeField]
	private ScrollRect scrollRect;

	private OffersDB offers;

	[SerializeField]
	private CurrencyPurchaseDialogNextButton nextButton;

	[SerializeField]
	private float animationDelayPerColumn = 0.1f;

	private List<CurrencyPurchaseDialogMultyPrefab> groupSmallItems = new List<CurrencyPurchaseDialogMultyPrefab>();

	private List<CurrencyPurchaseDialogSmallPrefab> smallItems = new List<CurrencyPurchaseDialogSmallPrefab>();

	private List<CurrencyPurchaseDialogBigPrefab> bigItems = new List<CurrencyPurchaseDialogBigPrefab>();

	private CurrencyPurchaseDialog.PageType type;

	private IEnumerator animateOutEnum;

	public enum PageType
	{
		FirstPage,
		AllPage
	}

	[Serializable]
	public class LimitTypePageConfig
	{
		public void Pack(List<CurrencyPurchaseDialog.PageItemInfo> infos)
		{
			this.pageSpace.Clear();
			infos.Sort(new Comparison<CurrencyPurchaseDialog.PageItemInfo>(CurrencyPurchaseDialog.PageConfig.Sort_Rank));
			CurrencyPurchaseDialog.PageItemInfo pageItemInfo = infos[0];
			CurrencyPurchaseDialog.PageItemInfo pageItemInfo2 = infos[1];
			this.pageSpace.Init(pageItemInfo.spacingInfo);
			int num = 0;
			while (num < pageItemInfo.count && pageItemInfo.results.Count < this.maxBigElements)
			{
				CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult = this.pageSpace.TryToFit(pageItemInfo.space);
				if (!fittingResult.succeeded)
				{
					break;
				}
				pageItemInfo.results.Add(fittingResult);
				num++;
			}
			this.pageSpace.occupiedSpace += Vector2.Scale(pageItemInfo.spacingInfo.groupOffset, pageItemInfo.spacingInfo.direction).magnitude;
			this.pageSpace.Init(pageItemInfo2.spacingInfo);
			int num2 = 0;
			while (num2 < pageItemInfo2.count && pageItemInfo2.results.Count < this.maxSmallElements)
			{
				CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult2 = this.pageSpace.TryToFit(pageItemInfo2.space);
				if (!fittingResult2.succeeded)
				{
					break;
				}
				pageItemInfo2.results.Add(fittingResult2);
				num2++;
			}
		}

		[SerializeField]
		private int maxBigElements;

		[SerializeField]
		private int maxSmallElements;

		private CurrencyPurchaseDialog.PageSpace pageSpace = new CurrencyPurchaseDialog.PageSpace();
	}

	[Serializable]
	public class PageConfig
	{
		public virtual void Pack(List<CurrencyPurchaseDialog.PageItemInfo> infos)
		{
			this.pageSpace.Clear();
			infos.Sort(new Comparison<CurrencyPurchaseDialog.PageItemInfo>(CurrencyPurchaseDialog.PageConfig.Sort_Rank));
			for (int i = 0; i < infos.Count; i++)
			{
				CurrencyPurchaseDialog.PageItemInfo pageItemInfo = infos[i];
				CurrencyPurchaseDialog.PageSpace.SpacingInfo spacingInfo = pageItemInfo.spacingInfo;
				this.pageSpace.Init(spacingInfo);
				for (int j = 0; j < pageItemInfo.count; j++)
				{
					CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult = this.pageSpace.TryToFit(pageItemInfo.space);
					if (!fittingResult.succeeded)
					{
						break;
					}
					pageItemInfo.results.Add(fittingResult);
				}
				this.pageSpace.occupiedSpace += Vector2.Scale(spacingInfo.groupOffset, spacingInfo.direction).magnitude;
			}
		}

		public static int Sort_Rank(CurrencyPurchaseDialog.PageItemInfo a, CurrencyPurchaseDialog.PageItemInfo b)
		{
			return a.rank.CompareTo(b.rank);
		}

		private CurrencyPurchaseDialog.PageSpace pageSpace = new CurrencyPurchaseDialog.PageSpace();
	}

	public class PageItemInfo
	{
		public Vector2 space;

		public int count;

		public List<CurrencyPurchaseDialog.PageSpace.FittingResult> results = new List<CurrencyPurchaseDialog.PageSpace.FittingResult>();

		public int rank;

		public CurrencyPurchaseDialog.PageSpace.SpacingInfo spacingInfo;
	}

	public class PageSpace
	{
		public void Clear()
		{
			this.occupiedSpace = 0f;
		}

		public float freeSpace
		{
			get
			{
				return this.spaceInfo.totalSize - this.occupiedSpace;
			}
		}

		public void Init(CurrencyPurchaseDialog.PageSpace.SpacingInfo info)
		{
			this.spaceInfo = info;
		}

		public CurrencyPurchaseDialog.PageSpace.FittingResult TryToFit(Vector2 space)
		{
			CurrencyPurchaseDialog.PageSpace.FittingResult fittingResult = new CurrencyPurchaseDialog.PageSpace.FittingResult();
			float num = Vector2.Scale(this.spaceInfo.direction, space).magnitude + Vector2.Scale(this.spaceInfo.direction, this.spaceInfo.offset).magnitude;
			if (num > this.freeSpace)
			{
				return fittingResult;
			}
			fittingResult.succeeded = true;
			fittingResult.position = this.spaceInfo.direction * this.occupiedSpace + this.spaceInfo.direction * num * 0.5f;
			this.occupiedSpace += num;
			return fittingResult;
		}

		public float occupiedSpace;

		private CurrencyPurchaseDialog.PageSpace.SpacingInfo spaceInfo;

		public class FittingResult
		{
			public bool succeeded;

			public Vector2 position;
		}

		public class SpacingInfo
		{
			public float totalSize
			{
				get
				{
					return Vector2.Scale(this.size, this.direction).magnitude;
				}
			}

			public Vector2 size;

			public Vector2 offset;

			public Vector2 groupOffset;

			public Vector2 direction;
		}
	}

	private sealed class _003CDoAnimateOut_003Ed__35 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimateOut_003Ed__35(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			int num = this._003C_003E1__state;
			CurrencyPurchaseDialog currencyPurchaseDialog = this._003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
			}
			else
			{
				this._003C_003E1__state = -1;
				float num2 = 0f;
				this._003CenumList_003E5__2 = new EnumeratorsList();
				this._003CenumList_003E5__2.Add(currencyPurchaseDialog.nextButton.DoAnimateOut(num2), 0f, null, null, false);
				for (int i = 0; i < currencyPurchaseDialog.smallItems.Count; i++)
				{
					if ((i + 1) % currencyPurchaseDialog.smallItemsPerGroup == 0)
					{
						num2 += currencyPurchaseDialog.animationDelayPerColumn;
					}
					CurrencyPurchaseDialogSmallPrefab currencyPurchaseDialogSmallPrefab = currencyPurchaseDialog.smallItems[currencyPurchaseDialog.smallItems.Count - 1 - i];
					this._003CenumList_003E5__2.Add(currencyPurchaseDialogSmallPrefab.DoAnimateOut(num2), 0f, null, null, false);
				}
				for (int j = 0; j < currencyPurchaseDialog.bigItems.Count; j++)
				{
					num2 += currencyPurchaseDialog.animationDelayPerColumn;
					CurrencyPurchaseDialogBigPrefab currencyPurchaseDialogBigPrefab = currencyPurchaseDialog.bigItems[currencyPurchaseDialog.bigItems.Count - 1 - j];
					this._003CenumList_003E5__2.Add(currencyPurchaseDialogBigPrefab.DoAnimateOut(num2), 0f, null, null, false);
				}
			}
			if (!this._003CenumList_003E5__2.Update())
			{
				currencyPurchaseDialog.animateOutEnum = null;
				if (this.onEnd != null)
				{
					this.onEnd();
				}
				return false;
			}
			this._003C_003E2__current = null;
			this._003C_003E1__state = 1;
			return true;
		}

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CurrencyPurchaseDialog _003C_003E4__this;

		public Action onEnd;

		private EnumeratorsList _003CenumList_003E5__2;
	}
}
