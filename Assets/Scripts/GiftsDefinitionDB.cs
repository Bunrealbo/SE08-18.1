using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class GiftsDefinitionDB : ScriptableObjectSingleton<GiftsDefinitionDB>
{
	public GiftsDefinitionDB.CombinedGifts GetCombinedGiftsTillStage(int stageIndex)
	{
		return this.GetCombinedGifts(this.GiftsTillStage(stageIndex));
	}

	public List<GiftsDefinitionDB.GiftDefinition> GiftsTillStage(int stageIndex)
	{
		this.giftsToStage.Clear();
		for (int i = 0; i < this.gifts.Count; i++)
		{
			GiftsDefinitionDB.GiftDefinition giftDefinition = this.gifts[i];
			if (giftDefinition.totalStagesPassedToGive <= stageIndex)
			{
				this.giftsToStage.Add(giftDefinition);
			}
		}
		return this.giftsToStage;
	}

	public GiftsDefinitionDB.CombinedGifts GetCombinedGifts(List<GiftsDefinitionDB.GiftDefinition> gifts)
	{
		GiftsDefinitionDB.CombinedGifts result = default(GiftsDefinitionDB.CombinedGifts);
		for (int i = 0; i < gifts.Count; i++)
		{
			GiftsDefinitionDB.GiftDefinition giftDefinition = gifts[i];
			for (int j = 0; j < giftDefinition.gifts.gifts.Count; j++)
			{
				GiftBoxScreen.Gift gift = giftDefinition.gifts.gifts[j];
				if (gift.giftType == GiftBoxScreen.GiftType.Booster && gift.boosterType == BoosterType.VerticalRocketBooster)
				{
					result.rocketCount += gift.amount;
				}
				if (gift.giftType == GiftBoxScreen.GiftType.Booster && gift.boosterType == BoosterType.BombBooster)
				{
					result.bombCount += gift.amount;
				}
				if (gift.giftType == GiftBoxScreen.GiftType.Booster && gift.boosterType == BoosterType.DiscoBooster)
				{
					result.discoCount += gift.amount;
				}
				if (gift.giftType == GiftBoxScreen.GiftType.Powerup && gift.powerupType == PowerupType.Hammer)
				{
					result.hammerCount += gift.amount;
				}
				if (gift.giftType == GiftBoxScreen.GiftType.Powerup && gift.powerupType == PowerupType.PowerHammer)
				{
					result.powerHammerCount += gift.amount;
				}
				if (gift.giftType == GiftBoxScreen.GiftType.Coins)
				{
					result.coinsCount += gift.amount;
				}
			}
		}
		return result;
	}

	public int lastStageWhenGivenGift
	{
		get
		{
			return GGPlayerSettings.instance.givenGifts.lastStageWhenGivenGift;
		}
		set
		{
			GGPlayerSettings.instance.givenGifts.lastStageWhenGivenGift = value;
			GGPlayerSettings.instance.Save();
		}
	}

	public GiftsDefinitionDB.GiftDefinition currentGift
	{
		get
		{
			int lastStageWhenGivenGift = this.lastStageWhenGivenGift;
			for (int i = 0; i < this.gifts.Count; i++)
			{
				GiftsDefinitionDB.GiftDefinition giftDefinition = this.gifts[i];
				if (giftDefinition.totalStagesPassedToGive > lastStageWhenGivenGift)
				{
					return giftDefinition;
				}
			}
			return null;
		}
	}

	protected void ClaimGift(int stagePassedToGive)
	{
		this.lastStageWhenGivenGift = Mathf.Max(this.lastStageWhenGivenGift, stagePassedToGive);
	}

	protected override void UpdateData()
	{
		base.UpdateData();
		for (int i = 0; i < this.gifts.Count; i++)
		{
			GiftsDefinitionDB.GiftDefinition giftDefinition = this.gifts[i];
			GiftsDefinitionDB.GiftDefinition previousGift = null;
			if (i > 0)
			{
				previousGift = this.gifts[i - 1];
			}
			giftDefinition.Init(this, previousGift);
		}
		this.dailyGifts.Init();
	}

	[SerializeField]
	private List<GiftsDefinitionDB.GiftDefinition> gifts = new List<GiftsDefinitionDB.GiftDefinition>();

	[SerializeField]
	public GiftsDefinitionDB.DailyGifts dailyGifts = new GiftsDefinitionDB.DailyGifts();

	private List<GiftsDefinitionDB.GiftDefinition> giftsToStage = new List<GiftsDefinitionDB.GiftDefinition>();

	[SerializeField]
	public GiftsDefinitionDB.BuildupBooster buildupBooster = new GiftsDefinitionDB.BuildupBooster();

	[Serializable]
	public class BuildupBooster
	{
		public int currentBoosterLevel
		{
			get
			{
				GiftsDefinitionDB.BuildupBooster.BoosterGift boosterGift = this.GetBoosterGift();
				if (boosterGift == null)
				{
					return 0;
				}
				return boosterGift.level;
			}
		}

		public GiftsDefinitionDB.BuildupBooster.BoosterGift GetBoosterGiftForLevel(int level)
		{
			if (level == 0)
			{
				return null;
			}
			if (this.boosters.Count == 0)
			{
				return null;
			}
			return this.boosters[Mathf.Clamp(level - 1, 0, this.boosters.Count - 1)];
		}

		public GiftsDefinitionDB.BuildupBooster.BoosterGift GetBoosterGift()
		{
			if (!this.isEnabled)
			{
				return null;
			}
			int num = Match3StagesDB.instance.PassedStagesInRow(this.minStageBeforeEnabled + 1);
			if (Match3StagesDB.instance.passedStages < this.minStageBeforeEnabled)
			{
				return null;
			}
			for (int i = this.boosters.Count - 1; i >= 0; i--)
			{
				GiftsDefinitionDB.BuildupBooster.BoosterGift boosterGift = this.boosters[i];
				if (num >= boosterGift.totalGamesWonInARow)
				{
					boosterGift.level = i + 1;
					return boosterGift;
				}
			}
			return null;
		}

		[SerializeField]
		private bool isEnabled;

		[SerializeField]
		private int minStageBeforeEnabled;

		[SerializeField]
		private List<GiftsDefinitionDB.BuildupBooster.BoosterGift> boosters = new List<GiftsDefinitionDB.BuildupBooster.BoosterGift>();

		[Serializable]
		public class BoosterGift
		{
			public int totalGamesWonInARow;

			[NonSerialized]
			public int level;

			public List<BoosterConfig> boosterConfig = new List<BoosterConfig>();
		}
	}

	[Serializable]
	public class GiftDefinition
	{
		public GiftsDefinitionDB.GiftDefinition.StagesPassedDescriptor stagesPassedDescriptor
		{
			get
			{
				return new GiftsDefinitionDB.GiftDefinition.StagesPassedDescriptor
				{
					currentStagesPassed = Mathf.Max(0, Match3StagesDB.instance.passedStages - this.previousStagePassedToGive),
					stagesNeededToPass = this.stagesPassedToGive
				};
			}
		}

		public void ClaimGifts()
		{
			this.db.ClaimGift(this.totalStagesPassedToGive);
		}

		public float progress
		{
			get
			{
				return Mathf.InverseLerp((float)this.previousStagePassedToGive, (float)this.totalStagesPassedToGive, (float)Match3StagesDB.instance.passedStages);
			}
		}

		public bool isAvailableToCollect
		{
			get
			{
				return this.progress >= 1f;
			}
		}

		public void Init(GiftsDefinitionDB db, GiftsDefinitionDB.GiftDefinition previousGift)
		{
			this.db = db;
			if (previousGift != null)
			{
				this.previousStagePassedToGive = previousGift.totalStagesPassedToGive;
			}
			else
			{
				this.previousStagePassedToGive = 0;
			}
			this.totalStagesPassedToGive = this.previousStagePassedToGive + this.stagesPassedToGive;
		}

		[SerializeField]
		private int stagesPassedToGive;

		public GiftBoxScreen.GiftsDefinition gifts = new GiftBoxScreen.GiftsDefinition();

		[NonSerialized]
		private int previousStagePassedToGive;

		[NonSerialized]
		public int totalStagesPassedToGive;

		[NonSerialized]
		private GiftsDefinitionDB db;

		public struct StagesPassedDescriptor
		{
			public int currentStagesPassed;

			public int stagesNeededToPass;
		}
	}

	public struct CombinedGifts
	{
		public int bombCount;

		public int discoCount;

		public int rocketCount;

		public int hammerCount;

		public int powerHammerCount;

		public int coinsCount;
	}

	[Serializable]
	public class DailyGifts
	{
		private int currentGiftIndex
		{
			get
			{
				return GGPlayerSettings.instance.Model.timesCollectedFreeCoins;
			}
			set
			{
				GGPlayerSettings instance = GGPlayerSettings.instance;
				instance.Model.timesCollectedFreeCoins = value;
				instance.Save();
			}
		}

		public GiftsDefinitionDB.DailyGifts.DailyGift currentDailyGift
		{
			get
			{
				if (this.gifts.Count == 0)
				{
					return null;
				}
				return this.gifts[Mathf.Clamp(this.currentGiftIndex, 0, this.gifts.Count - 1)];
			}
		}

		public bool IsSelected(int index)
		{
			int num = Mathf.Clamp(this.currentGiftIndex, 0, this.gifts.Count - 1);
			return index == num;
		}

		public void Init()
		{
			GGPlayerSettings instance = GGPlayerSettings.instance;
			if (instance.Model.timeWhenLastCollectedDailyCoins == 0L)
			{
				instance.Model.timeWhenLastCollectedDailyCoins = DateTime.Now.Ticks;
				instance.Save();
			}
			for (int i = 0; i < this.gifts.Count; i++)
			{
				this.gifts[i].index = i;
			}
		}

		public void OnClaimedDailyCoins()
		{
			GGPlayerSettings instance = GGPlayerSettings.instance;
			instance.Model.timeWhenLastCollectedDailyCoins = DateTime.Now.Ticks;
			this.currentGiftIndex++;
			instance.Save();
		}

		[SerializeField]
		private int hoursTillDailyCoinsAvailable = 18;

		[SerializeField]
		private List<GiftsDefinitionDB.DailyGifts.DailyGift> gifts = new List<GiftsDefinitionDB.DailyGifts.DailyGift>();

		[Serializable]
		public class DailyGift
		{
			[NonSerialized]
			public int index;

			[SerializeField]
			public GiftBoxScreen.GiftsDefinition gifts = new GiftBoxScreen.GiftsDefinition();
		}
	}
}
