using System;
using System.Collections.Generic;
using UnityEngine;

public class LivesPriceConfig : ScriptableObjectSingleton<LivesPriceConfig>
{
	public LivesPriceConfig.PriceConfig GetPriceForLevelOrDefault(int levelIndex)
	{
		for (int i = 0; i < this.priceConfigs.Count; i++)
		{
			LivesPriceConfig.PriceConfig priceConfig = this.priceConfigs[i];
			if (priceConfig.levelIndex == levelIndex)
			{
				return priceConfig;
			}
		}
		return this.defaultConfig;
	}

	[SerializeField]
	private List<LivesPriceConfig.PriceConfig> priceConfigs = new List<LivesPriceConfig.PriceConfig>();

	[SerializeField]
	private LivesPriceConfig.PriceConfig defaultConfig = new LivesPriceConfig.PriceConfig();

	[Serializable]
	public class PriceConfig
	{
		public SingleCurrencyPrice GetPriceForLives(int lives)
		{
			return new SingleCurrencyPrice
			{
				cost = lives * this.pricePerLife.cost,
				currency = this.pricePerLife.currency
			};
		}

		public SingleCurrencyPrice pricePerLife;

		public int levelIndex;
	}
}
