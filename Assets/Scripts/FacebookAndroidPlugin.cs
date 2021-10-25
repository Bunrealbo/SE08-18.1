using System;
using UnityEngine;

public class FacebookAndroidPlugin : IFacebookProvider
{
	public FacebookAndroidPlugin()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.giraffegames.unityutil.GGFacebook"))
		{
			this.javaInstance = androidJavaClass.CallStatic<AndroidJavaObject>("instance", Array.Empty<object>());
		}
	}

	public override void Login(FBLoginParams loginParams)
	{
		FacebookAndroidPlugin._003C_003Ec__DisplayClass2_0 _003C_003Ec__DisplayClass2_ = new FacebookAndroidPlugin._003C_003Ec__DisplayClass2_0();
		_003C_003Ec__DisplayClass2_.loginParams = loginParams;
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		string text = BehaviourSingletonInit<PluginCallbackManager>.instance.RegisterCallback(new PluginCallbackManager.CallbackDelegate(_003C_003Ec__DisplayClass2_._003CLogin_003Eb__0));
		this.javaInstance.Call("login", new object[]
		{
			_003C_003Ec__DisplayClass2_.loginParams.scope,
			text,
			_003C_003Ec__DisplayClass2_.loginParams.isPublishPermissionLogin
		});
	}

	public override bool IsInitialized()
	{
		return Application.platform != RuntimePlatform.Android || this.javaInstance.Call<bool>("isInitialized", Array.Empty<object>());
	}

	private AndroidJavaObject javaInstance;

	private sealed class _003C_003Ec__DisplayClass2_0
	{
		internal void _003CLogin_003Eb__0(PluginCallbackManager.Response response)
		{
			FBLoginResponse obj = JsonUtility.FromJson<FBLoginResponse>(response.jsonResponse);
			Action<FBLoginResponse> onComplete = this.loginParams.onComplete;
			if (onComplete == null)
			{
				return;
			}
			onComplete(obj);
		}

		public FBLoginParams loginParams;
	}
}
