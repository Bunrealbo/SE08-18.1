using System;
using System.Collections.Generic;
using GGMatch3;
using ProtoModels;
using UnityEngine;

public class InAppBackend : BehaviourSingletonInit<InAppBackend>
{
	public void AddListener(InAppBackend.Listener listener)
	{
		if (this.listeners.Contains(listener))
		{
			return;
		}
		this.listeners.Add(listener);
	}

	public void RemoveListener(InAppBackend.Listener listener)
	{
		this.listeners.Remove(listener);
	}

	public void PurchaseItem(string productId)
	{
		InAppBackend.PurchaseEventArguments purchaseEventArguments = default(InAppBackend.PurchaseEventArguments);
		purchaseEventArguments.productId = productId;
		purchaseEventArguments.isSuccess = false;
		if (this.FindInAppForId(productId) == null)
		{
			this.CallListenersOnPurchase(purchaseEventArguments);
			return;
		}
		this.inApp.buy(productId);
	}

	private void CallListenersOnInitialized(InAppBackend.InitializeArguments initializeArguments)
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnInitialized(initializeArguments);
		}
	}

	private void CallListenersOnPurchase(InAppBackend.PurchaseEventArguments purchaseEventArguments)
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnPurchase(purchaseEventArguments);
		}
	}

	public override void Init()
	{
		this.InitializePurchasing();
	}

	private bool IsInitialized()
	{
		return this.inApp != null;
	}

	private void InitializePurchasing()
	{
		if (this.IsInitialized())
		{
			return;
		}
		this.inApp = GGInAppPurchase.instance;
		this.inApp.onSetupComplete += this.OnSetupComplete;
		this.inApp.onPurchaseComplete += this.OnProductPurchased;
		InAppBackend.PurchasesList purchasesList = new InAppBackend.PurchasesList();
		purchasesList.Add(ScriptableObjectSingleton<OffersDB>.instance.products);
		List<string> consumableProductIds = purchasesList.consumableProductIds;
		List<string> nonConsumableProductIds = purchasesList.nonConsumableProductIds;
		this.inApp.start(consumableProductIds.ToArray(), nonConsumableProductIds.ToArray(), ScriptableObjectSingleton<OffersDB>.instance.base64EncodedPublicKey);
	}

	public string LocalisedPriceString(string productId)
	{
		if (this.inApp == null)
		{
			return "Buy";
		}
		return this.inApp.GetFormatedPrice(productId);
	}

	protected void OnSetupComplete(bool success)
	{
		if (success)
		{
			this.inApp.restorePurchases();
		}
		this.CallListenersOnInitialized(new InAppBackend.InitializeArguments
		{
			isSuccess = success
		});
	}

	public void OnProductPurchased(GGInAppPurchase.PurchaseResponse response)
	{
		string productId = response.productId;
		bool success = response.success;
		InAppBackend.PurchaseEventArguments purchaseEventArguments = default(InAppBackend.PurchaseEventArguments);
		purchaseEventArguments.productId = productId;
		if (!success)
		{
			this.CallListenersOnPurchase(purchaseEventArguments);
			return;
		}
		OffersDB.ProductDefinition product = ScriptableObjectSingleton<OffersDB>.instance.GetProduct(productId);
		if (product == null)
		{
			this.CallListenersOnPurchase(purchaseEventArguments);
			return;
		}
		if (GGPlayerSettings.instance.IsPurchaseConsumed(response.purchaseToken))
		{
			UnityEngine.Debug.Log("PURCHASE ALREADY CONSUMED");
			this.inApp.consumePurchase(response.purchaseToken);
			this.CallListenersOnPurchase(purchaseEventArguments);
			return;
		}
		product.ConsumeProduct();
		this.inApp.consumePurchase(response.purchaseToken);
		purchaseEventArguments.isSuccess = true;
		InAppPurchaseDAO inAppPurchaseDAO = new InAppPurchaseDAO();
		inAppPurchaseDAO.productId = productId;
		inAppPurchaseDAO.purchasedSomething = true;
		inAppPurchaseDAO.receipt = response.purchaseToken;
		inAppPurchaseDAO.timeUtc = DateTime.UtcNow.Ticks;
		GGPlayerSettings.instance.AddPurchase(inAppPurchaseDAO);
		new Analytics.IAPEvent
		{
			purchaseArguments = purchaseEventArguments,
			inAppObject = product,
			purchaseToken = response.purchaseToken
		}.Send();
		this.CallListenersOnPurchase(purchaseEventArguments);
		NavigationManager instance = NavigationManager.instance;
		InAppPurchaseConfirmScreen @object = instance.GetObject<InAppPurchaseConfirmScreen>();
		if (instance.CurrentScreen.gameObject != @object.gameObject)
		{
			@object.Show(new InAppPurchaseConfirmScreen.PurchaseArguments
			{
				isProductBought = true,
				productToBuy = product
			});
			return;
		}
		@object.OnPurchase(purchaseEventArguments);
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.inApp.restorePurchases();
		}
	}

	public OffersDB.ProductDefinition FindInAppForId(string productId)
	{
		return ScriptableObjectSingleton<OffersDB>.instance.GetProduct(productId);
	}

	private GGInAppPurchase inApp;

	private List<InAppBackend.Listener> listeners = new List<InAppBackend.Listener>();

	public class PurchasesList
	{
		public void Add(List<OffersDB.ProductDefinition> objects)
		{
			if (objects == null)
			{
				return;
			}
			for (int i = 0; i < objects.Count; i++)
			{
				OffersDB.ProductDefinition inAppObject = objects[i];
				this.Add(inAppObject);
			}
		}

		public void Add(OffersDB.ProductDefinition inAppObject)
		{
			if (inAppObject == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(inAppObject.productID))
			{
				return;
			}
			this.objectsThatCanBePurchased.Add(inAppObject.productID, inAppObject);
		}

		public List<string> consumableProductIds
		{
			get
			{
				List<string> list = new List<string>();
				foreach (string text in this.objectsThatCanBePurchased.Keys)
				{
					OffersDB.ProductDefinition productDefinition = this.objectsThatCanBePurchased[text];
					if (!string.IsNullOrEmpty(text) && productDefinition.isConsumable)
					{
						list.Add(text);
					}
				}
				return list;
			}
		}

		public List<string> nonConsumableProductIds
		{
			get
			{
				List<string> list = new List<string>();
				foreach (string text in this.objectsThatCanBePurchased.Keys)
				{
					OffersDB.ProductDefinition productDefinition = this.objectsThatCanBePurchased[text];
					if (!string.IsNullOrEmpty(text) && !productDefinition.isConsumable)
					{
						list.Add(text);
					}
				}
				return list;
			}
		}

		protected Dictionary<string, OffersDB.ProductDefinition> objectsThatCanBePurchased = new Dictionary<string, OffersDB.ProductDefinition>();
	}

	public struct InitializeArguments
	{
		public bool isSuccess;
	}

	public struct PurchaseEventArguments
	{
		public bool isSuccess;

		public string productId;
	}

	public interface Listener
	{
		void OnInitialized(InAppBackend.InitializeArguments initializeArguments);

		void OnPurchase(InAppBackend.PurchaseEventArguments purchaseParams);
	}
}
