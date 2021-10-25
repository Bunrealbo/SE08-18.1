using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;

public class WellDoneScreen : MonoBehaviour
{
	public void Show(WellDoneScreen.InitArguments initArguments)
	{
		this.initArguments = initArguments;
		NavigationManager.instance.Push(this, true);
	}

	private void Init()
	{
		GGUtil.ChangeText(this.mainTextLabel, this.initArguments.mainText);
		this.animation = this.DoAnimation();
		this.animation.MoveNext();
	}

	private IEnumerator DoAnimation()
	{
		return new WellDoneScreen._003CDoAnimation_003Ed__7(0)
		{
			_003C_003E4__this = this
		};
	}

	private void OnEnable()
	{
		this.Init();
	}

	private void Update()
	{
		if (this.animation != null)
		{
			this.animation.MoveNext();
		}
	}

	[SerializeField]
	private WellDoneContainer container;

	[SerializeField]
	private TextMeshProUGUI mainTextLabel;

	private IEnumerator animation;

	private WellDoneScreen.InitArguments initArguments;

	public struct InitArguments
	{
		public string mainText;

		public Action onComplete;
	}

	private sealed class _003C_003Ec__DisplayClass7_0
	{
		internal void _003CDoAnimation_003Eb__0()
		{
			this.complete = true;
		}

		public bool complete;
	}

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
			WellDoneScreen wellDoneScreen = this._003C_003E4__this;
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
				this._003C_003E8__1 = new WellDoneScreen._003C_003Ec__DisplayClass7_0();
				WellDoneContainer.InitArguments initArguments = default(WellDoneContainer.InitArguments);
				this._003C_003E8__1.complete = false;
				initArguments.onComplete = new Action(this._003C_003E8__1._003CDoAnimation_003Eb__0);
				wellDoneScreen.container.Show(initArguments);
			}
			if (this._003C_003E8__1.complete)
			{
				wellDoneScreen.container.Hide();
				if (wellDoneScreen.initArguments.onComplete != null)
				{
					wellDoneScreen.initArguments.onComplete();
				}
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

		public WellDoneScreen _003C_003E4__this;

		private WellDoneScreen._003C_003Ec__DisplayClass7_0 _003C_003E8__1;
	}
}
