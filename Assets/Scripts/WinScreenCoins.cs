using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;

public class WinScreenCoins : MonoBehaviour
{
	public void Init(long wonCoins, WinScreen winScreen)
	{
		this.winScreen = winScreen;
		GGUtil.SetActive(this, wonCoins > 0L);
		this.coinsPool.Clear();
		this.coinsPool.HideNotUsed();
		GGUtil.ChangeText(this.coinsLabel, wonCoins);
		GGUtil.Show(this.coinImage);
	}

	public IEnumerator DoMoveCoins(int count, RectTransform destination, long startCoins, long endCoins)
	{
		return new WinScreenCoins._003CDoMoveCoins_003Ed__5(0)
		{
			_003C_003E4__this = this,
			count = count,
			destination = destination,
			startCoins = startCoins,
			endCoins = endCoins
		};
	}

	private WinScreen.Settings settings
	{
		get
		{
			return Match3Settings.instance.winScreenSettings;
		}
	}

	private IEnumerator DoMoveCoin(GameObject coinGameObject, Vector3 destinationLocalPosition, float delay, long coinCount)
	{
		return new WinScreenCoins._003CDoMoveCoin_003Ed__9(0)
		{
			_003C_003E4__this = this,
			coinGameObject = coinGameObject,
			destinationLocalPosition = destinationLocalPosition,
			delay = delay,
			coinCount = coinCount
		};
	}

	[SerializeField]
	private ComponentPool coinsPool = new ComponentPool();

	[SerializeField]
	private RectTransform coinImage;

	[SerializeField]
	private TextMeshProUGUI coinsLabel;

	private WinScreen winScreen;

	private EnumeratorsList enumList = new EnumeratorsList();

	private sealed class _003CDoMoveCoins_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoMoveCoins_003Ed__5(int _003C_003E1__state)
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
			WinScreenCoins winScreenCoins = this._003C_003E4__this;
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
				if (this.count == 0)
				{
					return false;
				}
				CurrencyPanel currencyPanel = winScreenCoins.winScreen.currencyPanel;
				this._003CcurrencyDisplay_003E5__2 = currencyPanel.DisplayForCurrency(CurrencyType.coins);
				this._003CcurrencyDisplay_003E5__2.DisplayCount(this.startCoins);
				Vector3 destinationLocalPosition = winScreenCoins.coinsPool.parent.InverseTransformPoint(this.destination.position);
				destinationLocalPosition.z = 0f;
				winScreenCoins.coinsPool.Clear();
				for (int i = 0; i < this.count; i++)
				{
					GGUtil.Hide(winScreenCoins.coinsPool.Next(false));
				}
				winScreenCoins.enumList.Clear();
				WinScreen.Settings settings = winScreenCoins.settings;
				List<GameObject> usedObjects = winScreenCoins.coinsPool.usedObjects;
				float num2 = 0f;
				float num3 = Mathf.Max(0f, settings.starTravelDuration - settings.coinTravelDuration) / (float)this.count;
				for (int j = usedObjects.Count - 1; j >= 0; j--)
				{
					GameObject coinGameObject = usedObjects[j];
					float num4 = Mathf.InverseLerp((float)(usedObjects.Count - 1), 0f, (float)j);
					long coinCount = (long)((float)(this.endCoins - this.startCoins) * num4 + (float)this.startCoins);
					winScreenCoins.enumList.Add(winScreenCoins.DoMoveCoin(coinGameObject, destinationLocalPosition, num2, coinCount), 0f, null, null, false);
					num2 += num3;
				}
			}
			if (!winScreenCoins.enumList.Update())
			{
				this._003CcurrencyDisplay_003E5__2.DisplayCount(this.endCoins);
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

		public int count;

		public WinScreenCoins _003C_003E4__this;

		public long startCoins;

		public RectTransform destination;

		public long endCoins;

		private CurrencyDisplay _003CcurrencyDisplay_003E5__2;
	}

	private sealed class _003CDoMoveCoin_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoMoveCoin_003Ed__9(int _003C_003E1__state)
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
			WinScreenCoins winScreenCoins = this._003C_003E4__this;
			switch (num)
			{
			case 0:
			{
				this._003C_003E1__state = -1;
				CurrencyPanel currencyPanel = winScreenCoins.winScreen.currencyPanel;
				this._003CcurrencyDisplay_003E5__2 = currencyPanel.DisplayForCurrency(CurrencyType.coins);
				this._003Ctime_003E5__3 = 0f;
				break;
			}
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_19F;
			default:
				return false;
			}
			if (this._003Ctime_003E5__3 < this.delay)
			{
				this._003Ctime_003E5__3 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003CcoinTransform_003E5__4 = this.coinGameObject.transform;
			this._003CcoinTransform_003E5__4.localPosition = Vector3.zero;
			this._003CcoinTransform_003E5__4.localScale = Vector3.one;
			GGUtil.Show(this.coinGameObject);
			this._003CstartLocalPosition_003E5__5 = Vector3.zero;
			this._003CendLocalPosition_003E5__6 = this.destinationLocalPosition;
			this._003CstartScale_003E5__7 = 1;
			this._003CendScale_003E5__8 = winScreenCoins.settings.coinEndScale;
			this._003Cduration_003E5__9 = winScreenCoins.settings.coinTravelDuration;
			this._003Ctime_003E5__3 = 0f;
			IL_19F:
			if (this._003Ctime_003E5__3 > this._003Cduration_003E5__9)
			{
				GGSoundSystem.Play(GGSoundSystem.SFXType.RecieveCoin);
				this._003CcurrencyDisplay_003E5__2.ShowShineParticle();
				this._003CcurrencyDisplay_003E5__2.DisplayCount(this.coinCount);
				GGUtil.Hide(this.coinGameObject);
				return false;
			}
			this._003Ctime_003E5__3 += Time.deltaTime;
			float t = Mathf.InverseLerp(0f, this._003Cduration_003E5__9, this._003Ctime_003E5__3);
			float d = Mathf.Lerp((float)this._003CstartScale_003E5__7, this._003CendScale_003E5__8, t);
			Vector3 localPosition = Vector3.Lerp(this._003CstartLocalPosition_003E5__5, this._003CendLocalPosition_003E5__6, t);
			this._003CcoinTransform_003E5__4.localPosition = localPosition;
			this._003CcoinTransform_003E5__4.localScale = Vector3.one * d;
			this._003C_003E2__current = null;
			this._003C_003E1__state = 2;
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

		public WinScreenCoins _003C_003E4__this;

		public float delay;

		public GameObject coinGameObject;

		public Vector3 destinationLocalPosition;

		public long coinCount;

		private CurrencyDisplay _003CcurrencyDisplay_003E5__2;

		private float _003Ctime_003E5__3;

		private Transform _003CcoinTransform_003E5__4;

		private Vector3 _003CstartLocalPosition_003E5__5;

		private Vector3 _003CendLocalPosition_003E5__6;

		private int _003CstartScale_003E5__7;

		private float _003CendScale_003E5__8;

		private float _003Cduration_003E5__9;
	}
}
