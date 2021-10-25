using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RankedBoostersStartAnimation : MonoBehaviour
{
	public void Show(int rankLevel)
	{
		this.boosters.Init(rankLevel);
		GGUtil.Show(this);
		this.animation = this.DoAnimation();
		this.animation.MoveNext();
	}

	private IEnumerator DoAnimation()
	{
		return new RankedBoostersStartAnimation._003CDoAnimation_003Ed__5(0)
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
	private RankedBoostersContainer boosters;

	[SerializeField]
	public float boosterDelay = 1f;

	[SerializeField]
	private float timeToLive;

	private IEnumerator animation;

	private sealed class _003CDoAnimation_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimation_003Ed__5(int _003C_003E1__state)
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
			RankedBoostersStartAnimation rankedBoostersStartAnimation = this._003C_003E4__this;
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
			if (this._003Ctime_003E5__2 > rankedBoostersStartAnimation.timeToLive)
			{
				GGUtil.Hide(rankedBoostersStartAnimation);
				return false;
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

		public RankedBoostersStartAnimation _003C_003E4__this;

		private float _003Ctime_003E5__2;
	}
}
