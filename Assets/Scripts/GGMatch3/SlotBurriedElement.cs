using System;

namespace GGMatch3
{
    public class SlotBurriedElement : SlotComponent
    {
        public override bool isBlockingChip
        {
            get
            {
                return false;
            }
        }

        public override bool isDestroyedByMatchingNextTo
        {
            get
            {
                return false;
            }
        }

        public override int blockerLevel
        {
            get
            {
                return 1;
            }
        }

        public override int sortingOrder
        {
            get
            {
                return 2;
            }
        }

        public BurriedElementBehaviour burriedElementBehaviour
        {
            get
            {
                TransformBehaviour componentBehaviour = base.GetComponentBehaviour<TransformBehaviour>();
                if (componentBehaviour == null)
                {
                    return null;
                }
                return componentBehaviour.GetComponent<BurriedElementBehaviour>();
            }
        }

        public void Init(LevelDefinition.BurriedElement elementDefinition)
        {
            this.elementDefinition = elementDefinition;
        }

        public override bool isMovingWithConveyor
        {
            get
            {
                return true;
            }
        }

        public override void AddToGoalsAtStart(Match3Goals goals)
        {
            goals.GetChipTypeCounter(new Match3Goals.ChipTypeDef
            {
                chipType = ChipType.BurriedElement,
                itemColor = ItemColor.Unknown
            }).countAtStart++;
        }

        public override bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
        {
            return chipTypeDef.chipType == ChipType.BurriedElement;
        }

        public override SlotDestroyResolution OnDestroySlotComponent(SlotDestroyParams destroyParams)
        {
            SlotDestroyResolution result = default(SlotDestroyResolution);
            if (this.slot.isBlockingBurriedElement)
            {
                return result;
            }
            result.isDestroyed = true;
            result.stopPropagation = false;
            Match3Game game = this.lastConnectedSlot.game;
            this.slot.RemoveComponent(this);
            Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
            chipTypeDef.chipType = ChipType.BurriedElement;
            chipTypeDef.itemColor = ItemColor.Unknown;
            Match3Goals.GoalBase activeGoal = game.goals.GetActiveGoal(chipTypeDef);
            GoalCollectParams goalResult = default(GoalCollectParams);

            goalResult.goal = activeGoal;
            CollectBurriedElementAction.CollectGoalParams collectParams = default(CollectBurriedElementAction.CollectGoalParams);
            collectParams.slotBurriedElement = this;
            collectParams.destroyParams = destroyParams;
            collectParams.game = game;
            collectParams.goal = activeGoal;
            collectParams.slotToLock = this.lastConnectedSlot;
            if (destroyParams != null)
            {
                collectParams.explosionCentre = destroyParams.explosionCentre;
            }
            CollectBurriedElementAction collectBurriedElementAction = new CollectBurriedElementAction();
            collectBurriedElementAction.Init(collectParams);
            game.board.actionManager.AddAction(collectBurriedElementAction);
            return result;
        }

        public LevelDefinition.BurriedElement elementDefinition;
    }
}
