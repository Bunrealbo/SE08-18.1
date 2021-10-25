using System;
using ProtoModels;
using UnityEngine;

public class EnergyManager : BehaviourSingleton<EnergyManager>
{
	public bool isFullLives
	{
		get
		{
			return this.ownedPlayCoins >= EnergyControlConfig.instance.totalCoin;
		}
	}

	public float GetCurrentEnergyPercent()
	{
		return this.GetCurrentEnergyValue() / EnergyControlConfig.instance.maxEnergy;
	}

	public int ownedPlayCoins
	{
		get
		{
			return Mathf.FloorToInt(this.GetCurrentEnergyValue() / (float)EnergyControlConfig.instance.energyPointPerCoin);
		}
		set
		{
			float energy = EnergyControlConfig.instance.CoinsToEnergy(value);
			this.SetEnergy(energy);
		}
	}

	public void SpendLifeIfNotFreeEnergy()
	{
		if (this.isFreeEnergy)
		{
			return;
		}
		int ownedPlayCoins = this.ownedPlayCoins;
		this.ownedPlayCoins = ownedPlayCoins - 1;
	}

	public void AddLifeIfNotFreeEnergy()
	{
		if (this.isFreeEnergy)
		{
			return;
		}
		int ownedPlayCoins = this.ownedPlayCoins;
		this.ownedPlayCoins = ownedPlayCoins + 1;
	}

	public bool HasEnergyForOneLife()
	{
		return this.isFreeEnergy || this.ownedPlayCoins > 0;
	}

	public void ConsumeCoin(int coinAmount)
	{
		if (this.isFreeEnergy)
		{
			return;
		}
		if (coinAmount <= 0)
		{
			return;
		}
		GGPlayerSettings.instance.IncreaseSessionCoins(coinAmount);
		this.AddCoins(-coinAmount);
	}

	public void AddCoins(int coinAmount)
	{
		int energyPointPerCoin = EnergyControlConfig.instance.energyPointPerCoin;
		float num = MathEx.Max(0f, this.GetCurrentEnergyValue() + EnergyControlConfig.instance.CoinsToEnergy(coinAmount));
		if (num > EnergyControlConfig.instance.maxEnergy)
		{
			this.SetEnergy((float)(Mathf.FloorToInt(num / (float)energyPointPerCoin) * energyPointPerCoin));
			return;
		}
		this.SetEnergy(num);
	}

	public void DebugChangeEnergy(float modifyEnergtPoints)
	{
		GGPlayerSettings.instance.SetEnergy(this.GetCurrentEnergyValue() + modifyEnergtPoints, DateTime.Now);
	}

	public void FillEnergy()
	{
		this.SetEnergy(EnergyControlConfig.instance.maxEnergy);
	}

	public float secPerCoin
	{
		get
		{
			EnergyControlConfig instance = EnergyControlConfig.instance;
			return instance.maxEnergy * (float)instance.secondsToRefreshPoint / (float)instance.totalCoin;
		}
	}

	public float secToNextCoin
	{
		get
		{
			int totalCoin = EnergyControlConfig.instance.totalCoin;
			if (this.ownedPlayCoins < totalCoin)
			{
				return (float)(this.ownedPlayCoins + 1) * this.secPerCoin - this.GetCurrentEnergyPercent() * this.secPerCoin * (float)totalCoin;
			}
			return 0f;
		}
	}

	private string freeEnergyString
	{
		get
		{
			if (this.freeEnergyString_ == null)
			{
				this.freeEnergyString_ = PlayerInventory.Item.FreeEnergy.ToString();
			}
			return this.freeEnergyString_;
		}
	}

	private string limitedFreeEnergyString
	{
		get
		{
			if (this.limitedFreeEnergyString_ == null)
			{
				this.limitedFreeEnergyString_ = PlayerInventory.Item.FreeEnergyLimited.ToString();
			}
			return this.limitedFreeEnergyString_;
		}
	}

	public bool isUnlimitedInfiniteEnergy
	{
		get
		{
			return PlayerInventory.instance.IsOwned(this.freeEnergyString);
		}
	}

	public bool isFreeEnergy
	{
		get
		{
			return this.isUnlimitedInfiniteEnergy || this.isLimitedFreeEnergyActive;
		}
	}

	public bool isLimitedFreeEnergyActive
	{
		get
		{
			OwnedItemDAO itemWithName = PlayerInventory.instance.owned.GetItemWithName(this.limitedFreeEnergyString);
			return itemWithName != null && this.IsActive(itemWithName);
		}
	}

