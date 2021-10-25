using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
    public class ConveyorBeltBoardComponent : BoardComponent
    {
        public ConveyorBeltBoardComponent.Settings settings
        {
            get
            {
                return Match3Settings.instance.conveyorBeltSettings;
            }
        }

        public bool isMoving
        {
            get
            {
                return this._003CisMoving_003Ek__BackingField;
            }
            protected set
            {
                this._003CisMoving_003Ek__BackingField = value;
            }
        }

        private void StartMove()
        {
            this.colorWhenStartMove = this.beh.GetColor();
            List<Slot> slots = this.GetSlotsToLock();
            this.globalLock.LockSlots(slots);
            this.movingTime = 0f;
            this.isMoving = true;
            for (int i = 0; i < this.moveList.Count; i++)
            {
                ConveyorBeltBoardComponent.Movement movement = this.moveList[i];
                movement.Reset();
                movement.moveComponents.Clear();
                for (int j = 0; j < movement.fromSlot.components.Count; j++)
                {
                    SlotComponent slotComponent = movement.fromSlot.components[j];
                    if (slotComponent.isMovingWithConveyor)
                    {
                        movement.moveComponents.Add(slotComponent);
                    }
                }
                List<SlotComponent> moveComponents = movement.moveComponents;
                for (int k = 0; k < moveComponents.Count; k++)
                {
                    moveComponents[k].RemoveFromSlot();
                }
            }
            for (int l = 0; l < this.moveList.Count; l++)
            {
                ConveyorBeltBoardComponent.Movement movement2 = this.moveList[l];
                List<SlotComponent> moveComponents2 = movement2.moveComponents;
                for (int m = 0; m < moveComponents2.Count; m++)
                {
                    SlotComponent c = moveComponents2[m];
                    movement2.toSlot.AddComponent(c);
                }
            }
        }

        private void SetPipeScale(PipeBehaviour pipe, float scale)
        {
            if (pipe == null)
            {
                return;
            }
            pipe.SetScale(scale);
        }

        public List<Slot> GetSlotsToLock()
        {
            this.slotsToLock.Clear();
            this.slotsToLock.AddRange(this.allSlotsList);
            return this.slotsToLock;
        }

        public List<Slot> GetSlotsToCheck()
        {
            this.slotsToCheck.Clear();
            this.slotsToCheck.AddRange(this.allSlotsList);
            return this.slotsToCheck;
        }

        private void UpdateMove(float deltaTime)
        {
            ConveyorBeltBoardComponent.Settings settings = this.settings;
            float duration = settings.duration;
            float delayBeforeMove = settings.delayBeforeMove;
            this.movingTime += deltaTime;
            if (this.movingTime < delayBeforeMove)
            {
                return;
            }
            float value = this.movingTime - delayBeforeMove;
            float num = Mathf.InverseLerp(0f, duration, value);
            float num2 = settings.travelCurve.Evaluate(num);
            this.beh.SetTile(num2);
            bool flag = false;
            for (int i = 0; i < this.moveList.Count; i++)
            {
                ConveyorBeltBoardComponent.Movement movement = this.moveList[i];
                List<SlotComponent> moveComponents = movement.moveComponents;
                Vector3 localPositionOfCenter = movement.fromSlot.localPositionOfCenter;
                Vector3 localPositionOfCenter2 = movement.toSlot.localPositionOfCenter;
                Vector3 localScale = Vector3.one;
                Vector3 vector = Vector3.LerpUnclamped(localPositionOfCenter, localPositionOfCenter2, num2);
                bool flag2 = false;
                if (movement.linearMoves.Count > 0)
                {
                    int currentLinearMoveIndex = movement.currentLinearMoveIndex;
                    float num3 = Mathf.InverseLerp(0f, settings.pipeDuration * movement.durationScale, value);
                    if (num3 < 1f)
                    {
                        flag = true;
                        flag2 = true;
                    }
                    num3 = settings.pipeTravelCurve.Evaluate(num3);
                    float num4 = Mathf.Lerp(0f, (float)movement.linearMoves.Count, num3);
                    int num5 = Mathf.FloorToInt(Mathf.Clamp(num4, 0f, (float)(movement.linearMoves.Count - 1)));
                    movement.currentLinearMoveIndex = num5;
                    for (int j = 0; j < num5; j++)
                    {
                        ConveyorBeltBoardComponent.LinearMove linearMove = movement.linearMoves[j];
                        if (linearMove.isStarted && !linearMove.isEnded)
                        {
                            linearMove.OnEnd(movement);
                        }
                    }
                    float num6 = Mathf.Clamp01(num4 - (float)num5);
                    PipeSettings pipeSettings = Match3Settings.instance.pipeSettings;
                    ConveyorBeltBoardComponent.LinearMove linearMove2 = movement.linearMoves[num5];
                    if (!linearMove2.isStarted)
                    {
                        linearMove2.OnStart(movement);
                        if (linearMove2.isJump)
                        {
                            for (int k = 0; k < moveComponents.Count; k++)
                            {
                                TransformBehaviour componentBehaviour = moveComponents[k].GetComponentBehaviour<TransformBehaviour>();
                                if (!(componentBehaviour == null))
                                {
                                    componentBehaviour.SaveSortingLayerSettings();
                                    componentBehaviour.SetSortingLayer(settings.sortingSettings);
                                }
                            }
                        }
                        this.SetPipeScale(linearMove2.pipe, settings.pipeScale);
                        if (linearMove2.pipe != null)
                        {
                            linearMove2.pipe.SetScale(Match3Settings.instance.pipeSettings.pipeScale);
                        }
                        if (linearMove2.pipe == this.beh.entrancePipe)
                        {
                            this.game.particles.CreateParticlesWorld(linearMove2.pipe.exitTransform.position, Match3Particles.PositionType.PipeEnterParticle, ChipType.Chip, ItemColor.Unknown);
                        }
                        if (linearMove2.pipe == this.beh.exitPipe)
                        {
                            this.game.particles.CreateParticlesWorld(linearMove2.pipe.exitTransform.position, Match3Particles.PositionType.PipeExitParticle, ChipType.Chip, ItemColor.Unknown);
                        }
                    }
                    vector = Vector3.LerpUnclamped(linearMove2.startPosition, linearMove2.endPosition, num6);
                    if (linearMove2.pipe != null)
                    {
                        float t = pipeSettings.offsetCurve.Evaluate(Mathf.PingPong(num6, 0.5f));
                        linearMove2.pipe.SetOffsetPosition(Vector3.Lerp(Vector3.zero, pipeSettings.offsetPosition, t));
                    }
                    Vector3 vector2 = linearMove2.endPosition - linearMove2.startPosition;
                    float scale = Match3Settings.instance.pipeSettings.scale;
                    float scale2 = Match3Settings.instance.pipeSettings.scale;
                    if (Mathf.Abs(vector2.x) > 0f)
                    {
                        localScale.y = scale;
                        localScale.x = scale2;
                    }
                    else
                    {
                        localScale.y = scale2;
                        localScale.x = scale;
                    }
                    if (linearMove2.isJump)
                    {
                        vector = Vector3.LerpUnclamped(linearMove2.startPosition, linearMove2.endPosition, settings.jumpDistanceCurve.Evaluate(num6));
                        float num7 = Mathf.PingPong(num6, 0.5f);
                        num7 = settings.jumpUpCurve.Evaluate(num7);
                        localScale = Vector3.Lerp(Vector3.one, settings.jumpScale, num7);
                        vector += Vector3.Cross(vector2, Vector3.forward).normalized * Mathf.Lerp(0f, settings.orthoDirectionUp, num7);
                    }
                }
                else if (num < 1f)
                {
                    flag = true;
                    flag2 = true;
                }
                if (!movement.isEnded)
                {
                    for (int l = 0; l < moveComponents.Count; l++)
                    {
                        TransformBehaviour componentBehaviour2 = moveComponents[l].GetComponentBehaviour<TransformBehaviour>();
                        if (!(componentBehaviour2 == null))
                        {
                            componentBehaviour2.localPosition = vector;
                            componentBehaviour2.localScale = localScale;
                        }
                    }
                    if (!flag2 && !movement.isEnded)
                    {
                        movement.isEnded = true;
                        if (movement.linearMoves.Count > 0)
                        {
                            for (int m = 0; m < movement.linearMoves.Count; m++)
                            {
                                ConveyorBeltBoardComponent.LinearMove linearMove3 = movement.linearMoves[m];
                                if (!linearMove3.isEnded)
                                {
                                    linearMove3.isEnded = true;
                                    linearMove3.OnEnd(movement);
                                    linearMove3.pipe = this.beh.exitPipe;
                                }
                            }
                        }
                        this.globalLock.Unlock(movement.toSlot);
                    }
                }
            }
            if (!this.conveyorBeltDef.isLoop)
            {
                Mathf.Max(duration, settings.pipeDuration);
            }
            Color color = Color.Lerp(this.colorWhenStartMove, settings.colorWhenMoved, num);
            this.beh.SetColor(color);
            if (flag)
            {
                return;
            }
            this.beh.SetColor(settings.colorWhenMoved);
            this.isMoving = false;
            this.GetSlotsToLock();
            this.globalLock.UnlockAll();
            for (int n = 0; n < this.moveList.Count; n++)
            {
                ConveyorBeltBoardComponent.Movement movement2 = this.moveList[n];
                for (int num8 = 0; num8 < movement2.moveComponents.Count; num8++)
                {
                    TransformBehaviour componentBehaviour3 = movement2.moveComponents[num8].GetComponentBehaviour<TransformBehaviour>();
                    if (!(componentBehaviour3 == null))
                    {
                        componentBehaviour3.localScale = Vector3.one;
                    }
                }
                for (int num9 = 0; num9 < movement2.linearMoves.Count; num9++)
                {
                    ConveyorBeltBoardComponent.LinearMove linearMove4 = movement2.linearMoves[num9];
                    if (linearMove4.isStarted && !linearMove4.isEnded)
                    {
                        linearMove4.OnEnd(movement2);
                        if (linearMove4.pipe == this.beh.exitPipe)
                        {
                            this.game.particles.CreateParticlesWorld(linearMove4.pipe.exitTransform.position, Match3Particles.PositionType.PipeExitParticle, ChipType.Chip, ItemColor.Unknown);
                        }
                    }
                }
            }
            this.game.ApplySlotGravityForAllSlots();
        }

        public void Init(Match3Game game, LevelDefinition.ConveyorBelt conveyorBeltDef, ConveyorBeltBehaviour beh)
        {
            this.game = game;
            this.conveyorBeltDef = conveyorBeltDef;
            this.beh = beh;
            List<LevelDefinition.ConveyorBeltLinearSegment> segmentList = conveyorBeltDef.segmentList;
            for (int i = 0; i < segmentList.Count; i++)
            {
                List<LevelDefinition.SlotDefinition> slotList = segmentList[i].slotList;
                for (int j = 0; j < slotList.Count; j++)
                {
                    LevelDefinition.SlotDefinition slotDefinition = slotList[j];
                    Slot slot = game.GetSlot(slotDefinition.position);
                    this.allSlotsList.Add(slot);
                }
            }
            this.globalLock = this.lockContainer.NewLock();
            this.globalLock.isAvailableForDiscoBombSuspended = true;
            this.globalLock.isSlotMatchingSuspended = true;
            this.globalLock.isChipGeneratorSuspended = true;
            this.globalLock.isDestroySuspended = true;
            this.globalLock.isSlotGravitySuspended = true;
            for (int k = 0; k < this.allSlotsList.Count; k++)
            {
                Slot fromSlot = this.allSlotsList[k];
                Slot toSlot = this.allSlotsList[(k + 1) % this.allSlotsList.Count];
                ConveyorBeltBoardComponent.Movement movement = new ConveyorBeltBoardComponent.Movement();
                movement.fromSlot = fromSlot;
                movement.toSlot = toSlot;
                this.moveList.Add(movement);
                int count = this.allSlotsList.Count;
            }
            if (conveyorBeltDef.isLoop)
            {
                return;
            }
            ConveyorBeltBoardComponent.Movement movement2 = this.moveList[this.moveList.Count - 1];
            if (this.settings.shouldJump)
            {
                ConveyorBeltBoardComponent.LinearMove linearMove = new ConveyorBeltBoardComponent.LinearMove();
                linearMove.startPosition = game.LocalPositionOfCenter(movement2.fromSlot.position);
                linearMove.isJump = true;
                linearMove.pipe = null;
                linearMove.endPosition = movement2.toSlot.localPositionOfCenter;
                movement2.linearMoves.Add(linearMove);
                movement2.durationScale = 3f;
            }
            else
            {
                ConveyorBeltBoardComponent.LinearMove linearMove2 = new ConveyorBeltBoardComponent.LinearMove();
                linearMove2.startPosition = movement2.fromSlot.localPositionOfCenter;
                IntVector2 direction = conveyorBeltDef.lastSegment.direction;
                linearMove2.endPosition = game.LocalPositionOfCenter(movement2.fromSlot.position + direction);
                linearMove2.pipe = null;
                linearMove2.resetVisuallyWhenStart = true;
                movement2.linearMoves.Add(linearMove2);
                linearMove2 = new ConveyorBeltBoardComponent.LinearMove();
                direction = conveyorBeltDef.lastSegment.direction;
                int b = game.SlotsDistanceToEndOfBoard(movement2.fromSlot.position, direction) + this.settings.slotsToExtendPastEndOfBoard;
                b = 1;
                linearMove2.startPosition = game.LocalPositionOfCenter(movement2.fromSlot.position + direction);
                linearMove2.endPosition = game.LocalPositionOfCenter(movement2.fromSlot.position + direction * b);
                linearMove2.pipe = beh.entrancePipe;
                movement2.linearMoves.Add(linearMove2);
                linearMove2 = new ConveyorBeltBoardComponent.LinearMove();
                IntVector2 direction2 = conveyorBeltDef.firstSegment.direction;
                b = game.SlotsDistanceToEndOfBoard(movement2.toSlot.position, -direction2) + this.settings.slotsToExtendPastEndOfBoard;
                b = 1;
                linearMove2.startPosition = game.LocalPositionOfCenter(movement2.toSlot.position - direction2 * b);
                linearMove2.endPosition = movement2.toSlot.localPositionOfCenter;
                linearMove2.resetVisuallyWhenStart = true;
                linearMove2.pipe = beh.exitPipe;
                movement2.linearMoves.Add(linearMove2);
                movement2.durationScale = 2f;
            }
            beh.SetColor(this.settings.colorWhenMoved);
        }

        public int lastMoveConveyorTookAction
        {
            get
            {
                return this.movesCountWhenTookAction;
            }
        }

        public bool IsMoveConveyorSuspended()
        {
            Slot[] slots = this.game.board.slots;
            float minTimeNotMoveBeforeCanStartConveyor = this.settings.minTimeNotMoveBeforeCanStartConveyor;
            if (this.game.input.isActive)
            {
                return true;
            }
            List<Slot> list = this.GetSlotsToCheck();
            FindMatches findMatches = this.game.board.findMatches;
            for (int i = 0; i < list.Count; i++)
            {
                Slot slot = list[i];
                if (findMatches.matches.GetIsland(slot.position) != null)
                {
                    return true;
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                Slot slot2 = list[j];
                if (slot2 != null)
                {
                    if (slot2.isMoveByConveyorSuspended)
                    {
                        return true;
                    }
                    if (slot2.isReacheableByGeneratorOrChip)
                    {
                        bool isEmpty = slot2.isEmpty;
                    }
                    if (slot2.isReacheableByGeneratorOrChip && slot2.isEmpty)
                    {
                        return true;
                    }
                    List<SlotComponent> components = slot2.components;
                    for (int k = 0; k < components.Count; k++)
                    {
                        SlotComponent slotComponent = components[k];
                        if (this.game.board.currentTime - slotComponent.lastMoveTime < minTimeNotMoveBeforeCanStartConveyor)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool needsToActivateConveyor
        {
            get
            {
                return this.game.board.userMovesCount > this.movesCountWhenTookAction;
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (this.isMoving)
            {
                this.UpdateMove(deltaTime);
                return;
            }
            this.lockContainer.UnlockAll();
            if (this.game.isConveyorSuspended)
            {
                return;
            }
            if (this.game.board.userMovesCount <= this.movesCountWhenTookAction)
            {
                return;
            }
            Color color = Color.Lerp(this.beh.GetColor(), this.settings.colorWhenActive, this.settings.colorChangeSpeed * deltaTime);
            this.beh.SetColor(color);
            if (this.game.board.actionManager.ActionCount > 0)
            {
                return;
            }
            if (this.game.board.isAnyConveyorMoveSuspended)
            {
                return;
            }
            this.movesCountWhenTookAction = this.game.board.userMovesCount;
            this.StartMove();
        }

        private Match3Game game;

        private LevelDefinition.ConveyorBelt conveyorBeltDef;

        private ConveyorBeltBehaviour beh;

        private int movesCountWhenTookAction;

        private LockContainer lockContainer = new LockContainer();

        private Lock globalLock;

        private List<ConveyorBeltBoardComponent.Movement> moveList = new List<ConveyorBeltBoardComponent.Movement>();

        private float movingTime;

        private bool _003CisMoving_003Ek__BackingField;

        private Color colorWhenStartMove;

        private List<Slot> slotsToLock = new List<Slot>();

        private List<Slot> slotsToCheck = new List<Slot>();

        private List<Slot> allSlotsList = new List<Slot>();

        [Serializable]
        public class Settings
        {
            public int slotsToExtendPastEndOfBoard = 2;

            public float delayBeforeMove;

            public float duration = 1f;

            public float pipeDuration = 1f;

            public AnimationCurve travelCurve;

            public AnimationCurve pipeTravelCurve;

            public float pipeScale = 0.95f;

            public float minTimeNotMoveBeforeCanStartConveyor = 0.4f;

            public Color colorWhenActive = Color.white;

            public Color colorWhenMoved = Color.white;

            public float colorChangeSpeed = 5f;

            public bool shouldJump;

            public float orthoDirectionUp;

            public Vector3 jumpScale;

            public SpriteSortingSettings sortingSettings;

            public AnimationCurve jumpUpCurve;

            public AnimationCurve jumpDistanceCurve;
        }

        public class LinearMove
        {
            public void Reset()
            {
                this.isStarted = false;
                this.isEnded = false;
            }

            public void OnStart(ConveyorBeltBoardComponent.Movement m)
            {
                this.isStarted = true;
                if (!this.resetVisuallyWhenStart)
                {
                    return;
                }
                for (int i = 0; i < m.moveComponents.Count; i++)
                {
                    ChipBehaviour componentBehaviour = m.moveComponents[i].GetComponentBehaviour<ChipBehaviour>();
                    if (!(componentBehaviour == null))
                    {
                        componentBehaviour.hasBounce = false;
                    }
                }
            }

            public void OnEnd(ConveyorBeltBoardComponent.Movement m)
            {
                this.isEnded = true;
                for (int i = 0; i < m.moveComponents.Count; i++)
                {
                    ChipBehaviour componentBehaviour = m.moveComponents[i].GetComponentBehaviour<ChipBehaviour>();
                    if (!(componentBehaviour == null))
                    {
                        componentBehaviour.hasBounce = true;
                    }
                }
                if (this.isJump)
                {
                    for (int j = 0; j < m.moveComponents.Count; j++)
                    {
                        TransformBehaviour componentBehaviour2 = m.moveComponents[j].GetComponentBehaviour<TransformBehaviour>();
                        if (!(componentBehaviour2 == null))
                        {
                            componentBehaviour2.ResetSortingLayerSettings();
                        }
                    }
                }
                if (this.pipe == null)
                {
                    return;
                }
                this.pipe.SetScale(1f);
                this.pipe.SetOffsetPosition(Vector3.zero);
            }

            public Vector3 startPosition;

            public Vector3 endPosition;

            public bool isStarted;

            public bool isEnded;

            public bool isJump;

            public bool resetVisuallyWhenStart;

            public PipeBehaviour pipe;
        }

        public class Movement
        {
            public void Reset()
            {
                this.currentLinearMoveIndex = 0;
                this.isEnded = false;
                for (int i = 0; i < this.linearMoves.Count; i++)
                {
                    this.linearMoves[i].Reset();
                }
            }

            public Slot fromSlot;

            public Slot toSlot;

            public bool isEnded;

            public List<SlotComponent> moveComponents = new List<SlotComponent>();

            public List<ConveyorBeltBoardComponent.LinearMove> linearMoves = new List<ConveyorBeltBoardComponent.LinearMove>();

            public int currentLinearMoveIndex;

            public float durationScale = 1f;
        }
    }
}
