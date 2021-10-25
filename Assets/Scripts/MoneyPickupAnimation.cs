using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class MoneyPickupAnimation : MonoBehaviour
{
	public MoneyPickupAnimation.Settings settings
	{
		get
		{
			return Match3Settings.instance.moneyPickupAnimationSettings;
		}
	}

	public void Show(MoneyPickupAnimation.ShowParams showParams)
	{
		this.showParams = showParams;
		this.isInAnimationComplete = false;
		this.isInTranslateAnimation = false;
		this.coinPool.Clear();
		this.moneyElements.Clear();
		this.elements.Clear();
		if (showParams.numberOfStars > 0)
		{
			MoneyPickupAnimation.ElementDefinition item = default(MoneyPickupAnimation.ElementDefinition);
			item.style = MoneyPickupAnimationMoney.Style.Star;
			item.count = showParams.numberOfStars;
			this.elements.Add(item);
		}
		if (showParams.numberOfCoins > 0)
		{
			MoneyPickupAnimation.ElementDefinition item2 = default(MoneyPickupAnimation.ElementDefinition);
			item2.style = MoneyPickupAnimationMoney.Style.Coin;
			item2.count = showParams.numberOfCoins;
			this.elements.Add(item2);
		}
		float num = this.elementWith * (float)this.elements.Count;
		for (int i = 0; i < this.elements.Count; i++)
		{
			MoneyPickupAnimation.ElementDefinition elementDefinition = this.elements[i];
			MoneyPickupAnimationMoney moneyPickupAnimationMoney = this.coinPool.Next<MoneyPickupAnimationMoney>(false);
			GGUtil.SetActive(moneyPickupAnimationMoney, true);
			Vector3 startLocalPosition = new Vector3(-num * 0.5f + ((float)i + 0.5f) * this.elementWith, 0f, 0f);
			moneyPickupAnimationMoney.Init(i, startLocalPosition, elementDefinition.style, elementDefinition.count);
			this.moneyElements.Add(moneyPickupAnimationMoney);
			GGUtil.SetActive(moneyPickupAnimationMoney, false);
		}
		this.coinPool.HideNotUsed();
		GGUtil.SetActive(this, true);
		this.animationEnumerator = this.DoInAnimation();
	}

	public void TravelToAnimation()
	{
		this.animationEnumerator = this.DoTravelToAnimation();
	}

	private IEnumerator DoInAnimation()
	{
		return new MoneyPickupAnimation._003CDoInAnimation_003Ed__16(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator DoTravelToAnimation()
	{
		return new MoneyPickupAnimation._003CDoTravelToAnimation_003Ed__17(0)
		{
			_003C_003E4__this = this
		};
	}

	public void Callback_OnClick()
	{
		if (!this.isInAnimationComplete || this.isInTranslateAnimation)
		{
			return;
		}
		this.isInTranslateAnimation = true;
		this.TravelToAnimation();
	}

	private void Update()
	{
		if (this.animationEnumerator == null)
		{
			return;
		}
		if (!this.animationEnumerator.MoveNext())
		{
			this.animationEnumerator = null;
		}
	}

	[SerializeField]
	private ComponentPool coinPool = new ComponentPool();

	[SerializeField]
	private RectTransform coinSpawnOrigin;

	[SerializeField]
	private float elementWith = 200f;

	private MoneyPickupAnimation.ShowParams showParams;

	private List<MoneyPickupAnimationMoney> moneyElements = new List<MoneyPickupAnimationMoney>();

	private bool isInAnimationComplete;

	private bool isInTranslateAnimation;

	private IEnumerator animationEnumerator;

	private List<MoneyPickupAnimation.ElementDefinition> elements = new List<MoneyPickupAnimation.ElementDefinition>();

	[Serializable]
	public class Settings
	{
		public float inAnimationDuration = 1f;

		public float totalDelayForInAnimationIndexes = 0.5f;

		public AnimationCurve inScaleCurve;

		public float startScale;

		public Vector3 randomRange;

		public float travelDuration = 0.75f;

		public float totalDelayForIndexes = 1f;

		public AnimationCurve travelCurve;

		public float travelEndScale;

		public AnimationCurve travelScaleCurve;

		public float bobDuration = 1f;

		public float bobScale = 1.1f;

		public AnimationCurve bobCurve;

		public int numberOfCoins = 10;

		public int numberOfStars = 10;
	}

	public struct ShowParams
	{
		public int numberOfCoins;

		public int numberOfStars;

		public RectTransform starDestinationTransform;

		public RectTransform coinDestinationTransform;

		public Action onComplete;
	}

	public struct ElementDefinition
	{
		public MoneyPickupAnimationMoney.Style style;

		public int count;
	}

	private sealed class _003CDoInAnimation_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoInAnimation_003Ed__16(int _003C_003E1__state)
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
			MoneyPickupAnimation moneyPickupAnimation = this._003C_003E4__this;
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
				this._003Csettings_003E5__2 = moneyPickupAnimation.settings;
				this._003Ctime_003E5__3 = 0f;
				this._003CdelayPerIndex_003E5__4 = this._003Csettings_003E5__2.totalDelayForInAnimationIndexes / (float)moneyPickupAnimation.moneyElements.Count;
			}
			this._003Ctime_003E5__3 += Time.deltaTime;
			bool flag = false;
			for (int i = 0; i < moneyPickupAnimation.moneyElements.Count; i++)
			{
				MoneyPickupAnimationMoney moneyPickupAnimationMoney = moneyPickupAnimation.moneyElements[i];
				GGUtil.SetActive(moneyPickupAnimationMoney, true);
				float num2 = this._003Ctime_003E5__3 - (float)moneyPickupAnimationMoney.index * this._003CdelayPerIndex_003E5__4;
				float num3 = Mathf.InverseLerp(0f, this._003Csettings_003E5__2.inAnimationDuration, num2);
				if (this._003Csettings_003E5__2.inScaleCurve != null)
				{
					num3 = this._003Csettings_003E5__2.inScaleCurve.Evaluate(num3);
				}
				Vector3 localScale = Vector3.LerpUnclamped(new Vector3(0f, 0f, 1f), Vector3.one, num3);
				if (num2 < this._003Csettings_003E5__2.inAnimationDuration)
				{
					flag = true;
				}
				if (num2 >= this._003Csettings_003E5__2.inAnimationDuration)
				{
					num2 -= this._003Csettings_003E5__2.inAnimationDuration;
					num3 = Mathf.PingPong(num2, this._003Csettings_003E5__2.bobDuration);
					if (this._003Csettings_003E5__2.bobCurve != null)
					{
						num3 = this._003Csettings_003E5__2.bobCurve.Evaluate(num3);
					}
					localScale = Vector3.LerpUnclamped(Vector3.one, new Vector3(this._003Csettings_003E5__2.bobScale, this._003Csettings_003E5__2.bobScale, 1f), num3);
				}
				moneyPickupAnimationMoney.transform.localScale = localScale;
			}
			if (!flag)
			{
				moneyPickupAnimation.isInAnimationComplete = true;
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

		public MoneyPickupAnimation _003C_003E4__this;

		private MoneyPickupAnimation.Settings _003Csettings_003E5__2;

		private float _003Ctime_003E5__3;

		private float _003CdelayPerIndex_003E5__4;
	}

	private sealed class _003CDoTravelToAnimation_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoTravelToAnimation_003Ed__17(int _003C_003E1__state)
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
			MoneyPickupAnimation moneyPickupAnimation = this._003C_003E4__this;
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
				this._003ClocalEndPositionStar_003E5__2 = moneyPickupAnimation.coinPool.parent.InverseTransformPoint(moneyPickupAnimation.showParams.starDestinationTransform.position);
				this._003ClocalEndPositionCoin_003E5__3 = moneyPickupAnimation.coinPool.parent.InverseTransformPoint(moneyPickupAnimation.showParams.coinDestinationTransform.position);
				this._003Csettings_003E5__4 = moneyPickupAnimation.settings;
				this._003Ctime_003E5__5 = 0f;
				this._003CdelayPerIndex_003E5__6 = this._003Csettings_003E5__4.totalDelayForIndexes / (float)moneyPickupAnimation.moneyElements.Count;
				for (int i = 0; i < moneyPickupAnimation.moneyElements.Count; i++)
				{
					MoneyPickupAnimationMoney moneyPickupAnimationMoney = moneyPickupAnimation.moneyElements[i];
					moneyPickupAnimationMoney.startTravelScale = moneyPickupAnimationMoney.transform.localScale;
				}
			}
			this._003Ctime_003E5__5 += Time.deltaTime;
			bool flag = false;
			for (int j = 0; j < moneyPickupAnimation.moneyElements.Count; j++)
			{
				MoneyPickupAnimationMoney moneyPickupAnimationMoney2 = moneyPickupAnimation.moneyElements[j];
				int num2 = moneyPickupAnimation.moneyElements.Count - 1 - moneyPickupAnimationMoney2.index;
				num2 = moneyPickupAnimationMoney2.index;
				float num3 = this._003Ctime_003E5__5 - (float)num2 * this._003CdelayPerIndex_003E5__6;
				float num4 = Mathf.InverseLerp(0f, this._003Csettings_003E5__4.travelDuration, num3);
				Vector3 zero = Vector3.zero;
				if (moneyPickupAnimationMoney2.style == MoneyPickupAnimationMoney.Style.Coin)
				{
					zero = this._003ClocalEndPositionCoin_003E5__3;
				}
				else
				{
					zero = this._003ClocalEndPositionStar_003E5__2;
				}
				float num5 = num4;
				if (this._003Csettings_003E5__4.travelCurve != null)
				{
					num5 = this._003Csettings_003E5__4.travelCurve.Evaluate(num5);
				}
				Vector3 localPosition = Vector3.LerpUnclamped(moneyPickupAnimationMoney2.startLocalPosition, zero, num5);
				if (num3 < this._003Csettings_003E5__4.travelDuration)
				{
					flag = true;
				}
				float num6 = num4;
				if (this._003Csettings_003E5__4.travelScaleCurve != null)
				{
					num6 = this._003Csettings_003E5__4.travelScaleCurve.Evaluate(num6);
				}
				Vector3 localScale = Vector3.LerpUnclamped(moneyPickupAnimationMoney2.startTravelScale, new Vector3(this._003Csettings_003E5__4.travelEndScale, this._003Csettings_003E5__4.travelEndScale, 0f), num6);
				moneyPickupAnimationMoney2.transform.localPosition = localPosition;
				moneyPickupAnimationMoney2.transform.localScale = localScale;
			}
			if (flag)
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			GGUtil.SetActive(moneyPickupAnimation, false);
			if (moneyPickupAnimation.showParams.onComplete != null)
			{
				moneyPickupAnimation.showParams.onComplete();
			}
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

		public MoneyPickupAnimation _003C_003E4__this;

		private Vector3 _003ClocalEndPositionStar_003E5__2;

		private Vector3 _003ClocalEndPositionCoin_003E5__3;

		private MoneyPickupAnimation.Settings _003Csettings_003E5__4;

		private float _003Ctime_003E5__5;

		private float _003CdelayPerIndex_003E5__6;
	}
}
