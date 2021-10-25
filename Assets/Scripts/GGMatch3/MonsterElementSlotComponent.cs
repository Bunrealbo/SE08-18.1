using System;

namespace GGMatch3
{
	public class MonsterElementSlotComponent : SlotComponent
	{
		public override int sortingOrder
		{
			get
			{
				return 1000;
			}
		}

		public override bool isMoveIntoSlotSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isSlotGravitySuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isSlotSwapSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isPreventingOtherChipsToFallIntoSlot
		{
			get
			{
				return true;
			}
		}

		public override bool isPreventingGravity
		{
			get
			{
				return true;
			}
		}

		public override bool isCreatePowerupWithThisSlotSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isMovingWithConveyor
		{
			get
			{
				return true;
			}
		}

		public override bool isAttachGrowingElementSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isPlaceBubbleSuspended
		{
			get
			{
				return true;
			}
		}
	}
}
