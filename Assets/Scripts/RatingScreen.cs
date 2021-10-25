using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;

public class RatingScreen : MonoBehaviour
{
	private void OnEnable()
	{
		this.Init();
	}

	private void Init()
	{
		GGUtil.Hide(this.widgetsToHide);
		this.animation = this.DoAnimation();
	}

	private void PlayInState()
	{
		this.mainAnimation.gameObject.SetActive(false);
		this.mainAnimation.gameObject.SetActive(true);
		this.mainAnimation.Play(this.inState, 0);
	}

	private void ShowYesNo(string yesText, string noText)
	{
		GGUtil.ChangeText(this.yesLabel, yesText);
		GGUtil.ChangeText(this.noLabel, noText);
		GGUtil.Hide(this.widgetsToHide);
		this.buttonStyle.Apply();
	}

	private IEnumerator DoAnimation()
	{
		return new RatingScreen._003CDoAnimation_003Ed__16(0)
		{
			_003C_003E4__this = this
		};
	}

	private void End(bool isLike, bool isGoingToRate)
	{
		NavigationManager.instance.Pop(true);
		new Analytics.RateDialog
		{
			timesShown = ScriptableObjectSingleton<RateCallerSettings>.instance.timesShown,
			isLike = isLike,
			isGoingToRate = isGoingToRate
		}.Send();
	}

	private IEnumerator WaitForButtonPress()
	{
		return new RatingScreen._003CWaitForButtonPress_003Ed__18(0)
		{
			_003C_003E4__this = this
		};
	}

	private void ButtonPress(bool success)
	{
		UnityEngine.Debug.Log("BUTTON PRESS");
		if (this.buttonState.time < this.minTimeBeforeClick)
		{
			return;
		}
		this.buttonState.isAccepted = success;
		this.buttonState.isDone = true;
		UnityEngine.Debug.Log("Is Done");
	}

	public void ButtonCallback_OnYes()
	{
		UnityEngine.Debug.Log("On Yes");
		this.ButtonPress(true);
	}

	public void ButtonCallback_OnNo()
	{
		UnityEngine.Debug.Log("On No");
		this.ButtonPress(false);
	}

	private void Update()
	{
		this.buttonState.time = this.buttonState.time + Time.unscaledDeltaTime;
		if (this.animation != null)
		{
			this.animation.MoveNext();
		}
	}

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private VisualStyleSet buttonStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet clickToContinueStyle = new VisualStyleSet();

	[SerializeField]
	private Animator mainAnimation;

	[SerializeField]
	private TextMeshProUGUI mainlabel;

	[SerializeField]
	private string inState;

	[SerializeField]
	private float minTimeBeforeClick;

	private IEnumerator animation;

	[SerializeField]
	private TextMeshProUGUI yesLabel;

	[SerializeField]
	private TextMeshProUGUI noLabel;

	private RatingScreen.ButtonState buttonState;

	public struct ButtonState
	{
		public bool isActive;

		public bool isAccepted;

		public bool isDone;

		public float time;
	}

	private sealed class _003CDoAnimation_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimation_003Ed__16(int _003C_003E1__state)
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
			RatingScreen ratingScreen = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
				GGUtil.ChangeText(ratingScreen.mainlabel, "Are you enjoying Home Design?");
				GGUtil.Hide(ratingScreen.widgetsToHide);
				ratingScreen.ShowYesNo("Yes", "No");
				this._003CwaitEnum_003E5__2 = ratingScreen.WaitForButtonPress();
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_10E;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_177;
			default:
				return false;
			}
			if (this._003CwaitEnum_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			if (!ratingScreen.buttonState.isAccepted)
			{
				ratingScreen.End(false, false);
				ScriptableObjectSingleton<RateCallerSettings>.instance.OnUserNotLike();
				return false;
			}
			ratingScreen.PlayInState();
			GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
			GGUtil.ChangeText(ratingScreen.mainlabel, "Wohoo!!!");
			GGUtil.Hide(ratingScreen.widgetsToHide);
			ratingScreen.clickToContinueStyle.Apply();
			ratingScreen.PlayInState();
			this._003CwaitEnum_003E5__2 = ratingScreen.WaitForButtonPress();
			IL_10E:
			if (this._003CwaitEnum_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
			GGUtil.ChangeText(ratingScreen.mainlabel, "Please leave us a Rating");
			GGUtil.Hide(ratingScreen.widgetsToHide);
			ratingScreen.ShowYesNo("Yes", "Later");
			ratingScreen.PlayInState();
			this._003CwaitEnum_003E5__2 = ratingScreen.WaitForButtonPress();
			IL_177:
			if (this._003CwaitEnum_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 3;
				return true;
			}
			if (!ratingScreen.buttonState.isAccepted)
			{
				ratingScreen.End(true, false);
				return false;
			}
			ScriptableObjectSingleton<RateCallerSettings>.instance.OnUserRated();
			GGSupportMenu.instance.showRateApp(ConfigBase.instance.platformRateProvider);
			ratingScreen.End(true, true);
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

		public RatingScreen _003C_003E4__this;

		private IEnumerator _003CwaitEnum_003E5__2;
	}

	private sealed class _003CWaitForButtonPress_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CWaitForButtonPress_003Ed__18(int _003C_003E1__state)
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
			RatingScreen ratingScreen = this._003C_003E4__this;
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
				ratingScreen.buttonState = default(RatingScreen.ButtonState);
				ratingScreen.buttonState.isActive = true;
			}
			if (ratingScreen.buttonState.isDone)
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

		public RatingScreen _003C_003E4__this;
	}
}
