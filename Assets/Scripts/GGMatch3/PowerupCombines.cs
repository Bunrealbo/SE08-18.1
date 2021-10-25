using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class PowerupCombines
	{
		private void ReturnToPool()
		{
			this.combinesPool.AddRange(this.combines);
			this.combines.Clear();
		}

		private PowerupCombines.PowerupCombine GetCombineFromPool()
		{
			if (this.combinesPool.Count == 0)
			{
				return new PowerupCombines.PowerupCombine();
			}
			int index = this.combinesPool.Count - 1;
			PowerupCombines.PowerupCombine result = this.combinesPool[index];
			this.combinesPool.RemoveAt(index);
			return result;
		}

		private bool IsValidPowerupInSlot(ChipType chipType, Slot slot)
		{
			Chip slotComponent = slot.GetSlotComponent<Chip>();
			if (slotComponent == null)
			{
				return false;
			}
			if (slot.isSlotGravitySuspended || slot.isSlotSwapSuspended)
			{
				return false;
			}
			if (chipType == ChipType.DiscoBall)
			{
				if (slot.isLockedForDiscoBomb)
				{
					return false;
				}
				if (!slotComponent.isPowerup && !slotComponent.canFormColorMatches)
				{
					return false;
				}
			}
			else if (!slotComponent.isPowerup)
			{
				return false;
			}
			return true;
		}

		private bool IsValidPowerupInSlot(Slot slot)
		{
			Chip slotComponent = slot.GetSlotComponent<Chip>();
			return slotComponent != null && slotComponent.isPowerup && !slot.isSlotGravitySuspended && !slot.isSlotSwapSuspended;
		}

		public List<PowerupCombines.PowerupCombine> FilterCombines(PowerupCombines.CombineType combineType)
		{
			this.filteredCombines.Clear();
			for (int i = 0; i < this.combines.Count; i++)
			{
				PowerupCombines.PowerupCombine powerupCombine = this.combines[i];
				if (powerupCombine.combineType == combineType)
				{
					this.filteredCombines.Add(powerupCombine);
				}
			}
			return this.filteredCombines;
		}

		public void Fill(Match3Game game)
		{
			this.ReturnToPool();
			foreach (Slot slot in game.board.slots)
			{
				if (slot != null && this.IsValidPowerupInSlot(slot))
				{
					ChipType chipType = slot.GetSlotComponent<Chip>().chipType;
					List<Slot> neigbourSlots = slot.neigbourSlots;
					for (int j = 0; j < neigbourSlots.Count; j++)
					{
						Slot slot2 = neigbourSlots[j];
						if (this.IsValidPowerupInSlot(chipType, slot2) && !Slot.IsPathBlockedBetween(slot, slot2))
						{
							PowerupCombines.PowerupCombine combineFromPool = this.GetCombineFromPool();
							combineFromPool.powerupSlot = slot;
							combineFromPool.exchangeSlot = slot2;
							this.combines.Add(combineFromPool);
							if (chipType != ChipType.DiscoBall)
							{
								PowerupCombines.PowerupCombine combineFromPool2 = this.GetCombineFromPool();
								combineFromPool2.powerupSlot = slot2;
								combineFromPool2.exchangeSlot = slot;
								this.combines.Add(combineFromPool2);
							}
						}
					}
				}
			}
		}

		public List<PowerupCombines.PowerupCombine> combines = new List<PowerupCombines.PowerupCombine>();

		private List<PowerupCombines.PowerupCombine> combinesPool = new List<PowerupCombines.PowerupCombine>();

		private List<PowerupCombines.PowerupCombine> filteredCombines = new List<PowerupCombines.PowerupCombine>();

		public enum CombineType
		{
			Unknown,
			DoubleSeekingMissle,
			DoubleRocket,
			DoubleBomb,
			DoubleDiscoBall,
			DiscoBallColor,
			DiscoBallSeekingMissle,
			DiscoBallRocket,
			DiscoBallBomb,
			RocketSeekingMissle,
			RocketBomb,
			BombSeekingMissle
		}

		public class PowerupCombine
		{
			public ActionScore GetActionScore(Match3Game game, Match3Goals goals)
			{
				ActionScore actionScore = default(ActionScore);
				Slot slot = this.exchangeSlot;
				IntVector2 position = slot.position;
				this.affectedSlotsList.Clear();
				List<Slot> list = this.affectedSlotsList;
				if (this.combineType == PowerupCombines.CombineType.DoubleSeekingMissle)
				{
					actionScore.goalsCount += 3;
					list = game.GetAreaOfEffect(ChipType.SeekingMissle, position);
				}
				else if (this.combineType == PowerupCombines.CombineType.DoubleRocket)
				{
					list = game.GetCross(position, 0);
				}
				else if (this.combineType == PowerupCombines.CombineType.DoubleBomb)
				{
					list = game.GetArea(position, 3);
				}
				else if (this.combineType == PowerupCombines.CombineType.DoubleDiscoBall)
				{
					list = game.GetAllPlayingSlots();
				}
				else if (this.combineType == PowerupCombines.CombineType.DiscoBallColor)
				{
					Chip slotComponent = this.exchangeSlot.GetSlotComponent<Chip>();
					if (slotComponent.canFormColorMatches)
					{
						ItemColor itemColor = slotComponent.itemColor;
						list = game.SlotsInDiscoBallDestroy(itemColor, false);
					}
				}
				else if (this.combineType == PowerupCombines.CombineType.DiscoBallSeekingMissle || this.combineType == PowerupCombines.CombineType.DiscoBallRocket || this.combineType == PowerupCombines.CombineType.DiscoBallBomb)
				{
					ItemColor itemColor2 = game.BestItemColorForDiscoBomb(true);
					List<Slot> list2 = game.SlotsInDiscoBallDestroy(itemColor2, true);
					ChipType chipType = ChipType.SeekingMissle;
					if (this.combineType == PowerupCombines.CombineType.DiscoBallSeekingMissle)
					{
						chipType = ChipType.SeekingMissle;
						actionScore.goalsCount += list2.Count;
					}
					else if (this.combineType == PowerupCombines.CombineType.DiscoBallBomb)
					{
						chipType = ChipType.Bomb;
					}
					this.affectedSlotsList.Clear();
					for (int i = 0; i < list2.Count; i++)
					{
						Slot slot2 = list2[i];
						if (this.combineType == PowerupCombines.CombineType.DiscoBallRocket)
						{
							chipType = ((i % 2 == 0) ? ChipType.HorizontalRocket : ChipType.VerticalRocket);
						}
						List<Slot> areaOfEffect = game.GetAreaOfEffect(chipType, slot2.position);
						this.affectedSlotsList.AddRange(areaOfEffect);
					}
					list = this.affectedSlotsList;
				}
				else if (this.combineType == PowerupCombines.CombineType.RocketBomb)
				{
					list = game.GetCross(position, 1);
				}
				else if (this.combineType == PowerupCombines.CombineType.RocketSeekingMissle || this.combineType == PowerupCombines.CombineType.BombSeekingMissle)
				{
					this.affectedSlotsList.Clear();
					List<Slot> areaOfEffect2 = game.GetAreaOfEffect(ChipType.SeekingMissle, position);
					this.affectedSlotsList.AddRange(areaOfEffect2);
					ChipType chipType2;
					if (this.Count(ChipType.HorizontalRocket) > 0)
					{
						chipType2 = ChipType.HorizontalRocket;
					}
					else if (this.Count(ChipType.VerticalRocket) > 0)
					{
						chipType2 = ChipType.VerticalRocket;
					}
					else
					{
						chipType2 = ChipType.Bomb;
					}
					List<Slot> list3 = game.goals.BestSlotsForSeekingMissle(game, slot);
					if (list3 != null && list3.Count > 0)
					{
						int index = 0;
						Slot slot3 = list3[index];
						areaOfEffect2 = game.GetAreaOfEffect(chipType2, slot3.position);
						this.affectedSlotsList.AddRange(areaOfEffect2);
					}
					list = this.affectedSlotsList;
				}
				bool includeNeighbours = this.combineType == PowerupCombines.CombineType.DiscoBallColor;
				bool isHavingCarpet = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].canCarpetSpreadFromHere)
					{
						isHavingCarpet = true;
						break;
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					Slot slot4 = list[k];
					actionScore += goals.ActionScoreForDestroyingSlot(slot4, game, isHavingCarpet, includeNeighbours);
				}
				return actionScore;
			}

			public PowerupCombines.CombineType combineType
			{
				get
				{
					if (this.Count(ChipType.SeekingMissle) == 2)
					{
						return PowerupCombines.CombineType.DoubleSeekingMissle;
					}
					if (this.Count(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 2)
					{
						return PowerupCombines.CombineType.DoubleRocket;
					}
					if (this.Count(ChipType.Bomb) == 2)
					{
						return PowerupCombines.CombineType.DoubleBomb;
					}
					if (this.Count(ChipType.DiscoBall) == 2)
					{
						return PowerupCombines.CombineType.DoubleDiscoBall;
					}
					if (this.Count(ChipType.DiscoBall) == 1 && this.Count(ChipType.Chip) == 1)
					{
						return PowerupCombines.CombineType.DiscoBallColor;
					}
					if (this.Count(ChipType.DiscoBall) == 1 && this.Count(ChipType.SeekingMissle) == 1)
					{
						return PowerupCombines.CombineType.DiscoBallSeekingMissle;
					}
					if (this.Count(ChipType.DiscoBall) == 1 && this.Count(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 1)
					{
						return PowerupCombines.CombineType.DiscoBallRocket;
					}
					if (this.Count(ChipType.DiscoBall) == 1 && this.Count(ChipType.Bomb) == 1)
					{
						return PowerupCombines.CombineType.DiscoBallBomb;
					}
					if (this.Count(ChipType.SeekingMissle) == 1 && this.Count(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 1)
					{
						return PowerupCombines.CombineType.RocketSeekingMissle;
					}
					if (this.Count(ChipType.Bomb) == 1 && this.Count(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 1)
					{
						return PowerupCombines.CombineType.RocketBomb;
					}
					if (this.Count(ChipType.SeekingMissle) == 1 && this.Count(ChipType.SeekingMissle) == 1)
					{
						return PowerupCombines.CombineType.BombSeekingMissle;
					}
					return PowerupCombines.CombineType.Unknown;
				}
			}

			private bool IsChipType(Slot slot, ChipType chipType)
			{
				Chip slotComponent = slot.GetSlotComponent<Chip>();
				return slotComponent != null && slotComponent.chipType == chipType;
			}

			public int Count(ChipType chipType)
			{
				int num = 0;
				if (this.IsChipType(this.powerupSlot, chipType))
				{
					num++;
				}
				if (this.IsChipType(this.exchangeSlot, chipType))
				{
					num++;
				}
				return num;
			}

			private bool IsChipType(Slot slot, ChipType chipType, ChipType chipType2)
			{
				Chip slotComponent = slot.GetSlotComponent<Chip>();
				return slotComponent != null && (slotComponent.chipType == chipType || slotComponent.chipType == chipType2);
			}

			public int Count(ChipType chipType, ChipType chipType2)
			{
				int num = 0;
				if (this.IsChipType(this.powerupSlot, chipType, chipType2))
				{
					num++;
				}
				if (this.IsChipType(this.exchangeSlot, chipType, chipType2))
				{
					num++;
				}
				return num;
			}

			public Slot powerupSlot;

			public Slot exchangeSlot;

			private List<Slot> affectedSlotsList = new List<Slot>();
		}
	}
}
