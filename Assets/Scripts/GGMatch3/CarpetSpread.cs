using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class CarpetSpread
	{
		public bool isCarpetPossible
		{
			get
			{
				return this.slotsWithCarpet > 0;
			}
		}

		public bool HasCarpet(IntVector2 position)
		{
			return position.x < this.game.board.size.x && position.x >= 0 && position.y < this.game.board.size.y && position.y >= 0 && this.slots[this.game.board.Index(position)].hasCarpet;
		}

		public void AddToGoalsAtStart(Match3Goals goals)
		{
			if (this.slotsWithCarpet == 0)
			{
				return;
			}
			Match3Goals.ChipTypeCounter chipTypeCounter = goals.GetChipTypeCounter(new Match3Goals.ChipTypeDef
			{
				chipType = ChipType.Carpet,
				itemColor = ItemColor.Unknown
			});
			for (int i = 0; i < this.slots.Length; i++)
			{
				CarpetSpread.SlotData slotData = this.slots[i];
				if (!slotData.isCarpetSuspended && !slotData.hasCarpet)
				{
					chipTypeCounter.countAtStart++;
				}
			}
		}

		public void Init(Match3Game game, LevelDefinition level)
		{
			this.game = game;
			this.slots = new CarpetSpread.SlotData[game.board.slots.Length];
			List<LevelDefinition.SlotDefinition> list = level.slots;
			for (int i = 0; i < this.slots.Length; i++)
			{
				CarpetSpread.SlotData[] array = this.slots;
				this.slots[i].position = game.board.PositionFromIndex(i);
			}
			for (int j = 0; j < list.Count; j++)
			{
				LevelDefinition.SlotDefinition slotDefinition = list[j];
				if (!slotDefinition.isPartOfConveyor)
				{
					if (slotDefinition.hasCarpet)
					{
						this.AddCarpetToSlot(slotDefinition.position);
					}
					else if (slotDefinition.slotType == SlotType.PlayingSpace)
					{
						this.slots[game.board.Index(slotDefinition.position)].isCarpetPossible = true;
					}
				}
			}
			this.carpetBehaviour = game.CreateCarpetSpread();
			if (this.carpetBehaviour != null)
			{
				this.carpetBehaviour.transform.localPosition = Vector3.zero;
				this.carpetBehaviour.transform.localScale = Vector3.one;
				GGUtil.SetActive(this.carpetBehaviour, true);
				this.carpetBehaviour.Init(game, this);
				this.RefreshVisually();
			}
		}

		private void RefreshVisually()
		{
			if (this.carpetBehaviour == null)
			{
				return;
			}
			this.carpetBehaviour.RefreshCarpet();
		}

		public bool IsPossibleToAddCarpet(IntVector2 slotPosition)
		{
			if (!this.isCarpetPossible)
			{
				return false;
			}
			int num = this.game.board.Index(slotPosition);
			CarpetSpread.SlotData slotData = this.slots[num];
			return !slotData.hasCarpet && !slotData.isCarpetSuspended;
		}

		public void AddCarpetFromGame(IntVector2 slotPosition)
		{
			int num = this.game.board.Index(slotPosition);
			CarpetSpread.SlotData slotData = this.slots[num];
			if (slotData.hasCarpet || slotData.isCarpetSuspended)
			{
				return;
			}
			this.slots[num].hasCarpet = true;
			Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
			chipTypeDef.chipType = ChipType.Carpet;
			chipTypeDef.itemColor = ItemColor.Unknown;
			Match3Goals.GoalBase activeGoal = this.game.goals.GetActiveGoal(chipTypeDef);
			if (activeGoal != null)
			{
				this.game.OnPickupGoal(new GoalCollectParams(activeGoal, null));
			}
			this.RefreshVisually();
		}

		private void AddCarpetToSlot(IntVector2 slotPosition)
		{
			int num = this.game.board.Index(slotPosition);
			CarpetSpread.SlotData slotData = this.slots[num];
			if (slotData.hasCarpet)
			{
				return;
			}
			slotData.hasCarpet = true;
			this.slotsWithCarpet++;
			this.slots[num] = slotData;
		}

		private int slotsWithCarpet;

		public CarpetSpread.SlotData[] slots;

		private CarpetSpreadBehaviour carpetBehaviour;

		private Match3Game game;

		public struct SlotData
		{
			public bool isCarpetSuspended
			{
				get
				{
					return !this.isCarpetPossible;
				}
			}

			public IntVector2 position;

			public bool hasCarpet;

			public bool isCarpetPossible;
		}
	}
}
