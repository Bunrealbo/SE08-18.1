using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class MovesContainer : MonoBehaviour
{
	public void Show(string text)
	{
		GGUtil.ChangeText(this.label, text);
		GGUtil.SetActive(this, true);
		this.animation = this.ShowAndHide();
		this.animation.MoveNext();
	}

	private void Update()
	{
		if (this.animation != null)
		{
			this.animation.MoveNext();
		}
	}

	public void Reset()
	{
		this.animator.Play("InitialState");
	}

	private IEnumerator ShowAndHide()
	{
		return new MovesContainer._003CShowAndHide_003Ed__7(0)
		{
			_003C_003E4__this = this
		};
	}

	public IEnumerator DoShow()
	{
		return new MovesContainer._003CDoShow_003Ed__8(0)
		{
			_003C_003E4__this = this
		};
	}

	public IEnumerator DoHide()
	{
		return new MovesContainer._003CDoHide_003Ed__9(0)
		{
			_003C_003E4__this = this
		};
	}

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private TextMeshProUGUI label;

	[SerializeField]
	private float showTime;

	private IEnumerator animation;

	private sealed class _003CShowAndHide_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CShowAndHide_003Ed__7(int _003C_003E1__state)
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
			MovesContainer movesContainer = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				this._003Cenumerator_003E5__2 = movesContainer.DoShow();
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_95;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_C8;
			default:
				return false;
			}
			if (this._003Cenumerator_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003Ctime_003E5__3 = 0f;
			IL_95:
			if (this._003Ctime_003E5__3 < movesContainer.showTime)
			{
				this._003Ctime_003E5__3 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			this._003Cenumerator_003E5__2 = movesContainer.DoHide();
			IL_C8:
			if (!this._003Cenumerator_003E5__2.MoveNext())
			{
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

		public MovesContainer _003C_003E4__this;

		private IEnumerator _003Cenumerator_003E5__2;

		private float _003Ctime_003E5__3;
	}

	private sealed class _003CDoShow_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShow_003Ed__8(int _003C_003E1__state)
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
			MovesContainer movesContainer = this._003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				AnimatorStateInfo currentAnimatorStateInfo = movesContainer.animator.GetCurrentAnimatorStateInfo(0);
				bool flag = currentAnimatorStateInfo.IsName("InAnimation");
				if (!flag || (flag && currentAnimatorStateInfo.normalizedTime >= 1f))
				{
					return false;
				}
			}
			else
			{
				this._003C_003E1__state = -1;
				GGUtil.SetActive(movesContainer, true);
				movesContainer.animator.Play("InAnimation");
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

		public MovesContainer _003C_003E4__this;
	}

	private sealed class _003CDoHide_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoHide_003Ed__9(int _003C_003E1__state)
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
			MovesContainer movesContainer = this._003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				AnimatorStateInfo currentAnimatorStateInfo = movesContainer.animator.GetCurrentAnimatorStateInfo(0);
				movesContainer.animator.GetAnimatorTransitionInfo(0);
				bool flag = currentAnimatorStateInfo.IsName("OutAnimation");
				if (!flag || (flag && currentAnimatorStateInfo.normalizedTime >= 1f))
				{
					GGUtil.SetActive(movesContainer, false);
					return false;
				}
			}
			else
			{
				this._003C_003E1__state = -1;
				movesContainer.animator.Play("OutAnimation");
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

		public MovesContainer _003C_003E4__this;
	}
}
