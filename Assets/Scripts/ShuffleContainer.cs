using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShuffleContainer : MonoBehaviour
{
	public void Reset()
	{
		this.animator.Play("InitialState");
	}

	public IEnumerator DoShow()
	{
		return new ShuffleContainer._003CDoShow_003Ed__2(0)
		{
			_003C_003E4__this = this
		};
	}

	public IEnumerator DoHide()
	{
		return new ShuffleContainer._003CDoHide_003Ed__3(0)
		{
			_003C_003E4__this = this
		};
	}

	[SerializeField]
	private Animator animator;

	private sealed class _003CDoShow_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShow_003Ed__2(int _003C_003E1__state)
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
			ShuffleContainer shuffleContainer = this._003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				AnimatorStateInfo currentAnimatorStateInfo = shuffleContainer.animator.GetCurrentAnimatorStateInfo(0);
				bool flag = currentAnimatorStateInfo.IsName("InAnimation");
				if (!flag || (flag && currentAnimatorStateInfo.normalizedTime >= 1f))
				{
					return false;
				}
			}
			else
			{
				this._003C_003E1__state = -1;
				GGUtil.SetActive(shuffleContainer, true);
				shuffleContainer.animator.Play("InAnimation");
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

		public ShuffleContainer _003C_003E4__this;
	}

	private sealed class _003CDoHide_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoHide_003Ed__3(int _003C_003E1__state)
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
			ShuffleContainer shuffleContainer = this._003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				AnimatorStateInfo currentAnimatorStateInfo = shuffleContainer.animator.GetCurrentAnimatorStateInfo(0);
				shuffleContainer.animator.GetAnimatorTransitionInfo(0);
				bool flag = currentAnimatorStateInfo.IsName("OutAnimation");
				if (!flag || (flag && currentAnimatorStateInfo.normalizedTime >= 1f))
				{
					GGUtil.SetActive(shuffleContainer, false);
					return false;
				}
			}
			else
			{
				this._003C_003E1__state = -1;
				shuffleContainer.animator.Play("OutAnimation");
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

		public ShuffleContainer _003C_003E4__this;
	}
}
