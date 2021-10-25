using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class PowerupActivations
	{
		private void ReturnToPool()
		{
			this.powerupsPool.AddRange(this.powerups);
			this.powerups.Clear();
		}

		private PowerupActivations.PowerupActivation GetPowerupActivation()
		{
			if (this.powerupsPool.Count == 0)
			{
				return new PowerupActivations.PowerupActivation();
			}
			int index = this.powerupsPool.Count - 1;
			PowerupActivations.PowerupActivation result = this.powerupsPool[index];
			this.powerupsPool.RemoveAt(index);
			return result;
		}

		private bool IsValidSpaceToSwapPowerup(Slot slot)
		{
			if (slot.isSlotGravitySuspended || slot.isSlotSwapSuspended || slot.isSlotMatchingSuspended)
			{
				return false;
			}
			Chip slotComponent = slot.GetSlotComponent<Chip>();
			return slotComponent == null || !slotComponent.isPowerup;
		}

		private bool IsValidPowerupInSlot(Slot slot)
		{
			Chip slotComponent = slot.GetSlotComponent<Chip>();
			return slotComponent != null && !slot.isSlotGravitySuspended && !slot.isSlotSwapSuspended && !slot.isTapToActivateSuspended && slotComponent.isPowerup && slotComponent.chipType != ChipType.DiscoBall;
		}

		public List<PowerupActivations.PowerupActivation> CreatePotentialActivations(ChipType chipType, Slot slot)
		{
			List<PowerupActivations.PowerupActivation> list = new List<PowerupActivations.PowerupActivation>();
			PowerupActivations.PowerupActivation powerupActivation = this.GetPowerupActivation();
			powerupActivation.InitWithTap(slot, chipType);
			list.Add(powerupActivation);
			List<Slot> neigbourSlots = slot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				Slot slot2 = neigbourSlots[i];
				if (this.IsValidSpaceToSwapPowerup(slot2))
				{
					PowerupActivations.PowerupActivation powerupActivation2 = this.GetPowerupActivation();
					powerupActivation2.InitWithSwap(slot, slot2, chipType);
					list.Add(powerupActivation2);
				}
			}
			return list;
		}

		public void Fill(Match3Game game)
		{
			this.ReturnToPool();
			foreach (Slot slot in game.board.slots)
			{
				if (slot != null && this.IsValidPowerupInSlot(slot))
				{
					ChipType chipType = slot.GetSlotComponent<Chip>().chipType;
					PowerupActivations.PowerupActivation powerupActivation = this.GetPowerupActivation();
					powerupActivation.InitWithTap(slot);
					this.powerups.Add(powerupActivation);
					List<Slot> neigbourSlots = slot.neigbourSlots;
					for (int j = 0; j < neigbourSlots.Count; j++)
					{
						Slot slot2 = neigbourSlots[j];
						if (this.IsValidSpaceToSwapPowerup(slot2) && !Slot.IsPathBlockedBetween(slot, slot2))
						{
							PowerupActivations.PowerupActivation powerupActivation2 = this.GetPowerupActivation();
							powerupActivation2.InitWithSwap(slot, slot2);
							this.powerups.Add(powerupActivation2);
						}
					}
				}
			}
		}

		public List<PowerupActivations.PowerupActivation> powerups = new List<PowerupActivations.PowerupActivation>();

		private List<PowerupActivations.PowerupActivation> powerupsPool = new List<PowerupActivations.PowerupActivation>();

		public class PowerupActivation
		{
			public bool isSwipe
			{
				get
				{
					return !this.isTap;
				}
			}

			public bool isTap
			{
				get
				{
					return this.exchangeSlot == null;
				}
			}

			public ActionScore GetActionScore(Match3Game game, Match3Goals goals)
			{
				ActionScore actionScore = default(ActionScore);
				IntVector2 position = this.powerupSlot.position;
				if (this.exchangeSlot != null)
				{
					position = this.exchangeSlot.position;
				}
				if (this.powerupType == ChipType.SeekingMissle)
				{
					actionScore.goalsCount++;
				}
				List<Slot> areaOfEffect = game.GetAreaOfEffect(this.powerupType, position);
				bool isHavingCarpet = false;
				for (int i = 0; i < areaOfEffect.Count; i++)
				{
					if (areaOfEffect[i].canCarpetSpreadFromHere)
					{
						isHavingCarpet = true;
						break;
					}
				}
				for (int j = 0; j < areaOfEffect.Count; j++)
				{
					Slot slot = areaOfEffect[j];
					if (slot == this.exchangeSlot && slot != this.powerupSlot)
					{
						actionScore += goals.ActionScoreForDestroyingSwitchingSlots(this.exchangeSlot, this.powerupSlot, game, isHavingCarpet, false);
					}
					else
					{
						actionScore += goals.ActionScoreForDestroyingSlot(slot, game, isHavingCarpet, false);
					}
				}
				return actionScore;
			}

			public void InitWithTap(Slot slot, ChipType chipType)
			{
				this.powerupSlot = slot;
				this.exchangeSlot = null;
				this.powerupType = chipType;
			}

			public void InitWithTap(Slot slot)
			{
				this.powerupSlot = slot;
				this.exchangeSlot = null;
				this.powerupType = slot.GetSlotComponent<Chip>().chipType;
			}

			public void InitWithSwap(Slot powerupSlot, Slot exchangeSlot)
			{
				this.powerupSlot = powerupSlot;
				this.exchangeSlot = exchangeSlot;
				this.powerupType = powerupSlot.GetSlotComponent<Chip>().chipType;
			}

			public void InitWithSwap(Slot powerupSlot, Slot exchangeSlot, ChipType chipType)
			{
				this.powerupSlot = powerupSlot;
				this.exchangeSlot = exchangeSlot;
				this.powerupType = chipType;
			}

			public Slot powerupSlot;

			public ChipType powerupType;

			public Slot exchangeSlot;
		}
	}
}
