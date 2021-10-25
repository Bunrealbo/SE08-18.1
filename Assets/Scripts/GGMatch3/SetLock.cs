using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class SetLock
	{
		public bool GetIsSwappingSuspended(Slot slot)
		{
			return this.isSwapingSuspended && !this.slots.Contains(slot);
		}

		public List<Slot> slots = new List<Slot>();

		public bool isSwapingSuspended;
	}
}
