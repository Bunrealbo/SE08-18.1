using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WinScreenAnimation : MonoBehaviour
{
	public void ShowAnimation(Action onMiddle, Action onEnd)
	{
		this.onMiddle = onMiddle;
		this.onEnd = onEnd;
		GGUtil.SetActive(this, true);
		GGUtil.SetActive(this.animationTransform, true);
		this.animationAnimator.Play(this.stateNameToPlay);
		this.animationEnumerator = this.DoPlay();
	}

	private IEnumerator DoPlay()
	{
		return new WinScreenAnimation._003CDoPlay_003Ed__8(0)
		{
			_003C_003E4__this = this
		};
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
	private Animator animationAnimator;

	[SerializeField]
	private Transform animationTransform;

	[SerializeField]
	private string stateNameToPlay;

	[SerializeField]
	private float normalizedTimeOfMiddle = 0.5f;

	private Action onMiddle;

	private Action onEnd;

	private IEnumerator animationEnumerator;

	private sealed class _003CDoPlay_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoPlay_003Ed__8(int _003C_003E1__state)
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
			WinScreenAnimation winScreenAnimation = this._003C_003E4__this;
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
				this._003CcalledOnMiddle_003E5__2 = false;
			}
			AnimatorStateInfo currentAnimatorStateInfo = winScreenAnimation.animationAnimator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName(winScreenAnimation.stateNameToPlay))
			{
				if (!this._003CcalledOnMiddle_003E5__2 && currentAnimatorStateInfo.normalizedTime >= winScreenAnimation.normalizedTimeOfMiddle)
				{
					this._003CcalledOnMiddle_003E5__2 = true;
					if (winScreenAnimation.onMiddle != null)
					{
						winScreenAnimation.onMiddle();
					}
				}
				if (currentAnimatorStateInfo.normalizedTime < 1f)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
			}
			if (!this._003CcalledOnMiddle_003E5__2 && winScreenAnimation.onMiddle != null)
			{
				winScreenAnimation.onMiddle();
			}
			if (winScreenAnimation.onEnd != null)
			{
				winScreenAnimation.onEnd();
			}
			GGUtil.SetActive(winScreenAnimation.animationTransform, false);
			GGUtil.SetActive(winScreenAnimation, false);
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

		public WinScreenAnimation _003C_003E4__this;

		private bool _003CcalledOnMiddle_003E5__2;
	}
}
