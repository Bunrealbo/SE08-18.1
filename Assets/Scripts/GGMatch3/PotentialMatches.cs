using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class PotentialMatches
	{
		public List<PotentialMatches.CompoundSlotsSet> FilterForTypeCompleatingGoals(Match3Game game)
		{
			this.filteredList.Clear();
			Match3Goals goals = game.goals;
			for (int i = 0; i < this.matchesList.Count; i++)
			{
				PotentialMatches.CompoundSlotsSet compoundSlotsSet = this.matchesList[i];
				if (compoundSlotsSet.GetActionScore(game, game.goals).goalsCount > 0)
				{
					this.filteredList.Add(compoundSlotsSet);
				}
			}
			return this.filteredList;
		}

		public List<PotentialMatches.CompoundSlotsSet> FilterForType(PotentialMatches.CompoundSlotsSet.MatchType matchType)
		{
			this.filteredList.Clear();
			for (int i = 0; i < this.matchesList.Count; i++)
			{
				PotentialMatches.CompoundSlotsSet compoundSlotsSet = this.matchesList[i];
				if (compoundSlotsSet.matchType == matchType)
				{
					this.filteredList.Add(compoundSlotsSet);
				}
			}
			return this.filteredList;
		}

		public int MatchesCount
		{
			get
			{
				return this.matchesList.Count;
			}
		}

		private void Clear()
		{
			this.slotSetPool.AddRange(this.setsThatCanFormMatches);
			this.slotSetPool.AddRange(this.setsThatAreInMatch);
			this.setsThatCanFormMatches.Clear();
			this.setsThatAreInMatch.Clear();
			this.compoundPool.AddRange(this.matchesList);
			this.matchesList.Clear();
		}

		private PotentialMatches.CompoundSlotsSet NextCompound()
		{
			PotentialMatches.CompoundSlotsSet compoundSlotsSet = null;
			if (this.compoundPool.Count > 0)
			{
				int index = this.compoundPool.Count - 1;
				compoundSlotsSet = this.compoundPool[index];
				this.compoundPool.RemoveAt(index);
			}
			if (compoundSlotsSet == null)
			{
				compoundSlotsSet = new PotentialMatches.CompoundSlotsSet();
			}
			compoundSlotsSet.Clear();
			return compoundSlotsSet;
		}

		private PotentialMatches.SlotsSet NextSlotsSet()
		{
			PotentialMatches.SlotsSet slotsSet = null;
			if (this.slotSetPool.Count > 0)
			{
				int index = this.slotSetPool.Count - 1;
				slotsSet = this.slotSetPool[index];
				this.slotSetPool.RemoveAt(index);
				return slotsSet;
			}
			if (slotsSet == null)
			{
				slotsSet = new PotentialMatches.SlotsSet();
			}
			return slotsSet;
		}

		private void AddMatch(PotentialMatches.CompoundSlotsSet c)
		{
			PotentialMatches.CompoundSlotsSet compoundSlotsSet = this.NextCompound();
			compoundSlotsSet.CopyFrom(c);
			this.matchesList.Add(compoundSlotsSet);
		}

		private bool IsPartOfMatchList(IntVector2 positionOfSlotMissingForMatch, IntVector2 positionToSwipeFrom)
		{
			for (int i = 0; i < this.matchesList.Count; i++)
			{
				PotentialMatches.CompoundSlotsSet compoundSlotsSet = this.matchesList[i];
				if (!(compoundSlotsSet.swipeSlot.position != positionToSwipeFrom) && !(compoundSlotsSet.positionOfSlotMissingForMatch != positionOfSlotMissingForMatch))
				{
					return true;
				}
			}
			return false;
		}

		private void AddSetThatCanFormMatches(PotentialMatches.SlotsSet slotsSet)
		{
			PotentialMatches.SlotsSet slotsSet2 = this.NextSlotsSet();
			slotsSet2.CopyFrom(slotsSet);
			this.setsThatCanFormMatches.Add(slotsSet2);
		}

		private void AddSetThatIsInMatch(PotentialMatches.SlotsSet slotsSet)
		{
			PotentialMatches.SlotsSet slotsSet2 = this.NextSlotsSet();
			slotsSet2.CopyFrom(slotsSet);
			this.setsThatAreInMatch.Add(slotsSet2);
		}

		public bool IsPartOfMatch(BoardRepresentation.RepresentationSlot slot)
		{
			for (int i = 0; i < this.setsThatAreInMatch.Count; i++)
			{
				if (this.setsThatAreInMatch[i].MatchingSlotsContains(slot))
				{
					return true;
				}
			}
			return false;
		}

		private PotentialMatches.SlotsSet FillLineSet(BoardRepresentation board, IntVector2 pos, IntVector2 direction, PotentialMatches.SlotsSet.ConnectionType connectionType)
		{
			PotentialMatches.SlotsSet slotsSet = this.searchingSlotSet;
			slotsSet.Clear(connectionType);
			for (int i = 0; i < 3; i++)
			{
				BoardRepresentation.RepresentationSlot slot = board.GetSlot(pos + i * direction);
				if (this.IsPartOfMatch(slot))
				{
					slotsSet.Clear(connectionType);
					return slotsSet;
				}
				slotsSet.AddToAllSlots(slot);
			}
			slotsSet.SortSlotsUsingDominantColor();
			if (slotsSet.isMatch)
			{
				int num = this.MatchLength(board, pos, direction);
				slotsSet.Clear(connectionType);
				for (int j = 0; j < num; j++)
				{
					IntVector2 pos2 = pos + direction * j;
					slotsSet.AddSlot(board.GetSlot(pos2));
				}
				return slotsSet;
			}
			if (!slotsSet.isPotentialMatch)
			{
				return slotsSet;
			}
			slotsSet.FillSlotsThatCanSwipeToMatch(this, board);
			return slotsSet;
		}

		private PotentialMatches.SlotsSet FillSquareSet(BoardRepresentation board, IntVector2 pos)
		{
			PotentialMatches.SlotsSet.ConnectionType connectionType = PotentialMatches.SlotsSet.ConnectionType.Square;
			PotentialMatches.SlotsSet slotsSet = this.searchingSlotSet;
			slotsSet.Clear(connectionType);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					BoardRepresentation.RepresentationSlot slot = board.GetSlot(pos + new IntVector2(i, j));
					if (this.IsPartOfMatch(slot))
					{
						slotsSet.Clear(connectionType);
						return slotsSet;
					}
					slotsSet.AddToAllSlots(slot);
				}
			}
			slotsSet.SortSlotsUsingDominantColor();
			if (slotsSet.isMatch)
			{
				return slotsSet;
			}
			if (!slotsSet.isPotentialMatch)
			{
				return slotsSet;
			}
			slotsSet.FillSlotsThatCanSwipeToMatch(this, board);
			return slotsSet;
		}

		public void FindPotentialMatches(Match3Game match3Game)
		{
			this.Clear();
			this.board.Init(match3Game);
			IntVector2 size = this.board.size;
			PotentialMatches.SlotsSet slotsSet = this.searchingSlotSet;
			for (int i = 0; i < size.y; i++)
			{
				for (int j = 0; j < size.x; j++)
				{
					IntVector2 pos = new IntVector2(j, i);
					IntVector2 right = IntVector2.right;
					PotentialMatches.SlotsSet slotsSet2 = this.FillLineSet(this.board, pos, right, PotentialMatches.SlotsSet.ConnectionType.Horizontal);
					if (slotsSet2.isMatch)
					{
						this.AddSetThatIsInMatch(slotsSet2);
					}
					else if (slotsSet2.isMatchWhenSwipe)
					{
						this.AddSetThatCanFormMatches(slotsSet2);
					}
				}
			}
			for (int k = 0; k < size.x; k++)
			{
				for (int l = 0; l < size.y; l++)
				{
					IntVector2 pos2 = new IntVector2(k, l);
					IntVector2 up = IntVector2.up;
					PotentialMatches.SlotsSet slotsSet3 = this.FillLineSet(this.board, pos2, up, PotentialMatches.SlotsSet.ConnectionType.Vertical);
					if (slotsSet3.isMatch)
					{
						this.AddSetThatIsInMatch(slotsSet3);
					}
					else if (slotsSet3.isMatchWhenSwipe)
					{
						this.AddSetThatCanFormMatches(slotsSet3);
					}
				}
			}
			for (int m = 0; m < size.x; m++)
			{
				for (int n = 0; n < size.y; n++)
				{
					IntVector2 pos3 = new IntVector2(m, n);
					PotentialMatches.SlotsSet slotsSet4 = this.FillSquareSet(this.board, pos3);
					if (slotsSet4.isMatch)
					{
						this.AddSetThatIsInMatch(slotsSet4);
					}
					else if (slotsSet4.isMatchWhenSwipe)
					{
						this.AddSetThatCanFormMatches(slotsSet4);
					}
				}
			}
			PotentialMatches.CompoundSlotsSet compoundSlotsSet = this.searchingCompoundSlotsSet;
			for (int num = 0; num < this.setsThatCanFormMatches.Count; num++)
			{
				PotentialMatches.SlotsSet slotsSet5 = this.setsThatCanFormMatches[num];
				for (int num2 = 0; num2 < slotsSet5.slotsThatCanSwipeToMatch.Count; num2++)
				{
					BoardRepresentation.RepresentationSlot representationSlot = slotsSet5.slotsThatCanSwipeToMatch[num2];
					if (!this.IsPartOfMatchList(slotsSet5.positionOfSlotMissingForMatch, representationSlot.position))
					{
						compoundSlotsSet.Clear();
						compoundSlotsSet.swipeSlot = representationSlot;
						compoundSlotsSet.Add(slotsSet5);
						for (int num3 = num + 1; num3 < this.setsThatCanFormMatches.Count; num3++)
						{
							PotentialMatches.SlotsSet slotsSet6 = this.setsThatCanFormMatches[num3];
							if (slotsSet5 != slotsSet6 && compoundSlotsSet.IsAcceptable(slotsSet6, representationSlot))
							{
								compoundSlotsSet.Add(slotsSet6);
							}
						}
						this.AddMatch(compoundSlotsSet);
					}
				}
			}
		}

		private int MatchLength(BoardRepresentation board, IntVector2 pos, IntVector2 direction)
		{
			int num = 1;
			BoardRepresentation.RepresentationSlot slot = board.GetSlot(pos);
			if (!slot.canFormColorMatches)
			{
				return num;
			}
			ItemColor itemColor = slot.itemColor;
			for (;;)
			{
				pos += direction;
				BoardRepresentation.RepresentationSlot slot2 = board.GetSlot(pos);
				if (slot2.isOutsideBoard || !slot2.canFormColorMatches || slot2.itemColor != itemColor)
				{
					break;
				}
				num++;
			}
			return num;
		}

		public List<PotentialMatches.SlotsSet> slotSetPool = new List<PotentialMatches.SlotsSet>();

		public List<PotentialMatches.CompoundSlotsSet> compoundPool = new List<PotentialMatches.CompoundSlotsSet>();

		private List<PotentialMatches.CompoundSlotsSet> filteredList = new List<PotentialMatches.CompoundSlotsSet>();

		public List<PotentialMatches.CompoundSlotsSet> matchesList = new List<PotentialMatches.CompoundSlotsSet>();

		public BoardRepresentation board = new BoardRepresentation();

		public List<PotentialMatches.SlotsSet> setsThatCanFormMatches = new List<PotentialMatches.SlotsSet>();

		public List<PotentialMatches.SlotsSet> setsThatAreInMatch = new List<PotentialMatches.SlotsSet>();

		private PotentialMatches.SlotsSet searchingSlotSet = new PotentialMatches.SlotsSet();

		private PotentialMatches.CompoundSlotsSet searchingCompoundSlotsSet = new PotentialMatches.CompoundSlotsSet();

		public class CompoundSlotsSet
		{
			public bool HasCarpet(Match3Game game)
			{
				for (int i = 0; i < this.slotsSets.Count; i++)
				{
					if (this.slotsSets[i].HasCarpet(game))
					{
						return true;
					}
				}
				return false;
			}

			public ActionScore GetActionScore(Match3Game game, Match3Goals goals)
			{
				ActionScore actionScore = default(ActionScore);
				PotentialMatches.CompoundSlotsSet.MatchType matchType = this.matchType;
				if (matchType == PotentialMatches.CompoundSlotsSet.MatchType.DiscoBall || matchType == PotentialMatches.CompoundSlotsSet.MatchType.Bomb || matchType == PotentialMatches.CompoundSlotsSet.MatchType.Rocket || matchType == PotentialMatches.CompoundSlotsSet.MatchType.SeekingMissle)
				{
					actionScore.powerupsCreated++;
				}
				bool isHavingCarpet = this.HasCarpet(game);
				for (int i = 0; i < this.slotsSets.Count; i++)
				{
					PotentialMatches.SlotsSet slotsSet = this.slotsSets[i];
					Slot slot = game.GetSlot(slotsSet.positionOfSlotMissingForMatch);
					Slot slot2 = game.GetSlot(this.swipeSlot.position);
					actionScore += goals.ActionScoreForDestroyingSwitchingSlots(slot, slot2, game, isHavingCarpet, true);
					List<BoardRepresentation.RepresentationSlot> sameColorSlots = slotsSet.sameColorSlots;
					for (int j = 0; j < sameColorSlots.Count; j++)
					{
						BoardRepresentation.RepresentationSlot representationSlot = sameColorSlots[j];
						Slot slot3 = game.GetSlot(representationSlot.position);
						actionScore += goals.ActionScoreForDestroyingSlot(slot3, game, isHavingCarpet, true);
					}
				}
				return actionScore;
			}

			public void CopyFrom(PotentialMatches.CompoundSlotsSet c)
			{
				this.slotsSets.Clear();
				this.slotsSets.AddRange(c.slotsSets);
				this.swipeSlot = c.swipeSlot;
			}

			public IntVector2 positionOfSlotMissingForMatch
			{
				get
				{
					return this.slotsSets[0].positionOfSlotMissingForMatch;
				}
			}

			public int CountOfType(PotentialMatches.SlotsSet.ConnectionType connectionType)
			{
				int num = 0;
				for (int i = 0; i < this.slotsSets.Count; i++)
				{
					if (this.slotsSets[i].connectionType == connectionType)
					{
						num++;
					}
				}
				return num;
			}

			public ChipType createdPowerup
			{
				get
				{
					if (this.isDiscoBall)
					{
						return ChipType.DiscoBall;
					}
					if (this.isBomb)
					{
						return ChipType.Bomb;
					}
					if (this.isRocket)
					{
						if (this.CountOfType(PotentialMatches.SlotsSet.ConnectionType.Horizontal) >= 2)
						{
							return ChipType.HorizontalRocket;
						}
						return ChipType.VerticalRocket;
					}
					else
					{
						if (this.isSeekingMissle)
						{
							return ChipType.SeekingMissle;
						}
						return ChipType.RandomChip;
					}
				}
			}

			public PotentialMatches.CompoundSlotsSet.MatchType matchType
			{
				get
				{
					if (this.isDiscoBall)
					{
						return PotentialMatches.CompoundSlotsSet.MatchType.DiscoBall;
					}
					if (this.isBomb)
					{
						return PotentialMatches.CompoundSlotsSet.MatchType.Bomb;
					}
					if (this.isRocket)
					{
						return PotentialMatches.CompoundSlotsSet.MatchType.Rocket;
					}
					if (this.isSeekingMissle)
					{
						return PotentialMatches.CompoundSlotsSet.MatchType.SeekingMissle;
					}
					return PotentialMatches.CompoundSlotsSet.MatchType.Match;
				}
			}

			private bool isDiscoBall
			{
				get
				{
					return this.CountOfType(PotentialMatches.SlotsSet.ConnectionType.Horizontal) >= 3 || this.CountOfType(PotentialMatches.SlotsSet.ConnectionType.Vertical) >= 3;
				}
			}

			private bool isBomb
			{
				get
				{
					return this.CountOfType(PotentialMatches.SlotsSet.ConnectionType.Horizontal) >= 1 && this.CountOfType(PotentialMatches.SlotsSet.ConnectionType.Vertical) >= 1;
				}
			}

			private bool isRocket
			{
				get
				{
					return this.CountOfType(PotentialMatches.SlotsSet.ConnectionType.Horizontal) >= 2 || this.CountOfType(PotentialMatches.SlotsSet.ConnectionType.Vertical) >= 2;
				}
			}

			private bool isSeekingMissle
			{
				get
				{
					return this.CountOfType(PotentialMatches.SlotsSet.ConnectionType.Square) > 0;
				}
			}

			public bool IsAcceptable(PotentialMatches.SlotsSet otherSlotSet, BoardRepresentation.RepresentationSlot slotToSwipe)
			{
				if (this.slotsSets.Count == 0)
				{
					return true;
				}
				if (otherSlotSet.connectionType == PotentialMatches.SlotsSet.ConnectionType.Square)
				{
					return false;
				}
				PotentialMatches.SlotsSet slotsSet = this.slotsSets[0];
				return slotsSet.itemColor == otherSlotSet.itemColor && !(slotsSet.positionOfSlotMissingForMatch != otherSlotSet.positionOfSlotMissingForMatch) && otherSlotSet.SwipeSlotsContains(slotToSwipe);
			}

			public void Add(PotentialMatches.SlotsSet slotsSet)
			{
				this.slotsSets.Add(slotsSet);
			}

			public void Clear()
			{
				this.slotsSets.Clear();
			}

			public List<PotentialMatches.SlotsSet> slotsSets = new List<PotentialMatches.SlotsSet>();

			public BoardRepresentation.RepresentationSlot swipeSlot;

			private List<PotentialMatches.CompoundSlotsSet.SlotsCount> slotsCount = new List<PotentialMatches.CompoundSlotsSet.SlotsCount>();

			private List<BoardRepresentation.RepresentationSlot> slotsThatCanSwipe_ = new List<BoardRepresentation.RepresentationSlot>();

			public enum MatchType
			{
				Match,
				DiscoBall,
				Bomb,
				Rocket,
				SeekingMissle,
				CompleatingGoals
			}

			public struct SlotsCount
			{
				public int count;

				public BoardRepresentation.RepresentationSlot slot;
			}
		}

		public class SlotsSet
		{
			public bool HasCarpet(Match3Game game)
			{
				for (int i = 0; i < this.allSlots.Count; i++)
				{
					BoardRepresentation.RepresentationSlot representationSlot = this.allSlots[i];
					Slot slot = game.GetSlot(representationSlot.position);
					if (slot != null)
					{
						bool flag = false;
						for (int j = 0; j < this.slotsThatCanSwipeToMatch.Count; j++)
						{
							if (this.slotsThatCanSwipeToMatch[j].position == representationSlot.position)
							{
								flag = true;
								break;
							}
						}
						if (!flag && slot.canCarpetSpreadFromHere)
						{
							return true;
						}
					}
				}
				return false;
			}

			public void CopyFrom(PotentialMatches.SlotsSet s)
			{
				this.sameColorSlots.Clear();
				this.differentColorSlots.Clear();
				this.slotsThatCanSwipeToMatch.Clear();
				this.allSlots.Clear();
				this.colorCount.Clear();
				this.sameColorSlots.AddRange(s.sameColorSlots);
				this.differentColorSlots.AddRange(s.differentColorSlots);
				this.slotsThatCanSwipeToMatch.AddRange(s.slotsThatCanSwipeToMatch);
				this.connectionType = s.connectionType;
				this.allSlots.AddRange(s.allSlots);
				this.colorCount.AddRange(s.colorCount);
			}

			public ItemColor itemColor
			{
				get
				{
					if (this.sameColorSlots.Count == 0)
					{
						return ItemColor.Unknown;
					}
					return this.sameColorSlots[0].itemColor;
				}
			}

			public IntVector2 positionOfSlotMissingForMatch
			{
				get
				{
					return this.differentColorSlots[0].position;
				}
			}

			public bool isMatch
			{
				get
				{
					return this.sameColorSlots.Count >= 3 && this.differentColorSlots.Count == 0;
				}
			}

			public bool isPotentialMatch
			{
				get
				{
					if (this.sameColorSlots.Count < 2)
					{
						return false;
					}
					int num = 1;
					return this.differentColorSlots.Count == num && this.differentColorSlots[0].canMove;
				}
			}

			public bool isMatchWhenSwipe
			{
				get
				{
					return this.slotsThatCanSwipeToMatch.Count > 0;
				}
			}

			public void AddToAllSlots(BoardRepresentation.RepresentationSlot slot)
			{
				if (!slot.canFormColorMatches)
				{
					this.differentColorSlots.Add(slot);
					return;
				}
				this.allSlots.Add(slot);
			}

			public void SortSlotsUsingDominantColor()
			{
				PotentialMatches.SlotsSet.ColorCount dominantColorCount = this.DominantColorCount;
				for (int i = 0; i < this.allSlots.Count; i++)
				{
					BoardRepresentation.RepresentationSlot representationSlot = this.allSlots[i];
					if (representationSlot.itemColor == dominantColorCount.color)
					{
						this.sameColorSlots.Add(representationSlot);
					}
					else
					{
						this.differentColorSlots.Add(representationSlot);
					}
				}
			}

			private void IncrementColorCount(ItemColor color)
			{
				for (int i = 0; i < this.colorCount.Count; i++)
				{
					PotentialMatches.SlotsSet.ColorCount colorCount = this.colorCount[i];
					if (colorCount.color == color)
					{
						colorCount.count++;
						this.colorCount[i] = colorCount;
						break;
					}
				}
				PotentialMatches.SlotsSet.ColorCount item = default(PotentialMatches.SlotsSet.ColorCount);
				item.count = 1;
				item.color = color;
				this.colorCount.Add(item);
			}

			public PotentialMatches.SlotsSet.ColorCount DominantColorCount
			{
				get
				{
					this.colorCount.Clear();
					for (int i = 0; i < this.allSlots.Count; i++)
					{
						BoardRepresentation.RepresentationSlot representationSlot = this.allSlots[i];
						this.IncrementColorCount(representationSlot.itemColor);
					}
					PotentialMatches.SlotsSet.ColorCount colorCount = default(PotentialMatches.SlotsSet.ColorCount);
					for (int j = 0; j < this.colorCount.Count; j++)
					{
						PotentialMatches.SlotsSet.ColorCount colorCount2 = this.colorCount[j];
						if (colorCount2.count > colorCount.count)
						{
							colorCount = colorCount2;
						}
					}
					return colorCount;
				}
			}

			public void AddSlot(BoardRepresentation.RepresentationSlot slot)
			{
				if (this.sameColorSlots.Count == 0)
				{
					this.sameColorSlots.Add(slot);
					return;
				}
				BoardRepresentation.RepresentationSlot representationSlot = this.sameColorSlots[0];
				if (slot.canFormColorMatches && representationSlot.itemColor == slot.itemColor)
				{
					this.sameColorSlots.Add(slot);
					return;
				}
				this.differentColorSlots.Add(slot);
			}

			public void Clear(PotentialMatches.SlotsSet.ConnectionType connectionType)
			{
				this.connectionType = connectionType;
				this.sameColorSlots.Clear();
				this.differentColorSlots.Clear();
				this.slotsThatCanSwipeToMatch.Clear();
				this.allSlots.Clear();
				this.colorCount.Clear();
			}

			public bool SwipeSlotsContains(BoardRepresentation.RepresentationSlot slot)
			{
				for (int i = 0; i < this.slotsThatCanSwipeToMatch.Count; i++)
				{
					if (this.slotsThatCanSwipeToMatch[i].position == slot.position)
					{
						return true;
					}
				}
				return false;
			}

			public bool MatchingSlotsContains(BoardRepresentation.RepresentationSlot slot)
			{
				for (int i = 0; i < this.sameColorSlots.Count; i++)
				{
					if (this.sameColorSlots[i].position == slot.position)
					{
						return true;
					}
				}
				return false;
			}

			private void TryAddSlotThatCanSwipeToMatch(PotentialMatches potentialMatches, BoardRepresentation.RepresentationSlot fromSlot, BoardRepresentation.RepresentationSlot slot, ItemColor desiredItemColor)
			{
				if (!slot.canFormColorMatches)
				{
					return;
				}
				if (!slot.canMove)
				{
					return;
				}
				if (!fromSlot.canMove)
				{
					return;
				}
				if (fromSlot.IsBlockedTo(slot))
				{
					return;
				}
				if (slot.itemColor != desiredItemColor)
				{
					return;
				}
				if (this.MatchingSlotsContains(slot))
				{
					return;
				}
				if (potentialMatches.IsPartOfMatch(slot))
				{
					return;
				}
				this.slotsThatCanSwipeToMatch.Add(slot);
			}

			public void FillSlotsThatCanSwipeToMatch(PotentialMatches potentialMatches, BoardRepresentation board)
			{
				BoardRepresentation.RepresentationSlot ptr = this.sameColorSlots[0];
				BoardRepresentation.RepresentationSlot representationSlot = this.differentColorSlots[0];
				ItemColor itemColor = ptr.itemColor;
				this.TryAddSlotThatCanSwipeToMatch(potentialMatches, representationSlot, board.GetSlot(representationSlot.position + IntVector2.right), itemColor);
				this.TryAddSlotThatCanSwipeToMatch(potentialMatches, representationSlot, board.GetSlot(representationSlot.position + IntVector2.left), itemColor);
				this.TryAddSlotThatCanSwipeToMatch(potentialMatches, representationSlot, board.GetSlot(representationSlot.position + IntVector2.up), itemColor);
				this.TryAddSlotThatCanSwipeToMatch(potentialMatches, representationSlot, board.GetSlot(representationSlot.position + IntVector2.down), itemColor);
			}

			public List<BoardRepresentation.RepresentationSlot> sameColorSlots = new List<BoardRepresentation.RepresentationSlot>();

			public List<BoardRepresentation.RepresentationSlot> differentColorSlots = new List<BoardRepresentation.RepresentationSlot>();

			public List<BoardRepresentation.RepresentationSlot> slotsThatCanSwipeToMatch = new List<BoardRepresentation.RepresentationSlot>();

			public List<BoardRepresentation.RepresentationSlot> allSlots = new List<BoardRepresentation.RepresentationSlot>();

			public List<PotentialMatches.SlotsSet.ColorCount> colorCount = new List<PotentialMatches.SlotsSet.ColorCount>();

			public PotentialMatches.SlotsSet.ConnectionType connectionType;

			public struct ColorCount
			{
				public ItemColor color;

				public int count;
			}

			public enum ConnectionType
			{
				Vertical,
				Horizontal,
				Square
			}
		}
	}
}
