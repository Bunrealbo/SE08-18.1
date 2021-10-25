using System;
using System.Collections.Generic;
using GGMatch3;

public class OffersDB : ScriptableObjectSingleton<OffersDB>
{
	public OffersDB.ProductDefinition GetProduct(string productId)
	{
		for (int i = 0; i < this.products.Count; i++)
		{
			OffersDB.ProductDefinition productDefinition = this.products[i];
			if (productDefinition.productID == productId)
			{
				return productDefinition;
			}
		}
		return null;
	}

	public string base64EncodedPublicKey;

	public List<OffersDB.ProductDefinition> products = new List<OffersDB.ProductDefinition>();

	[Serializable]
	public class OfferConfig
	{
		public bool useBoosterType;

		public BoosterType boosterType;

		public int count;

		public bool usePrice;

		public SingleCurrencyPrice price = new SingleCurrencyPrice();
	}

	[Serializable]
	public class OfferDefinition
	{
		public bool isNamedOffer
		{
			get
			{
				return !string.IsNullOrEmpty(this.name);
			}
		}

		public string name;

		public List<OffersDB.OfferConfig> config = new List<OffersDB.OfferConfig>();
	}

	[Serializable]
	public class ProductDefinition
	{
		public bool isConsumable
		{
			get
			{
				return this.productType == OffersDB.ProductDefinition.ProductType.Consumable;
			}
		}

		public void ConsumeProduct()
		{
			List<OffersDB.OfferConfig> config = this.offer.config;
			for (int i = 0; i < config.Count; i++)
			{
				OffersDB.OfferConfig offerConfig = config[i];
				if (offerConfig.usePrice)
				{
					GGPlayerSettings.instance.walletManager.AddCurrency(offerConfig.price.currency, offerConfig.price.cost);
				}
				else if (offerConfig.useBoosterType)
				{
					PlayerInventory.instance.Add(offerConfig.boosterType, offerConfig.count);
				}
			}
		}

		public string editorName;

		public OffersDB.OfferDefinition offer = new OffersDB.OfferDefinition();

		public string productID;

		public OffersDB.ProductDefinition.ProductType productType;

		public string mocupPrice;

		public bool active;

		public enum ProductType
		{
			Consumable,
			Permanent
		}
	}
}
