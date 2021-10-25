using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Playables;

public class WellDoneContainer : MonoBehaviour
{
	public void Show(WellDoneContainer.InitArguments initArguments)
	{
		this.initArguments = initArguments;
		GGUtil.SetActive(this, true);
		if (this.animationType == WellDoneContainer.AnimationType.Director)
		{
			this.runningAnimation = this.DoShowAnimatioDirector();
		}
		else if (this.animationType == WellDoneContainer.AnimationType.InAndOout)
		{
			this.runningAnimation = this.DoShowAnimation();
		}
		else
		{
			this.runningAnimation = this.DoShowSingleAnimation();
		}
		this.runningAnimation.MoveNext();
		GGSoundSystem.Play(GGSoundSystem.SFXType.YouWinAnimation);
	}

	public void Hide()
	{
		GGUtil.SetActive(this, false);
		this.runningAnimation = null;
	}

	private IEnumerator DoShowAnimatioDirector()
	{
		return new WellDoneContainer._003CDoShowAnimatioDirector_003Ed__12(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator DoShowSingleAnimation()
	{
		return new WellDoneContainer._003CDoShowSingleAnimation_003Ed__13(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator DoShowAnimation()
	{
		return new WellDoneContainer._003CDoShowAnimation_003Ed__14(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator WaitTillStateFinishes(Animator animator, string stateName)
	{
		return new WellDoneContainer._003CWaitTillStateFinishes_003Ed__15(0)
		{
			animator = animator,
			stateName = stateName
		};
	}

	private void Update()
	{
		if (this.runningAnimation != null && !this.runningAnimation.MoveNext())
		{
			this.runningAnimation = null;
		}
	}

	[SerializeField]
	private string inStateName;

	[SerializeField]
	private string outStateName;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float centerDuration;

	[SerializeField]
	private PlayableDirector director;

	[SerializeField]
	private WellDoneContainer.AnimationType animationType;

	private WellDoneContainer.InitArguments initArguments;

	private IEnumerator runningAnimation;

	public enum AnimationType
	{
		InAndOout,
		SingleAnimation,
		Director
	}

	public struct InitArguments
	{
		public InitArguments(Action onComplete)
		{
			this.onComplete = onComplete;
		}

		public void CallOnComplete()
		{
			if (this.onComplete == null)
			{
				return;
			}
			this.onComplete();
		}

		public Action onComplete;
	}

	private sealed class _003CDoShowAnimatioDirector_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShowAnimatioDirector_003Ed__12(int _003C_003E1__state)
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
			WellDoneContainer wellDoneContainer = this._003C_003E4__this;
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
				wellDoneContainer.director.Play();
			}
			if (wellDoneContainer.director.state != PlayState.Playing)
			{
				wellDoneContainer.Hide();
				wellDoneContainer.initArguments.CallOnComplete();
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

		public WellDoneContainer _003C_003E4__this;
	}

	private sealed class _003CDoShowSingleAnimation_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShowSingleAnimation_003Ed__13(int _003C_003E1__state)
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
			WellDoneContainer wellDoneContainer = this._003C_003E4__this;
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
				wellDoneContainer.animator.Play(wellDoneContainer.inStateName, 0);
				this._003CwaitingEnum_003E5__2 = wellDoneContainer.WaitTillStateFinishes(wellDoneContainer.animator, wellDoneContainer.inStateName);
			}
			if (!this._003CwaitingEnum_003E5__2.MoveNext())
			{
				wellDoneContainer.Hide();
				wellDoneContainer.initArguments.CallOnComplete();
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

		public WellDoneContainer _003C_003E4__this;

		private IEnumerator _003CwaitingEnum_003E5__2;
	}

	private sealed class _003CDoShowAnimation_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShowAnimation_003Ed__14(int _003C_003E1__state)
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
			WellDoneContainer wellDoneContainer = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				wellDoneContainer.animator.Play(wellDoneContainer.inStateName, 0);
				this._003CwaitingEnum_003E5__2 = wellDoneContainer.WaitTillStateFinishes(wellDoneContainer.animator, wellDoneContainer.inStateName);
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_B3;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_104;
			default:
				return false;
			}
			if (this._003CwaitingEnum_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003Ctime_003E5__3 = 0f;
			IL_B3:
			if (this._003Ctime_003E5__3 < wellDoneContainer.centerDuration)
			{
				this._003Ctime_003E5__3 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			wellDoneContainer.animator.Play(wellDoneContainer.outStateName, 0);
			this._003CwaitingEnum_003E5__2 = wellDoneContainer.WaitTillStateFinishes(wellDoneContainer.animator, wellDoneContainer.outStateName);
			IL_104:
			if (!this._003CwaitingEnum_003E5__2.MoveNext())
			{
				wellDoneContainer.Hide();
				wellDoneContainer.initArguments.CallOnComplete();
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

		public WellDoneContainer _003C_003E4__this;

		private IEnumerator _003CwaitingEnum_003E5__2;

		private float _003Ctime_003E5__3;
	}

	private sealed class _003CWaitTillStateFinishes_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CWaitTillStateFinishes_003Ed__15(int _003C_003E1__state)
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
}