	public TimeSpan limitedEnergyTimespanLeft
	{
		get
		{
			OwnedItemDAO itemWithName = PlayerInventory.instance.owned.GetItemWithName(this.limitedFreeEnergyString);
			return this.ActiveTimespanLeft(itemWithName);
		}
	}

	public void UpdateLimitedEnergy(float passedTimeSec)
	{
		if (!this.isLimitedFreeEnergyActive)
		{
			return;
		}
		OwnedItemDAO itemWithName = PlayerInventory.instance.owned.GetItemWithName(this.limitedFreeEnergyString);
		if (itemWithName == null)
		{
			return;
		}
		long ticks = DateTime.Now.Ticks;
		itemWithName.lastCheckTime = MathEx.Max(itemWithName.purchaseTime, MathEx.Max(itemWithName.lastCheckTime, ticks));
		itemWithName.timeLeft -= TimeSpan.FromSeconds((double)passedTimeSec).Ticks;
		PlayerInventory.instance.owned.Save();
	}

	public bool IsActive(OwnedItemDAO owned)
	{
		return owned.totalDuration == 0L || (owned.timeLeft > 0L && this.ActiveTimespanLeft(owned).TotalSeconds >= 0.0);
	}

	public TimeSpan ActiveTimespanLeft(OwnedItemDAO owned)
	{
		if (owned == null)
		{
			return TimeSpan.FromSeconds(-1.0);
		}
		if (owned.totalDuration <= 0L)
		{
			return TimeSpan.FromDays(365.0);
		}
		if (owned.timeLeft <= 0L)
		{
			return TimeSpan.FromSeconds(-1.0);
		}
		TimeSpan value = TimeSpan.FromTicks(owned.totalDuration);
		DateTime d = new DateTime(owned.purchaseTime).Add(value);
		DateTime dateTime = DateTime.Now;
		DateTime dateTime2 = new DateTime(owned.lastCheckTime);
		if (dateTime2 > dateTime)
		{
			dateTime = dateTime2;
		}
		long num = (d - dateTime).Ticks;
		if (owned.timeLeft < num)
		{
			num = owned.timeLeft;
		}
		return TimeSpan.FromTicks(num);
	}

	public TimeSpan TimeSpanTillEnergyFull()
	{
		TimeSpan timeSpan = DateTime.Now.Subtract(new DateTime(GGPlayerSettings.instance.Model.lastTimeTookEnergy));
		EnergyControlConfig instance = EnergyControlConfig.instance;
		float num = instance.maxEnergy * (float)instance.secondsToRefreshPoint;
		double num2 = timeSpan.TotalSeconds + (double)(this.GetCurrentEnergyValue() * (float)instance.secondsToRefreshPoint);
		return TimeSpan.FromSeconds(MathEx.Max(0.0, (double)num - num2));
	}

	public float GetCurrentEnergyValue()
	{
		if (GGPlayerSettings.instance.Model.energy >= EnergyControlConfig.instance.maxEnergy)
		{
			return GGPlayerSettings.instance.Model.energy;
		}
		TimeSpan timeSpan = DateTime.Now.Subtract(new DateTime(GGPlayerSettings.instance.Model.lastTimeTookEnergy));
		float num = Mathf.Min(EnergyControlConfig.instance.GetEnergyForTimespan(timeSpan), EnergyControlConfig.instance.maxEnergy - GGPlayerSettings.instance.Model.energy);
		num = Mathf.Max(num, 0f);
		return GGPlayerSettings.instance.Model.energy + num;
	}

	public bool HasEnoughEnergy(float energyNeededToHave)
	{
		return this.isFreeEnergy || this.GetCurrentEnergyValue() >= energyNeededToHave;
	}

	public void SpendEnergy(float energyToSpend)
	{
		if (this.isFreeEnergy)
		{
			return;
		}
		float energy = MathEx.Max(0f, this.GetCurrentEnergyValue() - energyToSpend);
		this.SetEnergy(energy);
	}

	public float MaxEnergy
	{
		get
		{
			if (EnergyControlConfig.instance == null)
			{
				return 100f;
			}
			return EnergyControlConfig.instance.maxEnergy;
		}
	}

	public void SetEnergy(float energy)
	{
		GGPlayerSettings.instance.SetEnergy(energy, DateTime.Now);
	}

	public void GainEnergy(float energyToGain)
	{
		this.SetEnergy(this.GetCurrentEnergyValue() + energyToGain);
	}

	private string freeEnergyString_;

	private string limitedFreeEnergyString_;
}
