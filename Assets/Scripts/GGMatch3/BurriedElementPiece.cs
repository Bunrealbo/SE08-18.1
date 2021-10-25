using System;
using UnityEngine;

namespace GGMatch3
{
    public class BurriedElementPiece
    {
        public void Wobble(WobbleAnimation.Settings settings)
        {
            if (settings == null)
            {
                return;
            }
            if (this.wobbleAnimation.isActive)
            {
                return;
            }
            this.wobbleAnimation.Init(settings, this.burriedElementBehaviour.GetComponent<TransformBehaviour>());
        }

        public BurriedElementPiece.Settings settings
        {
            get
            {
                return Match3Settings.instance.burriedElementPieceSettings;
            }
        }

        public TransformBehaviour transformBehaviour
        {
            get
            {
                if (this.burriedElementBehaviour == null)
                {
                    return null;
                }
                return this.burriedElementBehaviour.GetComponent<TransformBehaviour>();
            }
        }

        public void Init(Match3Game game, BurriedElements burriedElements, LevelDefinition.BurriedElement elementDefinition)
        {
            this.game = game;
            this.elementDefinition = elementDefinition;
            this.burriedElements = burriedElements;
            BurriedElementBehaviour burriedElementBehaviour = game.CreateBurriedElement();
            this.burriedElementBehaviour = burriedElementBehaviour;
            if (burriedElementBehaviour != null)
            {
                burriedElementBehaviour.Init(elementDefinition);
                GGUtil.SetActive(burriedElementBehaviour, true);
                TransformBehaviour component = burriedElementBehaviour.GetComponent<TransformBehaviour>();
                Vector3 a = game.LocalPositionOfCenter(elementDefinition.position);
                Vector3 b = game.LocalPositionOfCenter(elementDefinition.oppositeCornerPosition);
                Vector3 localPosition = Vector3.Lerp(a, b, 0.5f);
                component.localPosition = localPosition;
                component.SetColor(this.settings.colorWhenBurried);
            }
        }

        public bool IsCompatibleWithPickupGoal(Slot slot, Match3Goals.ChipTypeDef chipTypeDef)
        {
            return chipTypeDef.chipType == ChipType.BurriedElement && this.ContainsPosition(slot.position) && this.IsSlotBlocking(slot);
        }

        public bool ContainsPosition(IntVector2 position)
        {
            return this.elementDefinition.ContainsPosition(position);
        }

        public bool IsSlotBlocking(Slot slot)
        {
            return slot != null && slot.isBlockingBurriedElement && slot.totalBlockerLevel > 0;
        }

        private void SetColor(Color color)
        {
            TransformBehaviour transformBehaviour = this.transformBehaviour;
            if (transformBehaviour == null)
            {
                return;
            }
            transformBehaviour.SetColor(color);
        }

        public void OnSlateDestroyed(Slot destroyedSlot, SlotDestroyParams destroyParams)
        {
            IntVector2 position = this.elementDefinition.position;
            IntVector2 oppositeCornerPosition = this.elementDefinition.oppositeCornerPosition;
            int num = Mathf.Min(position.x, oppositeCornerPosition.x);
            int num2 = Mathf.Max(position.x, oppositeCornerPosition.x);
            int num3 = Mathf.Min(position.y, oppositeCornerPosition.y);
            int num4 = Mathf.Max(position.y, oppositeCornerPosition.y);
            if (Application.isEditor)
            {
                this.SetColor(this.settings.colorWhenBurried);
            }
            for (int i = num; i <= num2; i++)
            {
                for (int j = num3; j <= num4; j++)
                {
                    Slot slot = this.game.GetSlot(new IntVector2(i, j));
                    if (this.IsSlotBlocking(slot))
                    {
                        return;
                    }
                }
            }
            this.SetColor(this.settings.colorWhenDugOut);
            this.RemoveFromElements();
            Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
            chipTypeDef.chipType = ChipType.BurriedElement;
            chipTypeDef.itemColor = ItemColor.Unknown;
            Match3Goals.GoalBase activeGoal = this.game.goals.GetActiveGoal(chipTypeDef);
            GoalCollectParams resultGoal = default(GoalCollectParams);

            resultGoal.goal = activeGoal;
            CollectBurriedElementAction.CollectGoalParams collectParams = default(CollectBurriedElementAction.CollectGoalParams);
            collectParams.burriedElement = this;
            collectParams.destroyParams = null;
            collectParams.game = this.game;
            collectParams.goal = activeGoal;
            collectParams.slotToLock = destroyedSlot;
            if (destroyParams != null)
            {
                collectParams.explosionCentre = destroyParams.explosionCentre;
            }
            CollectBurriedElementAction collectBurriedElementAction = new CollectBurriedElementAction();
            collectBurriedElementAction.Init(collectParams);
            this.game.board.actionManager.AddAction(collectBurriedElementAction);
        }

        public void Update(float deltaTime)
        {
            this.OnSlateDestroyed(null, null);
            this.wobbleAnimation.Update(deltaTime);
        }

        private void RemoveFromElements()
        {
            this.burriedElements.Remove(this);
        }

        public void RemoveFromGame()
        {
            this.RemoveFromElements();
            if (this.burriedElementBehaviour != null)
            {
                this.burriedElementBehaviour.RemoveFromGame();
            }
        }

        public LevelDefinition.BurriedElement elementDefinition;

        private Match3Game game;

        public BurriedElementBehaviour burriedElementBehaviour;

        private BurriedElements burriedElements;

        private WobbleAnimation wobbleAnimation = new WobbleAnimation();

        [Serializable]
        public class Settings
        {
            public Color colorWhenBurried = Color.white;

            public Color colorWhenDugOut = Color.white;
        }
    }
}
