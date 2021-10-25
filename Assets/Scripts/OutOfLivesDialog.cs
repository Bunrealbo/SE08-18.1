using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;

public class OutOfLivesDialog : MonoBehaviour
{
	public void Show(LivesPriceConfig.PriceConfig priceConfig, Action onAllLivesRefilled, Action onMinLivesAvailable, Action onHide)
	{
		this.onAllLivesRefilled = onAllLivesRefilled;
		this.onMinLivesAvailable = onMinLivesAvailable;
		this.onHide = onHide;
		this.priceConfig = priceConfig;
		NavigationManager.instance.Push(base.gameObject, true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
	}

	public void Init()
	{
		this.initState.lives = BehaviourSingleton<EnergyManager>.instance.ownedPlayCoins;
		this.initState.secsToNextLife = BehaviourSingleton<EnergyManager>.instance.secToNextCoin;
		this.UpdateState();
		this.UpdateVisuals();
	}

	public void Hide()
	{
		if (this.onHide != null)
		{
			this.onHide();
		}
		NavigationManager.instance.Pop(true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void OnEnable()
	{
		this.Init();
	}

	public void OnDisable()
	{
		this.updateLivesEnum = null;
	}

	public void UpdateVisuals()
	{
		long price = GGPlayerSettings.instance.walletManager.CurrencyCount(CurrencyType.coins);
		if(this.coinsLabel != null)
			this.coinsLabel.text = GGFormat.FormatPrice(price, false);
		SingleCurrencyPrice priceForLives = this.priceConfig.GetPriceForLives(this.maxLives - this.currentState.lives);
		if (this.priceLabel != null)
			this.priceLabel.text = string.Format(this.priceFormat, priceForLives.cost);
		if (this.livesCountLabel != null)
			this.livesCountLabel.text = this.currentState.lives.ToString();
		if (this.timeCountLabel != null)
			this.timeCountLabel.text = GGFormat.FormatTimeSpan(TimeSpan.FromSeconds((double)this.currentState.secsToNextLife));
		GGUtil.SetActive(this.widgetsToHide, false);
		if (this.currentState.lives == 0)
		{
			this.noLivesStyle.Apply();
			return;
		}
		if (this.currentState.lives == this.maxLives)
		{
			this.fullLivesStyle.Apply();
			return;
		}
		this.someLivesStyle.Apply();
	}

	public void UpdateState()
	{
		this.currentState.lives = BehaviourSingleton<EnergyManager>.instance.ownedPlayCoins;
		this.currentState.secsToNextLife = BehaviourSingleton<EnergyManager>.instance.secToNextCoin;
		if (this.onMinLivesAvailable != null && this.initState.lives == 0 && this.currentState.lives > 0)
		{
			this.onMinLivesAvailable();
		}
		if (this.onAllLivesRefilled != null && this.currentState.lives == this.maxLives && this.currentState.lives > this.initState.lives)
		{
			this.onAllLivesRefilled();
		}
	}

	public void Update()
	{
		if (this.updateLivesEnum == null)
		{
			this.updateLivesEnum = this.UpdateLives();
		}
		this.updateLivesEnum.MoveNext();
	}

	public IEnumerator UpdateLives()
	{
		return new OutOfLivesDialog._003CUpdateLives_003Ed__24(0)
		{
			_003C_003E4__this = this
		};
	}

	public void ButtonCallback_RefillLives()
	{
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
		if (this.currentState.lives == this.maxLives)
		{
			this.Hide();
			return;
		}
		SingleCurrencyPrice priceForLives = this.priceConfig.GetPriceForLives(this.maxLives - this.currentState.lives);
		WalletManager walletManager = GGPlayerSettings.instance.walletManager;
		if (!walletManager.CanBuyItemWithPrice(priceForLives))
		{
			NavigationManager.instance.GetObject<CurrencyPurchaseDialog>().Show(ScriptableObjectSingleton<OffersDB>.instance, null);
			return;
		}
		new Analytics.LivesRefillBoughtEvent
		{
			config = this.priceConfig,
			livesBeforeRefill = this.currentState.lives,
			livesAfterRefill = this.maxLives
		}.Send();
		walletManager.BuyItem(priceForLives);
		BehaviourSingleton<EnergyManager>.instance.FillEnergy();
		this.UpdateState();
		this.UpdateVisuals();
	}

	private int maxLives
	{
		get
		{
			return EnergyControlConfig.instance.totalCoin;
		}
	}

	[SerializeField]
	private TextMeshProUGUI livesCountLabel;

	[SerializeField]
	private TextMeshProUGUI timeCountLabel;

	[SerializeField]
	private TextMeshProUGUI priceLabel;

	[SerializeField]
	private TextMeshProUGUI coinsLabel;

	[SerializeField]
	private string priceFormat;

	private Action onAllLivesRefilled;

	private Action onMinLivesAvailable;

	private Action onHide;

	private OutOfLivesDialog.State initState = new OutOfLivesDialog.State();

	private OutOfLivesDialog.State currentState = new OutOfLivesDialog.State();

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private VisualStyleSet noLivesStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet someLivesStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet fullLivesStyle = new VisualStyleSet();

	private LivesPriceConfig.PriceConfig priceConfig;

	private IEnumerator updateLivesEnum;

	public class State
	{
		public int lives;

		public float secsToNextLife;
	}

	private sealed class _003CUpdateLives_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CUpdateLives_003Ed__24(int _003C_003E1__state)
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
			OutOfLivesDialog outOfLivesDialog = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				break;
			case 1:
				this._003C_003E1__state = -1;
				goto IL_5F;
			case 2:
				this._003C_003E1__state = -1;
				break;
			default:
				return false;
			}
			this._003Ctime_003E5__2 = 0f;
			IL_5F:
			if (this._003Ctime_003E5__2 >= 1f)
			{
				outOfLivesDialog.UpdateState();
				outOfLivesDialog.UpdateVisuals();
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
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

		public OutOfLivesDialog _003C_003E4__this;

		private float _003Ctime_003E5__2;
	}
}
