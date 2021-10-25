using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
    public class CollectBurriedElementAction : BoardAction
    {
        private CollectBurriedElementAction.Settings settings
        {
            get
            {
                return Match3Settings.instance.collectBurriedEelementSettings;
            }
        }

        public void Init(CollectBurriedElementAction.CollectGoalParams collectParams)
        {
            this.collectParams = collectParams;
            this.globalLock = this.lockContainer.NewLock();
            this.globalLock.isSlotGravitySuspended = true;
            this.globalLock.isChipGeneratorSuspended = true;
            this.globalLock.LockSlot(collectParams.slotToLock);
        }

        private IEnumerator ScalePart()
        {
            return new CollectBurriedElementAction._003CScalePart_003Ed__13(0)
            {
                _003C_003E4__this = this
            };
        }

        private float TravelDuration(Vector3 startPos, Vector3 endPos)
        {
            float num = this.settings.travelDuration;
            if (this.settings.travelSpeed > 0f)
            {
                num = Vector3.Distance(startPos, endPos) / this.settings.travelSpeed;
                num = Mathf.Clamp(num, this.settings.minTime, this.settings.maxTime);
            }
            return num;
        }

        private IEnumerator TravelJumpPart()
        {
            return new CollectBurriedElementAction._003CTravelJumpPart_003Ed__15(0)
            {
                _003C_003E4__this = this
            };
        }

        private IEnumerator TravelPart()
        {
            return new CollectBurriedElementAction._003CTravelPart_003Ed__16(0)
            {
                _003C_003E4__this = this
            };
        }

        private IEnumerator DoAnimation()
        {
            return new CollectBurriedElementAction._003CDoAnimation_003Ed__17(0)
            {
                _003C_003E4__this = this
            };
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            this.deltaTime = deltaTime;
            this.lockedTime += deltaTime;
            if (this.animationEnum == null)
            {
                this.animationEnum = this.DoAnimation();
            }
            this.animationEnum.MoveNext();
            bool flag = this.lockedTime > this.settings.timeToLockSlot;
            if (!this.isUnlocked && flag)
            {
                this.isUnlocked = true;
                this.globalLock.UnlockAll();
            }
        }

        private GameObject travelParticles;

        private CollectBurriedElementAction.CollectGoalParams collectParams;

        private float deltaTime;

        private IEnumerator animationEnum;

        private Lock globalLock;

        private float lockedTime;

        private bool isUnlocked;

        private float scaleUpAngle;

        public struct CollectGoalParams
        {
            public void RemoveFromGame()
            {
                if (this.burriedElement != null)
                {
                    this.burriedElement.RemoveFromGame();
                }
                if (this.slotBurriedElement != null)
                {
                    this.slotBurriedElement.RemoveFromGame();
                }
            }

            public LevelDefinition.BurriedElement burriedElementDefinition
            {
                get
                {
                    if (this.burriedElement != null)
                    {
                        return this.burriedElement.elementDefinition;
                    }
                    if (this.slotBurriedElement != null)
                    {
                        return this.slotBurriedElement.elementDefinition;
                    }
                    return null;
                }
            }

            public BurriedElementBehaviour burriedElementBehaviour
            {
                get
                {
                    if (this.burriedElement != null)
                    {
                        return this.burriedElement.burriedElementBehaviour;
                    }
                    if (this.slotBurriedElement != null)
                    {
                        return this.slotBurriedElement.burriedElementBehaviour;
                    }
                    return null;
                }
            }

            public Quaternion localRotation
            {
                get
                {
                    if (this.burriedElementBehaviour == null)
                    {
                        return Quaternion.identity;
                    }
                    return this.burriedElementBehaviour.rotationTransform.localRotation;
                }
                set
                {
                    if (this.burriedElementBehaviour == null)
                    {
                        return;
                    }
                    this.burriedElementBehaviour.rotationTransform.localRotation = value;
                }
            }

            public TransformBehaviour transformBehaviour
            {
                get
                {
                    if (this.slotBurriedElement != null)
                    {
                        return this.slotBurriedElement.GetComponentBehaviour<TransformBehaviour>();
                    }
                    if (this.burriedElement != null)
                    {
                        return this.burriedElement.transformBehaviour;
                    }
                    return null;
                }
            }

            public Match3Goals.GoalBase goal;

            public Match3Game game;

            public BurriedElementPiece burriedElement;

            public SlotBurriedElement slotBurriedElement;

            public SlotDestroyParams destroyParams;

            public Slot slotToLock;

            public IntVector2 explosionCentre;
        }

        [Serializable]
        public class Settings
        {
            public SpriteSortingSettings sortingLayer = new SpriteSortingSettings();

            public SpriteSortingSettings sortingLayerFly = new SpriteSortingSettings();

            public float travelDuration = 1f;

            public float additionalParticlesDuration = 0.5f;

            public float travelSpeed;

            public float maxTime = 10f;

            public float minTime;

            public AnimationCurve travelCurve;

            public float scaleUpScale = 1f;

            public float scaleUpScalEndRange = 1f;

            public float scaleUpDuration = 1f;

            public AnimationCurve scaleUpCurve;

            public AnimationCurve rotationCurve;

            public float timeToLockSlot;

            public float rotationAngle;

            public float scaleUpAngle;

            public float scaleUpAngleRangeEnd;

            public AnimationCurve bombCurve;

            public float distance;

            public bool useColor;

            public Color color;

            public float brightness;

            public float ortoDistance;

            public AnimationCurve ortoCurve;

            public bool useTravelScaleCurve;

            public AnimationCurve travelScaleCurve;

            public bool useJump;

            public float jumpDuration;

            public Vector3 jumpOffset;

            public AnimationCurve jumpTravelCurve;

            public float angleSpeed;

            public float jumpScale1;

            public AnimationCurve jumpScale1Curve;

            public float jumpScale2;

            public AnimationCurve jumpScale2Curve;
        }

        private sealed class _003CScalePart_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CScalePart_003Ed__13(int _003C_003E1__state)
            {
                this._003C_003E1__state = _003C_003E1__state;
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
            }

            bool IEnumerator.MoveNext()
            {
                int num = this._003C_003E1__state;
                CollectBurriedElementAction collectBurriedElementAction = this._003C_003E4__this;
                if (num != 0)
                {
                    if (num != 1)
                    {
                        return false;
                    }
                    this._003C_003E1__state = -1;
                    if (this._003Ctime_003E5__12 >= this._003Cduration_003E5__13)
                    {
                        return false;
                    }
                }
                else
                {
                    this._003C_003E1__state = -1;
                    this._003CtransformBehaviour_003E5__2 = collectBurriedElementAction.collectParams.transformBehaviour;
                    this._003CstartRotation_003E5__3 = Quaternion.identity;
                    if (this._003CtransformBehaviour_003E5__2 != null)
                    {
                        this._003CstartRotation_003E5__3 = collectBurriedElementAction.collectParams.localRotation;
                    }
                    this._003CendRotation_003E5__4 = Quaternion.identity;
                    this._003Csettings_003E5__5 = collectBurriedElementAction.settings;
                    this._003CstartScale_003E5__6 = Vector3.one;
                    if (this._003CtransformBehaviour_003E5__2 != null)
                    {
                        this._003CstartScale_003E5__6 = this._003CtransformBehaviour_003E5__2.localScale;
                        if (this._003Csettings_003E5__5.useColor)
                        {
                            this._003CtransformBehaviour_003E5__2.SetColor(this._003Csettings_003E5__5.color);
                            this._003CtransformBehaviour_003E5__2.SetBrightness(this._003Csettings_003E5__5.brightness);
                        }
                    }
                    float num2 = UnityEngine.Random.Range(this._003Csettings_003E5__5.scaleUpScale, this._003Csettings_003E5__5.scaleUpScalEndRange);
                    this._003CendScale_003E5__7 = new Vector3(num2, num2, 1f);
                    this._003ClocalPosition_003E5__8 = Vector3.zero;
                    if (this._003CtransformBehaviour_003E5__2 != null)
                    {
                        this._003ClocalPosition_003E5__8 = this._003CtransformBehaviour_003E5__2.localPosition;
                    }
                    this._003Cdirection_003E5__9 = (this._003ClocalPosition_003E5__8 - collectBurriedElementAction.collectParams.game.LocalPositionOfCenter(collectBurriedElementAction.collectParams.explosionCentre)).normalized;
                    this._003CstartAngle_003E5__10 = 0f;
                    this._003CendAngle_003E5__11 = UnityEngine.Random.Range(this._003Csettings_003E5__5.scaleUpAngle, this._003Csettings_003E5__5.scaleUpAngleRangeEnd);
                    collectBurriedElementAction.scaleUpAngle = this._003CendAngle_003E5__11;
                    this._003Ctime_003E5__12 = 0f;
                    this._003Cduration_003E5__13 = this._003Csettings_003E5__5.scaleUpDuration;
                }
                this._003Ctime_003E5__12 += collectBurriedElementAction.deltaTime;
                float num3 = Mathf.InverseLerp(0f, this._003Cduration_003E5__13, this._003Ctime_003E5__12);
                num3 = this._003Csettings_003E5__5.scaleUpCurve.Evaluate(num3);
                float t = this._003Csettings_003E5__5.rotationCurve.Evaluate(num3);
                float t2 = this._003Csettings_003E5__5.bombCurve.Evaluate(num3);
                Vector3 localScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__6, this._003CendScale_003E5__7, num3);
                float angle = Mathf.Lerp(this._003CstartAngle_003E5__10, this._003CendAngle_003E5__11, num3);
                if (this._003CtransformBehaviour_003E5__2 != null)
                {
                    this._003CtransformBehaviour_003E5__2.localScale = localScale;
                    this._003CtransformBehaviour_003E5__2.localRotationOffset = Quaternion.AngleAxis(angle, Vector3.forward);
                    this._003CtransformBehaviour_003E5__2.localPosition = Vector3.LerpUnclamped(this._003ClocalPosition_003E5__8, this._003ClocalPosition_003E5__8 + this._003Cdirection_003E5__9 * this._003Csettings_003E5__5.distance, t2);
                }
                collectBurriedElementAction.collectParams.localRotation = Quaternion.LerpUnclamped(this._003CstartRotation_003E5__3, this._003CendRotation_003E5__4, t);
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            private int _003C_003E1__state;

            private object _003C_003E2__current;

            public CollectBurriedElementAction _003C_003E4__this;

            private TransformBehaviour _003CtransformBehaviour_003E5__2;

            private Quaternion _003CstartRotation_003E5__3;

            private Quaternion _003CendRotation_003E5__4;

            private CollectBurriedElementAction.Settings _003Csettings_003E5__5;

            private Vector3 _003CstartScale_003E5__6;

            private Vector3 _003CendScale_003E5__7;

            private Vector3 _003ClocalPosition_003E5__8;

            private Vector3 _003Cdirection_003E5__9;

            private float _003CstartAngle_003E5__10;

            private float _003CendAngle_003E5__11;

            private float _003Ctime_003E5__12;

            private float _003Cduration_003E5__13;
        }

        private sealed class _003CTravelJumpPart_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CTravelJumpPart_003Ed__15(int _003C_003E1__state)
            {
                this._003C_003E1__state = _003C_003E1__state;
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
            }

            bool IEnumerator.MoveNext()
            {
                int num = this._003C_003E1__state;
                CollectBurriedElementAction collectBurriedElementAction = this._003C_003E4__this;
                switch (num)
                {
                    case 0:
                        {
                            this._003C_003E1__state = -1;
                            this._003CtransformBehaviour_003E5__2 = collectBurriedElementAction.collectParams.transformBehaviour;
                            this._003Csettings_003E5__3 = collectBurriedElementAction.settings;
                            Vector3 vector = Vector3.zero;
                            this._003CstartScale_003E5__4 = Vector3.one;
                            if (this._003CtransformBehaviour_003E5__2 != null)
                            {
                                vector = this._003CtransformBehaviour_003E5__2.localPosition;
                                this._003CstartScale_003E5__4 = this._003CtransformBehaviour_003E5__2.localScale;
                            }
                            this._003Cangle_003E5__5 = this._003Csettings_003E5__3.scaleUpAngle;
                            this._003CstartLocalPos_003E5__6 = vector;
                            Match3Game game = collectBurriedElementAction.collectParams.game;
                            GoalsPanelGoal goal = game.gameScreen.goalsPanel.GetGoal(collectBurriedElementAction.collectParams.goal);
                            this._003CendLocalPos_003E5__7 = this._003Csettings_003E5__3.jumpOffset + this._003CstartLocalPos_003E5__6;
                            this._003CendLocalPos_003E5__7.z = 0f;
                            this._003CuiEndLocalPos_003E5__8 = Vector3.zero;
                            if (this._003CtransformBehaviour_003E5__2 != null)
                            {
                                this._003CuiEndLocalPos_003E5__8 = this._003CtransformBehaviour_003E5__2.WorldToLocalPosition(game.gameScreen.goalsPanel.transform.position);
                                if (goal != null)
                                {
                                    this._003CuiEndLocalPos_003E5__8 = this._003CtransformBehaviour_003E5__2.WorldToLocalPosition(goal.transform.position);
                                }
                            }
                            this._003CuiEndLocalPos_003E5__8.z = 0f;
                            this._003Ctime_003E5__9 = 0f;
                            this._003Cduration_003E5__10 = this._003Csettings_003E5__3.jumpDuration;
                            float scaleUpAngle = collectBurriedElementAction.scaleUpAngle;
                            float rotationAngle = this._003Csettings_003E5__3.rotationAngle;
                            this._003CendScale_003E5__11 = this._003Csettings_003E5__3.jumpScale1 * Vector3.one;
                            break;
                        }
                    case 1:
                        this._003C_003E1__state = -1;
                        if (this._003Ctime_003E5__9 >= this._003Cduration_003E5__10)
                        {
                            this._003CstartScale_003E5__4 = this._003CendScale_003E5__11;
                            this._003CendScale_003E5__11 = this._003Csettings_003E5__3.jumpScale2 * Vector3.one;
                            this._003Ctime_003E5__9 = 0f;
                            this._003Cduration_003E5__10 = collectBurriedElementAction.TravelDuration(this._003CendLocalPos_003E5__7, this._003CuiEndLocalPos_003E5__8);
                            goto IL_32C;
                        }
                        break;
                    case 2:
                        this._003C_003E1__state = -1;
                        if (this._003Ctime_003E5__9 >= this._003Cduration_003E5__10)
                        {
                            return false;
                        }
                        goto IL_32C;
                    default:
                        return false;
                }
                this._003Ctime_003E5__9 += collectBurriedElementAction.deltaTime;
                float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__10, this._003Ctime_003E5__9);
                float t = this._003Csettings_003E5__3.jumpTravelCurve.Evaluate(time);
                Vector3 localPosition = Vector3Ex.CatmullRomLerp(this._003CstartLocalPos_003E5__6, this._003CstartLocalPos_003E5__6, this._003CendLocalPos_003E5__7, this._003CuiEndLocalPos_003E5__8, t);
                this._003Cangle_003E5__5 += this._003Csettings_003E5__3.angleSpeed * collectBurriedElementAction.deltaTime;
                float t2 = this._003Csettings_003E5__3.jumpScale1Curve.Evaluate(time);
                Vector3 localScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__4, this._003CendScale_003E5__11, t2);
                if (this._003CtransformBehaviour_003E5__2 != null)
                {
                    this._003CtransformBehaviour_003E5__2.localPosition = localPosition;
                    this._003CtransformBehaviour_003E5__2.localRotationOffset = Quaternion.AngleAxis(this._003Cangle_003E5__5, Vector3.forward);
                    this._003CtransformBehaviour_003E5__2.localScale = localScale;
                }
                if (collectBurriedElementAction.travelParticles != null)
                {
                    collectBurriedElementAction.travelParticles.transform.localPosition = localPosition;
                }
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
                IL_32C:
                this._003Ctime_003E5__9 += collectBurriedElementAction.deltaTime;
                float time2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__10, this._003Ctime_003E5__9);
                float t3 = this._003Csettings_003E5__3.travelCurve.Evaluate(time2);
                this._003Cangle_003E5__5 += this._003Csettings_003E5__3.angleSpeed * collectBurriedElementAction.deltaTime;
                Vector3 localPosition2 = Vector3Ex.CatmullRomLerp(this._003CstartLocalPos_003E5__6, this._003CendLocalPos_003E5__7, this._003CuiEndLocalPos_003E5__8, this._003CuiEndLocalPos_003E5__8, t3);
                float t4 = this._003Csettings_003E5__3.jumpScale1Curve.Evaluate(time2);
                Vector3 localScale2 = Vector3.LerpUnclamped(this._003CstartScale_003E5__4, this._003CendScale_003E5__11, t4);
                if (this._003CtransformBehaviour_003E5__2 != null)
                {
                    this._003CtransformBehaviour_003E5__2.localPosition = localPosition2;
                    this._003CtransformBehaviour_003E5__2.localRotationOffset = Quaternion.AngleAxis(this._003Cangle_003E5__5, Vector3.forward);
                    this._003CtransformBehaviour_003E5__2.localScale = localScale2;
                }
                if (collectBurriedElementAction.travelParticles != null)
                {
                    collectBurriedElementAction.travelParticles.transform.localPosition = localPosition2;
                }
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            private int _003C_003E1__state;

            private object _003C_003E2__current;

            public CollectBurriedElementAction _003C_003E4__this;

            private TransformBehaviour _003CtransformBehaviour_003E5__2;

            private CollectBurriedElementAction.Settings _003Csettings_003E5__3;

            private Vector3 _003CstartScale_003E5__4;

            private float _003Cangle_003E5__5;

            private Vector3 _003CstartLocalPos_003E5__6;

            private Vector3 _003CendLocalPos_003E5__7;

            private Vector3 _003CuiEndLocalPos_003E5__8;

            private float _003Ctime_003E5__9;

            private float _003Cduration_003E5__10;

            private Vector3 _003CendScale_003E5__11;
        }

        private sealed class _003CTravelPart_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CTravelPart_003Ed__16(int _003C_003E1__state)
            {
                this._003C_003E1__state = _003C_003E1__state;
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
            }

            bool IEnumerator.MoveNext()
            {
                int num = this._003C_003E1__state;
                CollectBurriedElementAction collectBurriedElementAction = this._003C_003E4__this;
                if (num != 0)
                {
                    if (num != 1)
                    {
                        return false;
                    }
                    this._003C_003E1__state = -1;
                    if (this._003Ctime_003E5__7 >= this._003Cduration_003E5__8)
                    {
                        return false;
                    }
                }
                else
                {
                    this._003C_003E1__state = -1;
                    this._003CtransformBehaviour_003E5__2 = collectBurriedElementAction.collectParams.transformBehaviour;
                    this._003Csettings_003E5__3 = collectBurriedElementAction.settings;
                    Vector3 vector = Vector3.zero;
                    this._003CstartScale_003E5__4 = Vector3.one;
                    if (this._003CtransformBehaviour_003E5__2 != null)
                    {
                        vector = this._003CtransformBehaviour_003E5__2.localPosition;
                        this._003CstartScale_003E5__4 = this._003CtransformBehaviour_003E5__2.localScale;
                    }
                    this._003CstartLocalPos_003E5__5 = vector;
                    Match3Game game = collectBurriedElementAction.collectParams.game;
                    GoalsPanelGoal goal = game.gameScreen.goalsPanel.GetGoal(collectBurriedElementAction.collectParams.goal);
                    this._003CendLocalPos_003E5__6 = this._003CstartLocalPos_003E5__5;
                    if (this._003CtransformBehaviour_003E5__2 != null)
                    {
                        this._003CendLocalPos_003E5__6 = this._003CtransformBehaviour_003E5__2.WorldToLocalPosition(game.gameScreen.goalsPanel.transform.position);
                        if (goal != null)
                        {
                            this._003CendLocalPos_003E5__6 = this._003CtransformBehaviour_003E5__2.WorldToLocalPosition(goal.transform.position);
                        }
                    }
                    this._003CendLocalPos_003E5__6.z = 0f;
                    this._003Ctime_003E5__7 = 0f;
                    this._003Cduration_003E5__8 = collectBurriedElementAction.TravelDuration(this._003CstartLocalPos_003E5__5, this._003CendLocalPos_003E5__6);
                    this._003CstartAngle_003E5__9 = collectBurriedElementAction.scaleUpAngle;
                    this._003CendAngle_003E5__10 = this._003Csettings_003E5__3.rotationAngle;
                }
                this._003Ctime_003E5__7 += collectBurriedElementAction.deltaTime;
                float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__8, this._003Ctime_003E5__7);
                float t = this._003Csettings_003E5__3.travelCurve.Evaluate(time);
                float angle = Mathf.Lerp(this._003CstartAngle_003E5__9, this._003CendAngle_003E5__10, t);
                Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartLocalPos_003E5__5, this._003CendLocalPos_003E5__6, t);
                if (this._003Csettings_003E5__3.ortoDistance != 0f)
                {
                    float t2 = this._003Csettings_003E5__3.ortoCurve.Evaluate(time);
                    localPosition.y += Mathf.LerpUnclamped(0f, this._003Csettings_003E5__3.ortoDistance, t2);
                }
                if (this._003Csettings_003E5__3.useTravelScaleCurve)
                {
                    float d = this._003Csettings_003E5__3.travelScaleCurve.Evaluate(time);
                    if (this._003CtransformBehaviour_003E5__2 != null)
                    {
                        this._003CtransformBehaviour_003E5__2.localScale = this._003CstartScale_003E5__4 * d;
                    }
                }
                if (this._003CtransformBehaviour_003E5__2 != null)
                {
                    this._003CtransformBehaviour_003E5__2.localPosition = localPosition;
                    this._003CtransformBehaviour_003E5__2.localRotationOffset = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                if (collectBurriedElementAction.travelParticles != null)
                {
                    collectBurriedElementAction.travelParticles.transform.localPosition = localPosition;
                }
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            private int _003C_003E1__state;

            private object _003C_003E2__current;

            public CollectBurriedElementAction _003C_003E4__this;

            private TransformBehaviour _003CtransformBehaviour_003E5__2;

            private CollectBurriedElementAction.Settings _003Csettings_003E5__3;

            private Vector3 _003CstartScale_003E5__4;

            private Vector3 _003CstartLocalPos_003E5__5;

            private Vector3 _003CendLocalPos_003E5__6;

            private float _003Ctime_003E5__7;

            private float _003Cduration_003E5__8;

            private float _003CstartAngle_003E5__9;

            private float _003CendAngle_003E5__10;
        }

        private sealed class _003CDoAnimation_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CDoAnimation_003Ed__17(int _003C_003E1__state)
            {
                this._003C_003E1__state = _003C_003E1__state;
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
            }

            bool IEnumerator.MoveNext()
            {
                int num = this._003C_003E1__state;
                CollectBurriedElementAction collectBurriedElementAction = this._003C_003E4__this;
                switch (num)
                {
                    case 0:
                        {
                            this._003C_003E1__state = -1;
                            LevelDefinition.BurriedElement burriedElementDefinition = collectBurriedElementAction.collectParams.burriedElementDefinition;
                            this._003Cgame_003E5__2 = collectBurriedElementAction.collectParams.game;
                            collectBurriedElementAction.collectParams.game.Play(GGSoundSystem.SFXType.CollectGoalStart);
                            this._003CtransformBehaviour_003E5__3 = collectBurriedElementAction.collectParams.transformBehaviour;
                            if (this._003CtransformBehaviour_003E5__3 != null)
                            {
                                this._003CtransformBehaviour_003E5__3.SetSortingLayer(collectBurriedElementAction.settings.sortingLayer);
                                this._003CtransformBehaviour_003E5__3.SetAlpha(1f);
                                this._003Cgame_003E5__2.particles.CreateParticles(this._003CtransformBehaviour_003E5__3.localPosition, Match3Particles.PositionType.BurriedElementBreak);
                                GameObject travelParticles = this._003Cgame_003E5__2.particles.CreateParticles(this._003CtransformBehaviour_003E5__3.localPosition, Match3Particles.PositionType.BurriedElementTravelParticle);
                                collectBurriedElementAction.travelParticles = travelParticles;
                            }
                            this._003Canimation_003E5__4 = null;
                            this._003CenumList_003E5__5 = new EnumeratorsList();
                            if (collectBurriedElementAction.settings.scaleUpDuration < 0f)
                            {
                                goto IL_14C;
                            }
                            this._003CenumList_003E5__5.Add(collectBurriedElementAction.ScalePart(), 0f, null, null, false);
                            break;
                        }
                    case 1:
                        this._003C_003E1__state = -1;
                        break;
                    case 2:
                        this._003C_003E1__state = -1;
                        goto IL_1A2;
                    case 3:
                        this._003C_003E1__state = -1;
                        goto IL_1D6;
                    case 4:
                        this._003C_003E1__state = -1;
                        goto IL_289;
                    default:
                        return false;
                }
                if (this._003CenumList_003E5__5.Update())
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 1;
                    return true;
                }
                IL_14C:
                if (this._003CtransformBehaviour_003E5__3 != null)
                {
                    this._003CtransformBehaviour_003E5__3.SetSortingLayer(collectBurriedElementAction.settings.sortingLayerFly);
                }
                if (!collectBurriedElementAction.settings.useJump)
                {
                    this._003Canimation_003E5__4 = collectBurriedElementAction.TravelPart();
                    goto IL_1D6;
                }
                this._003Canimation_003E5__4 = collectBurriedElementAction.TravelJumpPart();
                IL_1A2:
                if (!this._003Canimation_003E5__4.MoveNext())
                {
                    goto IL_1E3;
                }
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
                IL_1D6:
                if (this._003Canimation_003E5__4.MoveNext())
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 3;
                    return true;
                }
                IL_1E3:
                collectBurriedElementAction.collectParams.RemoveFromGame();
                this._003Cgame_003E5__2.OnPickupGoal(new GoalCollectParams(collectBurriedElementAction.collectParams.goal, collectBurriedElementAction.collectParams.destroyParams));
                collectBurriedElementAction.globalLock.UnlockAll();
                if (!(collectBurriedElementAction.travelParticles != null))
                {
                    goto IL_2A7;
                }
                ParticleSystem component = collectBurriedElementAction.travelParticles.GetComponent<ParticleSystem>();
                if (component != null)
                {
                    var temp = component.emission;

                    temp.enabled = false;
                }
                this._003Ctime_003E5__6 = 0f;
                IL_289:
                if (this._003Ctime_003E5__6 < collectBurriedElementAction.settings.additionalParticlesDuration)
                {
                    this._003Ctime_003E5__6 += collectBurriedElementAction.deltaTime;
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 4;
                    return true;
                }
                UnityEngine.Object.Destroy(collectBurriedElementAction.travelParticles);
                IL_2A7:
                collectBurriedElementAction.isAlive = false;
                return false;
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            private int _003C_003E1__state;

            private object _003C_003E2__current;

            public CollectBurriedElementAction _003C_003E4__this;

            private Match3Game _003Cgame_003E5__2;

            private TransformBehaviour _003CtransformBehaviour_003E5__3;

            private IEnumerator _003Canimation_003E5__4;

            private EnumeratorsList _003CenumList_003E5__5;

            private float _003Ctime_003E5__6;
        }
    }
}
