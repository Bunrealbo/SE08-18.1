using System;
using UnityEngine;

public class GGInAppPurchaseAmazon : GGInAppPurchase
{
	protected override void Init()
	{
		if (Application.platform != this.platform)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.giraffegames.unityutil.GGAmazonInAppPurchase"))
		{
			this.javaInstance = androidJavaClass.CallStatic<AndroidJavaObject>("instance", Array.Empty<object>());
		}
	}

	public void startSetup(string base64EncodedPublicKey, string csvConsumableSkuList, bool enableDebugLogging)
	{
		if (Application.platform != this.platform)
		{
			return;
		}
		this.javaInstance.Call("startSetup", new object[]
		{
			base64EncodedPublicKey,
			csvConsumableSkuList,
			enableDebugLogging
		});
	}

	public bool isSetupFinished()
	{
		return Application.platform == this.platform && this.javaInstance.Call<bool>("isSetupFinished", Array.Empty<object>());
	}

	public bool isSetupStarted()
	{
		return Application.platform == this.platform && this.javaInstance.Call<bool>("isSetupStarted", Array.Empty<object>());
	}

	public void queryInventory()
	{
		if (Application.platform != this.platform)
		{
			return;
		}
		this.javaInstance.Call("queryInventory", Array.Empty<object>());
	}

	public void startPurchaseFlow(string sku)
	{
		if (Application.platform != this.platform)
		{
			return;
		}
		this.javaInstance.Call("startPurchaseFlow", new object[]
		{
			sku
		});
	}

	public override void start(string[] productIds, string[] nonConsumableProductIds, string publicKey)
	{
		if (this.isSetupStarted())
		{
			return;
		}
		this.startSetup(publicKey, GGFormat.Implode(productIds, ","), true);
	}

	public override void buy(string productId)
	{
		this.startPurchaseFlow(productId);
	}

	public override void restorePurchases()
	{
		if (this.isSetupFinished())
		{
			this.queryInventory();
		}
	}

	public override string GetFormatedPrice(string productId)
	{
		if (Application.platform != this.platform)
		{
			return base.GetFormatedPrice(productId);
		}
		return this.javaInstance.Call<string>("getFormatedPrice", new object[]
		{
			productId
		});
	}

	public override void QueryInventory()
	{
		if (Application.platform != this.platform)
		{
			base.QueryInventory();
			return;
		}
		this.queryInventory();
	}

	public override bool IsInventoryAvailable()
	{
		if (Application.platform != this.platform)
		{
			return base.IsInventoryAvailable();
		}
		return this.javaInstance.Call<bool>("isInventoryAvailable", Array.Empty<object>());
	}

	private AndroidJavaObject javaInstance;

	private RuntimePlatform platform = RuntimePlatform.Android;
}
