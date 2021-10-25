using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TriggerAnimationEvery : MonoBehaviour
{
	private void OnEnable()
	{
		this.animation = this.DoAnimation();
	}

	private IEnumerator DoAnimation()
	{
		return new TriggerAnimationEvery._003CDoAnimation_003Ed__7(0)
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

	[SerializeField]
	private float fromTime;

	[SerializeField]
	private float toTime;

	[SerializeField]
	private float animationWait;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private string stateToPlay;

	private IEnumerator animation;

	private sealed class _003CDoAnimation_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimation_003Ed__7(int _003C_003E1__state)
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
			TriggerAnimationEvery triggerAnimationEvery = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				break;
			case 1:
				this._003C_003E1__state = -1;
				goto IL_76;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_E6;
			default:
				return false;
			}
			IL_29:
			this._003Cdelay_003E5__2 = UnityEngine.Random.Range(triggerAnimationEvery.fromTime, triggerAnimationEvery.toTime);
			this._003Ctime_003E5__3 = 0f;
			IL_76:
			if (this._003Ctime_003E5__3 <= this._003Cdelay_003E5__2)
			{
				this._003Ctime_003E5__3 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			if (triggerAnimationEvery.animator != null)
			{
				triggerAnimationEvery.animator.Play(triggerAnimationEvery.stateToPlay, 0);
			}
			this._003Cdelay_003E5__2 = triggerAnimationEvery.animationWait;
			this._003Ctime_003E5__3 = 0f;
			IL_E6:
			if (this._003Ctime_003E5__3 > this._003Cdelay_003E5__2)
			{
				goto IL_29;
			}
			this._003Ctime_003E5__3 += Time.deltaTime;
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

		public TriggerAnimationEvery _003C_003E4__this;

		private float _003Cdelay_003E5__2;

		private float _003Ctime_003E5__3;
	}
}
