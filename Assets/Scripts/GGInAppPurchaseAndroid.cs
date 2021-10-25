using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GGInAppPurchaseAndroid : GGInAppPurchase
{
	protected override void Init()
	{
		if (Application.platform != this.platform)
		{
			return;
		}
		
	}

	public void startSetup(string base64EncodedPublicKey, string csvConsumableSkuList, string csvNonConsumableSkuList, bool enableDebugLogging)
	{
		if (Application.platform != this.platform)
		{
			return;
		}
		string text = "";
		string text2 = "";
		if (ConfigBase.instance.verifyPlayInApp)
		{
			GGDebug.Log("verify");
			text = GGServerConstants.instance.appName;
			text2 = GGServerConstants.instance.verifyInAppPurchasesUrl;
		}
		//this.javaInstance.Call("startSetup2", new object[]
		//{
		//	base64EncodedPublicKey,
		//	text,
		//	text2,
		//	csvConsumableSkuList,
		//	csvNonConsumableSkuList,
		//	enableDebugLogging
		//});
	}

	public override void updateProductList(string csvConsumableSkuList, string csvNonConsumableSkuList)
	{
		if (Application.platform != this.platform)
		{
			UnityEngine.Debug.Log("CONSUMABLE: " + csvConsumableSkuList);
			UnityEngine.Debug.Log("NON CONSUMABLE: " + csvNonConsumableSkuList);
			return;
		}
		UnityEngine.Debug.Log("CONSUMABLE: " + csvConsumableSkuList);
		UnityEngine.Debug.Log("NON CONSUMABLE: " + csvNonConsumableSkuList);
		//this.javaInstance.Call("updateProductList", new object[]
		//{
		//	csvConsumableSkuList,
		//	csvNonConsumableSkuList
		//});
	}

	public bool isSetupFinished()
	{
        //return Application.platform == this.platform && this.javaInstance.Call<bool>("isSetupFinished", Array.Empty<object>());
        return false;
	}

	public bool isSetupStarted()
	{
        //return Application.platform == this.platform && this.javaInstance.Call<bool>("isSetupStarted", Array.Empty<object>());
        return false;
    }

	public void queryInventory()
	{
		if (Application.platform != this.platform)
		{
			return;
		}
		//this.javaInstance.Call("queryInventory", Array.Empty<object>());
	}

	public void startPurchaseFlow(string sku)
	{
		if (Application.platform != this.platform)
		{
			return;
		}
		//this.javaInstance.Call("startPurchaseFlow", new object[]
		//{
		//	sku
		//});
	}

	public override void start(string[] productIds, string[] nonConsumableProductIds, string publicKey)
	{
		if (this.isSetupStarted())
		{
			return;
		}
		this.startSetup(publicKey, GGFormat.Implode(productIds, ","), GGFormat.Implode(nonConsumableProductIds, ","), true);
	}

	public override void consumePurchase(string purchaseToken)
	{
		if (Application.platform != this.platform)
		{
			UnityEngine.Debug.Log("CONSUME PURCHASE " + purchaseToken);
			return;
		}
		//this.javaInstance.Call("consumePurchaseWithToken", new object[]
		//{
		//	purchaseToken
		//});
	}

	public override void buy(string productId)
	{
		if (!this.isSetupFinished() && !Application.isEditor)
		{
			base.purchaseCanceled(productId);
			return;
		}
		this.startPurchaseFlow(productId);
	}

	public override void restorePurchases()
	{
		if (!this.isSetupFinished())
		{
			return;
		}
		this.queryInventory();
	}

	public override string GetFormatedPrice(string productId)
	{
		if (Application.platform != this.platform)
		{
			return base.GetFormatedPrice(productId);
		}
        return string.Empty;
		//return this.javaInstance.Call<string>("getFormatedPrice", new object[]
		//{
		//	productId
		//});
	}

	public override string GetPriceCurrencyCode(string productId)
	{
		if (Application.platform != this.platform)
		{
			return base.GetPriceCurrencyCode(productId);
		}
        return string.Empty;
        //return this.javaInstance.Call<string>("getPriceCurrencyCode", new object[]
        //{
        //	productId
        //});
    }

	public override string GetPriceAmountMicros(string productId)
	{
		if (Application.platform != this.platform)
		{
			return base.GetPriceAmountMicros(productId);
		}
        return string.Empty;
        //return this.javaInstance.Call<string>("getPriceAmountMicros", new object[]
        //{
        //	productId
        //});
    }

	public string getPurchaseOriginalJSON(string productId)
	{
		if (Application.platform != this.platform)
		{
			return "";
		}
        return string.Empty;
        //return this.javaInstance.Call<string>("getPurchaseOriginalJSON", new object[]
        //{
        //	productId
        //});
    }

	public string getPurchaseSignature(string productId)
	{
		if (Application.platform != this.platform)
		{
			return "";
		}
        return string.Empty;
        //return this.javaInstance.Call<string>("getPurchaseSignature", new object[]
        //{
        //	productId
        //});
    }

	public override void ValidatePurchase(string productId, GGInAppPurchase.OnValidateDelegate onComplete)
	{
		GGInAppPurchaseAndroid.PurchaseData purchaseData = new GGInAppPurchaseAndroid.PurchaseData();
		purchaseData.originalJson = this.getPurchaseOriginalJSON(productId);
		purchaseData.signature = this.getPurchaseSignature(productId);
		purchaseData.productId = productId;
		try
		{
			Hashtable hashtable = NGUIJson.jsonDecode(purchaseData.originalJson) as Hashtable;
			if (hashtable != null && hashtable.ContainsKey("purchaseToken"))
			{
				purchaseData.purchaseToken = (hashtable["purchaseToken"] as string);
			}
		}
		catch
		{
		}
		base.StartCoroutine(this.DoVerifyInAppData(purchaseData, onComplete));
	}

	protected IEnumerator DoVerifyInAppData(GGInAppPurchaseAndroid.PurchaseData data, GGInAppPurchase.OnValidateDelegate onComplete)
	{
		return new GGInAppPurchaseAndroid._003CDoVerifyInAppData_003Ed__20(0)
		{
			data = data,
			onComplete = onComplete
		};
	}

	public override void QueryInventory()
	{
		if (Application.platform != this.platform)
		{
			base.QueryInventory();
			return;
		}
		if (this.isSetupFinished())
		{
			this.queryInventory();
		}
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

	public class PurchaseData
	{
		public string originalJson;

		public string signature;

		public string productId;

		public string purchaseToken;

		public bool isValid;
	}

	private sealed class _003CDoVerifyInAppData_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoVerifyInAppData_003Ed__20(int _003C_003E1__state)
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
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(this._003Cw_003E5__2.error))
				{
					UnityEngine.Debug.Log("Error with request " + this._003Cw_003E5__2.error);
				}
				else if (this._003Cw_003E5__2.text == "true")
				{
					this.data.isValid = true;
				}
				if (this.onComplete != null)
				{
					this.onComplete(this.data.productId, this.data.isValid, this.data);
				}
				return false;
			}
			else
			{
				this._003C_003E1__state = -1;
				string appName = ConfigBase.instance.appName;
				string originalJson = this.data.originalJson;
				string signature = this.data.signature;
				if (string.IsNullOrEmpty(originalJson) || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(appName))
				{
					if (this.onComplete != null)
					{
						this.onComplete(this.data.productId, this.data.isValid, this.data);
					}
					return false;
				}
				WWWForm wwwform = new WWWForm();
				wwwform.AddField("app", appName);
				wwwform.AddField("data", originalJson);
				wwwform.AddField("signature", signature);
				this._003Cw_003E5__2 = new WWW(GGServerConstants.instance.verifyInAppPurchasesUrl, wwwform);
				this._003C_003E2__current = this._003Cw_003E5__2;
				this._003C_003E1__state = 1;
				return true;
			}
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

		public GGInAppPurchaseAndroid.PurchaseData data;

		public GGInAppPurchase.OnValidateDelegate onComplete;

		private WWW _003Cw_003E5__2;
	}
}
