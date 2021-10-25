using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
    public class Slot
    {
        public bool isBackgroundPatternActive
        {
            get
            {
                return (this.position.x + this.position.y) % 2 == 0;
            }
        }

        public Vector2 normalizedPositionWithinBoard
        {
            get
            {
                return new Vector3((float)this.position.x / (float)this.game.board.size.x, (float)this.position.y / (float)this.game.board.size.y);
            }
        }

        public int maxDistanceToEnd
        {
            get
            {
                return this._003CmaxDistanceToEnd_003Ek__BackingField;
            }
            protected set
            {
                this._003CmaxDistanceToEnd_003Ek__BackingField = value;
            }
        }

        public Vector3 localPositionOfCenter
        {
            get
            {
                return this.game.LocalPositionOfCenter(this.position);
            }
        }

        public int LockCount
        {
            get
            {
                return this.slotLocks.Count;
            }
        }

        public void AddLock(Lock slotLock)
        {
            if (slotLock == null)
            {
                return;
            }
            if (this.slotLocks.Contains(slotLock))
            {
                return;
            }
            this.slotLocks.Add(slotLock);
        }

        public void ClearLocks()
        {
            this.slotLocks.Clear();
        }

        public void RemoveLock(Lock slotLock)
        {
            if (slotLock == null)
            {
                return;
            }
            this.slotLocks.Remove(slotLock);
        }

        public void AddSetLock(SetLock slotLock)
        {
            if (slotLock == null)
            {
                return;
            }
            if (this.setSlotLocks.Contains(slotLock))
            {
                return;
            }
            this.setSlotLocks.Add(slotLock);
        }

        public void RemoveSetLock(SetLock slotLock)
        {
            if (slotLock == null)
            {
                return;
            }
            this.setSlotLocks.Remove(slotLock);
        }

        public void Init(Match3Game game)
        {
            this.game = game;
        }

        public LightSlotComponent light
        {
            get
            {
                return this.GetSlotComponent<LightSlotComponent>();
            }
        }

        public void AddToGoalsAtStart(Match3Goals goals)
        {
            for (int i = 0; i < this.components.Count; i++)
            {
                this.components[i].AddToGoalsAtStart(goals);
            }
        }

        public T GetSlotComponent<T>() where T : SlotComponent
        {
            for (int i = 0; i < this.components.Count; i++)
            {
                T t = this.components[i] as T;
                if (t != null)
                {
                    return t;
                }
            }
            return default(T);
        }

        public bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
        {
            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i].IsCompatibleWithPickupGoal(chipTypeDef))
                {
                    return true;
                }
            }
            return this.game.board.burriedElements.IsCompatibleWithPickupGoal(this, chipTypeDef) || this.game.board.monsterElements.IsCompatibleWithPickupGoal(this, chipTypeDef);
        }

        public bool isPreventingOtherChipsToFallIntoSlot
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isPreventingOtherChipsToFallIntoSlot)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isEmpty
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isPreventingOtherChipsToFallIntoSlot)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool isDestroyedByMatchingNextTo
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isDestroyedByMatchingNextTo)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isReacheableByGeneratorOrChip
        {
            get
            {
                return Slot.IsReacheableByGeneratorOrChip(this, 0);
            }
        }

        public void FillIncomingGravitySlots()
        {
            List<Slot.MoveToSlot> allMoveToSlots = this.allMoveToSlots;
            for (int i = 0; i < allMoveToSlots.Count; i++)
            {
                Slot.MoveToSlot moveToSlot = allMoveToSlots[i];
                Slot slot = moveToSlot.slot;
                if (moveToSlot.type != Slot.MoveToSlotType.Sandflow && !slot.incomingGravitySlots.Contains(this))
                {
                    slot.incomingGravitySlots.Add(this);
                }
            }
        }

        private static bool IsReacheableByGeneratorOrChip(Slot slot, int depth = 0)
        {
            if (slot.isPreventingGravity)
            {
                return false;
            }
            if (slot.canGenerateChip)
            {
                return true;
            }
            if (slot.isSomethingMoveableByGravityInSlot)
            {
                return true;
            }
            if (depth > 30)
            {
                UnityEngine.Debug.LogError("MAX DEPTH REACHED");
                return false;
            }
            for (int i = 0; i < slot.incomingGravitySlots.Count; i++)
            {
                Slot slot2 = slot.incomingGravitySlots[i];
                if (slot2 != null && Slot.IsReacheableByGeneratorOrChip(slot2, depth + 1))
                {
                    return true;
                }
            }
            return false;
        }

        private static Chip FirstReacheableChip(Slot slot, int depth = 0)
        {
            if (slot == null)
            {
                return null;
            }
            Chip slotComponent = slot.GetSlotComponent<Chip>();
            if (slotComponent != null)
            {
                return slotComponent;
            }
            if (slot.isPreventingGravity)
            {
                return null;
            }
            if (depth > 30)
            {
                UnityEngine.Debug.LogError("MAX DEPTH REACHED");
                return null;
            }
            for (int i = 0; i < slot.incomingGravitySlots.Count; i++)
            {
                Slot slot2 = slot.incomingGravitySlots[i];
                if (slot2 != null)
                {
                    Chip chip = Slot.FirstReacheableChip(slot2, depth + 1);
                    if (chip != null)
                    {
                        return chip;
                    }
                }
            }
            return null;
        }

        public void OnSlotComponentMadeAStartMove(SlotComponent.SlotStartMoveParams startMoveParams)
        {
            for (int i = 0; i < this.components.Count; i++)
            {
                this.components[i].OnSlotComponentMadeAStartMove(startMoveParams);
            }
        }

        public void OnSlotComponentMadeATransformChange(SlotComponent component)
        {
            for (int i = 0; i < this.components.Count; i++)
            {
                this.components[i].OnSlotComponentMadeATransformChange(component);
            }
        }

        public bool isMovingElementRequired
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isMovingElementRequired)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool CanAcceptFallingChip(Slot slotFromWhichToAccept)
        {
            if (this.isMoveIntoSlotSuspended)
            {
                return false;
            }
            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i].isPreventingOtherChipsToFallIntoSlot)
                {
                    return false;
                }
            }
            return true;
        }

        public bool isLockedForDiscoBomb
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isAvailableForDiscoBombSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isAttachGrowingElementSuspended
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isAttachGrowingElementSuspended)
                    {
                        return true;
                    }
                }
                for (int j = 0; j < this.components.Count; j++)
                {
                    if (this.components[j].isAttachGrowingElementSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool CanAttachGrowingElement()
        {
            if (this.isAttachGrowingElementSuspended)
            {
                return false;
            }
            if (this.isDestroySuspended)
            {
                return false;
            }
            if (this.GetSlotComponent<Chip>() != null)
            {
                return true;
            }
            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i].isPreventingOtherChipsToFallIntoSlot)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanParticipateInDiscoBombAffectedArea(ItemColor itemColor, bool replaceWithBombs)
        {
            Chip slotComponent = this.GetSlotComponent<Chip>();
            if (slotComponent == null)
            {
                return false;
            }
            if (!slotComponent.canFormColorMatches)
            {
                return false;
            }
            if (slotComponent.itemColor != itemColor)
            {
                return false;
            }
            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i].IsAvailableForDiscoBombSuspended(replaceWithBombs))
                {
                    return false;
                }
            }
            for (int j = 0; j < this.slotLocks.Count; j++)
            {
                if (this.slotLocks[j].isAvailableForDiscoBombSuspended)
                {
                    return false;
                }
            }
            return true;
        }

        public int IsAboutToBeDestroyedLocksCount()
        {
            int num = 0;
            for (int i = 0; i < this.slotLocks.Count; i++)
            {
                if (this.slotLocks[i].isAboutToBeDestroyed)
                {
                    num++;
                }
            }
            return num;
        }

        public bool hasCarpet
        {
            get
            {
                return this.game.board.carpet.HasCarpet(this.position);
            }
        }

        public bool isBlockingCarpetSpread
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isBlockingCarpetSpread)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool canCarpetSpreadFromHere
        {
            get
            {
                if (!this.hasCarpet)
                {
                    return false;
                }
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isBlockingCarpetSpread)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool isCreatePowerupWithThisSlotSuspended
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isCreatePowerupWithThisSlotSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool canBeTappedToActivate
        {
            get
            {
                Chip slotComponent = this.GetSlotComponent<Chip>();
                return slotComponent != null && slotComponent.canBeTappedToActivate;
            }
        }

        public bool isTapToActivateSuspended
        {
            get
            {
                return this.isSlotMatchingSuspended || this.isSlotGravitySuspended;
            }
        }

        public bool isChipGeneratorSuspended
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isChipGeneratorSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isMoveByConveyorSuspended
        {
            get
            {
                if (this.canGenerateChip && !this.isSomethingMoveableByGravityInSlot && !this.isPreventingOtherChipsToFallIntoSlot)
                {
                    return true;
                }
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isMoveByConveyorSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isBlockForGravity
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    SlotComponent slotComponent = this.components[i];
                    if (!(slotComponent is Chip) && slotComponent.isSlotGravitySuspended)
                    {
                        return true;
                    }
                }
                for (int j = 0; j < this.slotLocks.Count; j++)
                {
                    if (this.slotLocks[j].isSlotGravitySuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public int totalBlockerLevelForFalling
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.components.Count; i++)
                {
                    SlotComponent slotComponent = this.components[i];
                    if (slotComponent.isBlockingChip)
                    {
                        num += slotComponent.blockerLevel;
                    }
                }
                return num;
            }
        }

        public int maxBlockerLevel
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.components.Count; i++)
                {
                    SlotComponent slotComponent = this.components[i];
                    num = Mathf.Max(num, slotComponent.blockerLevel);
                }
                return num;
            }
        }

        public bool isBlockingBurriedElement
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isBlockingBurriedElement)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public int totalBlockerLevel
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.components.Count; i++)
                {
                    SlotComponent slotComponent = this.components[i];
                    num += slotComponent.blockerLevel;
                }
                return num;
            }
        }

        public bool isMoveIntoSlotSuspended
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isMoveIntoSlotSuspended)
                    {
                        return true;
                    }
                }
                for (int j = 0; j < this.slotLocks.Count; j++)
                {
                    if (this.slotLocks[j].isSlotGravitySuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isSlotGravitySuspendedByComponentOtherThan(SlotComponent excludedComponent)
        {
            for (int i = 0; i < this.components.Count; i++)
            {
                SlotComponent slotComponent = this.components[i];
                if (slotComponent != excludedComponent && slotComponent.isSlotGravitySuspended)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isSlotGravitySuspendedByComponent
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isSlotGravitySuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isMoving
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isMoving)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isSlotGravitySuspended
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isSlotGravitySuspended)
                    {
                        return true;
                    }
                }
                for (int j = 0; j < this.slotLocks.Count; j++)
                {
                    if (this.slotLocks[j].isSlotGravitySuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isPlaceBubbleSuspended
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isPlaceBubbleSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isDestroySuspended
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isDestroySuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isChipGravitySuspended
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isChipGravitySuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isSlotSwapSuspended
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isSlotSwapSuspended)
                    {
                        return true;
                    }
                }
                return this.isSlotMatchingSuspended || this.isSlotGravitySuspended;
            }
        }

        public bool isPowerupReplacementSuspended
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isPowerupReplacementSuspended)
                    {
                        return true;
                    }
                }
                for (int j = 0; j < this.components.Count; j++)
                {
                    if (this.components[j].isPowerupReplacementSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isSlotMatchingSuspended
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isSlotMatchingSuspended)
                    {
                        return true;
                    }
                }
                for (int j = 0; j < this.components.Count; j++)
                {
                    if (this.components[j].isSlotMatchingSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isSlotTouchingSuspended
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isSlotTouchingSuspended)
                    {
                        return true;
                    }
                }
                for (int j = 0; j < this.components.Count; j++)
                {
                    if (this.components[j].isSlotTapSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private bool IsBlockingPassTo(Slot to)
        {
            IntVector2 direction = to.position - this.position;
            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i].isBlockingDirection(direction))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isSwipeSuspendedTo(Slot slot)
        {
            return this.IsBlockingPassTo(slot) || slot.IsBlockingPassTo(this);
        }

        public bool isSlotSwipingSuspendedForSlot(Slot slot)
        {
            for (int i = 0; i < this.setSlotLocks.Count; i++)
            {
                if (this.setSlotLocks[i].GetIsSwappingSuspended(slot))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isSlotSwipingSuspended
        {
            get
            {
                for (int i = 0; i < this.slotLocks.Count; i++)
                {
                    if (this.slotLocks[i].isSlotSwipeSuspended)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isPreventingGravity
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isPreventingGravity)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isSomethingMoveableByGravityInSlot
        {
            get
            {
                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].isMovedByGravity)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public float lastMoveTime
        {
            get
            {
                Chip slotComponent = this.GetSlotComponent<Chip>();
                if (slotComponent == null)
                {
                    return 0f;
                }
                return slotComponent.lastMoveTime;
            }
        }

        public long lastMoveFrameIndex
        {
            get
            {
                Chip slotComponent = this.GetSlotComponent<Chip>();
                if (slotComponent == null)
                {
                    return 0L;
                }
                return slotComponent.lastMoveFrameIndex;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            this.backLight.OnUpdate(deltaTime);
            this.ApplySlotGravity();
            for (int i = 0; i < this.components.Count; i++)
            {
                this.components[i].OnUpdate(deltaTime);
            }
        }

        public Slot NextSlotToPushToWithoutSandflow()
        {
            List<Slot.MoveToSlot> allMoveToSlots = this.allMoveToSlots;
            for (int i = 0; i < allMoveToSlots.Count; i++)
            {
                Slot.MoveToSlot moveToSlot = allMoveToSlots[i];
                if (moveToSlot.type != Slot.MoveToSlotType.Sandflow)
                {
                    return moveToSlot.slot;
                }
            }
            return null;
        }

        protected List<Slot.MoveToSlot> allMoveToSlots
        {
            get
            {
                this.allMoveToSlots_.Clear();
                List<IntVector2> forceDirections = this.gravity.forceDirections;
                for (int i = 0; i < forceDirections.Count; i++)
                {
                    IntVector2 intVector = forceDirections[i];
                    IntVector2 intVector2 = this.position + intVector;
                    Slot slot = this.game.GetSlot(intVector2);
                    if (slot != null && !this.IsBlockingPath(intVector) && !slot.IsBlockingPath(-intVector))
                    {
                        this.allMoveToSlots_.Add(new Slot.MoveToSlot(Slot.MoveToSlotType.Gravity, slot));
                    }
                }
                for (int j = 0; j < this.portalDestinationSlots.Count; j++)
                {
                    Slot slot2 = this.portalDestinationSlots[j];
                    if (slot2 != null)
                    {
                        IntVector2 direction = forceDirections[0];
                        IntVector2 direction2 = -slot2.gravity.forceDirections[0];
                        if (!this.IsBlockingPath(direction) && !slot2.IsBlockingPath(direction2))
                        {
                            this.allMoveToSlots_.Add(new Slot.MoveToSlot(Slot.MoveToSlotType.Portal, slot2));
                        }
                    }
                }
                for (int k = 0; k < this.jumpDestinationSlots.Count; k++)
                {
                    Slot slot3 = this.jumpDestinationSlots[k];
                    if (slot3 != null)
                    {
                        IntVector2 intVector3 = forceDirections[0];
                        if (!this.IsBlockingPath(intVector3) && !slot3.IsBlockingPath(-intVector3))
                        {
                            this.allMoveToSlots_.Add(new Slot.MoveToSlot(Slot.MoveToSlotType.Jump, slot3));
                        }
                    }
                }
                List<Gravity.SandflowDirection> sandflowDirections = this.gravity.sandflowDirections;
                for (int l = 0; l < sandflowDirections.Count; l++)
                {
                    Gravity.SandflowDirection sandflowDirection = sandflowDirections[l];
                    IntVector2 intVector4 = this.position + sandflowDirection.offset;
                    Slot slot4 = this.game.GetSlot(intVector4);
                    if (slot4 != null)
                    {
                        IntVector2 intVector5 = this.position + sandflowDirection.direction;
                        Slot slot5 = this.game.GetSlot(intVector5);
                        IntVector2 intVector6 = this.position + sandflowDirection.forceDirection;
                        Slot slot6 = this.game.GetSlot(intVector6);
                        if ((!Slot.IsPathBlockedBetween(this, slot5) || !Slot.IsPathBlockedBetween(this, slot6) || (!Slot.IsWallBetween(this.game, this.position, intVector5) && !Slot.IsWallBetween(this.game, this.position, intVector6))) && (!Slot.IsPathBlockedBetween(slot5, slot4) || !Slot.IsPathBlockedBetween(slot6, slot4) || (!Slot.IsWallBetween(this.game, intVector5, intVector4) && !Slot.IsWallBetween(this.game, intVector6, intVector4))) && (!Slot.IsPathBlockedBetween(slot5, slot4) || !Slot.IsPathBlockedBetween(this, slot6) || (!Slot.IsWallBetween(this.game, intVector5, intVector4) && !Slot.IsWallBetween(this.game, this.position, intVector6))) && (!Slot.IsPathBlockedBetween(this, slot5) || !Slot.IsPathBlockedBetween(slot4, slot6) || (!Slot.IsWallBetween(this.game, this.position, intVector5) && !Slot.IsWallBetween(this.game, intVector4, intVector6))))
                        {
                            this.allMoveToSlots_.Add(new Slot.MoveToSlot(Slot.MoveToSlotType.Sandflow, slot4));
                        }
                    }
                }
                return this.allMoveToSlots_;
            }
        }

        public void Wobble(WobbleAnimation.Settings wobbleSettings)
        {
            for (int i = 0; i < this.components.Count; i++)
            {
                this.components[i].Wobble(wobbleSettings);
            }
        }

        public void SetMaxDistanceToEnd(int depth = 0)
        {
            if (this.isMaxDistanceToEndSet)
            {
                return;
            }
            if (depth > 100)
            {
                UnityEngine.Debug.LogError("LOOP DETECTED");
                this.isMaxDistanceToEndSet = true;
                this.maxDistanceToEnd = depth;
                return;
            }
            int num = 0;
            List<Slot.MoveToSlot> allMoveToSlots = this.allMoveToSlots;
            for (int i = 0; i < allMoveToSlots.Count; i++)
            {
                Slot slot = allMoveToSlots[i].slot;
                slot.SetMaxDistanceToEnd(depth + 1);
                num = Mathf.Max(slot.maxDistanceToEnd, num);
            }
            this.maxDistanceToEnd = num + 1;
            this.isMaxDistanceToEndSet = true;
        }

        public static bool IsWallBetween(Match3Game game, IntVector2 originPos, IntVector2 destinationPos)
        {
            Slot slot = game.GetSlot(originPos);
            IntVector2 intVector = destinationPos - originPos;
            if (slot != null && slot.IsBlockingPath(intVector))
            {
                return true;
            }
            Slot slot2 = game.GetSlot(destinationPos);
            return slot2 != null && slot2.IsBlockingPath(-intVector);
        }

        public static bool IsPathBlockedBetween(Slot origin, Slot destination)
        {
            if (origin == null || destination == null)
            {
                return true;
            }
            IntVector2 intVector = destination.position - origin.position;
            return origin.IsBlockingPath(intVector) || destination.IsBlockingPath(-intVector);
        }

        public bool IsBlockingPath(IntVector2 direction)
        {
            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i].isBlockingDirection(direction))
                {
                    return true;
                }
            }
            return false;
        }

        public void ApplySlotGravity()
        {
            if (this.isSlotGravitySuspended)
            {
                return;
            }
            if (this.canGenerateChip && !this.isSomethingMoveableByGravityInSlot)
            {
                this.GenerateChip();
                return;
            }
            if (!this.isSomethingMoveableByGravityInSlot)
            {
                return;
            }
            MoveContentsToSlotParams moveParams = default(MoveContentsToSlotParams);
            List<Slot.MoveToSlot> allMoveToSlots = this.allMoveToSlots;
            for (int i = 0; i < allMoveToSlots.Count; i++)
            {
                Slot.MoveToSlot moveToSlot = allMoveToSlots[i];
                Slot slot = moveToSlot.slot;
                if (slot.CanAcceptFallingChip(this) && (moveToSlot.type != Slot.MoveToSlotType.Sandflow || !slot.isReacheableByGeneratorOrChip))
                {
                    moveParams.isFromPortal = (moveToSlot.type == Slot.MoveToSlotType.Portal);
                    this.MoveContentsToSlotByGravity(slot, moveParams);
                    return;
                }
            }
        }

        public void AddComponent(SlotComponent c)
        {
            if (c == null)
            {
                return;
            }
            c.slot = this;
            c.lastConnectedSlot = this;
            if (c is Chip && this.GetSlotComponent<Chip>() != null)
            {
                UnityEngine.Debug.LogError("CHIP ADDED TWICE TO SLOT " + this.position);
            }
            bool flag = true;
            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i].sortingOrder < c.sortingOrder)
                {
                    this.components.Insert(i, c);
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                this.components.Add(c);
            }
            c.OnAddedToSlot(this);
        }

        public void RemoveComponent(SlotComponent c)
        {
            if (c == null)
            {
                return;
            }
            c.slot = null;
            this.components.Remove(c);
            c.OnRemovedFromSlot(this);
        }

        public void OnDestroyNeighbourSlot(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
        {
            this.tempSlotComponentsList.Clear();
            this.tempSlotComponentsList.AddRange(this.components);
            int num = 0;
            while (num < this.tempSlotComponentsList.Count && !this.tempSlotComponentsList[num].OnDestroyNeighbourSlotComponent(slotBeingDestroyed, destroyParams).stopPropagation)
            {
                num++;
            }
        }

        public List<Slot> neigbourSlots
        {
            get
            {
                if (this.isNeigbourSlotsListSet)
                {
                    return this.neigbourSlots_;
                }
                List<Slot> list = this.neigbourSlots_;
                list.Clear();
                for (int i = this.position.x - 1; i <= this.position.x + 1; i++)
                {
                    for (int j = this.position.y - 1; j <= this.position.y + 1; j++)
                    {
                        IntVector2 intVector = new IntVector2(i, j);
                        if (Mathf.Abs(intVector.x - this.position.x) + Mathf.Abs(intVector.y - this.position.y) == 1)
                        {
                            Slot slot = this.game.GetSlot(intVector);
                            if (slot != null && slot != this)
                            {
                                list.Add(slot);
                            }
                        }
                    }
                }
                this.isNeigbourSlotsListSet = true;
                return list;
            }
        }

        public void OnDestroySlot(SlotDestroyParams destroyParams)
        {
            if (this.isDestroySuspended)
            {
                return;
            }
            bool flag = false;
            destroyParams.StartSlot(this);
            this.tempSlotComponentsList.Clear();
            this.tempSlotComponentsList.AddRange(this.components);
            bool flag2 = false;
            for (int i = 0; i < this.tempSlotComponentsList.Count; i++)
            {
                SlotComponent slotComponent = this.tempSlotComponentsList[i];
                bool isBlockingChip = slotComponent.isBlockingChip;
                bool flag3 = slotComponent is Chip;
                SlotDestroyResolution slotDestroyResolution = slotComponent.OnDestroySlotComponent(destroyParams);
                if (slotDestroyResolution.isDestroyed && flag3)
                {
                    destroyParams.chipsDestroyed++;
                }
                if (slotDestroyResolution.isDestroyed && isBlockingChip)
                {
                    destroyParams.chipBlockersDestroyed++;
                }
                if (slotDestroyResolution.isNeigbourDestroySuspendedForThisChipOnly)
                {
                    flag = true;
                }
                if (slotDestroyResolution.isNeigbourDestroySuspended)
                {
                    destroyParams.isNeigbourDestroySuspended = true;
                }
                if (slotDestroyResolution.stopPropagation)
                {
                    flag2 = true;
                    break;
                }
            }
            CollectPointsAction.OnSlotDestroy(this, destroyParams);
            destroyParams.EndSlot(this);
            if (!flag2 && destroyParams.isHavingCarpet)
            {
                this.game.board.carpet.AddCarpetFromGame(this.position);
            }
            if (flag)
            {
                destroyParams.AddSlotForSuspendedNeighbor(this);
            }
            this.tempSlotComponentsList.Clear();
        }

        private void MoveContentsToSlotByGravity(Slot nextSlot, MoveContentsToSlotParams moveParams)
        {
            if (nextSlot.isMoveIntoSlotSuspended)
            {
                return;
            }
            this.componentsToRemove.Clear();
            for (int i = 0; i < this.components.Count; i++)
            {
                SlotComponent slotComponent = this.components[i];
                if (slotComponent.isMovedByGravity)
                {
                    this.componentsToRemove.Add(slotComponent);
                }
            }
            for (int j = 0; j < this.componentsToRemove.Count; j++)
            {
                SlotComponent slotComponent2 = this.componentsToRemove[j];
                this.RemoveComponent(slotComponent2);
                nextSlot.AddComponent(slotComponent2);
                slotComponent2.OnMovedBySlotGravity(this, nextSlot, moveParams);
            }
        }

        private ItemColor ColorToIgnore()
        {
            if (this.selectedColors.Count == 0)
            {
                return ItemColor.Unknown;
            }
            ItemColor itemColor = this.selectedColors[this.selectedColors.Count - 1];
            int num = 0;
            int num2 = this.selectedColors.Count - 1;
            while (num2 >= 0 && this.selectedColors[num2] == itemColor)
            {
                num++;
                num2--;
            }
            if (num < 2)
            {
                return ItemColor.Unknown;
            }
            return itemColor;
        }

        private bool TryGenerateFallingChip()
        {
            ExtraFallingChips extraFallingChips = this.game.extraFallingChips;
            if (this.generatorSettings.maxFallingElementsToGenerate <= this.generatedFallingElements)
            {
                return false;
            }
            if (!extraFallingChips.ShouldGenerateExtraFallingChip(this))
            {
                return false;
            }
            this.statsToBottom.Fill(this);
            int placesToGoDown = Mathf.Max(0, this.statsToBottom.totalDepth - this.statsToBottom.movingChips - 1);
            if (this.statsToBottom.GetPathSlot(placesToGoDown) != this)
            {
                return false;
            }
            Chip chip = this.game.CreateFallingElement(this, ChipType.FallingGingerbreadMan);
            chip.chipTag = this.generatorSettings.chipTag;
            chip.OnCreatedBySlot(this);
            this.generatedFallingElements++;
            extraFallingChips.OnExtraFallingChipGenerated(this);
            return true;
        }

        private void GenerateChip()
        {
            if (this.isChipGeneratorSuspended)
            {
                return;
            }
            if (this.isSlotGravitySuspended)
            {
                return;
            }
            if (this.TryGenerateFallingChip())
            {
                this.createdChips++;
                return;
            }
            if (this.generatorSetup != null && this.generatorSetup.chips.Count > this.createdChips)
            {
                ItemColor itemColor = this.generatorSetup.chips[this.createdChips].itemColor;
                this.game.CreateChipInSlot(this, itemColor).OnCreatedBySlot(this);
            }
            else if (this.generatorSettings.generateOnlyBunnies)
            {
                this.game.CreateCharacterInSlot(this, ChipType.BunnyChip, 0).OnCreatedBySlot(this);
            }
            else
            {
                Match3Board.ChipCreateParams chipCreateParams = default(Match3Board.ChipCreateParams);
                chipCreateParams.chipType = ChipType.Unknown;
                if (this.generatorSlotSettings != null)
                {
                    chipCreateParams = this.game.board.RandomChip(this.generatorSlotSettings);
                }
                if (chipCreateParams.chipType == ChipType.Unknown)
                {
                    chipCreateParams = this.game.board.RandomChip(this.ColorToIgnore());
                }
                if (chipCreateParams.chipType == ChipType.Bomb || chipCreateParams.chipType == ChipType.HorizontalRocket || chipCreateParams.chipType == ChipType.VerticalRocket || chipCreateParams.chipType == ChipType.DiscoBall)
                {
                    this.game.CreatePowerupInSlot(this, chipCreateParams.chipType).OnCreatedBySlot(this);
                }
                else if (chipCreateParams.chipType == ChipType.BunnyChip)
                {
                    this.game.CreateCharacterInSlot(this, ChipType.BunnyChip, 0).OnCreatedBySlot(this);
                }
                else
                {
                    ItemColor itemColor2 = chipCreateParams.itemColor;
                    bool flag = this.game.IsPreventAutomatedMachesIfPossible();
                    bool strictAsPossibleToprevent = this.game.strictAsPossibleToprevent;
                    if (flag)
                    {
                        this.statsToBottom.Fill(this);
                        int placesToGoDown = Mathf.Max(0, this.statsToBottom.totalDepth - this.statsToBottom.movingChips - 1);
                        Slot pathSlot = this.statsToBottom.GetPathSlot(placesToGoDown);
                        this.statsToBottom.availableColors.Clear();
                        this.statsToBottom.availableColors.AddRange(this.game.board.availableColors);
                        List<IntVector2> orthoDirections = pathSlot.gravity.orthoDirections;
                        for (int i = 0; i < orthoDirections.Count; i++)
                        {
                            IntVector2 intVector = orthoDirections[i];
                            Slot slot = this.game.GetSlot(this.position + intVector);
                            Slot slot2 = this.game.GetSlot(this.position + 2 * intVector);
                            Slot slot3 = this.game.GetSlot(this.position - intVector);
                            Chip chip = Slot.FirstReacheableChip(slot, 0);
                            Chip chip2 = Slot.FirstReacheableChip(slot2, 0);
                            Chip chip3 = Slot.FirstReacheableChip(slot3, 0);
                            if (strictAsPossibleToprevent)
                            {
                                if (chip != null)
                                {
                                    this.statsToBottom.availableColors.Remove(chip.itemColor);
                                }
                            }
                            else
                            {
                                if (chip != null && chip2 != null && chip.itemColor == chip2.itemColor)
                                {
                                    this.statsToBottom.availableColors.Remove(chip.itemColor);
                                }
                                if (chip != null && chip3 != null && chip.itemColor == chip3.itemColor)
                                {
                                    this.statsToBottom.availableColors.Remove(chip.itemColor);
                                }
                            }
                        }
                        Chip firstChipBelow = this.statsToBottom.firstChipBelow;
                        if (firstChipBelow != null)
                        {
                            this.statsToBottom.availableColors.Remove(firstChipBelow.itemColor);
                        }
                        itemColor2 = this.game.board.RandomColor(ItemColor.Unknown);
                        if (this.statsToBottom.availableColors.Count > 0)
                        {
                            itemColor2 = this.statsToBottom.availableColors[this.game.RandomRange(0, this.statsToBottom.availableColors.Count) % this.statsToBottom.availableColors.Count];
                        }
                        this.game.board.generatedChipsCount++;
                    }
                    this.selectedColors.Add(itemColor2);
                    if (this.selectedColors.Count > this.maxSelectedColorCount)
                    {
                        this.selectedColors.RemoveAt(0);
                    }
                    Chip chip4;
                    if (chipCreateParams.chipType == ChipType.MonsterChip)
                    {
                        chip4 = this.game.CreateChipInSlot(this, chipCreateParams.chipType, itemColor2);
                    }
                    else
                    {
                        chip4 = this.game.CreateChipInSlot(this, itemColor2);
                    }
                    if (chip4 != null)
                    {
                        chip4.OnCreatedBySlot(this);
                    }
                }
                if (chipCreateParams.hasIce)
                {
                    this.game.AddIceToSlot(this, chipCreateParams.iceLevel);
                }
            }
            this.createdChips++;
        }

        public static void GetStatsToBottom(Slot slot, Slot.StatsToBottom sb)
        {
            if (slot == null)
            {
                return;
            }
            if (slot.isBlockForGravity)
            {
                return;
            }
            sb.pathSlots.Add(slot);
            sb.totalDepth++;
            Chip slotComponent = slot.GetSlotComponent<Chip>();
            sb.TryAddChip(slotComponent);
            if (slotComponent != null)
            {
                if (!slotComponent.isSlotGravitySuspended)
                {
                    sb.totalDepth--;
                    return;
                }
                sb.movingChips++;
            }
            List<Slot.MoveToSlot> allMoveToSlots = slot.allMoveToSlots;
            for (int i = 0; i < allMoveToSlots.Count; i++)
            {
                Slot.MoveToSlot moveToSlot = allMoveToSlots[i];
                if (moveToSlot.type != Slot.MoveToSlotType.Sandflow)
                {
                    Slot.GetStatsToBottom(moveToSlot.slot, sb);
                    return;
                }
            }
        }

        public static void SwitchChips(Slot slot1, Slot slot2, bool changePosition = false)
        {
            if (slot1 == null || slot2 == null)
            {
                return;
            }
            Chip slotComponent = slot1.GetSlotComponent<Chip>();
            Chip slotComponent2 = slot2.GetSlotComponent<Chip>();
            slot1.RemoveComponent(slotComponent);
            slot2.RemoveComponent(slotComponent2);
            slot1.AddComponent(slotComponent2);
            slot2.AddComponent(slotComponent);
            if (!changePosition)
            {
                return;
            }
            TransformBehaviour transformBehaviour = null;
            TransformBehaviour transformBehaviour2 = null;
            if (slotComponent != null)
            {
                transformBehaviour = slotComponent.GetComponentBehaviour<TransformBehaviour>();
            }
            if (slotComponent2 != null)
            {
                transformBehaviour2 = slotComponent2.GetComponentBehaviour<TransformBehaviour>();
            }
            if (transformBehaviour != null)
            {
                transformBehaviour.localPosition = slot2.localPositionOfCenter;
            }
            if (transformBehaviour2 != null)
            {
                transformBehaviour2.localPosition = slot1.localPositionOfCenter;
            }
        }

        public static bool HasNeighboursAffectedByMatchingSlots(List<Slot> matchingSlots, Match3Game game)
        {
            for (int i = 0; i < matchingSlots.Count; i++)
            {
                Slot slot = matchingSlots[i];
                foreach (IntVector2 b in IntVector2.upDownLeftRight)
                {
                    Slot slot2 = game.GetSlot(slot.position + b);
                    if (slot2 != null && slot2.isDestroyedByMatchingNextTo && !matchingSlots.Contains(slot2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void RemoveLocks(Slot slot, Lock slotLock)
        {
            if (slotLock == null)
            {
                return;
            }
            if (slot == null)
            {
                return;
            }
            slot.RemoveLock(slotLock);
        }

        public static void RemoveLocks(List<Slot> slots, Lock slotLock)
        {
            if (slotLock == null)
            {
                return;
            }
            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                if (slot != null)
                {
                    slot.RemoveLock(slotLock);
                }
            }
        }

        public bool wasRenderedForChocolateLastFrame;

        public Match3Game game;

        public IntVector2 position;

        public PipeBehaviour entrancePipe;

        public PipeBehaviour exitPipe;

        [NonSerialized]
        public GeneratorSetup generatorSetup;

        [NonSerialized]
        public GeneratorSlotSettings generatorSlotSettings;

        private bool isMaxDistanceToEndSet;

        private int _003CmaxDistanceToEnd_003Ek__BackingField;

        public Gravity gravity;

        public bool isExitForFallingChip;

        public Slot.PositionIntegrator positionIntegrator;

        public Vector3 prevOffsetPosition;

        public Vector3 offsetPosition;

        public Vector3 offsetScale = Vector3.one;

        public List<Slot> portalDestinationSlots = new List<Slot>();

        public List<Slot> jumpOriginSlots = new List<Slot>();

        public List<Slot> jumpDestinationSlots = new List<Slot>();

        public List<Slot> incomingGravitySlots = new List<Slot>();

        public List<SlotComponent> components = new List<SlotComponent>();

        private List<Lock> slotLocks = new List<Lock>();

        private List<SetLock> setSlotLocks = new List<SetLock>();

        public bool canGenerateChip;

        private int generatedFallingElements;

        public LevelDefinition.GeneratorSettings generatorSettings;

        public LightSlotComponent backLight;

        private List<Slot.MoveToSlot> allMoveToSlots_ = new List<Slot.MoveToSlot>();

        private List<SlotComponent> componentsToRemove = new List<SlotComponent>();

        private List<Slot> neigbourSlots_ = new List<Slot>();

        private bool isNeigbourSlotsListSet;

        private List<SlotComponent> tempSlotComponentsList = new List<SlotComponent>();

        private Slot.StatsToBottom statsToBottom = new Slot.StatsToBottom();

        private List<ItemColor> selectedColors = new List<ItemColor>();

        private int maxSelectedColorCount = 10;

        [NonSerialized]
        public int createdChips;

        public struct PositionIntegrator
        {
            public void ResetAcceleration()
            {
                this.acceleration = Vector3.zero;
            }

            public void SetPosition(Vector3 position)
            {
                this.prevPosition = position;
                this.currentPosition = position;
            }

            public void Update(float deltaTime, float dampingFactor, float stiffness)
            {
                float num = 0.0166666675f;
                this.time += deltaTime;
                while (this.time >= num)
                {
                    this.time -= num;
                    this.FixedUpdate(num, dampingFactor, stiffness);
                }
            }

            private void FixedUpdate(float fixedTime, float dampingFactor, float stiffness)
            {
                Vector3 a = this.currentPosition - this.prevPosition;
                this.prevPosition = this.currentPosition;
                this.currentPosition += a * (1f - dampingFactor) + this.acceleration * fixedTime * fixedTime;
                Vector3 a2 = this.currentPosition;
                float magnitude = a2.magnitude;
                Vector3 b = stiffness * magnitude * 0.5f * a2;
                this.currentPosition -= b;
            }

            public Vector3 currentPosition;

            public Vector3 prevPosition;

            public Vector3 acceleration;

            public float time;
        }

        public enum MoveToSlotType
        {
            Gravity,
            Sandflow,
            Portal,
            Jump
        }

        public struct MoveToSlot
        {
            public MoveToSlot(Slot.MoveToSlotType type, Slot slot)
            {
                this.type = type;
                this.slot = slot;
            }

            public Slot.MoveToSlotType type;

            public Slot slot;
        }

        public class StatsToBottom
        {
            public Slot GetPathSlot(int placesToGoDown)
            {
                return this.pathSlots[Mathf.Clamp(placesToGoDown, 0, this.pathSlots.Count - 1)];
            }

            public void Fill(Slot firstSlot)
            {
                this.Clear();
                Slot.GetStatsToBottom(firstSlot, this);
            }

            public void Clear()
            {
                this.pathSlots.Clear();
                this.availableColors.Clear();
                this.emptySpaces = 0;
                this.movingChips = 0;
                this.totalDepth = 0;
                this.firstChipBelow = null;
            }

            public void TryAddChip(Chip firstChip)
            {
                if (this.firstChipBelow != null)
                {
                    return;
                }
                this.firstChipBelow = firstChip;
            }

            public int emptySpaces;

            public int movingChips;

            public int totalDepth;

            public Chip firstChipBelow;

            public List<Slot> pathSlots = new List<Slot>();

            public List<ItemColor> availableColors = new List<ItemColor>();
        }
    }
}
