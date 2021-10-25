using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
	private void OnEnable()
	{
		this.Init();
	}

	private void Init()
	{
		this.animationEnumerator = this.DoLoadFirstScene();
		this.animationEnumerator.MoveNext();
	}

	private IEnumerator DoLoadFirstScene()
	{
		return new SplashScreen._003CDoLoadFirstScene_003Ed__6(0)
		{
			_003C_003E4__this = this
		};
	}

	private void Update()
	{
		if (this.animationEnumerator != null)
		{
			this.animationEnumerator.MoveNext();
		}
	}

	[SerializeField]
	private string sceneName = "MainUI";

	[SerializeField]
	private bool showTermsOfServiceDialog;

	[SerializeField]
	private Image progressBarSprite;

	private IEnumerator animationEnumerator;

	private sealed class _003C_003Ec__DisplayClass6_0
	{
		internal void _003CDoLoadFirstScene_003Eb__0(bool success)
		{
			if (!success)
			{
				Application.Quit();
				return;
			}
			GGPlayerSettings.instance.Model.acceptedTermsOfService = true;
			GGPlayerSettings.instance.Save();
			this.nav.Pop(true);
			this.termsOfServiceDone = true;
		}

		public NavigationManager nav;

		public bool termsOfServiceDone;

		public Action<bool> _003C_003E9__0;
	}

	private sealed class _003CDoLoadFirstScene_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoLoadFirstScene_003Ed__6(int _003C_003E1__state)
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
			SplashScreen splashScreen = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				this._003C_003E8__1 = new SplashScreen._003C_003Ec__DisplayClass6_0();
				GGUtil.SetFill(splashScreen.progressBarSprite, 0f);
				this._003C_003E8__1.nav = NavigationManager.instance;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			case 1:
				this._003C_003E1__state = -1;
				this._003CasyncOperation_003E5__2 = SceneManager.LoadSceneAsync(splashScreen.sceneName);
				this._003CneedsToShowTermsOfService_003E5__3 = (splashScreen.showTermsOfServiceDialog && !GGPlayerSettings.instance.Model.acceptedTermsOfService);
				this._003CasyncOperation_003E5__2.allowSceneActivation = !this._003CneedsToShowTermsOfService_003E5__3;
				this._003C_003E8__1.termsOfServiceDone = false;
				goto IL_194;
			case 2:
				this._003C_003E1__state = -1;
				break;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_194;
			default:
				return false;
			}
			IL_164:
			if (!this._003C_003E8__1.termsOfServiceDone)
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			IL_171:
			this._003CasyncOperation_003E5__2.allowSceneActivation = true;
			IL_17D:
			this._003C_003E2__current = null;
			this._003C_003E1__state = 3;
			return true;
			IL_194:
			if (this._003CasyncOperation_003E5__2.isDone)
			{
				return false;
			}
			GGUtil.SetFill(splashScreen.progressBarSprite, this._003CasyncOperation_003E5__2.progress);
			if (this._003CasyncOperation_003E5__2.allowSceneActivation || this._003CasyncOperation_003E5__2.progress < 0.9f)
			{
				goto IL_17D;
			}
			if (this._003CneedsToShowTermsOfService_003E5__3)
			{
				TermsOfServiceDialog @object = this._003C_003E8__1.nav.GetObject<TermsOfServiceDialog>();
				Action<bool> onComplete;
				if ((onComplete = this._003C_003E8__1._003C_003E9__0) == null)
				{
					onComplete = (this._003C_003E8__1._003C_003E9__0 = new Action<bool>(this._003C_003E8__1._003CDoLoadFirstScene_003Eb__0));
				}
				@object.Show(onComplete);
				goto IL_164;
			}
			goto IL_171;
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

		public SplashScreen _003C_003E4__this;

		private SplashScreen._003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		private AsyncOperation _003CasyncOperation_003E5__2;

		private bool _003CneedsToShowTermsOfService_003E5__3;
	}
}
