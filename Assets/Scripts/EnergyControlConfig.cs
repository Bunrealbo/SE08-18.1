using System;
using System.Collections.Generic;
using UnityEngine;

public class EnergyControlConfig : ScriptableObject
{
	public static EnergyControlConfig instance
	{
		get
		{
			if (EnergyControlConfig.instance_ == null)
			{
				EnergyControlConfig.instance_ = (Resources.Load("EnergyControlConfig", typeof(EnergyControlConfig)) as EnergyControlConfig);
			}
			return EnergyControlConfig.instance_;
		}
	}

	public float CoinsToEnergy(int coins)
	{
		return (float)(coins * this.energyPointPerCoin);
	}

	public int energyPointPerCoin
	{
		get
		{
			float num = this.maxEnergy / (float)this.totalCoin;
			if ((float)((int)(this.maxEnergy / (float)this.totalCoin)) != num)
			{
				UnityEngine.Debug.LogWarning("maxEnergy can not be devided by energyCoin exactly!");
			}
			return (int)(this.maxEnergy / (float)this.totalCoin);
		}
	}

	public float GetEnergyForTimespan(TimeSpan timeSpan)
	{
		return (float)timeSpan.TotalSeconds / (float)this.secondsToRefreshPoint;
	}

	public TimeSpan TimeToGainEnergy(float energyGain)
	{
		return new TimeSpan(0, 0, (int)energyGain * this.secondsToRefreshPoint);
	}

	public string energyNotificationsName = "Energy";

	private static EnergyControlConfig instance_;

	public float maxEnergy = 50f;

	public int secondsToRefreshPoint = 30;

	public int totalCoin = 5;

	public SingleCurrencyPrice price;

	public SingleCurrencyPrice freePlay24hPrice;

	public List<EnergyControlConfig.EnergyPackBundle> energyPackBundles = new List<EnergyControlConfig.EnergyPackBundle>();

	public enum TimeLimitation
	{
		NotLimited,
		InstallTime,
		ExactTime
	}

	public enum CardPrefabType
	{
		EnergyDrinkPrefab,
		OfferPrefab,
		InAppPurchasePrefab
	}

	[Serializable]
	public class EnergyPackConfig
	{
		public string packID;

		public SingleCurrencyPrice price;

		public int drinkCount;

		public string nameSuffix = "Drink";

		public string packStyle;

		public string packBckStyle;

		public string labelStyle;

		public int packWidth;

		public EnergyControlConfig.CardPrefabType cardPrefabType;

		public string cueName;

		public string cueStyle;

		public EnergyControlConfig.TimeLimitation timeLimitation;

		public float durationInDays;

		public string datetimeWhenFirstAvailable;

		public bool canBuyOneTime;

		public bool showDealSticker;

		public string dealStickerText;

		public string inAppName;

		public string centerText;
	}

	[Serializable]
	public class EnergyPackBundle
	{
		public string packName;

		public List<EnergyControlConfig.EnergyPackConfig> packs = new List<EnergyControlConfig.EnergyPackConfig>();
	}
}
