using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class InAppPurchaseConfirmScreen : UILayer, InAppBackend.Listener
{
	public InAppPurchaseConfirmScreen.PurchaseArguments purchaseArguments
	{
		get
		{
			return this._003CpurchaseArguments_003Ek__BackingField;
		}
		private set
		{
			this._003CpurchaseArguments_003Ek__BackingField = value;
		}
	}

	public void SuspendShow()
	{
		this.isShowSuspended = true;
	}

	public void ResumeShow()
	{
		this.isShowSuspended = false;
		if (this.suspendedArguments.Count > 0)
		{
			InAppPurchaseConfirmScreen.PurchaseArguments purchaseArguments = this.suspendedArguments[this.suspendedArguments.Count - 1];
			this.suspendedArguments.Clear();
			this.Show(purchaseArguments);
		}
	}

	public void Show(InAppPurchaseConfirmScreen.PurchaseArguments purchaseArguments)
	{
		if (this.isShowSuspended && purchaseArguments.isProductBought)
		{
			this.suspendedArguments.Add(purchaseArguments);
			return;
		}
		this.suspendedArguments.Clear();
		this.isShowSuspended = false;
		this.purchaseArguments = purchaseArguments;
		NavigationManager.instance.Push(base.gameObject, false);
	}

	private void Init()
	{
		this.updateAnimation = null;
		GGUtil.SetActive(this.widgetsToHide, false);
		OffersDB.ProductDefinition productToBuy = this.purchaseArguments.productToBuy;
		if (productToBuy.offer.isNamedOffer)
		{
			this.namedPrefab.Init(productToBuy);
			GGUtil.SetActive(this.namedPrefab, true);
		}
		else
		{
			this.notNamedPrefab.Init(productToBuy);
			GGUtil.SetActive(this.notNamedPrefab, true);
		}
		this.confirmState = default(InAppPurchaseConfirmScreen.ConfirmState);
		this.loadingStyle.Apply();
		if (this.purchaseArguments.isProductBought)
		{
			this.updateAnimation = this.DoShowPurchasedItem();
			return;
		}
		BehaviourSingletonInit<InAppBackend>.instance.PurchaseItem(this.purchaseArguments.productToBuy.productID);
	}

	private void OnEnable()
	{
		if (BehaviourSingletonInit<InAppBackend>.instance != null)
			BehaviourSingletonInit<InAppBackend>.instance.AddListener(this);
		this.Init();
	}

	private void OnDisable()
	{
		if(BehaviourSingletonInit<InAppBackend>.instance != null)
			BehaviourSingletonInit<InAppBackend>.instance.RemoveListener(this);
	}

	public void OnInitialized(InAppBackend.InitializeArguments initializeArguments)
	{
	}

	public void OnPurchase(InAppBackend.PurchaseEventArguments purchaseParams)
	{
		if (this.updateAnimation != null)
		{
			return;
		}
		if (purchaseParams.isSuccess)
		{
			if (purchaseParams.productId != this.purchaseArguments.productToBuy.productID)
			{
				GGUtil.Hide(this.namedPrefab);
				GGUtil.Hide(this.notNamedPrefab);
			}
			this.updateAnimation = this.DoShowPurchasedItem();
			return;
		}
		this.Hide();
	}

	private void Hide()
	{
		NavigationManager.instance.Pop(true);
	}

	private IEnumerator DoShowPurchasedItem()
	{
		return new InAppPurchaseConfirmScreen._003CDoShowPurchasedItem_003Ed__26(0)
		{
			_003C_003E4__this = this
		};
	}

	private void ShowConfettiParticle()
	{
		GGUtil.SetActive(UnityEngine.Object.Instantiate<GameObject>(this.confettiParticle, base.transform), true);
	}

	private IEnumerator DoWaitForConfirm(float maxSeconds)
	{
		return new InAppPurchaseConfirmScreen._003CDoWaitForConfirm_003Ed__28(0)
		{
			_003C_003E4__this = this,
			maxSeconds = maxSeconds
		};
	}

	private void Update()
	{
		if (this.updateAnimation != null)
		{
			this.updateAnimation.MoveNext();
		}
	}

	public void ButtonCallback_OnConfirm()
	{
		if (!this.confirmState.isWaitingForConfirm)
		{
			return;
		}
		this.confirmState.isConfirmed = true;
	}

	public override void OnGoBack(NavigationManager nav)
	{
		if (this.updateAnimation != null)
		{
			return;
		}
		this.Hide();
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	[SerializeField]
	private GameObject confettiParticle;

	[SerializeField]
	private RectTransform successAnimationContainer;

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private CurrencyPurchaseDialogBigPrefab namedPrefab;

	[SerializeField]
	private CurrencyPurchaseDialogSmallPrefab notNamedPrefab;

	[SerializeField]
	private VisualStyleSet loadingStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet purchaseSuccessStyle = new VisualStyleSet();

	private InAppPurchaseConfirmScreen.PurchaseArguments _003CpurchaseArguments_003Ek__BackingField;

	private IEnumerator updateAnimation;

	private InAppPurchaseConfirmScreen.ConfirmState confirmState;

	private bool isShowSuspended;

	private List<InAppPurchaseConfirmScreen.PurchaseArguments> suspendedArguments = new List<InAppPurchaseConfirmScreen.PurchaseArguments>();

	public struct ConfirmState
	{
		public bool isWaitingForConfirm;

		public bool isConfirmed;
	}

	public struct PurchaseArguments
	{
		public OffersDB.ProductDefinition productToBuy;

		public bool isProductBought;
	}

	private sealed class _003CDoShowPurchasedItem_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShowPurchasedItem_003Ed__26(int _003C_003E1__state)
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
			InAppPurchaseConfirmScreen inAppPurchaseConfirmScreen = this._003C_003E4__this;
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
				inAppPurchaseConfirmScreen.purchaseSuccessStyle.Apply();
				inAppPurchaseConfirmScreen.ShowConfettiParticle();
				GGUtil.Show(inAppPurchaseConfirmScreen.successAnimationContainer);
				GGSoundSystem.Play(GGSoundSystem.SFXType.PurchaseSuccess);
				this._003CwaitingEnum_003E5__2 = inAppPurchaseConfirmScreen.DoWaitForConfirm(3.5f);
			}
			if (!this._003CwaitingEnum_003E5__2.MoveNext())
			{
				inAppPurchaseConfirmScreen.Hide();
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

		public InAppPurchaseConfirmScreen _003C_003E4__this;

		private IEnumerator _003CwaitingEnum_003E5__2;
	}

	private sealed class _003CDoWaitForConfirm_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoWaitForConfirm_003Ed__28(int _003C_003E1__state)
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
			InAppPurchaseConfirmScreen inAppPurchaseConfirmScreen = this._003C_003E4__this;
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
				this._003Ctime_003E5__2 = 0f;
				inAppPurchaseConfirmScreen.confirmState = default(InAppPurchaseConfirmScreen.ConfirmState);
				inAppPurchaseConfirmScreen.confirmState.isWaitingForConfirm = true;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			if ((this.maxSeconds <= 0f || this._003Ctime_003E5__2 <= this.maxSeconds) && !inAppPurchaseConfirmScreen.confirmState.isConfirmed)
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			inAppPurchaseConfirmScreen.confirmState = default(InAppPurchaseConfirmScreen.ConfirmState);
			return false;
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

		public InAppPurchaseConfirmScreen _003C_003E4__this;

		public float maxSeconds;

		private float _003Ctime_003E5__2;
	}
}
