using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class Connection
	{
		public bool ContainsPosition(IntVector2 position)
		{
			for (int i = 0; i < this.slotsList.Count; i++)
			{
				if (this.slotsList[i].position == position)
				{
					return true;
				}
			}
			return false;
		}

		public ItemColor itemColor
		{
			get
			{
				if (this.slotsList.Count == 0)
				{
					return ItemColor.Unknown;
				}
				return this.slotsList[0].GetSlotComponent<Chip>().itemColor;
			}
		}

		public bool isUsable
		{
			get
			{
				if (this.type == Connection.ConnectionType.Square)
				{
					return this.slotsList.Count >= 4;
				}
				return this.slotsList.Count >= 3;
			}
		}

		public bool IsIntersecting(Connection c)
		{
			for (int i = 0; i < this.slotsList.Count; i++)
			{
				Slot slot = this.slotsList[i];
				for (int j = 0; j < c.slotsList.Count; j++)
				{
					if (c.slotsList[j] == slot)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsChipAcceptable(Chip chip)
		{
			return chip != null && chip.canFormColorMatches && (this.itemColor == ItemColor.Unknown || chip.itemColor == this.itemColor);
		}

		public void Clear()
		{
			this.slotsList.Clear();
		}

		public void CopyFrom(Connection c)
		{
			this.slotsList.Clear();
			this.slotsList.AddRange(c.slotsList);
			this.type = c.type;
		}

		public List<Slot> slotsList = new List<Slot>();

		public Connection.ConnectionType type;

		public enum ConnectionType
		{
			Vertical,
			Horizontal,
			Square
		}
	}
}
