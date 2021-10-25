using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class BoltCollection
	{
		public void AddUsedBolt(LightingBolt bolt)
		{
			if (this.bolts == null)
			{
				return;
			}
			if (bolt == null)
			{
				return;
			}
			this.bolts.Remove(bolt);
		}

		public LightingBolt GetBoltEndingOnSlot(Slot slot)
		{
			if (this.bolts == null)
			{
				return null;
			}
			for (int i = 0; i < this.bolts.Count; i++)
			{
				LightingBolt lightingBolt = this.bolts[i];
				if (lightingBolt.endSlot == slot)
				{
					return lightingBolt;
				}
			}
			return null;
		}

		public void Clear()
		{
			if (this.bolts == null)
			{
				return;
			}
			for (int i = 0; i < this.bolts.Count; i++)
			{
				this.bolts[i].RemoveFromGame();
			}
		}

		public List<LightingBolt> bolts = new List<LightingBolt>();
	}
}
