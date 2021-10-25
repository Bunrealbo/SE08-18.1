using System;

namespace GGMatch3
{
	public struct SlotDestroyResolution
	{
		public bool isDestroyed;

		public bool stopPropagation;

		public bool isNeigbourDestroySuspended;

		public bool isNeigbourDestroySuspendedForThisChipOnly;
	}
}
