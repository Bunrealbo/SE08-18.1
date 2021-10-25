using System;
using System.Collections.Generic;
using UnityEngine;

public class GGInAppPurchase : MonoBehaviour
{
	public event GGInAppPurchase.PurchaseCompleteDelegate onPurchaseComplete;

	public event GGInAppPurchase.SetupCompleteDelegate onSetupComplete;

	public event GGInAppPurchase.SetupCompleteDelegate onQueryInventoryComplete;

	public static GGInAppPurchase instance
	{
		get
		{
			if (GGInAppPurchase.instance_ == null)
			{
				GGInAppPurchase.instanceGameObject_ = GameObject.Find("GGInAppPurchase");
				if (GGInAppPurchase.instanceGameObject_ == null)
				{
					GGInAppPurchase.instanceGameObject_ = new GameObject("GGInAppPurchase");
				}
				GGInAppPurchase.instance_ = GGInAppPurchase.instanceGameObject_.GetComponent<GGInAppPurchase>();
				if (GGInAppPurchase.instance_ == null)
				{
					if (ConfigBase.instance.inAppProvider == ConfigBase.InAppProvider.AmazonInApp)
					{
						GGInAppPurchase.instance_ = GGInAppPurchase.instanceGameObject_.AddComponent<GGInAppPurchaseAmazon>();
					}
					else
					{
						GGInAppPurchase.instance_ = GGInAppPurchase.instanceGameObject_.AddComponent<GGInAppPurchaseAndroid>();
					}
					GGInAppPurchase.instance_.Init();
				}
				UnityEngine.Object.DontDestroyOnLoad(GGInAppPurchase.instanceGameObject_);
			}
			return GGInAppPurchase.instance_;
		}
	}

	public virtual void ValidatePurchase(string productId, GGInAppPurchase.OnValidateDelegate onComplete)
	{
		if (onComplete != null)
		{
			onComplete(productId, true, null);
		}
	}

	public virtual void updateProductList(List<string> consumableSkuList, List<string> nonConsumableSkuList)
	{
		string csvConsumableSkuList = GGFormat.Implode(consumableSkuList, ",");
		string csvNonConsumableSkuList = GGFormat.Implode(nonConsumableSkuList, ",");
		this.updateProductList(csvConsumableSkuList, csvNonConsumableSkuList);
	}

	public virtual void updateProductList(string csvConsumableSkuList, string csvNonConsumableSkuList)
	{
	}

	public void setupFinished(string success)
	{
		bool success2 = success.ToLower().Equals("success");
		if (this.onSetupComplete != null)
		{
			this.onSetupComplete(success2);
		}
	}

	public void purchaseCompleteJSON(string json)
	{
		GGInAppPurchase.PurchaseJSON purchaseJSON = JsonUtility.FromJson<GGInAppPurchase.PurchaseJSON>(json);
		if (purchaseJSON == null)
		{
			UnityEngine.Debug.Log("NOT KNOWN PURCHASE");
			return;
		}
		GGInAppPurchase.PurchaseResponse purchaseResponse = new GGInAppPurchase.PurchaseResponse(purchaseJSON.sku, GGInAppPurchase.PurchaseResponseCode.Success);
		purchaseResponse.purchaseToken = purchaseJSON.purchaseToken;
		if (this.onPurchaseComplete != null)
		{
			this.onPurchaseComplete(purchaseResponse);
		}
	}

	public void purchaseComplete(string productId)
	{
		if (this.onPurchaseComplete != null)
		{
			this.onPurchaseComplete(new GGInAppPurchase.PurchaseResponse(productId, GGInAppPurchase.PurchaseResponseCode.Success));
		}
	}

	public void queryInventoryFinished(string success)
	{
		bool success2 = success.ToLower().Equals("success");
		UnityEngine.Debug.Log("Query inventory " + success2.ToString());
		if (this.onQueryInventoryComplete != null)
		{
			this.onQueryInventoryComplete(success2);
		}
	}

