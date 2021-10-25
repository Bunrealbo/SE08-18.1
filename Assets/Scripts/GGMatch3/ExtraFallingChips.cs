using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class ExtraFallingChips
	{
		private int generatedFallingChips
		{
			get
			{
				return this.createdList.Count;
			}
		}

		public int LastSlotCreatedChipsNum(Slot slot)
		{
			int num = -1;
			for (int i = 0; i < this.createdList.Count; i++)
			{
				ExtraFallingChips.FallingChipCreation fallingChipCreation = this.createdList[i];
				if (fallingChipCreation.slot == slot)
				{
					num = Mathf.Max(fallingChipCreation.slotCreatedChipsNum, num);
				}
			}
			return num;
		}

		public void Init(ExtraFallingElements extraFallingElements)
		{
			this.setup = extraFallingElements;
		}

		private int GeneratedFallingChipsForTag(int chipTag)
		{
			int num = 0;
			for (int i = 0; i < this.createdList.Count; i++)
			{
				if (this.createdList[i].chipTag == chipTag)
				{
					num++;
				}
			}
			return num;
		}

		private int PickedUpChipsForTag(int chipTag)
		{
			int num = 0;
			for (int i = 0; i < this.pickedUpList.Count; i++)
			{
				if (this.pickedUpList[i].chipTag == chipTag)
				{
					num++;
				}
			}
			return num;
		}

		public bool ShouldGenerateExtraFallingChip(Slot slot)
		{
			if (this.setup == null)
			{
				return false;
			}
			Match3Game game = slot.game;
			int chipTag = slot.generatorSettings.chipTag;
			if (this.PickedUpChipsForTag(chipTag) <= this.GeneratedFallingChipsForTag(chipTag))
			{
				return false;
			}
			if (this.generatedFallingChips >= this.setup.fallingElementsList.Count)
			{
				return false;
			}
			ExtraFallingElements.ExtraFallingElement extraFallingElement = this.setup.fallingElementsList[this.generatedFallingChips];
			if (extraFallingElement.minMovesBeforeActive > game.board.userMovesCount)
			{
				return false;
			}
			int num = this.LastSlotCreatedChipsNum(slot);
			return slot.createdChips - num >= extraFallingElement.minCreatedChipsBeforeReactivate;
		}

		public void OnExtraFallingChipGenerated(Slot slot)
		{
			Match3Game game = slot.game;
			ExtraFallingChips.FallingChipCreation fallingChipCreation = new ExtraFallingChips.FallingChipCreation();
			fallingChipCreation.userMovesCount = game.board.userMovesCount;
			fallingChipCreation.slot = slot;
			fallingChipCreation.slotCreatedChipsNum = slot.createdChips;
			fallingChipCreation.chipTag = slot.generatorSettings.chipTag;
			this.createdList.Add(fallingChipCreation);
		}

		public void OnFallingElementPickup(Chip chip)
		{
			ExtraFallingChips.FallingChipPickup fallingChipPickup = new ExtraFallingChips.FallingChipPickup();
			fallingChipPickup.chipTag = chip.chipTag;
			this.pickedUpList.Add(fallingChipPickup);
		}

		public ExtraFallingElements setup;

		private List<ExtraFallingChips.FallingChipCreation> createdList = new List<ExtraFallingChips.FallingChipCreation>();

		private List<ExtraFallingChips.FallingChipPickup> pickedUpList = new List<ExtraFallingChips.FallingChipPickup>();

		public class FallingChipPickup
		{
			public int chipTag;
		}

		public class FallingChipCreation
		{
			public Slot slot;

			public int userMovesCount;

			public int slotCreatedChipsNum;

			public int chipTag;
		}
	}
}
