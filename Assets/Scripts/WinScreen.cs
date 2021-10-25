using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;
using UnityEngine.Playables;

public class WinScreen : MonoBehaviour
{
	public void Show(WinScreen.InitArguments initArguments)
	{
		GGUtil.Show(this);
		this.Init(initArguments);
	}

	public CurrencyPanel currencyPanel
	{
		get
		{
			return this.initArguments.currencyPanel;
		}
	}

	private void Init(WinScreen.InitArguments initArguments)
	{
		this.initArguments = initArguments;
		this.tapState = default(WinScreen.TapState);
		GGUtil.Hide(this.tapContainer);
		Match3Game game = initArguments.game;
		game.StartWinScreenBoardAnimation();
		game.gameScreen.HideVisibleObjects();
		game.SuspendGameSounds();
		this.SetAlpha(this.thingsTofadeAtEnd, 1f);
		this.star.Show(this);
		this.coins.Init(initArguments.coinsWon, this);
		this.particles.DestroyCreatedObjects();
		this.animationEnum = this.DoPlainAnimation();
		this.animationEnum.MoveNext();
	}

	private void SetAlpha(List<CanvasGroup> list, float alpha)
	{
		for (int i = 0; i < list.Count; i++)
		{
			CanvasGroup canvasGroup = list[i];
			if (!(canvasGroup == null))
			{
				canvasGroup.alpha = alpha;
			}
		}
	}