	public void purchaseAlreadyOwned(string productId)
	{
		UnityEngine.Debug.Log("purchaseAlreadyOwned");
		if (this.onPurchaseComplete != null)
		{
			this.onPurchaseComplete(new GGInAppPurchase.PurchaseResponse(productId, GGInAppPurchase.PurchaseResponseCode.AlreadyOwned));
		}
	}

	public void purchaseCantVerifySignature(string productId)
	{
		UnityEngine.Debug.Log("purchaseCantVerifySignature");
		if (this.onPurchaseComplete != null)
		{
			this.onPurchaseComplete(new GGInAppPurchase.PurchaseResponse(productId, GGInAppPurchase.PurchaseResponseCode.CantVerifySignature));
		}
	}

	public void purchaseSignatureNotAccepted(string productId)
	{
		UnityEngine.Debug.Log("purchaseSignatureNotAccepted");
		if (this.onPurchaseComplete != null)
		{
			this.onPurchaseComplete(new GGInAppPurchase.PurchaseResponse(productId, GGInAppPurchase.PurchaseResponseCode.SignatureNotAccepted));
		}
	}

	public void purchaseConsumeFailed(string token)
	{
		UnityEngine.Debug.Log("purchaseConsumeFailed " + token);
	}

	public void purchaseConsumeSuccess(string token)
	{
		UnityEngine.Debug.Log("purchaseConsumeSuccess " + token);
	}

	public void purchaseUnknownError(string productId)
	{
		UnityEngine.Debug.Log("purchaseUnknownError");
		if (this.onPurchaseComplete != null)
		{
			this.onPurchaseComplete(new GGInAppPurchase.PurchaseResponse(productId, GGInAppPurchase.PurchaseResponseCode.UnknownError));
		}
	}

	public void purchaseFailed(string productId)
	{
		UnityEngine.Debug.Log("purchaseFailed");
		if (this.onPurchaseComplete != null)
		{
			this.onPurchaseComplete(new GGInAppPurchase.PurchaseResponse(productId, GGInAppPurchase.PurchaseResponseCode.Failed));
		}
	}

	public void purchaseCanceled(string productId)
	{
		UnityEngine.Debug.Log("purchaseCanceled");
		if (this.onPurchaseComplete != null)
		{
			this.onPurchaseComplete(new GGInAppPurchase.PurchaseResponse(productId, GGInAppPurchase.PurchaseResponseCode.Canceled));
		}
	}

	protected virtual void Init()
	{
	}

	public virtual void start(string[] productIds, string[] nonConsumableProductIds, string publicKey)
	{
	}

	public virtual void buy(string productId)
	{
	}

	public virtual void consumePurchase(string purchaseToken)
	{
	}

	public virtual void restorePurchases()
	{
	}

	public virtual string GetFormatedPrice(string productId)
	{
		return "- Buy -";
	}

	public virtual void QueryInventory()
	{
	}

	public virtual bool IsInventoryAvailable()
	{
		return false;
	}

	public virtual string GetPriceCurrencyCode(string productId)
	{
		return "";
	}

	public virtual string GetPriceAmountMicros(string productId)
	{
		return "";
	}

	private static GGInAppPurchase instance_;

	private static GameObject instanceGameObject_;

	public enum PurchaseResponseCode
	{
		AlreadyOwned,
		CantVerifySignature,
		SignatureNotAccepted,
		ConsumeFailed,
		UnknownError,
		Failed,
		Canceled,
		Success
	}

	public class PurchaseResponse
	{
		public bool success
		{
			get
			{
				return this.responseCode == GGInAppPurchase.PurchaseResponseCode.Success;
			}
		}

		public PurchaseResponse(string productId, GGInAppPurchase.PurchaseResponseCode responseCode)
		{
			this.productId = productId;
			this.responseCode = responseCode;
		}

		public string productId;

		public string purchaseToken;

		public GGInAppPurchase.PurchaseResponseCode responseCode;
	}

	public delegate void PurchaseCompleteDelegate(GGInAppPurchase.PurchaseResponse response);

	public delegate void SetupCompleteDelegate(bool success);

	public delegate void OnValidateDelegate(string productId, bool isValid, object data);

	[Serializable]
	public class PurchaseJSON
	{
		public string sku;

		public string purchaseToken;
	}
}
