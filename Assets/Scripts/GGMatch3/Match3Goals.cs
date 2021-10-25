using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class Match3Goals
	{
		public ActionScore ActionScoreForDestroyingSwitchingSlots(Slot exchangeSlot, Slot slotToSwipe, Match3Game game, bool isHavingCarpet, bool includeNeighbours)
		{
			ActionScore actionScore = default(ActionScore);
			actionScore = this.ActionScoreForDestroyingSwitchingSlots(exchangeSlot, slotToSwipe, isHavingCarpet);
			if (!includeNeighbours)
			{
				return actionScore;
			}
			List<Slot> neigbourSlots = exchangeSlot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				Slot slot = neigbourSlots[i];
				if (slot != null && slot.isDestroyedByMatchingNextTo)
				{
					actionScore += this.ActionScoreForDestroyingSlot(slot, false);
				}
			}
			return actionScore;
		}

		public ActionScore ActionScoreForDestroyingSlot(Slot slot, Match3Game game, bool isHavingCarpet, bool includeNeighbours)
		{
			ActionScore actionScore = default(ActionScore);
			if (slot == null)
			{
				return actionScore;
			}
			actionScore = this.ActionScoreForDestroyingSlot(slot, isHavingCarpet);
			if (!includeNeighbours)
			{
				return actionScore;
			}
			List<Slot> neigbourSlots = slot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				Slot slot2 = neigbourSlots[i];
				if (slot2 != null && slot2.isDestroyedByMatchingNextTo)
				{
					actionScore += this.ActionScoreForDestroyingSlot(slot2, false);
				}
			}
			return actionScore;
		}

		private ActionScore ActionScoreForDestroyingSwitchingSlots(Slot exchangeSlot, Slot slotToSwipe, bool isHavingCarpet)
		{
			ActionScore result = default(ActionScore);
			if (exchangeSlot == null || slotToSwipe == null)
			{
				return result;
			}
			if (this.slotData == null)
			{
				return result;
			}
			Match3Game game = exchangeSlot.game;
			int num = game.board.Index(exchangeSlot.position);
			Match3Goals.SlotData slotData = this.slotData[num];
			int num2 = game.board.Index(slotToSwipe.position);
			Match3Goals.SlotData slotData2 = this.slotData[num2];
			if (!slotData.IsDestroyable(0))
			{
				return result;
			}
			if (isHavingCarpet && slotData.carpetScore > 0)
			{
				result.goalsCount++;
			}
			if (slotData.score - slotData.chipScore + slotData2.chipScore > 0)
			{
				result.goalsCount++;
			}
			if (slotData.blockingLevel > 0)
			{
				result.obstaclesDestroyed = 1;
			}
			return result;
		}

		private ActionScore ActionScoreForDestroyingSlot(Slot slot, bool isHavingCarpet)
		{
			ActionScore result = default(ActionScore);
			if (slot == null)
			{
				return result;
			}
			if (this.slotData == null)
			{
				return result;
			}
			int num = slot.game.board.Index(slot.position);
			Match3Goals.SlotData slotData = this.slotData[num];
			if (!slotData.IsDestroyable(0))
			{
				return result;
			}
			if (isHavingCarpet && slotData.carpetScore > 0)
			{
				result.goalsCount++;
			}
			if (slotData.isAvailableToSelect)
			{
				result.goalsCount++;
			}
			if (slotData.blockingLevel > 0)
			{
				result.obstaclesDestroyed = 1;
			}
			return result;
		}

		public ActionScore FreshActionScoreForDestroyingSlot(Slot slot)
		{
			ActionScore result = default(ActionScore);
			if (slot == null)
			{
				return result;
			}
			for (int i = 0; i < this.goals.Count; i++)
			{
				Match3Goals.GoalBase goalBase = this.goals[i];
				if (slot.IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef.Create(goalBase.config)))
				{
					result.goalsCount++;
				}
				result.obstaclesDestroyed = slot.totalBlockerLevel;
			}
			return result;
		}

		public bool IsDestroyingSlotCompleatingAGoal(Slot slot, Match3Game game, bool includeNeighbours)
		{
			if (slot == null)
			{
				return false;
			}
			if (this.IsDestroyingSlotCompleatingAGoal(slot))
			{
				return true;
			}
			if (!includeNeighbours)
			{
				return false;
			}
			List<Slot> neigbourSlots = slot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				Slot slot2 = neigbourSlots[i];
				if (slot2 != null && slot2.isDestroyedByMatchingNextTo && this.IsDestroyingSlotCompleatingAGoal(slot2))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsDestroyingSlotCompleatingAGoal(Slot slot)
		{
			if (slot == null)
			{
				return false;
			}
			if (this.slotData == null)
			{
				return false;
			}
			int num = slot.game.board.Index(slot.position);
			Match3Goals.SlotData slotData = this.slotData[num];
			return slotData.isAvailableToSelect && slotData.IsDestroyable(0);
		}

		public void FillSlotData(Match3Game game)
		{
			Slot[] slots = game.board.slots;
			this.slotsWithFallingPickups.Clear();
			if (this.slotData == null)
			{
				this.slotData = new Match3Goals.SlotData[slots.Length];
				for (int i = 0; i < slots.Length; i++)
				{
					Slot slot = slots[i];
					this.slotData[i] = new Match3Goals.SlotData(slot, 0);
				}
			}
			for (int j = 0; j < slots.Length; j++)
			{
				Slot slot2 = slots[j];
				this.slotData[j].Init(slot2);
				if (slot2 != null)
				{
					Chip slotComponent = slot2.GetSlotComponent<Chip>();
					if (slotComponent != null && slotComponent.isFallingPickupElement)
					{
						this.slotsWithFallingPickups.Add(slot2);
					}
					int num = 0;
					int num2 = 0;
					for (int k = 0; k < this.goals.Count; k++)
					{
						Match3Goals.GoalBase goalBase = this.goals[k];
						int num3 = goalBase.ScoreForSlot(slot2);
						num2 += num3;
						if (goalBase.config.chipType == ChipType.Chip)
						{
							num += num3;
						}
					}
					Match3Goals.SlotData slotData = this.slotData[j];
					slotData.score = num2;
					slotData.chipScore = num;
					slotData.blockingLevel = slot2.totalBlockerLevel;
					if (game.board.carpet.IsPossibleToAddCarpet(slot2.position))
					{
						slotData.carpetScore++;
					}
					slotData.isAvailableToSelect = (num2 > 0);
					this.slotData[j] = slotData;
				}
			}
			for (int l = 0; l < this.slotsWithFallingPickups.Count; l++)
			{
				Slot slot3 = this.slotsWithFallingPickups[l];
				Chip slotComponent2 = slot3.GetSlotComponent<Chip>();
				if (slotComponent2 != null)
				{
					Match3Goals.ChipTypeDef chipType = Match3Goals.ChipTypeDef.Create(slotComponent2);
					for (int m = 0; m < this.goals.Count; m++)
					{
						if (this.goals[m].IsPartOfGoal(chipType))
						{
							int num4 = 0;
							Slot slot4 = slot3;
							while (slot4 != null && slot4 != null)
							{
								bool flag = slot4.totalBlockerLevelForFalling > 0;
								Chip slotComponent3 = slot4.GetSlotComponent<Chip>();
								bool flag2 = slotComponent3 != null && (slotComponent3.chipType == ChipType.Chip || slotComponent3.isPickupElement);
								if (!flag && !flag2)
								{
									num4++;
									slot4 = slot4.NextSlotToPushToWithoutSandflow();
								}
								else
								{
									int num5 = game.board.Index(slot4.position);
									Match3Goals.SlotData slotData2 = this.slotData[num5];
									slotData2.score++;
									int b = Mathf.Max(game.board.size.x, game.board.size.y) - num4;
									slotData2.fallingPickupScore = Mathf.Max(slotData2.fallingPickupScore, b);
									slotData2.isAvailableToSelect = true;
									this.slotData[num5] = slotData2;
									slot4 = slot4.NextSlotToPushToWithoutSandflow();
									num4++;
									if (flag)
									{
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		public List<Slot> BestSlotsForSeekingMissleWithChip(Match3Game game, Slot originSlot, ChipType otherChipType)
		{
			this.bestSlotsForSeekingMissle.Clear();
			this.potentialSlotsForSeekingMissleWithChip.Clear();
			this.potentialSlotsForSeekingMissleJustBlockers.Clear();
			this.FillSlotData(game);
			bool flag = originSlot.canCarpetSpreadFromHere;
			List<Slot> neigbourSlots = originSlot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				if (neigbourSlots[i].canCarpetSpreadFromHere)
				{
					flag = true;
					break;
				}
			}
			int b = 0;
			int b2 = 0;
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < this.slotData.Length; j++)
			{
				Match3Goals.SlotData slotData = this.slotData[j];
				Slot slot = slotData.slot;
				bool flag2 = slotData.isAvailableToSelect || (flag && slotData.carpetScore > 0);
				int blockingLevel = slotData.blockingLevel;
				if (blockingLevel > 0)
				{
					this.potentialSlotsForSeekingMissleJustBlockers.Add(slotData);
					num2 = Mathf.Max(blockingLevel, blockingLevel);
				}
				if (flag2 && slot != originSlot)
				{
					int additionalLevelsDown = 0;
					if (slotData.IsDestroyable(additionalLevelsDown))
					{
						List<Slot> areaOfEffect = game.GetAreaOfEffect(otherChipType, slot.position);
						int num3 = 0;
						int num4 = 0;
						int num5 = 0;
						for (int k = 0; k < areaOfEffect.Count; k++)
						{
							Slot slot2 = areaOfEffect[k];
							Match3Goals.SlotData slotData2 = this.slotData[game.board.Index(slot2.position)];
							num3 += slotData2.TotalScore(flag);
							num4 += slotData.fallingPickupScore;
							if (slotData2.score == 0)
							{
								bool flag3 = slot2.totalBlockerLevel > 0;
								num5 += (flag3 ? 1 : 0);
							}
						}
						int num6 = num3 + num4;
						ActionScore actionScore = default(ActionScore);
						actionScore.goalsCount = num6;
						actionScore.obstaclesDestroyed = num5;
						num6 = actionScore.GoalsAndObstaclesScore(2);
						b = Mathf.Max(num3, b);
						b2 = Mathf.Max(num4, b2);
						num = Mathf.Max(num6, num);
						Match3Goals.SlotWithScore item = default(Match3Goals.SlotWithScore);
						item.slot = slotData.slot;
						item.score = num6;
						this.potentialSlotsForSeekingMissleWithChip.Add(item);
					}
				}
			}
			for (int l = 0; l < this.potentialSlotsForSeekingMissleWithChip.Count; l++)
			{
				Match3Goals.SlotWithScore slotWithScore = this.potentialSlotsForSeekingMissleWithChip[l];
				if (slotWithScore.score == num)
				{
					this.bestSlotsForSeekingMissle.Add(slotWithScore.slot);
				}
			}
			if (this.bestSlotsForSeekingMissle.Count == 0)
			{
				for (int m = 0; m < this.potentialSlotsForSeekingMissleJustBlockers.Count; m++)
				{
					Match3Goals.SlotData slotData3 = this.potentialSlotsForSeekingMissleJustBlockers[m];
					if (slotData3.blockingLevel == num2)
					{
						this.bestSlotsForSeekingMissle.Add(slotData3.slot);
					}
				}
			}
			return this.bestSlotsForSeekingMissle;
		}

		public List<Slot> BestSlotsForSeekingMissle(Match3Game game, Slot originSlot)
		{
			this.bestSlotsForSeekingMissle.Clear();
			this.potentialSlotsForSeekingMissle.Clear();
			this.potentialSlotsForSeekingMissleJustBlockers.Clear();
			this.FillSlotData(game);
			bool flag = originSlot.canCarpetSpreadFromHere;
			List<Slot> neigbourSlots = originSlot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				if (neigbourSlots[i].canCarpetSpreadFromHere)
				{
					flag = true;
					break;
				}
			}
			int b = 0;
			int b2 = 0;
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < this.slotData.Length; j++)
			{
				Match3Goals.SlotData slotData = this.slotData[j];
				Slot slot = slotData.slot;
				bool flag2 = slotData.isAvailableToSelect || (flag && slotData.carpetScore > 0);
				int blockingLevel = slotData.blockingLevel;
				if (blockingLevel > 0)
				{
					this.potentialSlotsForSeekingMissleJustBlockers.Add(slotData);
					num2 = Mathf.Max(blockingLevel, blockingLevel);
				}
				if (flag2)
				{
					int additionalLevelsDown = 0;
					if (slotData.IsDestroyable(additionalLevelsDown))
					{
						b = Mathf.Max(slotData.TotalScore(flag), b);
						b2 = Mathf.Max(slotData.fallingPickupScore, b2);
						num = Mathf.Max(slotData.TotalScore(flag) + slotData.fallingPickupScore, num);
						this.potentialSlotsForSeekingMissle.Add(slotData);
					}
				}
			}
			for (int k = 0; k < this.potentialSlotsForSeekingMissle.Count; k++)
			{
				Match3Goals.SlotData slotData2 = this.potentialSlotsForSeekingMissle[k];
				if (slotData2.TotalScore(flag) + slotData2.fallingPickupScore == num)
				{
					this.bestSlotsForSeekingMissle.Add(slotData2.slot);
				}
			}
			if (this.bestSlotsForSeekingMissle.Count == 0)
			{
				for (int l = 0; l < this.potentialSlotsForSeekingMissleJustBlockers.Count; l++)
				{
					Match3Goals.SlotData slotData3 = this.potentialSlotsForSeekingMissleJustBlockers[l];
					if (slotData3.blockingLevel == num2)
					{
						this.bestSlotsForSeekingMissle.Add(slotData3.slot);
					}
				}
			}
			return this.bestSlotsForSeekingMissle;
		}

		public bool isAllGoalsComplete
		{
			get
			{
				for (int i = 0; i < this.goals.Count; i++)
				{
					if (!this.goals[i].IsComplete())
					{
						return false;
					}
				}
				return true;
			}
		}

		public int TotalMovesCount
		{
			get
			{
				return this.goalsDefinition.movesCount;
			}
		}

		public Match3Goals.GoalBase GetActiveGoal(Match3Goals.ChipTypeDef chipTypeDef)
		{
			for (int i = 0; i < this.goals.Count; i++)
			{
				Match3Goals.GoalBase goalBase = this.goals[i];
				if (!goalBase.IsComplete() && goalBase.IsPartOfGoal(chipTypeDef))
				{
					return goalBase;
				}
			}
			return null;
		}

		public List<Match3Goals.GoalBase> GetActiveGoals()
		{
			List<Match3Goals.GoalBase> list = new List<Match3Goals.GoalBase>();
			for (int i = 0; i < this.goals.Count; i++)
			{
				Match3Goals.GoalBase goalBase = this.goals[i];
				if (!goalBase.IsComplete())
				{
					list.Add(goalBase);
				}
			}
			return list;
		}

		public void OnPickupGoal(Match3Goals.GoalBase goal)
		{
			if (goal == null)
			{
				return;
			}
			this.GetChipTypeCounter(Match3Goals.ChipTypeDef.Create(goal.config)).count++;
		}

		public Match3Goals.ChipTypeCounter GetChipTypeCounter(Match3Goals.ChipTypeDef chipTypeDef)
		{
			for (int i = 0; i < this.chipTypeCounters.Count; i++)
			{
				Match3Goals.ChipTypeCounter chipTypeCounter = this.chipTypeCounters[i];
				if (chipTypeCounter.chipTypeDef.IsEqual(chipTypeDef))
				{
					return chipTypeCounter;
				}
			}
			Match3Goals.ChipTypeCounter chipTypeCounter2 = new Match3Goals.ChipTypeCounter();
			chipTypeCounter2.chipTypeDef = chipTypeDef;
			this.chipTypeCounters.Add(chipTypeCounter2);
			return chipTypeCounter2;
		}

		public void Init(LevelDefinition levelDefinition, Match3Game game)
		{
			this.goalsDefinition = levelDefinition.goals;
			List<GoalConfig> list = this.goalsDefinition.goals;
			for (int i = 0; i < list.Count; i++)
			{
				GoalConfig goalConfig = list[i];
				Match3Goals.GoalBase item = this.Create(goalConfig);
				this.goals.Add(item);
			}
			int count = levelDefinition.extraFallingElements.fallingElementsList.Count;
			if (count > 0)
			{
				this.GetChipTypeCounter(new Match3Goals.ChipTypeDef
				{
					chipType = ChipType.FallingGingerbreadMan,
					itemColor = ItemColor.Unknown
				}).countAtStart += count;
			}
			foreach (Slot slot in game.board.slots)
			{
				if (slot != null)
				{
					slot.AddToGoalsAtStart(this);
				}
			}
			game.board.burriedElements.AddToGoalsAtStart(this);
			game.board.monsterElements.AddToGoalsAtStart(this);
			game.board.carpet.AddToGoalsAtStart(this);
		}

		public Match3Goals.GoalBase Create(GoalConfig goalConfig)
		{
			bool goalType = goalConfig.goalType != GoalType.CollectItems;
			Match3Goals.GoalBase goalBase = null;
			if (!goalType)
			{
				goalBase = new Match3Goals.CollectItemsGoal();
			}
			if (goalBase == null)
			{
				return goalBase;
			}
			goalBase.Init(goalConfig, this);
			return goalBase;
		}

		private List<Match3Goals.ChipTypeCounter> chipTypeCounters = new List<Match3Goals.ChipTypeCounter>();

		public List<Match3Goals.GoalBase> goals = new List<Match3Goals.GoalBase>();

		private GoalsDefinition goalsDefinition;

		private Match3Goals.SlotData[] slotData;

		private List<Slot> bestSlotsForSeekingMissle = new List<Slot>();

		private List<Match3Goals.SlotData> potentialSlotsForSeekingMissle = new List<Match3Goals.SlotData>();

		private List<Match3Goals.SlotData> potentialSlotsForSeekingMissleJustBlockers = new List<Match3Goals.SlotData>();

		private List<Match3Goals.SlotWithScore> potentialSlotsForSeekingMissleWithChip = new List<Match3Goals.SlotWithScore>();

		private List<Slot> slotsWithFallingPickups = new List<Slot>();

		public struct ChipTypeDef
		{
			public static bool HasColor(ChipType chipType)
			{
				return chipType == ChipType.Chip || chipType == ChipType.MonsterBlocker;
			}

			public static Match3Goals.ChipTypeDef Create(GoalConfig goalConfig)
			{
				Match3Goals.ChipTypeDef result = default(Match3Goals.ChipTypeDef);
				result.chipType = goalConfig.chipType;
				if (Match3Goals.ChipTypeDef.HasColor(goalConfig.chipType))
				{
					result.itemColor = goalConfig.itemColor;
				}
				else
				{
					result.itemColor = ItemColor.Uncolored;
				}
				return result;
			}

			public static Match3Goals.ChipTypeDef Create(Chip chip)
			{
				Match3Goals.ChipTypeDef result = default(Match3Goals.ChipTypeDef);
				result.chipType = chip.chipType;
				if (Match3Goals.ChipTypeDef.HasColor(chip.chipType))
				{
					result.itemColor = chip.itemColor;
				}
				else
				{
					result.itemColor = ItemColor.Uncolored;
				}
				return result;
			}

			private bool IsColorCompatible(ItemColor a, ItemColor b)
			{
				return a == ItemColor.AnyColor || b == ItemColor.AnyColor || a == b;
			}

			public bool IsEqual(Match3Goals.ChipTypeDef b)
			{
				return this.chipType == b.chipType && (!Match3Goals.ChipTypeDef.HasColor(b.chipType) || this.IsColorCompatible(this.itemColor, b.itemColor));
			}

			public ChipType chipType;

			public ItemColor itemColor;
		}

		public class ChipTypeCounter
		{
			public Match3Goals.ChipTypeDef chipTypeDef;

			public int count;

			public int countAtStart;
		}

		private struct SlotData
		{
			public int TotalScore(bool hasCarpet)
			{
				int num = this.score;
				if (hasCarpet)
				{
					num += this.carpetScore;
				}
				return num;
			}

			public void Init(Slot slot)
			{
				this.slot = slot;
				this.score = 0;
				this.isAvailableToSelect = false;
				this.fallingPickupScore = 0;
				this.blockingLevel = 0;
				this.carpetScore = 0;
				this.chipScore = 0;
			}

			public bool IsDestroyable(int additionalLevelsDown)
			{
				return this.slot != null && !this.slot.isDestroySuspended && this.slot.IsAboutToBeDestroyedLocksCount() + additionalLevelsDown <= this.slot.maxBlockerLevel;
			}

			public SlotData(Slot slot, int score)
			{
				this.slot = slot;
				this.score = score;
				this.isAvailableToSelect = false;
				this.blockingLevel = 0;
				this.fallingPickupScore = 0;
				this.carpetScore = 0;
				this.chipScore = 0;
			}

			public Slot slot;

			public int score;

			public int carpetScore;

			public int blockingLevel;

			public bool isAvailableToSelect;

			public int fallingPickupScore;

			public int chipScore;
		}

		private struct SlotWithScore
		{
			public Slot slot;

			public int score;

			private ActionScore actionScore;
		}

		public class GoalBase
		{
			public virtual void Init(GoalConfig config, Match3Goals goals)
			{
				this.config = config;
				this.goals = goals;
			}

			public virtual int CountAtStart
			{
				get
				{
					return this._003CCountAtStart_003Ek__BackingField;
				}
			}

			public virtual int RemainingCount
			{
				get
				{
					return this._003CRemainingCount_003Ek__BackingField;
				}
			}

			public virtual bool IsComplete()
			{
				return false;
			}

			public virtual bool IsPartOfGoal(Match3Goals.ChipTypeDef chipType)
			{
				return false;
			}

			public virtual int ScoreForSlot(Slot slot)
			{
				return 0;
			}

			public GoalConfig config;

			public Match3Goals goals;

			private readonly int _003CCountAtStart_003Ek__BackingField;

			private readonly int _003CRemainingCount_003Ek__BackingField;
		}

		public class CollectItemsGoal : Match3Goals.GoalBase
		{
			private Match3Goals.ChipTypeDef chipTypeDef
			{
				get
				{
					return Match3Goals.ChipTypeDef.Create(this.config);
				}
			}

			public override int CountAtStart
			{
				get
				{
					Match3Goals.ChipTypeCounter chipTypeCounter = this.goals.GetChipTypeCounter(this.chipTypeDef);
					int result = this.config.collectCount;
					if (this.config.isCollectAllPresentAtStart)
					{
						result = chipTypeCounter.countAtStart;
					}
					return result;
				}
			}

			public override int RemainingCount
			{
				get
				{
					Match3Goals.ChipTypeCounter chipTypeCounter = this.goals.GetChipTypeCounter(this.chipTypeDef);
					int num = this.config.collectCount;
					if (this.config.isCollectAllPresentAtStart)
					{
						num = chipTypeCounter.countAtStart;
					}
					return num - chipTypeCounter.count;
				}
			}

			public override bool IsPartOfGoal(Match3Goals.ChipTypeDef chipType)
			{
				return chipType.IsEqual(this.chipTypeDef);
			}

			public override bool IsComplete()
			{
				return this.RemainingCount <= 0;
			}

			public override int ScoreForSlot(Slot slot)
			{
				if (this.chipTypeDef.chipType == ChipType.FallingGingerbreadMan)
				{
					return 0;
				}
				if (this.IsComplete())
				{
					return 0;
				}
				int result = 0;
				if (slot.IsCompatibleWithPickupGoal(this.chipTypeDef))
				{
					if (this.chipTypeDef.chipType == ChipType.Chip)
					{
						result = 1;
					}
					else if (this.chipTypeDef.chipType == ChipType.GrowingElementPiece)
					{
						if (slot.GetSlotComponent<GrowingElementChip>() != null)
						{
							result = 1;
						}
						else
						{
							result = 2;
						}
					}
					else
					{
						result = 2;
					}
				}
				return result;
			}
		}
	}
}
