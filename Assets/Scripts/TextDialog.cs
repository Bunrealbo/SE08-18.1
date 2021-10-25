using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDialog : MonoBehaviour
{
	public void ShowOk(TextDialog.MessageArguments messageArguments, Action onComplete)
	{
		this.messageArguments = messageArguments;
		string message = messageArguments.message;
		GGUtil.ChangeText(this.textLabel, message);
		this.onComplete = onComplete;
		this.time = 0f;
		NavigationManager.instance.Push(base.gameObject, true);
		this.animation = null;
		if (!messageArguments.showProgress)
		{
			GGUtil.Hide(this.progressContainer);
		}
		else
		{
			this.animation = this.DoProgressBar();
			this.animation.MoveNext();
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
	}

	public void ButtonCallback_OnClick()
	{
		if (this.time <= this.introDuration)
		{
			return;
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
		NavigationManager.instance.Pop(true);
		if (this.onComplete != null)
		{
			this.onComplete();
		}
	}

	private IEnumerator DoProgressBar()
	{
		return new TextDialog._003CDoProgressBar_003Ed__13(0)
		{
			_003C_003E4__this = this
		};
	}

	private void Update()
	{
		this.time += Time.deltaTime;
		if (this.animation != null)
		{
			this.animation.MoveNext();
		}
	}

	[SerializeField]
	private TextMeshProUGUI textLabel;

	[SerializeField]
	private float introDuration = 0.5f;

	[SerializeField]
	private float time;

	[SerializeField]
	private Transform progressContainer;

	[SerializeField]
	private Image progressBar;

	[SerializeField]
	private float progressBarDuration = 1f;

	[SerializeField]
	private TextMeshProUGUI progressTextLabel;

	private Action onComplete;

	private IEnumerator animation;

	private TextDialog.MessageArguments messageArguments;

	public struct MessageArguments
	{
		public string message;

		public bool showProgress;

		public float fromProgress;

		public float toProgress;
	}

	private sealed class _003CDoProgressBar_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoProgressBar_003Ed__13(int _003C_003E1__state)
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
			TextDialog textDialog = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				GGUtil.Show(textDialog.progressContainer);
				textDialog.progressBar.fillAmount = textDialog.messageArguments.fromProgress;
				GGUtil.ChangeText(textDialog.progressTextLabel, string.Format("{0}%", GGFormat.FormatPercent(textDialog.messageArguments.fromProgress)));
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			case 1:
				this._003C_003E1__state = -1;
				this._003Ctime_003E5__2 = 0f;
				break;
			case 2:
				this._003C_003E1__state = -1;
				break;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_162;
			default:
				return false;
			}
			if (this._003Ctime_003E5__2 < textDialog.introDuration)
			{
				this._003Ctime_003E5__2 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			this._003Ctime_003E5__2 = 0f;
			IL_162:
			if (this._003Ctime_003E5__2 >= textDialog.progressBarDuration)
			{
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float t = Mathf.InverseLerp(0f, textDialog.progressBarDuration, this._003Ctime_003E5__2);
			float num2 = Mathf.Lerp(textDialog.messageArguments.fromProgress, textDialog.messageArguments.toProgress, t);
			GGUtil.ChangeText(textDialog.progressTextLabel, string.Format("{0}%", GGFormat.FormatPercent(num2)));
			textDialog.progressBar.fillAmount = num2;
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

		public TextDialog _003C_003E4__this;

		private float _003Ctime_003E5__2;
	}
}
