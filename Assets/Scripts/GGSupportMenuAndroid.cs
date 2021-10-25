using System;
using UnityEngine;

public class GGSupportMenuAndroid : GGSupportMenu
{
	public GGSupportMenuAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.giraffegames.unityutil.SupportMenu"))
		{
			this.javaInstance = androidJavaClass.CallStatic<AndroidJavaObject>("instance", Array.Empty<object>());
		}
	}

	public override void showRateApp(string rateProvider)
	{
		GGDebug.DebugLog("show rate");
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this.javaInstance.Call("showRateApp", new object[]
		{
			rateProvider
		});
	}

	public override bool isNetworkConnected()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return base.isNetworkConnected();
		}
		bool result;
		try
		{
			result = this.javaInstance.Call<bool>("isNetworkConnected", Array.Empty<object>());
		}
		catch
		{
			UnityEngine.Debug.Log("PROBLEM WITH LOADING IS NETWORK CONNECTED");
			result = base.isNetworkConnected();
		}
		return result;
	}

	private AndroidJavaObject javaInstance;
}