	private IEnumerator DoPlainAnimation()
	{
		return new WinScreen._003CDoPlainAnimation_003Ed__21(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator DoDirectorAnimation()
	{
		return new WinScreen._003CDoDirectorAnimation_003Ed__22(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator DoAnimation()
	{
		return new WinScreen._003CDoAnimation_003Ed__23(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator Fade(List<CanvasGroup> fadeItems, float from, float to, float duration)
	{
		return new WinScreen._003CFade_003Ed__24(0)
		{
			_003C_003E4__this = this,
			fadeItems = fadeItems,
			from = from,
			to = to,
			duration = duration
		};
	}

	private IEnumerator Fade(CanvasGroup visualItem, float from, float to, float duration)
	{
		return new WinScreen._003CFade_003Ed__25(0)
		{
			visualItem = visualItem,
			from = from,
			to = to,
			duration = duration
		};
	}

	private IEnumerator WaitTillStateFinishes(Animator animator, string stateName)
	{
		return new WinScreen._003CWaitTillStateFinishes_003Ed__26(0)
		{
			animator = animator,
			stateName = stateName
		};
	}

	private void Hide()
	{
		GGUtil.Hide(this);
	}

	private void Update()
	{
		if (this.animationEnum != null && !this.animationEnum.MoveNext())
		{
			this.animationEnum = null;
		}
	}

	private WinScreen.Settings settings
	{
		get
		{
			return Match3Settings.instance.winScreenSettings;
		}
	}

	private void OnTap(Action onTap)
	{
		this.tapState = default(WinScreen.TapState);
		this.tapState.onTap = onTap;
		this.tapState.isWaitingForTap = true;
		GGUtil.Show(this.tapContainer);
	}

	private IEnumerator DoWaitForTap(bool showTapContainer = true)
	{
		return new WinScreen._003CDoWaitForTap_003Ed__35(0)
		{
			_003C_003E4__this = this,
			showTapContainer = showTapContainer
		};
	}

	public void ButtonCallback_OnTap()
	{
		if (!this.tapState.isWaitingForTap)
		{
			return;
		}
		if (this.tapState.isTapped)
		{
			return;
		}
		this.tapState.isTapped = true;
		this.tapState.CallOnTap();
	}

	[SerializeField]
	private Animator starAnimator;

	[SerializeField]
	private UIGGParticleCreator particles;

	[SerializeField]
	private string inStarAnimatorState;

	[SerializeField]
	private RectTransform tapContainer;

	[SerializeField]
	private CanvasGroup background;

	[SerializeField]
	private List<CanvasGroup> thingsTofadeAtEnd = new List<CanvasGroup>();

	[SerializeField]
	private List<Transform> thingsToHideAtEnd = new List<Transform>();

	[SerializeField]
	private WinScreenStar star;

	[SerializeField]
	private WinScreenCoins coins;

	[SerializeField]
	private PlayableDirector playableDirector;

	[SerializeField]
	private Transform normalAnimationContainer;

	[SerializeField]
	private Animator normalAnimator;

	[SerializeField]
	private float minTimeBeforeCanTap = 1f;

	private WinScreen.InitArguments initArguments;

	private IEnumerator animationEnum;

	private WinScreen.TapState tapState;

	public class InitArguments
	{
		public long currentCoins
		{
			get
			{
				return this.previousCoins + this.coinsWon;
			}
		}

		public long coinsWon
		{
			get
			{
				return this.baseStageWonCoins + this.additionalCoins;
			}
		}

		public void CallOnComplete()
		{
			GGUtil.Call(this.onComplete);
		}

		public void CallOnMiddle()
		{
			GGUtil.Call(this.onMiddle);
		}

		public long baseStageWonCoins;

		public long additionalCoins;

		public long previousCoins;

		public long previousStars;

		public long currentStars;

		public Action onComplete;

		public Action onMiddle;

		public Match3Game game;

		public DecorateRoomScreen decorateRoomScreen;

		public CurrencyPanel currencyPanel;

		public RectTransform starRect;

		public RectTransform coinRect;
	}

	[Serializable]
	public class Settings
	{
		public float starTravelDuration = 1f;

		public float starRotationAngle = 760f;

		public float starEndScale;

		public float backgroundFadeOutDuration = 0.5f;

		public float coinTravelDuration = 0.75f;

		public float coinEndScale;

		public int maxCoinsToAnimate = 100;
	}

	private struct TapState
	{
		public void CallOnTap()
		{
			if (this.onTap != null)
			{
				this.onTap();
			}
		}

		public bool isTapped;

		public bool isWaitingForTap;

		public Action onTap;
	}

	private sealed class _003CDoPlainAnimation_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoPlainAnimation_003Ed__21(int _003C_003E1__state)
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
			WinScreen winScreen = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				GGSoundSystem.Play(GGSoundSystem.SFXType.WinScreenStart);
				GGUtil.SetActive(winScreen.thingsToHideAtEnd, true);
				GGUtil.Show(winScreen.normalAnimationContainer);
				this._003Ctime_003E5__2 = 0f;
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_B5;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_211;
			default:
				return false;
			}
			if (this._003Ctime_003E5__2 < winScreen.minTimeBeforeCanTap)
			{
				this._003Ctime_003E5__2 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003CwaitForTapEnum_003E5__3 = winScreen.DoWaitForTap(true);
			IL_B5:
			if (this._003CwaitForTapEnum_003E5__3.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			winScreen.normalAnimator.StopPlayback();
			winScreen.initArguments.CallOnMiddle();
			CurrencyPanel currencyPanel = winScreen.initArguments.currencyPanel;
			this._003CstarsDisplay_003E5__4 = currencyPanel.DisplayForCurrency(CurrencyType.diamonds);
			CurrencyDisplay currencyDisplay = currencyPanel.DisplayForCurrency(CurrencyType.coins);
			this._003CstarsDisplay_003E5__4.DisplayCount(winScreen.initArguments.previousStars);
			currencyDisplay.DisplayCount(winScreen.initArguments.previousCoins);
			this._003CenumList_003E5__5 = new EnumeratorsList();
			this._003CenumList_003E5__5.Clear();
			GGUtil.Hide(winScreen.thingsToHideAtEnd);
			WinScreen.Settings settings = winScreen.settings;
			this._003CenumList_003E5__5.Add(winScreen.Fade(winScreen.thingsTofadeAtEnd, 1f, 0f, settings.backgroundFadeOutDuration), 0f, null, null, false);
			this._003CenumList_003E5__5.Add(winScreen.star.DoMoveTo(winScreen.initArguments.starRect), 0f, null, null, false);
			this._003CenumList_003E5__5.Add(winScreen.coins.DoMoveCoins(Mathf.Min((int)winScreen.initArguments.coinsWon, settings.maxCoinsToAnimate), winScreen.initArguments.coinRect, winScreen.initArguments.previousCoins, winScreen.initArguments.currentCoins), 0f, null, null, false);
			IL_211:
			if (!this._003CenumList_003E5__5.Update())
			{
				this._003CstarsDisplay_003E5__4.DisplayCount(winScreen.initArguments.currentStars);
				winScreen.Hide();
				winScreen.initArguments.CallOnComplete();
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

		public WinScreen _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private IEnumerator _003CwaitForTapEnum_003E5__3;

		private CurrencyDisplay _003CstarsDisplay_003E5__4;

		private EnumeratorsList _003CenumList_003E5__5;
	}

	private sealed class _003CDoDirectorAnimation_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoDirectorAnimation_003Ed__22(int _003C_003E1__state)
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
			WinScreen winScreen = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				GGSoundSystem.Play(GGSoundSystem.SFXType.WinScreenStart);
				GGUtil.SetActive(winScreen.thingsToHideAtEnd, true);
				winScreen.playableDirector.Play();
				this._003Ctime_003E5__2 = 0f;
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_B5;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_211;
			default:
				return false;
			}
			if (this._003Ctime_003E5__2 < winScreen.minTimeBeforeCanTap)
			{
				this._003Ctime_003E5__2 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003CwaitForTapEnum_003E5__3 = winScreen.DoWaitForTap(true);
			IL_B5:
			if (this._003CwaitForTapEnum_003E5__3.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			winScreen.playableDirector.Stop();
			winScreen.initArguments.CallOnMiddle();
			CurrencyPanel currencyPanel = winScreen.initArguments.currencyPanel;
			this._003CstarsDisplay_003E5__4 = currencyPanel.DisplayForCurrency(CurrencyType.diamonds);
			CurrencyDisplay currencyDisplay = currencyPanel.DisplayForCurrency(CurrencyType.coins);
			this._003CstarsDisplay_003E5__4.DisplayCount(winScreen.initArguments.previousStars);
			currencyDisplay.DisplayCount(winScreen.initArguments.previousCoins);
			this._003CenumList_003E5__5 = new EnumeratorsList();
			this._003CenumList_003E5__5.Clear();
			GGUtil.Hide(winScreen.thingsToHideAtEnd);
			WinScreen.Settings settings = winScreen.settings;
			this._003CenumList_003E5__5.Add(winScreen.Fade(winScreen.thingsTofadeAtEnd, 1f, 0f, settings.backgroundFadeOutDuration), 0f, null, null, false);
			this._003CenumList_003E5__5.Add(winScreen.star.DoMoveTo(winScreen.initArguments.starRect), 0f, null, null, false);
			this._003CenumList_003E5__5.Add(winScreen.coins.DoMoveCoins(Mathf.Min((int)winScreen.initArguments.coinsWon, settings.maxCoinsToAnimate), winScreen.initArguments.coinRect, winScreen.initArguments.previousCoins, winScreen.initArguments.currentCoins), 0f, null, null, false);
			IL_211:
			if (!this._003CenumList_003E5__5.Update())
			{
				this._003CstarsDisplay_003E5__4.DisplayCount(winScreen.initArguments.currentStars);
				winScreen.Hide();
				winScreen.initArguments.CallOnComplete();
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

		public WinScreen _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private IEnumerator _003CwaitForTapEnum_003E5__3;

		private CurrencyDisplay _003CstarsDisplay_003E5__4;

		private EnumeratorsList _003CenumList_003E5__5;
	}

	private sealed class _003CDoAnimation_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimation_003Ed__23(int _003C_003E1__state)
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
			WinScreen winScreen = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				winScreen.particles.CreateAndRunParticles("StarParticle", winScreen.star.transform);
				GGSoundSystem.Play(GGSoundSystem.SFXType.WinScreenStart);
				winScreen.starAnimator.Play(winScreen.inStarAnimatorState, 0);
				this._003CwaitingTillAnimationFinish_003E5__2 = winScreen.WaitTillStateFinishes(winScreen.starAnimator, winScreen.inStarAnimatorState);
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_C5;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_212;
			default:
				return false;
			}
			if (this._003CwaitingTillAnimationFinish_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003CwaitForTapEnum_003E5__3 = winScreen.DoWaitForTap(true);
			IL_C5:
			if (this._003CwaitForTapEnum_003E5__3.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			GGSoundSystem.Play(GGSoundSystem.SFXType.WinScreenReceieveAnimationStart);
			winScreen.initArguments.CallOnMiddle();
			CurrencyPanel currencyPanel = winScreen.initArguments.currencyPanel;
			this._003CstarsDisplay_003E5__4 = currencyPanel.DisplayForCurrency(CurrencyType.diamonds);
			CurrencyDisplay currencyDisplay = currencyPanel.DisplayForCurrency(CurrencyType.coins);
			this._003CstarsDisplay_003E5__4.DisplayCount(winScreen.initArguments.previousStars);
			currencyDisplay.DisplayCount(winScreen.initArguments.previousCoins);
			this._003CenumList_003E5__5 = new EnumeratorsList();
			this._003CenumList_003E5__5.Clear();
			WinScreen.Settings settings = winScreen.settings;
			this._003CenumList_003E5__5.Add(winScreen.Fade(winScreen.thingsTofadeAtEnd, 1f, 0f, settings.backgroundFadeOutDuration), 0f, null, null, false);
			this._003CenumList_003E5__5.Add(winScreen.star.DoMoveTo(winScreen.initArguments.starRect), 0f, null, null, false);
			this._003CenumList_003E5__5.Add(winScreen.coins.DoMoveCoins(Mathf.Min((int)winScreen.initArguments.coinsWon, settings.maxCoinsToAnimate), winScreen.initArguments.coinRect, winScreen.initArguments.previousCoins, winScreen.initArguments.currentCoins), 0f, null, null, false);
			IL_212:
			if (!this._003CenumList_003E5__5.Update())
			{
				this._003CstarsDisplay_003E5__4.DisplayCount(winScreen.initArguments.currentStars);
				winScreen.Hide();
				winScreen.initArguments.CallOnComplete();
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

		public WinScreen _003C_003E4__this;

		private IEnumerator _003CwaitingTillAnimationFinish_003E5__2;

		private IEnumerator _003CwaitForTapEnum_003E5__3;

		private CurrencyDisplay _003CstarsDisplay_003E5__4;

		private EnumeratorsList _003CenumList_003E5__5;
	}

	private sealed class _003CFade_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CFade_003Ed__24(int _003C_003E1__state)
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
			WinScreen winScreen = this._003C_003E4__this;
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
			}
			if (this._003Ctime_003E5__2 > this.duration)
			{
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float t = Mathf.InverseLerp(0f, this.duration, this._003Ctime_003E5__2);
			float alpha = Mathf.Lerp(this.from, this.to, t);
			winScreen.SetAlpha(this.fadeItems, alpha);
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

		public float duration;

		public float from;

		public float to;

		public WinScreen _003C_003E4__this;

		public List<CanvasGroup> fadeItems;

		private float _003Ctime_003E5__2;
	}

	private sealed class _003CFade_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CFade_003Ed__25(int _003C_003E1__state)
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
			}
			if (this._003Ctime_003E5__2 > this.duration)
			{
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float t = Mathf.InverseLerp(0f, this.duration, this._003Ctime_003E5__2);
			float alpha = Mathf.Lerp(this.from, this.to, t);
			this.visualItem.alpha = alpha;
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

		public float duration;

		public float from;

		public float to;

		public CanvasGroup visualItem;

		private float _003Ctime_003E5__2;
	}

	private sealed class _003CWaitTillStateFinishes_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CWaitTillStateFinishes_003Ed__26(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			switch (this._003C_003E1__state)
			{
			case 0:
				this._003C_003E1__state = -1;
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				break;
			case 3:
				this._003C_003E1__state = -1;
				break;
			default:
				return false;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
			if (!currentAnimatorStateInfo.IsName(this.stateName))
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			if (this.animator.IsInTransition(0))
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			if (currentAnimatorStateInfo.normalizedTime < 1f)
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 3;
				return true;
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

		public Animator animator;

		public string stateName;
	}

	private sealed class _003CDoWaitForTap_003Ed__35 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoWaitForTap_003Ed__35(int _003C_003E1__state)
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
			WinScreen winScreen = this._003C_003E4__this;
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
				winScreen.tapState = default(WinScreen.TapState);
				winScreen.tapState.isWaitingForTap = true;
				GGUtil.SetActive(winScreen.tapContainer, this.showTapContainer);
			}
			if (!winScreen.tapState.isWaitingForTap || winScreen.tapState.isTapped)
			{
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

		public WinScreen _003C_003E4__this;

		public bool showTapContainer;
	}
}
