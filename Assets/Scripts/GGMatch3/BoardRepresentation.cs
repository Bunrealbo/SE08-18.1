using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class BoardRepresentation
	{
		public BoardRepresentation.RepresentationSlot GetSlot(IntVector2 pos)
		{
			if (pos.x < 0 || pos.y < 0 || pos.x >= this.size.x || pos.y >= this.size.y)
			{
				return new BoardRepresentation.RepresentationSlot
				{
					position = pos,
					isOutsideBoard = true
				};
			}
			int index = pos.x + pos.y * this.size.x;
			return this.slots[index];
		}

		public void Init(Match3Game match3Game)
		{
			this.slots.Clear();
			this.size = match3Game.board.size;
			for (int i = 0; i < this.size.y; i++)
			{
				for (int j = 0; j < this.size.x; j++)
				{
					IntVector2 intVector = new IntVector2(j, i);
					BoardRepresentation.RepresentationSlot item = default(BoardRepresentation.RepresentationSlot);
					item.position = intVector;
					Slot slot = match3Game.GetSlot(intVector);
					Chip chip = null;
					if (slot == null)
					{
						item.isOutOfPlayArea = true;
					}
					if (slot != null)
					{
						chip = slot.GetSlotComponent<Chip>();
						item.canMove = !slot.isSlotSwapSuspended;
						item.wallUp = Slot.IsPathBlockedBetween(slot, match3Game.GetSlot(intVector + IntVector2.up));
						item.wallDown = Slot.IsPathBlockedBetween(slot, match3Game.GetSlot(intVector + IntVector2.down));
						item.wallLeft = Slot.IsPathBlockedBetween(slot, match3Game.GetSlot(intVector + IntVector2.left));
						item.wallRight = Slot.IsPathBlockedBetween(slot, match3Game.GetSlot(intVector + IntVector2.right));
					}
					if (chip == null || !chip.canFormColorMatches)
					{
						this.slots.Add(item);
					}
					else
					{
						item.itemColor = chip.itemColor;
						item.canFormColorMatches = !slot.isSlotMatchingSuspended;
						this.slots.Add(item);
					}
				}
			}
		}

		public IntVector2 size;

		public List<BoardRepresentation.RepresentationSlot> slots = new List<BoardRepresentation.RepresentationSlot>();

		public struct RepresentationSlot
		{
			private bool IsBlocked(IntVector2 direction)
			{
				return (direction.x < 0 && this.wallLeft) || (direction.x > 0 && this.wallRight) || (direction.y < 0 && this.wallDown) || (direction.y > 0 && this.wallUp);
			}

			public bool IsBlockedTo(BoardRepresentation.RepresentationSlot slot)
			{
				IntVector2 intVector = slot.position - this.position;
				return this.IsBlocked(intVector) || slot.IsBlocked(-intVector);
			}

			public bool canFormColorMatches;

			public IntVector2 position;

			public ItemColor itemColor;

			public bool isOutsideBoard;

			public bool canMove;

			public bool wallLeft;

			public bool wallRight;

			public bool wallUp;

			public bool wallDown;

			public bool isOutOfPlayArea;
		}
	}
}
