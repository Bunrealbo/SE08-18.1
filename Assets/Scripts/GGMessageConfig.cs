using System;
using System.Collections.Generic;
using UnityEngine;

public class GGMessageConfig : ScriptableObject
{
	public static GGMessageConfig instance
	{
		get
		{
			if (GGMessageConfig.instance_ == null)
			{
				GGMessageConfig.instance_ = (Resources.Load("GGServerAssets/GGMessageConfig", typeof(GGMessageConfig)) as GGMessageConfig);
			}
			return GGMessageConfig.instance_;
		}
	}

	public GGMessageConfig.FacebookGiftObject GetGiftForType(GGMessageConfig.FbObjectType type)
	{
		foreach (GGMessageConfig.FacebookGiftObject facebookGiftObject in this.giftDefinitions)
		{
			if (facebookGiftObject.objectType == type)
			{
				return facebookGiftObject;
			}
		}
		return null;
	}

	private static GGMessageConfig instance_;

	public List<GGMessageConfig.FacebookGiftObject> giftDefinitions = new List<GGMessageConfig.FacebookGiftObject>();

	[Serializable]
	public enum FbObjectType
	{
		EnergyGift,
		CoinGift,
		DollarGift,
		CinemaTrip,
		None
	}

	[Serializable]
	public class FacebookGiftObject
	{
		public GGMessageConfig.FbObjectType objectType;

		public string objectId;

		public string message;

		public float popularityBoost;

		public float moodBoost;

		public int coinCost;
	}
}
