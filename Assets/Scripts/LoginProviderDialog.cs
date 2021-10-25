using System;
using GGMatch3;
using UnityEngine;

public class LoginProviderDialog : MonoBehaviour
{
	public void Show(Action<LoginProviderDialog.LoginResponse> onComplete)
	{
		this.onComplete = onComplete;
		NavigationManager.instance.Push(base.gameObject, true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
	}

	public void ButtonCallback_OnCancel()
	{
		LoginProviderDialog.LoginResponse obj = default(LoginProviderDialog.LoginResponse);
		obj.isCancelled = true;
		Action<LoginProviderDialog.LoginResponse> action = this.onComplete;
		if (action != null)
		{
			action(obj);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void ButtonCallback_OnAppleLogin()
	{
		this.Complete(LoginProvider.AppleLogin);
	}

	public void ButtonCallback_OnFacebookLogin()
	{
		this.Complete(LoginProvider.FacebookLogin);
	}

	private void Complete(LoginProvider provider)
	{
		LoginProviderDialog.LoginResponse obj = default(LoginProviderDialog.LoginResponse);
		obj.loginProvider = provider;
		Action<LoginProviderDialog.LoginResponse> action = this.onComplete;
		if (action != null)
		{
			action(obj);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	private Action<LoginProviderDialog.LoginResponse> onComplete;

	public struct LoginResponse
	{
		public bool isCancelled;

		public LoginProvider loginProvider;
	}
}
