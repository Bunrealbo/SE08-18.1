using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class TilesSlotsProvider
	{
		public virtual int MaxSlots
		{
			get
			{
				return 100;
			}
		}

		public virtual Vector2 StartPosition(float slotSize)
		{
			return Vector2.zero;
		}

		public virtual TilesSlotsProvider.Slot GetSlot(IntVector2 position)
		{
			return default(TilesSlotsProvider.Slot);
		}

		public virtual List<TilesSlotsProvider.Slot> GetAllSlots()
		{
			return new List<TilesSlotsProvider.Slot>();
		}

		public struct Slot
		{
			public bool isEmpty
			{
				get
				{
					return !this.isOccupied;
				}
			}

			public Slot(IntVector2 position, bool isOccupied)
			{
				this.position = position;
				this.isOccupied = isOccupied;
			}

			public bool isOccupied;

			public IntVector2 position;
		}
	}
}
