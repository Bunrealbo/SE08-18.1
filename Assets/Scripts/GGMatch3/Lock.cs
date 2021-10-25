using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class Lock
	{
		public void SuspendAll()
		{
			this.isSlotGravitySuspended = true;
			this.isChipGravitySuspended = true;
			this.isChipGeneratorSuspended = true;
			this.isSlotMatchingSuspended = true;
			this.isSlotTouchingSuspended = true;
			this.isSlotSwipeSuspended = true;
			this.isDestroySuspended = true;
			this.isAvailableForDiscoBombSuspended = true;
			this.isAvailableForSeekingMissileSuspended = true;
			this.isPowerupReplacementSuspended = true;
			this.isAttachGrowingElementSuspended = true;
		}

		public void LockSlots(List<Slot> slots)
		{
			if (slots == null)
			{
				return;
			}
			for (int i = 0; i < slots.Count; i++)
			{
				Slot slot = slots[i];
				this.LockSlot(slot);
			}
		}

		public void LockSlot(Slot slot)
		{
			if (slot == null)
			{
				return;
			}
			slot.AddLock(this);
			if (this.connectedSlots.Contains(slot))
			{
				return;
			}
			this.connectedSlots.Add(slot);
		}

		public void UnlockAllAndSaveToTemporaryList()
		{
			this.SaveToTemporaryList();
			this.UnlockAll();
		}

		public void SaveToTemporaryList()
		{
			if (this.temporaryList == null)
			{
				this.temporaryList = new List<Slot>();
			}
			this.temporaryList.Clear();
			this.temporaryList.AddRange(this.connectedSlots);
		}

		public void LockTemporaryListAndClear()
		{
			this.LockSlots(this.temporaryList);
			this.temporaryList.Clear();
		}

		public void Unlock(Slot slot)
		{
			if (slot == null)
			{
				return;
			}
			this.connectedSlots.Remove(slot);
			slot.RemoveLock(this);
		}

		public void UnlockAll()
		{
			for (int i = 0; i < this.connectedSlots.Count; i++)
			{
				Slot slot = this.connectedSlots[i];
				if (slot != null)
				{
					slot.RemoveLock(this);
				}
			}
			this.connectedSlots.Clear();
		}

		public bool isSlotGravitySuspended;

		public bool isChipGravitySuspended;

		public bool isChipGeneratorSuspended;

		public bool isSlotMatchingSuspended;

		public bool isSlotTouchingSuspended;

		public bool isSlotSwipeSuspended;

		public bool isDestroySuspended;

		public bool isAvailableForDiscoBombSuspended;

		public bool isAvailableForSeekingMissileSuspended;

		public bool isAboutToBeDestroyed;

		public bool isPowerupReplacementSuspended;

		public bool isAttachGrowingElementSuspended;

		private List<Slot> connectedSlots = new List<Slot>();

		private List<Slot> temporaryList;
	}
}
