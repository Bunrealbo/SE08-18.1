using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class SlotComponent
	{
		public virtual int sortingOrder
		{
			get
			{
				return 0;
			}
		}

		public virtual void OnSlotComponentMadeAStartMove(SlotComponent.SlotStartMoveParams moveParams)
		{
		}

		public virtual void OnSlotComponentMadeATransformChange(SlotComponent component)
		{
		}

		public virtual bool isMovingElementRequired
		{
			get
			{
				return false;
			}
		}

		public virtual void AddToGoalsAtStart(Match3Goals goals)
		{
		}

		public virtual long lastMoveFrameIndex
		{
			get
			{
				return 0L;
			}
		}

		public virtual float lastMoveTime
		{
			get
			{
				return this._003ClastMoveTime_003Ek__BackingField;
			}
		}

		public bool isRemoveFromGameDestroySuspended
		{
			get
			{
				for (int i = 0; i < this.slotComponentLocks.Count; i++)
				{
					if (this.slotComponentLocks[i].isRemoveFromGameDestroySuspended)
					{
						return true;
					}
				}
				return false;
			}
		}

		public virtual SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			return new SlotDestroyResolution
			{
				isDestroyed = false
			};
		}

		public virtual SlotDestroyResolution OnDestroySlotComponent(SlotDestroyParams destroyParams)
		{
			return new SlotDestroyResolution
			{
				isDestroyed = false
			};
		}

		public void RemoveFromSlot()
		{
			if (this.slot == null)
			{
				return;
			}
			this.slot.RemoveComponent(this);
		}

		public void RemoveFromGame()
		{
			this.isRemovedFromGame = true;
			this.RemoveFromSlot();
			if (this.isRemoveFromGameDestroySuspended)
			{
				UnityEngine.Debug.LogError("REMOVING FROM GAME SOMETHING THAT IS LOCKED!");
				return;
			}
			for (int i = 0; i < this.monoBehaviours.Count; i++)
			{
				this.monoBehaviours[i].RemoveFromGame();
			}
			this.monoBehaviours.Clear();
		}

		public void Add(SlotComponentBehavoiour beh)
		{
			if (beh == null)
			{
				return;
			}
			beh.OnAddedToSlotComponent(this);
			this.monoBehaviours.Add(beh);
		}

		public T GetComponentBehaviour<T>() where T : SlotComponentBehavoiour
		{
			for (int i = 0; i < this.monoBehaviours.Count; i++)
			{
				T t = this.monoBehaviours[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		public void AddLock(SlotComponentLock slotComponentLock)
		{
			if (slotComponentLock == null)
			{
				return;
			}
			if (this.slotComponentLocks.Contains(slotComponentLock))
			{
				return;
			}
			this.slotComponentLocks.Add(slotComponentLock);
		}

		public void RemoveLock(SlotComponentLock slotComponentLock)
		{
			if (slotComponentLock == null)
			{
				return;
			}
			this.slotComponentLocks.Remove(slotComponentLock);
		}

		public virtual bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
		{
			return false;
		}

		public virtual bool isBlockingDirection(IntVector2 direction)
		{
			return false;
		}

		public virtual bool isSlotSwapSuspended
		{
			get
			{
				return this._003CisSlotSwapSuspended_003Ek__BackingField;
			}
		}

		public virtual bool isSlotTapSuspended
		{
			get
			{
				return this._003CisSlotTapSuspended_003Ek__BackingField;
			}
		}

		public virtual bool isSlotMatchingSuspended
		{
			get
			{
				return this._003CisSlotMatchingSuspended_003Ek__BackingField;
			}
		}

		public virtual bool isSlotGravitySuspended
		{
			get
			{
				return this._003CisSlotGravitySuspended_003Ek__BackingField;
			}
		}

		public virtual bool isPlaceBubbleSuspended
		{
			get
			{
				return this._003CisPlaceBubbleSuspended_003Ek__BackingField;
			}
		}

		public virtual bool isMoving
		{
			get
			{
				return this._003CisMoving_003Ek__BackingField;
			}
		}

		public virtual bool isMoveIntoSlotSuspended
		{
			get
			{
				return this._003CisMoveIntoSlotSuspended_003Ek__BackingField;
			}
		}

		public virtual int blockerLevel
		{
			get
			{
				return this._003CblockerLevel_003Ek__BackingField;
			}
		}

		public virtual bool isBlockingBurriedElement
		{
			get
			{
				return this._003CisBlockingBurriedElement_003Ek__BackingField;
			}
		}

		public virtual bool isBlockingCarpetSpread
		{
			get
			{
				return this._003CisBlockingCarpetSpread_003Ek__BackingField;
			}
		}

		public virtual bool isBlockingChip
		{
			get
			{
				return this._003CisBlockingChip_003Ek__BackingField;
			}
		}

		public virtual bool isMoveByConveyorSuspended
		{
			get
			{
				return this._003CisMoveByConveyorSuspended_003Ek__BackingField;
			}
		}

		public virtual bool canReactWithBomb
		{
			get
			{
				return this._003CcanReactWithBomb_003Ek__BackingField;
			}
		}

		public virtual bool IsAvailableForDiscoBombSuspended(bool replaceWithBombs)
		{
			return false;
		}

		public virtual bool isAttachGrowingElementSuspended
		{
			get
			{
				return false;
			}
		}

		public virtual bool isPowerupReplacementSuspended
		{
			get
			{
				return false;
			}
		}

		public virtual bool isCreatePowerupWithThisSlotSuspended
		{
			get
			{
				return this._003CisCreatePowerupWithThisSlotSuspended_003Ek__BackingField;
			}
		}

		public virtual bool isMovingWithConveyor
		{
			get
			{
				return this._003CisMovingWithConveyor_003Ek__BackingField;
			}
		}

		public virtual bool isMovedByGravity
		{
			get
			{
				return this._003CisMovedByGravity_003Ek__BackingField;
			}
		}

		public virtual bool isDestroyedByMatchingNextTo
		{
			get
			{
				return false;
			}
		}

		public virtual void Wobble(WobbleAnimation.Settings settings)
		{
		}

		public virtual bool isPreventingGravity
		{
			get
			{
				return this._003CisPreventingGravity_003Ek__BackingField;
			}
		}

		public virtual bool isPreventingOtherChipsToFallIntoSlot
		{
			get
			{
				return this._003CisPreventingOtherChipsToFallIntoSlot_003Ek__BackingField;
			}
		}

		public virtual bool isPreventingReplaceByOtherChips
		{
			get
			{
				return this._003CisPreventingReplaceByOtherChips_003Ek__BackingField;
			}
		}

		public virtual void OnUpdate(float deltaTime)
		{
		}

		public virtual void OnMovedBySlotGravity(Slot fromSlot, Slot toSlot, MoveContentsToSlotParams moveParams)
		{
		}

		public virtual void OnCreatedBySlot(Slot toSlot)
		{
		}

		public virtual void OnAddedToSlot(Slot slot)
		{
		}

		public virtual void OnRemovedFromSlot(Slot slot)
		{
		}

		public Slot slot;

		public Slot lastConnectedSlot;

		public bool isRemovedFromGame;

		private readonly float _003ClastMoveTime_003Ek__BackingField;

		protected List<SlotComponentBehavoiour> monoBehaviours = new List<SlotComponentBehavoiour>();

		public List<SlotComponentLock> slotComponentLocks = new List<SlotComponentLock>();

		private readonly bool _003CisSlotSwapSuspended_003Ek__BackingField;

		private readonly bool _003CisSlotTapSuspended_003Ek__BackingField;

		private readonly bool _003CisSlotMatchingSuspended_003Ek__BackingField;

		private readonly bool _003CisSlotGravitySuspended_003Ek__BackingField;

		private readonly bool _003CisPlaceBubbleSuspended_003Ek__BackingField;

		private readonly bool _003CisMoving_003Ek__BackingField;

		private readonly bool _003CisMoveIntoSlotSuspended_003Ek__BackingField;

		private readonly int _003CblockerLevel_003Ek__BackingField;

		private readonly bool _003CisBlockingBurriedElement_003Ek__BackingField;

		private readonly bool _003CisBlockingCarpetSpread_003Ek__BackingField;

		private readonly bool _003CisBlockingChip_003Ek__BackingField;

		private readonly bool _003CisMoveByConveyorSuspended_003Ek__BackingField;

		private readonly bool _003CcanReactWithBomb_003Ek__BackingField;

		private readonly bool _003CisCreatePowerupWithThisSlotSuspended_003Ek__BackingField;

		private readonly bool _003CisMovingWithConveyor_003Ek__BackingField;

		private readonly bool _003CisMovedByGravity_003Ek__BackingField;

		private readonly bool _003CisPreventingGravity_003Ek__BackingField;

		private readonly bool _003CisPreventingOtherChipsToFallIntoSlot_003Ek__BackingField;

		private readonly bool _003CisPreventingReplaceByOtherChips_003Ek__BackingField;

		public struct SlotStartMoveParams
		{
			public Slot fromSlot;

			public Slot toSlot;

			public SlotComponent slotComponent;
		}
	}
}
