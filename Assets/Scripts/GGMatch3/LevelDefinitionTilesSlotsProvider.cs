using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class LevelDefinitionTilesSlotsProvider : TilesSlotsProvider
	{
		public LevelDefinitionTilesSlotsProvider(LevelDefinition level)
		{
			this.level = level;
		}

		public override int MaxSlots
		{
			get
			{
				return this.level.size.width * this.level.size.height;
			}
		}

		public override Vector2 StartPosition(float slotSize)
		{
			return new Vector2((float)(-(float)this.level.size.width) * slotSize * 0.5f, (float)(-(float)this.level.size.height) * slotSize * 0.5f);
		}

		public override TilesSlotsProvider.Slot GetSlot(IntVector2 position)
		{
			TilesSlotsProvider.Slot result = default(TilesSlotsProvider.Slot);
			LevelDefinition.SlotDefinition slot = this.level.GetSlot(position);
			result.position = position;
			if (slot != null)
			{
				result.isOccupied = this.IsOccupied(slot);
			}
			return result;
		}

		private bool IsOccupied(LevelDefinition.SlotDefinition levelSlot)
		{
			return levelSlot.slotType == SlotType.PlayingSpace;
		}

		public override List<TilesSlotsProvider.Slot> GetAllSlots()
		{
			this.allSlots.Clear();
			for (int i = 0; i < this.level.slots.Count; i++)
			{
				LevelDefinition.SlotDefinition slotDefinition = this.level.slots[i];
				TilesSlotsProvider.Slot item = default(TilesSlotsProvider.Slot);
				item.position = slotDefinition.position;
				item.isOccupied = this.IsOccupied(slotDefinition);
				this.allSlots.Add(item);
			}
			return this.allSlots;
		}

		public LevelDefinition level;

		private List<TilesSlotsProvider.Slot> allSlots = new List<TilesSlotsProvider.Slot>();
	}
}
