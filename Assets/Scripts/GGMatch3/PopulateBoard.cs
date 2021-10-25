using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class PopulateBoard
	{
		private void Clear()
		{
			this.mustEnsureNoMatching.Clear();
			this.canHaveAnyColor.Clear();
			this.canNotFormMatches.Clear();
			this.cachedList.Clear();
		}

		private ItemColor RandomColor()
		{
			return this.RandomColor(this.initParams.availableColors);
		}

		private ItemColor RandomColor(List<ItemColor> availableColors)
		{
			return availableColors[this.initParams.randomProvider.Range(0, availableColors.Count)];
		}

		public void RandomPopulate(LevelDefinition level, PopulateBoard.Params initParams)
		{
			this.board.Init(level);
			this.RandomPopulate(this.board, initParams);
		}

		public bool RandomPopulate(PopulateBoard.BoardRepresentation board, PopulateBoard.Params initParams)
		{
			this.initParams = initParams;
			this.board = board;
			this.Clear();
			List<IntVector2> list = this.cachedList;
			list.Clear();
			list.Add(IntVector2.up);
			list.Add(IntVector2.down);
			list.Add(IntVector2.left);
			list.Add(IntVector2.right);
			List<PopulateBoard.BoardRepresentation.RepresentationSlot> slots = board.slots;
			for (int i = 0; i < slots.Count; i++)
			{
				PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot = slots[i];
				if (representationSlot.needsToBeGenerated && !representationSlot.isOutOfPlayArea)
				{
					if (representationSlot.isFormingColorMatchesSuspended)
					{
						this.canNotFormMatches.Add(representationSlot);
					}
					else
					{
						bool flag = false;
						for (int j = 0; j < list.Count; j++)
						{
							IntVector2 b = list[j];
							IntVector2 pos = representationSlot.position + b;
							PopulateBoard.BoardRepresentation.RepresentationSlot slot = board.GetSlot(pos);
							if (slot != null && (slot.canFormColorMatches || slot.isPositionInEmptyNeighbourhoodAtStart))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							representationSlot.isMatchCheckingRequired = true;
							this.mustEnsureNoMatching.Add(representationSlot);
						}
						else
						{
							representationSlot.isPositionInEmptyNeighbourhoodAtStart = true;
							this.canHaveAnyColor.Add(representationSlot);
						}
					}
				}
			}
			for (int k = 0; k < this.canNotFormMatches.Count; k++)
			{
				PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot2 = this.canNotFormMatches[k];
				representationSlot2.isGenerated = true;
				representationSlot2.itemColor = this.RandomColor();
			}
			List<PopulateBoard.PotentialMatch> list2 = new List<PopulateBoard.PotentialMatch>();
			for (int l = 0; l < this.canHaveAnyColor.Count; l++)
			{
				PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot3 = this.canHaveAnyColor[l];
				representationSlot3.isGenerated = true;
				representationSlot3.itemColor = this.RandomColor();
				PopulateBoard.BoardRepresentation.RepresentationSlot slot2 = board.GetSlot(representationSlot3.position + IntVector2.right);
				PopulateBoard.BoardRepresentation.RepresentationSlot slot3 = board.GetSlot(representationSlot3.position + IntVector2.right * 2);
				PopulateBoard.BoardRepresentation.RepresentationSlot slot4 = board.GetSlot(representationSlot3.position + IntVector2.right + IntVector2.up);
				PopulateBoard.BoardRepresentation.RepresentationSlot slot5 = board.GetSlot(representationSlot3.position + IntVector2.right + IntVector2.down);
				PopulateBoard.BoardRepresentation.RepresentationSlot slot6 = board.GetSlot(representationSlot3.position + IntVector2.up);
				PopulateBoard.BoardRepresentation.RepresentationSlot slot7 = board.GetSlot(representationSlot3.position + IntVector2.up * 2);
				bool flag2 = this.IsAvailableForSwap(representationSlot3, slot2) && this.IsInCanHaveAnyColor(slot3);
				if (flag2 && this.IsInCanHaveAnyColor(slot4))
				{
					PopulateBoard.PotentialMatch item = new PopulateBoard.PotentialMatch(representationSlot3, slot3, slot4);
					list2.Add(item);
				}
				if (flag2 && this.IsInCanHaveAnyColor(slot5))
				{
					PopulateBoard.PotentialMatch item2 = new PopulateBoard.PotentialMatch(representationSlot3, slot3, slot5);
					list2.Add(item2);
				}
				object obj = this.IsAvailableForSwap(representationSlot3, slot6) && this.IsInCanHaveAnyColor(slot7);
				PopulateBoard.BoardRepresentation.RepresentationSlot slot8 = board.GetSlot(representationSlot3.position + IntVector2.left + IntVector2.up);
				PopulateBoard.BoardRepresentation.RepresentationSlot slot9 = board.GetSlot(representationSlot3.position + IntVector2.right + IntVector2.up);
				object obj2 = obj;
				if (obj2 != null && this.IsInCanHaveAnyColor(slot8))
				{
					PopulateBoard.PotentialMatch item3 = new PopulateBoard.PotentialMatch(representationSlot3, slot7, slot8);
					list2.Add(item3);
				}
				if (obj2 != null && this.IsInCanHaveAnyColor(slot9))
				{
					PopulateBoard.PotentialMatch item4 = new PopulateBoard.PotentialMatch(representationSlot3, slot7, slot9);
					list2.Add(item4);
				}
			}
			GGUtil.Shuffle<PopulateBoard.PotentialMatch>(list2, initParams.randomProvider);
			int maxPotentialMatches = initParams.maxPotentialMatches;
			for (int m = 0; m < Mathf.Min(list2.Count, maxPotentialMatches); m++)
			{
				PopulateBoard.PotentialMatch potentialMatch = list2[m];
				ItemColor itemColor = this.RandomColor();
				for (int n = 0; n < potentialMatch.slotsThatNeedToBeTheSame.Count; n++)
				{
					if(potentialMatch.slotsThatNeedToBeTheSame[n] != null)
						potentialMatch.slotsThatNeedToBeTheSame[n].itemColor = itemColor;
				}
			}
			for (int num = 0; num < this.mustEnsureNoMatching.Count; num++)
			{
				PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot4 = this.mustEnsureNoMatching[num];
				this.matchingCheck.Check(board, representationSlot4.position, initParams.availableColors);
				List<ItemColor> availableColors = this.matchingCheck.availableColors;
				if (availableColors.Count > 0)
				{
					ItemColor itemColor2 = this.RandomColor(availableColors);
					representationSlot4.itemColor = itemColor2;
					representationSlot4.isGenerated = true;
				}
				else
				{
					List<IntVector2> list3 = this.cachedList;
					list3.Clear();
					list3.Add(IntVector2.up);
					list3.Add(IntVector2.down);
					list3.Add(IntVector2.left);
					list3.Add(IntVector2.right);
					list3.Add(2 * IntVector2.up);
					list3.Add(2 * IntVector2.down);
					list3.Add(2 * IntVector2.left);
					list3.Add(2 * IntVector2.right);
					PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot5 = null;
					for (int num2 = 0; num2 < list3.Count; num2++)
					{
						IntVector2 pos2 = representationSlot4.position + list3[num2];
						PopulateBoard.BoardRepresentation.RepresentationSlot slot10 = board.GetSlot(pos2);
						if (slot10 != null && slot10.needsToBeGenerated && slot10.canFormColorMatches)
						{
							this.matchingCheck.Check(board, slot10.position, initParams.availableColors);
							availableColors = this.matchingCheck.availableColors;
							if (availableColors.Count >= 2)
							{
								representationSlot5 = slot10;
								break;
							}
						}
					}
					if (representationSlot5 == null)
					{
						this.GenerateSlotInMatch(representationSlot4);
					}
					else
					{
						representationSlot5.itemColor = this.RandomColor(availableColors);
						this.matchingCheck.Check(board, representationSlot4.position, initParams.availableColors);
						availableColors = this.matchingCheck.availableColors;
						if (availableColors.Count <= 0)
						{
							this.GenerateSlotInMatch(representationSlot4);
						}
						else
						{
							representationSlot4.itemColor = this.RandomColor(availableColors);
							representationSlot4.isGenerated = true;
						}
					}
				}
			}
			if (list2.Count > 0)
			{
				return true;
			}
			List<PopulateBoard.MatchBuilder.Match> list4 = new List<PopulateBoard.MatchBuilder.Match>();
			IntVector2 intVector = IntVector2.right;
			for (int num3 = 0; num3 < board.size.y; num3++)
			{
				for (int num4 = 0; num4 < board.size.x - 2; num4++)
				{
					IntVector2 intVector2 = new IntVector2(num4, num3);
					PopulateBoard.BoardRepresentation.RepresentationSlot slot11 = board.GetSlot(intVector2);
					PopulateBoard.BoardRepresentation.RepresentationSlot slot12 = board.GetSlot(intVector2 + intVector);
					PopulateBoard.BoardRepresentation.RepresentationSlot slot13 = board.GetSlot(intVector2 + intVector * 2);
					this.matchBuilder.Init(slot11, slot12, slot13);
					if (this.matchBuilder.Find(this.matchingCheck, board, initParams.availableColors))
					{
						this.matchBuilder.FillMatchesIn(list4);
					}
				}
			}
			intVector = IntVector2.up;
			for (int num5 = 0; num5 < board.size.x; num5++)
			{
				for (int num6 = 0; num6 < board.size.y - 2; num6++)
				{
					IntVector2 intVector3 = new IntVector2(num5, num6);
					PopulateBoard.BoardRepresentation.RepresentationSlot slot14 = board.GetSlot(intVector3);
					PopulateBoard.BoardRepresentation.RepresentationSlot slot15 = board.GetSlot(intVector3 + intVector);
					PopulateBoard.BoardRepresentation.RepresentationSlot slot16 = board.GetSlot(intVector3 + intVector * 2);
					this.matchBuilder.Init(slot14, slot15, slot16);
					if (this.matchBuilder.Find(this.matchingCheck, board, initParams.availableColors))
					{
						this.matchBuilder.FillMatchesIn(list4);
					}
				}
			}
			if (list4.Count == 0)
			{
				return false;
			}
			GGUtil.Shuffle<PopulateBoard.MatchBuilder.Match>(list4, initParams.randomProvider);
			List<ItemColor> list5 = new List<ItemColor>();
			List<PopulateBoard.MatchBuilder.Match> list6 = new List<PopulateBoard.MatchBuilder.Match>();
			for (int num7 = 0; num7 < list4.Count; num7++)
			{
				PopulateBoard.MatchBuilder.Match match = list4[num7];
				if (!this.IsIntersectingWithList(match, list6))
				{
					for (int num8 = 0; num8 < match.slots.Count; num8++)
					{
						PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot6 = match.slots[num8];
						representationSlot6.cachedColor = representationSlot6.itemColor;
					}
					bool flag3 = false;
					for (int num9 = 0; num9 < match.availableColor.Count; num9++)
					{
						ItemColor itemColor3 = match.availableColor[num9];
						list5.Clear();
						list5.Add(itemColor3);
						for (int num10 = 0; num10 < match.slots.Count; num10++)
						{
							PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot7 = match.slots[num10];
							representationSlot7.isGenerated = true;
							representationSlot7.itemColor = itemColor3;
						}
						bool flag4 = true;
						for (int num11 = 0; num11 < match.slots.Count; num11++)
						{
							PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot8 = match.slots[num11];
							this.matchingCheck.Check(board, representationSlot8.position, list5);
							if (this.matchingCheck.availableColors.Count == 0)
							{
								flag4 = false;
								break;
							}
						}
						if (flag4)
						{
							flag3 = true;
							break;
						}
					}
					if (!flag3)
					{
						for (int num12 = 0; num12 < match.slots.Count; num12++)
						{
							PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot9 = match.slots[num12];
							representationSlot9.isGenerated = true;
							representationSlot9.itemColor = representationSlot9.cachedColor;
						}
					}
					else
					{
						list6.Add(match);
						if (list6.Count >= maxPotentialMatches)
						{
							break;
						}
					}
				}
			}
			return true;
		}

		private bool IsIntersectingWithList(PopulateBoard.MatchBuilder.Match match, List<PopulateBoard.MatchBuilder.Match> list)
		{
			List<PopulateBoard.BoardRepresentation.RepresentationSlot> allSlots = match.allSlots;
			for (int i = 0; i < allSlots.Count; i++)
			{
				PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot = allSlots[i];
				for (int j = 0; j < list.Count; j++)
				{
					List<PopulateBoard.BoardRepresentation.RepresentationSlot> allSlots2 = list[j].allSlots;
					for (int k = 0; k < allSlots2.Count; k++)
					{
						if (allSlots2[k] == representationSlot)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private bool IsAvailableForSwap(PopulateBoard.BoardRepresentation.RepresentationSlot fromSlot, PopulateBoard.BoardRepresentation.RepresentationSlot slot)
		{
			return slot != null && fromSlot != null && !slot.isOutOfPlayArea && !fromSlot.IsBlockedTo(slot) && slot.canMove;
		}

		private bool IsInCanHaveAnyColor(PopulateBoard.BoardRepresentation.RepresentationSlot slot)
		{
			return slot != null && slot.isPositionInEmptyNeighbourhoodAtStart;
		}

		private void GenerateSlotInMatch(PopulateBoard.BoardRepresentation.RepresentationSlot slot)
		{
			UnityEngine.Debug.LogError("Can't generate slots that are free of matches");
			slot.itemColor = this.RandomColor();
			slot.isGenerated = true;
		}

		private List<PopulateBoard.BoardRepresentation.RepresentationSlot> mustEnsureNoMatching = new List<PopulateBoard.BoardRepresentation.RepresentationSlot>();

		private List<PopulateBoard.BoardRepresentation.RepresentationSlot> canHaveAnyColor = new List<PopulateBoard.BoardRepresentation.RepresentationSlot>();

		private List<PopulateBoard.BoardRepresentation.RepresentationSlot> canNotFormMatches = new List<PopulateBoard.BoardRepresentation.RepresentationSlot>();

		private List<IntVector2> cachedList = new List<IntVector2>();

		private PopulateBoard.Params initParams;

		private PopulateBoard.MatchingCheck matchingCheck = new PopulateBoard.MatchingCheck();

		private PopulateBoard.MatchBuilder matchBuilder = new PopulateBoard.MatchBuilder();

		public PopulateBoard.BoardRepresentation board = new PopulateBoard.BoardRepresentation();

		public class BoardRepresentation
		{
			public PopulateBoard.BoardRepresentation.RepresentationSlot GetSlot(IntVector2 pos)
			{
				if (pos.x < 0 || pos.y < 0 || pos.x >= this.size.x || pos.y >= this.size.y)
				{
					return null;
				}
				int index = pos.x + pos.y * this.size.x;
				return this.slots[index];
			}

			public List<IntVector2> slotNeighboursOffsets
			{
				get
				{
					if (this.slotNeighboursOffsets_ == null)
					{
						this.slotNeighboursOffsets_ = new List<IntVector2>(4);
						this.slotNeighboursOffsets_.Add(IntVector2.up);
						this.slotNeighboursOffsets_.Add(IntVector2.down);
						this.slotNeighboursOffsets_.Add(IntVector2.left);
						this.slotNeighboursOffsets_.Add(IntVector2.right);
					}
					return this.slotNeighboursOffsets_;
				}
			}

			public void Init(LevelDefinition level)
			{
				this.slots.Clear();
				this.size.x = level.size.width;
				this.size.y = level.size.height;
				List<LevelDefinition.SlotDefinition> list = level.slots;
				for (int i = 0; i < list.Count; i++)
				{
					LevelDefinition.SlotDefinition slotDefinition = list[i];
					PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot = new PopulateBoard.BoardRepresentation.RepresentationSlot();
					representationSlot.position = slotDefinition.position;
					if (slotDefinition.slotType == SlotType.Empty)
					{
						representationSlot.isOutOfPlayArea = true;
						this.slots.Add(representationSlot);
					}
					else
					{
						representationSlot.needsToBeGenerated = slotDefinition.needsToBeGenerated;
						representationSlot.itemColor = slotDefinition.itemColor;
						representationSlot.isFormingColorMatchesSuspended = slotDefinition.isFormingMatchesSuspended(level);
						representationSlot.canMove = !slotDefinition.isMoveSuspended(level);
						representationSlot.wallUp = slotDefinition.wallSettings.up;
						representationSlot.wallDown = slotDefinition.wallSettings.down;
						representationSlot.wallLeft = slotDefinition.wallSettings.left;
						representationSlot.wallRight = slotDefinition.wallSettings.right;
						this.slots.Add(representationSlot);
					}
				}
			}

			public void Init(Match3Game game, bool generateFlowerChips)
			{
				this.slots.Clear();
				Match3Board board = game.board;
				this.size = board.size;
				Slot[] array = board.slots;
				for (int i = 0; i < this.size.y; i++)
				{
					for (int j = 0; j < this.size.x; j++)
					{
						IntVector2 intVector = new IntVector2(j, i);
						Slot slot = board.GetSlot(intVector);
						PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot = new PopulateBoard.BoardRepresentation.RepresentationSlot();
						representationSlot.position = intVector;
						if (slot == null)
						{
							representationSlot.isOutOfPlayArea = true;
							this.slots.Add(representationSlot);
						}
						else
						{
							representationSlot.canMove = !slot.isSlotSwapSuspended;
							representationSlot.wallUp = Slot.IsPathBlockedBetween(slot, board.GetSlot(intVector + IntVector2.up));
							representationSlot.wallDown = Slot.IsPathBlockedBetween(slot, board.GetSlot(intVector + IntVector2.down));
							representationSlot.wallLeft = Slot.IsPathBlockedBetween(slot, board.GetSlot(intVector + IntVector2.left));
							representationSlot.wallRight = Slot.IsPathBlockedBetween(slot, board.GetSlot(intVector + IntVector2.right));
							representationSlot.isFormingColorMatchesSuspended = slot.isSlotMatchingSuspended;
							Chip slotComponent = slot.GetSlotComponent<Chip>();
							if (slotComponent == null)
							{
								representationSlot.needsToBeGenerated = false;
								representationSlot.itemColor = ItemColor.Unknown;
								this.slots.Add(representationSlot);
							}
							else
							{
								ChipType chipType = slotComponent.chipType;
								representationSlot.needsToBeGenerated = (chipType == ChipType.Chip);
								if (!generateFlowerChips && slotComponent.hasGrowingElement)
								{
									representationSlot.needsToBeGenerated = false;
								}
								representationSlot.itemColor = slotComponent.itemColor;
								representationSlot.isFormingColorMatchesSuspended = !slotComponent.canFormColorMatches;
								this.slots.Add(representationSlot);
							}
						}
					}
				}
			}

			public IntVector2 size;

			public List<PopulateBoard.BoardRepresentation.RepresentationSlot> slots = new List<PopulateBoard.BoardRepresentation.RepresentationSlot>();

			private List<IntVector2> slotNeighboursOffsets_;

			public class RepresentationSlot
			{
				private bool IsBlocked(IntVector2 direction)
				{
					return (direction.x < 0 && this.wallLeft) || (direction.x > 0 && this.wallRight) || (direction.y < 0 && this.wallDown) || (direction.y > 0 && this.wallUp);
				}

				public bool IsBlockedTo(PopulateBoard.BoardRepresentation.RepresentationSlot slot)
				{
					IntVector2 intVector = slot.position - this.position;
					return this.IsBlocked(intVector) || slot.IsBlocked(-intVector);
				}

				public bool canFormColorMatches
				{
					get
					{
						return !this.isFormingColorMatchesSuspended && (!this.needsToBeGenerated || this.isGenerated) && !this.isOutOfPlayArea;
					}
				}

				public bool needsToBeGenerated;

				public bool isGenerated;

				public ItemColor cachedColor;

				public bool wallLeft;

				public bool wallRight;

				public bool wallUp;

				public bool wallDown;

				public bool isFormingColorMatchesSuspended;

				public IntVector2 position;

				public ItemColor itemColor;

				public bool canMove;

				public bool isOutOfPlayArea;

				public bool isPositionInEmptyNeighbourhoodAtStart;

				public bool isMatchCheckingRequired;
			}
		}

		public class PotentialMatch
		{
			public PotentialMatch(PopulateBoard.BoardRepresentation.RepresentationSlot slot1, PopulateBoard.BoardRepresentation.RepresentationSlot slot2, PopulateBoard.BoardRepresentation.RepresentationSlot slot3)
			{
				this.slotsThatNeedToBeTheSame.Add(slot1);
				this.slotsThatNeedToBeTheSame.Add(slot2);
				this.slotsThatNeedToBeTheSame.Add(slot3);
			}

			public List<PopulateBoard.BoardRepresentation.RepresentationSlot> slotsThatNeedToBeTheSame = new List<PopulateBoard.BoardRepresentation.RepresentationSlot>();
		}

		public class Params
		{
			public List<ItemColor> availableColors = new List<ItemColor>();

			public int maxPotentialMatches = 4;

			public RandomProvider randomProvider;
		}

		public class MatchBuilder
		{
			public void Init(PopulateBoard.BoardRepresentation.RepresentationSlot slot1, PopulateBoard.BoardRepresentation.RepresentationSlot slot2, PopulateBoard.BoardRepresentation.RepresentationSlot slot3)
			{
				while (this.candidates.Count < 3)
				{
					this.candidates.Add(new PopulateBoard.MatchBuilder.SlotCandidate());
				}
				if (this.matchPatterns.Count < 3)
				{
					this.matchPatterns.Clear();
					this.matchPatterns.Add(new PopulateBoard.MatchBuilder.MatchPattern(0, 1, 2));
					this.matchPatterns.Add(new PopulateBoard.MatchBuilder.MatchPattern(1, 2, 0));
					this.matchPatterns.Add(new PopulateBoard.MatchBuilder.MatchPattern(0, 2, 1));
				}
				this.candidates[0].Init(slot1);
				this.candidates[1].Init(slot2);
				this.candidates[2].Init(slot3);
				for (int i = 0; i < this.matchPatterns.Count; i++)
				{
					this.matchPatterns[i].Clear();
				}
			}

			public bool Find(PopulateBoard.MatchingCheck matchCheck, PopulateBoard.BoardRepresentation board, List<ItemColor> availableColors)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < this.candidates.Count; i++)
				{
					PopulateBoard.MatchBuilder.SlotCandidate slotCandidate = this.candidates[i];
					if (slotCandidate.slot == null)
					{
						return false;
					}
					if (slotCandidate.slot.canFormColorMatches)
					{
						num++;
					}
					if (!slotCandidate.slot.canFormColorMatches && slotCandidate.slot.canMove)
					{
						num3++;
					}
					if (slotCandidate.slot.canMove)
					{
						num2++;
					}
				}
				if ((num != 2 || num3 != 1) && (num != 3 || num2 < 1))
				{
					return false;
				}
				this.FillAvailableColorsForAllCandidates(matchCheck, board, availableColors);
				bool result = false;
				for (int j = 0; j < this.matchPatterns.Count; j++)
				{
					PopulateBoard.MatchBuilder.MatchPattern matchPattern = this.matchPatterns[j];
					matchPattern.Init(this.candidates);
					matchPattern.FillAvailableColors();
					if (matchPattern.availableColors.Count != 0 && matchPattern.moving.slot.canMove)
					{
						matchPattern.FindSwipeSlot(board, matchCheck);
						if (matchPattern.swipeSlot != null)
						{
							result = true;
						}
					}
				}
				return result;
			}

			public void FillMatchesIn(List<PopulateBoard.MatchBuilder.Match> matchesList)
			{
				for (int i = 0; i < this.matchPatterns.Count; i++)
				{
					PopulateBoard.MatchBuilder.MatchPattern matchPattern = this.matchPatterns[i];
					if (matchPattern.isMatch)
					{
						PopulateBoard.MatchBuilder.Match match = new PopulateBoard.MatchBuilder.Match();
						for (int j = 0; j < matchPattern.matching.Count; j++)
						{
							PopulateBoard.MatchBuilder.SlotCandidate slotCandidate = matchPattern.matching[j];
							match.slots.Add(slotCandidate.slot);
						}
						match.slots.Add(matchPattern.swipeSlot);
						match.availableColor.AddRange(matchPattern.swipeSlotAvailableColors);
						matchesList.Add(match);
						match.movingSlot = matchPattern.moving.slot;
					}
				}
			}

			private void FillAvailableColorsForAllCandidates(PopulateBoard.MatchingCheck matchCheck, PopulateBoard.BoardRepresentation board, List<ItemColor> availableColors)
			{
				for (int i = 0; i < this.candidates.Count; i++)
				{
					PopulateBoard.MatchBuilder.SlotCandidate slotCandidate = this.candidates[i];
					if (slotCandidate.slot.needsToBeGenerated)
					{
						slotCandidate.slot.isGenerated = false;
					}
				}
				for (int j = 0; j < this.candidates.Count; j++)
				{
					PopulateBoard.MatchBuilder.SlotCandidate slotCandidate2 = this.candidates[j];
					if (!slotCandidate2.slot.isFormingColorMatchesSuspended)
					{
						if (!slotCandidate2.slot.needsToBeGenerated)
						{
							slotCandidate2.availableColors.Add(slotCandidate2.originalItemColor);
						}
						else
						{
							matchCheck.Check(board, slotCandidate2.slot.position, availableColors);
							slotCandidate2.availableColors.AddRange(matchCheck.availableColors);
						}
					}
				}
				for (int k = 0; k < this.candidates.Count; k++)
				{
					PopulateBoard.MatchBuilder.SlotCandidate slotCandidate3 = this.candidates[k];
					if (slotCandidate3.slot.needsToBeGenerated)
					{
						slotCandidate3.slot.isGenerated = slotCandidate3.originalIsGenerated;
					}
				}
			}

			public List<PopulateBoard.MatchBuilder.SlotCandidate> candidates = new List<PopulateBoard.MatchBuilder.SlotCandidate>();

			private List<PopulateBoard.MatchBuilder.MatchPattern> matchPatterns = new List<PopulateBoard.MatchBuilder.MatchPattern>();

			public class Match
			{
				public List<PopulateBoard.BoardRepresentation.RepresentationSlot> allSlots
				{
					get
					{
						this.allSlots_.Clear();
						this.allSlots_.AddRange(this.slots);
						this.allSlots_.Add(this.movingSlot);
						return this.allSlots_;
					}
				}

				public List<PopulateBoard.BoardRepresentation.RepresentationSlot> slots = new List<PopulateBoard.BoardRepresentation.RepresentationSlot>();

				public List<PopulateBoard.BoardRepresentation.RepresentationSlot> allSlots_ = new List<PopulateBoard.BoardRepresentation.RepresentationSlot>();

				public PopulateBoard.BoardRepresentation.RepresentationSlot movingSlot;

				public List<ItemColor> availableColor = new List<ItemColor>();
			}

			public class MatchPattern
			{
				public bool isMatch
				{
					get
					{
						return this.swipeSlot != null && this.swipeSlotAvailableColors.Count > 0;
					}
				}

				public MatchPattern(int match1Index, int match2Index, int movingIndex)
				{
					this.match1Index = match1Index;
					this.match2Index = match2Index;
					this.movingIndex = movingIndex;
				}

				public void Clear()
				{
					this.matching.Clear();
					this.availableColors.Clear();
					this.swipeSlot = null;
					this.swipeSlotAvailableColors.Clear();
				}

				public void Init(List<PopulateBoard.MatchBuilder.SlotCandidate> candidates)
				{
					this.Clear();
					this.matching.Add(candidates[this.match1Index]);
					this.matching.Add(candidates[this.match2Index]);
					this.moving = candidates[this.movingIndex];
				}

				public void FillAvailableColors()
				{
					List<ItemColor> a = this.matching[0].availableColors;
					List<ItemColor> b = this.matching[1].availableColors;
					GGUtil.Intersection<ItemColor>(a, b, this.availableColors);
					if (!this.moving.slot.canFormColorMatches)
					{
						return;
					}
					this.availableColors.Remove(this.moving.slot.itemColor);
				}

				private bool IsInList(PopulateBoard.BoardRepresentation.RepresentationSlot slot, List<PopulateBoard.MatchBuilder.SlotCandidate> list)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].slot == slot)
						{
							return true;
						}
					}
					return false;
				}

				public void FindSwipeSlot(PopulateBoard.BoardRepresentation board, PopulateBoard.MatchingCheck matchingCheck)
				{
					List<IntVector2> slotNeighboursOffsets = board.slotNeighboursOffsets;
					PopulateBoard.BoardRepresentation.RepresentationSlot slot = this.moving.slot;
					IntVector2 position = this.moving.slot.position;
					for (int i = 0; i < slotNeighboursOffsets.Count; i++)
					{
						IntVector2 b = slotNeighboursOffsets[i];
						IntVector2 pos = position + b;
						PopulateBoard.BoardRepresentation.RepresentationSlot slot2 = board.GetSlot(pos);
						if (slot2 != null && slot2.canFormColorMatches && slot2.canMove && !slot2.IsBlockedTo(slot) && !this.IsInList(slot2, this.matching))
						{
							this.swipeSlotAvailableColors.Clear();
							if (!slot2.needsToBeGenerated)
							{
								if (!this.availableColors.Contains(slot2.itemColor))
								{
									goto IL_F6;
								}
								this.swipeSlotAvailableColors.Add(slot2.itemColor);
							}
							else
							{
								matchingCheck.Check(board, pos, this.availableColors);
								this.swipeSlotAvailableColors.AddRange(matchingCheck.availableColors);
							}
							if (this.swipeSlotAvailableColors.Count != 0)
							{
								this.swipeSlot = slot2;
								return;
							}
						}
						IL_F6:;
					}
				}

				public List<PopulateBoard.MatchBuilder.SlotCandidate> matching = new List<PopulateBoard.MatchBuilder.SlotCandidate>();

				public PopulateBoard.MatchBuilder.SlotCandidate moving;

				public PopulateBoard.BoardRepresentation.RepresentationSlot swipeSlot;

				public List<ItemColor> availableColors = new List<ItemColor>();

				public List<ItemColor> swipeSlotAvailableColors = new List<ItemColor>();

				public int match1Index;

				public int match2Index;

				public int movingIndex;
			}

			public class SlotCandidate
			{
				public void Init(PopulateBoard.BoardRepresentation.RepresentationSlot slot)
				{
					this.slot = slot;
					this.availableColors.Clear();
					if (slot == null)
					{
						return;
					}
					this.originalItemColor = slot.itemColor;
					this.originalIsGenerated = slot.isGenerated;
				}

				public PopulateBoard.BoardRepresentation.RepresentationSlot slot;

				public List<ItemColor> availableColors = new List<ItemColor>();

				public ItemColor originalItemColor;

				public bool originalIsGenerated;
			}
		}

		public class MatchingCheck
		{
			public MatchingCheck()
			{
				this.Init();
			}

			private void Init()
			{
				this.matchingPositionsList.Clear();
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.left, IntVector2.right));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.up, IntVector2.down));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.up, 2 * IntVector2.up));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.down, 2 * IntVector2.down));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.left, 2 * IntVector2.left));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.right, 2 * IntVector2.right));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.up, IntVector2.right, IntVector2.up + IntVector2.right));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.up, IntVector2.left, IntVector2.up + IntVector2.left));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.down, IntVector2.right, IntVector2.down + IntVector2.right));
				this.matchingPositionsList.Add(new PopulateBoard.MatchingCheck.MatchPositionList(IntVector2.down, IntVector2.left, IntVector2.down + IntVector2.left));
			}

			private void Clear()
			{
				this.colorsThatWouldFormAMatch.Clear();
				this.availableColors.Clear();
			}

			private void AddColorThatWouldFormAMatch(ItemColor color)
			{
				if (this.colorsThatWouldFormAMatch.Contains(color))
				{
					return;
				}
				this.colorsThatWouldFormAMatch.Add(color);
				int num = this.availableColors.IndexOf(color);
				if (num >= 0)
				{
					this.availableColors.RemoveAt(num);
				}
			}

			public void Check(PopulateBoard.BoardRepresentation board, IntVector2 pos, List<ItemColor> availableColors)
			{
				this.Clear();
				this.board = board;
				this.originPos = pos;
				if (availableColors != null)
				{
					this.availableColors.AddRange(availableColors);
				}
				for (int i = 0; i < this.matchingPositionsList.Count; i++)
				{
					PopulateBoard.MatchingCheck.MatchingResult matchingResult = this.matchingPositionsList[i].GetMatchingResult(board, pos);
					if (matchingResult.isMatching)
					{
						this.AddColorThatWouldFormAMatch(matchingResult.itemColor);
					}
				}
			}

			public static bool IsMatching(PopulateBoard.BoardRepresentation.RepresentationSlot slot1, PopulateBoard.BoardRepresentation.RepresentationSlot slot2)
			{
				return slot1 != null && slot2 != null && slot1.canFormColorMatches && slot2.canFormColorMatches && slot1.itemColor == slot2.itemColor;
			}

			private PopulateBoard.BoardRepresentation board;

			private IntVector2 originPos;

			public List<ItemColor> colorsThatWouldFormAMatch = new List<ItemColor>();

			public List<ItemColor> availableColors = new List<ItemColor>();

			private List<PopulateBoard.MatchingCheck.MatchPositionList> matchingPositionsList = new List<PopulateBoard.MatchingCheck.MatchPositionList>();

			public struct MatchingResult
			{
				public bool isMatching;

				public ItemColor itemColor;
			}

			public class MatchPositionList
			{
				public MatchPositionList(IntVector2 pos1, IntVector2 pos2)
				{
					this.positionOffsets.Add(pos1);
					this.positionOffsets.Add(pos2);
				}

				public MatchPositionList(IntVector2 pos1, IntVector2 pos2, IntVector2 pos3)
				{
					this.positionOffsets.Add(pos1);
					this.positionOffsets.Add(pos2);
					this.positionOffsets.Add(pos3);
				}

				public PopulateBoard.MatchingCheck.MatchingResult GetMatchingResult(PopulateBoard.BoardRepresentation board, IntVector2 originPos)
				{
					PopulateBoard.MatchingCheck.MatchingResult result = default(PopulateBoard.MatchingCheck.MatchingResult);
					PopulateBoard.BoardRepresentation.RepresentationSlot representationSlot = null;
					for (int i = 0; i < this.positionOffsets.Count; i++)
					{
						PopulateBoard.BoardRepresentation.RepresentationSlot slot = board.GetSlot(originPos + this.positionOffsets[i]);
						if (slot == null)
						{
							return result;
						}
						if (!slot.canFormColorMatches)
						{
							return result;
						}
						if (representationSlot == null)
						{
							representationSlot = slot;
						}
						else if (!PopulateBoard.MatchingCheck.IsMatching(representationSlot, slot))
						{
							return result;
						}
					}
					if (representationSlot == null)
					{
						return result;
					}
					result.isMatching = true;
					result.itemColor = representationSlot.itemColor;
					return result;
				}

				public List<IntVector2> positionOffsets = new List<IntVector2>();
			}
		}
	}
}
