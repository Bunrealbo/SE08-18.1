using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class ListSlotsProvider : TilesSlotsProvider
	{
		public bool ContainsPosition(IntVector2 position)
		{
			for (int i = 0; i < this.allSlots.Count; i++)
			{
				if (this.allSlots[i].position == position)
				{
					return true;
				}
			}
			return false;
		}

		public void Clear()
		{
			this.allSlots.Clear();
		}

		public void AddSlot(TilesSlotsProvider.Slot slot)
		{
			this.allSlots.Add(slot);
		}

		public void Init(Match3Game game)
		{
			this.game = game;
		}

		public override int MaxSlots
		{
			get
			{
				return this.allSlots.Count;
			}
		}

		public override Vector2 StartPosition(float slotSize)
		{
			return new Vector2((float)(-(float)this.game.board.size.x) * slotSize * 0.5f, (float)(-(float)this.game.board.size.y) * slotSize * 0.5f);
		}

		public override TilesSlotsProvider.Slot GetSlot(IntVector2 position)
		{
			for (int i = 0; i < this.allSlots.Count; i++)
			{
				TilesSlotsProvider.Slot slot = this.allSlots[i];
				if (slot.position == position)
				{
					return slot;
				}
			}
			return new TilesSlotsProvider.Slot
			{
				position = position
			};
		}

		public override List<TilesSlotsProvider.Slot> GetAllSlots()
		{
			return this.allSlots;
		}

		public Match3Game game;

		public List<TilesSlotsProvider.Slot> allSlots = new List<TilesSlotsProvider.Slot>();
	}
}
