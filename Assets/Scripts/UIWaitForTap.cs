using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UIWaitForTap : MonoBehaviour
{
	public void Hide()
	{
		this.tapState = default(UIWaitForTap.TapState);
		GGUtil.Hide(this.tapContainer);
	}

	public void OnTap(Action onTap)
	{
		this.tapState = default(UIWaitForTap.TapState);
		this.tapState.onTap = onTap;
		this.tapState.isWaitingForTap = true;
		GGUtil.Show(this.tapContainer);
	}

	public IEnumerator DoWaitForTap()
	{
		return new UIWaitForTap._003CDoWaitForTap_003Ed__5(0)
		{
			_003C_003E4__this = this
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
		GGUtil.Hide(this);
		this.tapState.isTapped = true;
		this.tapState.CallOnTap();
	}

	[SerializeField]
	private RectTransform tapContainer;

	private UIWaitForTap.TapState tapState;

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

	private sealed class _003CDoWaitForTap_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoWaitForTap_003Ed__5(int _003C_003E1__state)
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
			UIWaitForTap uiwaitForTap = this._003C_003E4__this;
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
				uiwaitForTap.tapState = default(UIWaitForTap.TapState);
				uiwaitForTap.tapState.isWaitingForTap = true;
				GGUtil.Show(uiwaitForTap.tapContainer);
			}
			if (!uiwaitForTap.tapState.isWaitingForTap || uiwaitForTap.tapState.isTapped)
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

		public UIWaitForTap _003C_003E4__this;
	}
}
