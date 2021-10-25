using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class TalkingDialog : MonoBehaviour
{
	public IEnumerator DoShow(TalkingDialog.ShowArguments showArguments)
	{
		return new TalkingDialog._003CDoShow_003Ed__8(0)
		{
			_003C_003E4__this = this,
			showArguments = showArguments
		};
	}

	public void ShowSingleLine(string toSay)
	{
		GGUtil.Hide(this.clickContainer);
		GGUtil.Show(this);
		GGUtil.ChangeText(this.talkLabel, toSay);
	}

	public void Show(TalkingDialog.ShowArguments showArguments)
	{
		this.isActive = true;
		this.showArguments = showArguments;
		this.animEnum = this.DoShowText();
		this.animEnum.MoveNext();
		GGUtil.Show(this);
		GGUtil.Hide(this.clickContainer);
	}

	public void Hide()
	{
		this.isActive = false;
		GGUtil.Hide(this);
	}

	private IEnumerator DoShowText()
	{
		return new TalkingDialog._003CDoShowText_003Ed__12(0)
		{
			_003C_003E4__this = this
		};
	}

	private void InputHandler_OnClick(Vector2 screenPosition)
	{
		this.clickParams.isClicked = true;
	}

	private IEnumerator DoWaitForClick()
	{
		return new TalkingDialog._003CDoWaitForClick_003Ed__14(0)
		{
			_003C_003E4__this = this
		};
	}

	public void ButtonCallback_OnClick()
	{
		if (!this.clickParams.isWaitingForClick)
		{
			return;
		}
		this.clickParams.isClicked = true;
	}

	private void Update()
	{
		if (this.animEnum != null)
		{
			this.animEnum.MoveNext();
		}
	}

	[SerializeField]
	private TextMeshProUGUI talkLabel;

	[SerializeField]
	private Transform clickContainer;

	[NonSerialized]
	private TalkingDialog.ShowArguments showArguments;

	[NonSerialized]
	private IEnumerator animEnum;

	[NonSerialized]
	private TalkingDialog.ClickParams clickParams;

	private bool isActive;

	public class ShowArguments
	{
		public List<string> thingsToSay = new List<string>();

		public Action onComplete;

		public InputHandler inputHandler;
	}

	private struct ClickParams
	{
		public bool isWaitingForClick;

		public bool isClicked;
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
			TalkingDialog talkingDialog = this._003C_003E4__this;
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
				talkingDialog.Show(this.showArguments);
			}
			if (!talkingDialog.isActive)
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

		public TalkingDialog _003C_003E4__this;

		public TalkingDialog.ShowArguments showArguments;
	}

	private sealed class _003CDoShowText_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShowText_003Ed__12(int _003C_003E1__state)
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
			TalkingDialog talkingDialog = this._003C_003E4__this;
			if (num == 0)
			{
				this._003C_003E1__state = -1;
				this._003CthingsToSay_003E5__2 = talkingDialog.showArguments.thingsToSay;
				this._003Ci_003E5__3 = 0;
				goto IL_9F;
			}
			if (num != 1)
			{
				return false;
			}
			this._003C_003E1__state = -1;
			IL_7B:
			if (this._003Ce_003E5__4.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003Ce_003E5__4 = null;
			int num2 = this._003Ci_003E5__3;
			this._003Ci_003E5__3 = num2 + 1;
			IL_9F:
			if (this._003Ci_003E5__3 >= this._003CthingsToSay_003E5__2.Count)
			{
				talkingDialog.Hide();
				Action onComplete = talkingDialog.showArguments.onComplete;
				if (onComplete != null)
				{
					onComplete();
				}
				return false;
			}
			string text = this._003CthingsToSay_003E5__2[this._003Ci_003E5__3];
			GGUtil.ChangeText(talkingDialog.talkLabel, text);
			this._003Ce_003E5__4 = talkingDialog.DoWaitForClick();
			goto IL_7B;
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

		public TalkingDialog _003C_003E4__this;

		private List<string> _003CthingsToSay_003E5__2;

		private int _003Ci_003E5__3;

		private IEnumerator _003Ce_003E5__4;
	}

	private sealed class _003CDoWaitForClick_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoWaitForClick_003Ed__14(int _003C_003E1__state)
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
			TalkingDialog talkingDialog = this._003C_003E4__this;
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
				talkingDialog.clickParams = default(TalkingDialog.ClickParams);
				talkingDialog.clickParams.isWaitingForClick = true;
				if (talkingDialog.showArguments.inputHandler != null)
				{
					talkingDialog.showArguments.inputHandler.Clear();
					talkingDialog.showArguments.inputHandler.onClick -= talkingDialog.InputHandler_OnClick;
					talkingDialog.showArguments.inputHandler.onClick += talkingDialog.InputHandler_OnClick;
				}
				else
				{
					GGUtil.Show(talkingDialog.clickContainer);
				}
			}
			if (talkingDialog.clickParams.isClicked)
			{
				if (talkingDialog.showArguments.inputHandler != null)
				{
					talkingDialog.showArguments.inputHandler.onClick -= talkingDialog.InputHandler_OnClick;
				}
				GGUtil.Hide(talkingDialog.clickContainer);
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

		public TalkingDialog _003C_003E4__this;
	}
}
