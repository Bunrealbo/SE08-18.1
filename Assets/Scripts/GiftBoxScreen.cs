using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;

public class GiftBoxScreen : MonoBehaviour
{
	public void Show(GiftBoxScreen.ShowArguments showArguments)
	{
		this.showArguments = showArguments;
		this.giftsDefinition = showArguments.giftsDefinition;
		NavigationManager.instance.Push(base.gameObject, false);
	}

	private void OnEnable()
	{
		this.giftsDefinition = this.showArguments.giftsDefinition;
		if (this.giftsDefinition == null)
		{
			this.giftsDefinition = new GiftBoxScreen.GiftsDefinition();
			this.giftsDefinition.Add(GiftBoxScreen.Gift.CreateCoins(1000));
			this.giftsDefinition.Add(GiftBoxScreen.Gift.CreatePowerup(PowerupType.Hammer, 2));
			this.giftsDefinition.Add(GiftBoxScreen.Gift.CreatePowerup(PowerupType.PowerHammer, 3));
			this.giftsDefinition.Add(GiftBoxScreen.Gift.CreateBooster(BoosterType.BombBooster, 4));
			this.giftsDefinition.Add(GiftBoxScreen.Gift.CreateBooster(BoosterType.DiscoBooster, 5));
			this.giftsDefinition.Add(GiftBoxScreen.Gift.CreateBooster(BoosterType.VerticalRocketBooster, 6));
			this.giftsDefinition.Add(GiftBoxScreen.Gift.CreateStars(2));
			this.giftsDefinition.Add(GiftBoxScreen.Gift.CreateEnergy(1));
		}
		this.giftsDefinition.ConsumeGift();
		this.showArguments.giftsDefinition = this.giftsDefinition;
		this.Init(this.showArguments);
	}

	private void Init(GiftBoxScreen.ShowArguments showArguments)
	{
		GiftBoxScreen.GiftsDefinition giftsDefinition = showArguments.giftsDefinition;
		GGUtil.ChangeText(this.titleText, showArguments.title);
		GGUtil.Hide(this.widgetsToHide);
		this.waitForTap.Hide();
		this.giftItemPool.Clear();
		Vector2 sizeDelta = this.giftItemPool.prefab.GetComponent<RectTransform>().sizeDelta;
		List<GiftBoxScreen.Gift> gifts = giftsDefinition.gifts;
		float x = sizeDelta.x;
		float num = -((float)gifts.Count * x + (float)(gifts.Count - 1) * this.padding) * 0.5f;
		for (int i = 0; i < gifts.Count; i++)
		{
			GiftBoxScreen.Gift gift = gifts[i];
			GiftBoxScreenGiftItem giftBoxScreenGiftItem = this.giftItemPool.Next<GiftBoxScreenGiftItem>(false);
			giftBoxScreenGiftItem.Init(gift);
			float num2 = x * 0.5f;
			float x2 = num + (float)i * (x + this.padding) + num2;
			giftBoxScreenGiftItem.transform.localPosition = new Vector3(x2, 0f, 0f);
		}
		this.giftItemPool.HideNotUsed();
		this.animation = this.InAnimation();
		this.animation.MoveNext();
	}

	private IEnumerator InAnimation()
	{
		return new GiftBoxScreen._003CInAnimation_003Ed__19(0)
		{
			_003C_003E4__this = this
		};
	}

	private void Update()
	{
		if (this.animation != null)
		{
			this.animation.MoveNext();
		}
	}

	private GiftBoxScreen.ShowArguments showArguments;

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private RectTransform giftBoxContainer;

	[SerializeField]
	private Animator giftBoxAnimator;

	[SerializeField]
	private UIWaitForTap waitForTap;

	[SerializeField]
	private Transform openContainer;

	[SerializeField]
	private ComponentPool giftItemPool;

	[SerializeField]
	private TextMeshProUGUI titleText;

	[SerializeField]
	private float padding = 10f;

	private IEnumerator animation;

	private GiftBoxScreen.GiftsDefinition giftsDefinition;

	public string giftBoxInAnimationName;

	public enum GiftType
	{
		Coins,
		Powerup,
		Booster,
		Energy,
		Stars
	}

	public struct ShowArguments
	{
		public string title;

		public Action onComplete;

		public GiftBoxScreen.GiftsDefinition giftsDefinition;
	}

	[Serializable]
	public class Gift
	{
		public TimeSpan duration
		{
			get
			{
				return TimeSpan.FromHours((double)this.hours);
			}
		}

