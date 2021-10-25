using System;
using System.Net.NetworkInformation;

public class GGSupportMenu
{
	public static GGSupportMenu instance
	{
		get
		{
			if (GGSupportMenu._instance == null)
			{
				GGSupportMenu._instance = new GGSupportMenuAndroid();
			}
			return GGSupportMenu._instance;
		}
	}

	public virtual void showRateApp(string rateProvider)
	{
		GGDebug.DebugLog("show rate");
	}

	public virtual bool isNetworkConnected()
	{
		bool result;
		try
		{
			result = NetworkInterface.GetIsNetworkAvailable();
		}
		catch
		{
			result = false;
		}
		return result;
	}

	private static GGSupportMenu _instance;
}
