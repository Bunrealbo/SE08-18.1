using System;
using GGMatch3;
using UnityEngine;

public class LoginToFBButton : MonoBehaviour
{
	public void ButtonCallback_OnButtonPress()
	{
		LoginToFBButton._003C_003Ec__DisplayClass0_0 _003C_003Ec__DisplayClass0_ = new LoginToFBButton._003C_003Ec__DisplayClass0_0();
		_003C_003Ec__DisplayClass0_._003C_003E4__this = this;
		_003C_003Ec__DisplayClass0_.nav = NavigationManager.instance;
		if (BehaviourSingletonInit<GGAppleSignIn>.instance.isAvailable)
		{
			_003C_003Ec__DisplayClass0_.nav.GetObject<LoginProviderDialog>().Show(new Action<LoginProviderDialog.LoginResponse>(_003C_003Ec__DisplayClass0_._003CButtonCallback_OnButtonPress_003Eb__0));
			return;
		}
		this.LoginToFacebook();
	}

	private void LoginToFacebook()
	{
		GGFacebook instance = BehaviourSingletonInit<GGFacebook>.instance;
		if (!instance.IsInitialized())
		{
			UnityEngine.Debug.Log("FACEBOOK NOT INITIALIZED");
			return;
		}
		instance.Login(new FBLoginParams
		{
			scope = "public_profile",
			onComplete = new Action<FBLoginResponse>(this.OnLoginComplete)
		});
	}

	private void LoginToApple()
	{
		GGAppleSignIn instance = BehaviourSingletonInit<GGAppleSignIn>.instance;
		if (!instance.isAvailable)
		{
			UnityEngine.Debug.Log("APPLE NOT AVAILABLE");
			return;
		}
		instance.SignIn(new Action<IAppleSignInProvider.SignInResponse>(this.OnAppleLoginComplete));
	}

	private void OnAppleLoginComplete(IAppleSignInProvider.SignInResponse response)
	{
		UnityEngine.Debug.LogFormat("Cancelled: {0}, UserId: {1}, Error: {2}", new object[]
		{
			response.cancelled,
			response.user_id,
			response.error
		});
		bool flag = GGUtil.HasText(response.user_id);
		if (response.isError)
		{
			UnityEngine.Debug.Log("ERROR: " + response.error);
		}
		if (!flag)
		{
			return;
		}
		this.LoginWithAppleId(response.user_id);
	}

	private void LoginWithAppleId(string userId)
	{
		NavigationManager.instance.GetObject<SyncGameScreen>().LoginToApple(userId);
	}

	private void OnLoginComplete(FBLoginResponse response)
	{
		if (ConfigBase.instance.debug)
		{
			UnityEngine.Debug.Log("Login Complete " + response.user_id);
		}
		bool flag = GGUtil.HasText(response.user_id);
		if (response.isError)
		{
			UnityEngine.Debug.Log("FACEBOK ERROR: " + response.error);
		}
		if (!flag)
		{
			return;
		}
		this.LoginWithFacebookId(response.user_id);
	}

	private void LoginWithFacebookId(string facebookUserId)
	{
		NavigationManager.instance.GetObject<SyncGameScreen>().LoginToFacebook(facebookUserId);
	}

	private sealed class _003C_003Ec__DisplayClass0_0
	{
		internal void _003CButtonCallback_OnButtonPress_003Eb__0(LoginProviderDialog.LoginResponse response)
		{
			this.nav.Pop(true);
			if (response.isCancelled)
			{
				return;
			}
			if (response.loginProvider == LoginProvider.FacebookLogin)
			{
				this._003C_003E4__this.LoginToFacebook();
				return;
			}
			if (response.loginProvider == LoginProvider.AppleLogin)
			{
				this._003C_003E4__this.LoginToApple();
			}
		}

		public NavigationManager nav;

		public LoginToFBButton _003C_003E4__this;
	}
}
