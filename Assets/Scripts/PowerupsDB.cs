using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsDB : ScriptableObjectSingleton<PowerupsDB>
{
	public PowerupsDB.PowerupDefinition Powerup(PowerupType powerupType)
	{
		for (int i = 0; i < this.powerups.Count; i++)
		{
			PowerupsDB.PowerupDefinition powerupDefinition = this.powerups[i];
			if (powerupDefinition.type == powerupType)
			{
				return powerupDefinition;
			}
		}
		return null;
	}

	protected override void UpdateData()
	{
		base.UpdateData();
	}

	[SerializeField]
	public List<PowerupsDB.PowerupDefinition> powerups = new List<PowerupsDB.PowerupDefinition>();

	[Serializable]
	public class PowerupDefinition
	{
		public long ownedCount
		{
			get
			{
				return PlayerInventory.instance.OwnedCount(this.type);
			}
			set
			{
				PlayerInventory.instance.SetOwned(this.type, Math.Max(value, 0L));
			}
		}

		public long usedCount
		{
			get
			{
				return PlayerInventory.instance.UsedCount(this.type);
			}
			set
			{
				PlayerInventory.instance.SetUsedCount(this.type, value);
			}
		}

		public PowerupType type;

		public string name;

		public string description;

		public int buyQuanitty = 3;

		public SingleCurrencyPrice buyPrice = new SingleCurrencyPrice();
	}
}