		public static GiftBoxScreen.Gift CreateCoins(int amount)
		{
			return new GiftBoxScreen.Gift
			{
				giftType = GiftBoxScreen.GiftType.Coins,
				amount = amount
			};
		}

		public static GiftBoxScreen.Gift CreatePowerup(PowerupType powerupType, int amount)
		{
			return new GiftBoxScreen.Gift
			{
				giftType = GiftBoxScreen.GiftType.Powerup,
				powerupType = powerupType,
				amount = amount
			};
		}

		public static GiftBoxScreen.Gift CreateBooster(BoosterType boosterType, int amount)
		{
			return new GiftBoxScreen.Gift
			{
				giftType = GiftBoxScreen.GiftType.Booster,
				boosterType = boosterType,
				amount = amount
			};
		}

		public static GiftBoxScreen.Gift CreateStars(int amount)
		{
			return new GiftBoxScreen.Gift
			{
				giftType = GiftBoxScreen.GiftType.Stars,
				amount = amount
			};
		}

		public static GiftBoxScreen.Gift CreateEnergy(int hours)
		{
			return new GiftBoxScreen.Gift
			{
				giftType = GiftBoxScreen.GiftType.Energy,
				hours = hours
			};
		}

		public void ConsumeGift()
		{
			GGPlayerSettings instance = GGPlayerSettings.instance;
			if (this.giftType == GiftBoxScreen.GiftType.Coins)
			{
				instance.walletManager.AddCurrency(CurrencyType.coins, this.amount);
				return;
			}
			if (this.giftType == GiftBoxScreen.GiftType.Booster)
			{
				PlayerInventory.instance.Add(this.boosterType, this.amount);
				return;
			}
			if (this.giftType == GiftBoxScreen.GiftType.Powerup)
			{
				PlayerInventory.instance.Add(this.powerupType, this.amount);
				return;
			}
			if (this.giftType == GiftBoxScreen.GiftType.Energy)
			{
				PlayerInventory.instance.BuyTimedItem(PlayerInventory.Item.FreeEnergyLimited, this.duration);
				return;
			}
			if (this.giftType == GiftBoxScreen.GiftType.Stars)
			{
				instance.walletManager.AddCurrency(CurrencyType.diamonds, this.amount);
			}
		}

		public GiftBoxScreen.GiftType giftType;

		public PowerupType powerupType;

		public BoosterType boosterType;

		public int amount;

		public int hours;
	}

	[Serializable]
	public class GiftsDefinition
	{
		public void ConsumeGift()
		{
			for (int i = 0; i < this.gifts.Count; i++)
			{
				this.gifts[i].ConsumeGift();
			}
		}

		public void Add(GiftBoxScreen.Gift gift)
		{
			this.gifts.Add(gift);
		}

		public List<GiftBoxScreen.Gift> gifts = new List<GiftBoxScreen.Gift>();
	}

	private sealed class _003CInAnimation_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CInAnimation_003Ed__19(int _003C_003E1__state)
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
			GiftBoxScreen giftBoxScreen = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				GGUtil.Show(giftBoxScreen.giftBoxContainer);
				GGSoundSystem.Play(GGSoundSystem.SFXType.GiftPresented);
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_AB;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_FF;
			default:
				return false;
			}
			AnimatorStateInfo currentAnimatorStateInfo = giftBoxScreen.giftBoxAnimator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName(giftBoxScreen.giftBoxInAnimationName) && currentAnimatorStateInfo.normalizedTime < 1f)
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003CwaitingEnum_003E5__2 = giftBoxScreen.waitForTap.DoWaitForTap();
			IL_AB:
			if (this._003CwaitingEnum_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			GGUtil.Hide(giftBoxScreen.giftBoxContainer);
			GGUtil.Show(giftBoxScreen.openContainer);
			GGSoundSystem.Play(GGSoundSystem.SFXType.GiftOpen);
			this._003CwaitingEnum_003E5__2 = giftBoxScreen.waitForTap.DoWaitForTap();
			IL_FF:
			if (!this._003CwaitingEnum_003E5__2.MoveNext())
			{
				GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
				if (giftBoxScreen.showArguments.onComplete != null)
				{
					giftBoxScreen.showArguments.onComplete();
				}
				else
				{
					NavigationManager.instance.Pop(true);
				}
				return false;
			}
			this._003C_003E2__current = null;
			this._003C_003E1__state = 3;
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

		public GiftBoxScreen _003C_003E4__this;

		private IEnumerator _003CwaitingEnum_003E5__2;
	}
}
