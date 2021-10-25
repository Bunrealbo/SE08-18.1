using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class SlotDestroyParams
	{
		public void StartSlot(Slot slot)
		{
			this.chipBlockersDestroyed = 0;
			this.chipsDestroyed = 0;
			this.scoreAdded = 0;
		}

		public void EndSlot(Slot slot)
		{
			this.chipBlockersDestroyed = 0;
			this.chipsDestroyed = 0;
			this.scoreAdded = 0;
		}

		public bool isFromSwapOrTap
		{
			get
			{
				return this.isFromTap || this.isFromSwap;
			}
		}

		public void AddChipForPowerupCreateAnimation(Chip chip)
		{
			if (this.chipsAvailableForPowerupCreateAnimation == null)
			{
				this.chipsAvailableForPowerupCreateAnimation = new List<Chip>();
			}
			this.chipsAvailableForPowerupCreateAnimation.Add(chip);
		}

		public void AddSlotForSuspendedNeighbor(Slot slot)
		{
			if (this.slotsWithSuspendCheckNeighbor == null)
			{
				this.slotsWithSuspendCheckNeighbor = new List<Slot>();
			}
			if (this.slotsWithSuspendCheckNeighbor.Contains(slot))
			{
				return;
			}
			this.slotsWithSuspendCheckNeighbor.Add(slot);
		}

		public bool IsNeigborDestraySuspended(Slot slot)
		{
			return this.slotsWithSuspendCheckNeighbor != null && this.slotsWithSuspendCheckNeighbor.Contains(slot);
		}

		public bool isFromTap;

		public bool isFromSwap;

		public bool isHitByBomb;

		public ChipType bombType;

		public float activationDelay;

		public Island matchIsland;

		public bool isBombAllowingNeighbourDestroy;

		public bool isHavingCarpet;

		public bool isCreatingPowerupFromThisMatch;

		public bool isNeigbourDestroySuspended;

		public bool isExplosion;

		public int goalsCollected;

		public IntVector2 explosionCentre;

		public bool isRocketStopped;

		public int chipBlockersDestroyed;

		public int chipsDestroyed;

		public int scoreAdded;

		public SwapParams swapParams;

		public List<Chip> chipsAvailableForPowerupCreateAnimation;

		public List<Slot> slotsWithSuspendCheckNeighbor;
	}
}
