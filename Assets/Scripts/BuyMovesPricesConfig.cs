using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class BuyMovesPricesConfig : ScriptableObjectSingleton<BuyMovesPricesConfig>
{
	public BuyMovesPricesConfig.OfferConfig GetOffer(int index)
	{
		index = Mathf.Clamp(index, 0, this.offers.Count - 1);
		return this.offers[index];
	}

	public BuyMovesPricesConfig.OfferConfig GetOffer(GameScreen.StageState state)
	{
		List<GameScreen.GameProgress> gameProgressList = state.gameProgressList;
		int num = 0;
		for (int i = 0; i < gameProgressList.Count; i++)
		{
			Match3Game game = gameProgressList[i].game;
			num += game.timesBoughtMoves;
		}
		return this.GetOffer(num);
	}

	[SerializeField]
	private List<BuyMovesPricesConfig.OfferConfig> offers = new List<BuyMovesPricesConfig.OfferConfig>();

	[Serializable]
	public class OfferConfig
	{
		public int movesCount;

		public SingleCurrencyPrice price;

		public List<BuyMovesPricesConfig.OfferConfig.PowerupDefinition> powerups = new List<BuyMovesPricesConfig.OfferConfig.PowerupDefinition>();

		[Serializable]
		public class PowerupDefinition
		{
			public ChipType powerupType;

			public int count = 1;
		}
	}
}
