using System;
using GGMatch3;
using ProtoModels;

public class PlayerInventory
{
	public static PlayerInventory instance
	{
		get
		{
			if (PlayerInventory.instance_ == null)
			{
				PlayerInventory.instance_ = new PlayerInventory();
			}
			return PlayerInventory.instance_;
		}
	}

	public OwnedItems owned
	{
		get
		{
			return this._003Cowned_003Ek__BackingField;
		}
		protected set
		{
			this._003Cowned_003Ek__BackingField = value;
		}
	}

	public PlayerInventory()
	{
		this.owned = new OwnedItems("playerInventory.bytes");
	}

	public void BuyItem(PlayerInventory.Item item, bool canStockpile)
	{
		this.owned.AddToOwned(item.ToString(), canStockpile);
	}

	private string PowerupId(PowerupType powerupType)
	{
		return "pwup_" + powerupType.ToString();
	}

	private string BoosterId(GGMatch3.BoosterType boosterType)
	{
		return "boost_" + boosterType.ToString();
	}

	public long UsedCount(PowerupType powerupType)
	{
		return this.owned.GetOrCreateUsedItemWithName(this.PowerupId(powerupType)).count;
	}

	public long SetUsedCount(PowerupType powerupType, long count)
	{
		UsedItemDAO orCreateUsedItemWithName = this.owned.GetOrCreateUsedItemWithName(this.PowerupId(powerupType));
		orCreateUsedItemWithName.count = count;
		this.owned.Save();
		return orCreateUsedItemWithName.count;
	}

	public long OwnedCount(PowerupType powerupType)
	{
		return this.owned.GetOrCreateItemWithName(this.PowerupId(powerupType)).count;
	}

	public void Add(PowerupType powerupType, int amount)
	{
		this.owned.GetOrCreateItemWithName(this.PowerupId(powerupType)).count += (long)amount;
		this.owned.Save();
	}

	public void BuyTimedItem(PlayerInventory.Item item, TimeSpan duration)
	{
		string name = item.ToString();
		if (this.owned.GetItemWithName(name) == null)
		{
			this.BuyItem(item, false);
		}
		OwnedItemDAO itemWithName = this.owned.GetItemWithName(name);
		if (itemWithName != null)
		{
			long ticks = DateTime.Now.Ticks;
			itemWithName.purchaseTime = ticks;
			itemWithName.lastCheckTime = ticks;
			itemWithName.totalDuration = duration.Ticks;
			itemWithName.timeLeft = itemWithName.totalDuration;
		}
		this.owned.Save();
	}

	public void SetOwned(PowerupType powerupType, long ownedNumber)
	{
		this.owned.GetOrCreateItemWithName(this.PowerupId(powerupType)).count = MathEx.Max(ownedNumber, 0L);
		this.owned.Save();
	}

	public long OwnedCount(GGMatch3.BoosterType boosterType)
	{
		return this.owned.GetOrCreateItemWithName(this.BoosterId(boosterType)).count;
	}

	public long UsedCount(GGMatch3.BoosterType boosterType)
	{
		return this.owned.GetOrCreateUsedItemWithName(this.BoosterId(boosterType)).count;
	}

	public void SetUsedCount(GGMatch3.BoosterType boosterType, long count)
	{
		this.owned.GetOrCreateUsedItemWithName(this.BoosterId(boosterType)).count = count;
		this.owned.Save();
	}

	public void Add(GGMatch3.BoosterType boosterType, int amount)
	{
		this.owned.GetOrCreateItemWithName(this.BoosterId(boosterType)).count += (long)amount;
		this.owned.Save();
	}

	public void SetOwned(GGMatch3.BoosterType boosterType, long ownedNumber)
	{
		this.owned.GetOrCreateItemWithName(this.BoosterId(boosterType)).count = MathEx.Max(ownedNumber, 0L);
		this.owned.Save();
	}

	public bool IsOwned(string name)
	{
		return this.owned.isOwned(name);
	}

	private static PlayerInventory instance_;

	private OwnedItems _003Cowned_003Ek__BackingField;

	public enum Item
	{
		EasyModeItem,
		MediumModeItem,
		HardModeItem,
		NoAds,
		Trainer,
		FreeEnergy,
		FreeEnergyLimited
	}
}
