using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
    public class AnimateGrowingElementOnChip : BoardAction
    {
        private AnimateGrowingElementOnChip.Settings settings
        {
            get
            {
                return Match3Settings.instance.animateGrowingElementOnChipSettings;
            }
        }

        public void Init(AnimateGrowingElementOnChip.InitArguments initArguments)
        {
            this.initArguments = initArguments;
            this.slotLock = this.lockContainer.NewLock();
            this.slotLock.SuspendAll();
            this.slotLock.LockSlot(initArguments.destinationSlot);
        }

        private IEnumerator ScalePart(TransformBehaviour transformBehaviour)
        {
            return new AnimateGrowingElementOnChip._003CScalePart_003Ed__9(0)
            {
                _003C_003E4__this = this,
                transformBehaviour = transformBehaviour
            };
        }

        private IEnumerator TravelPart(TransformBehaviour transformBehaviour, Vector3 endLocalPos)
        {
            return new AnimateGrowingElementOnChip._003CTravelPart_003Ed__10(0)
            {
                _003C_003E4__this = this,
                transformBehaviour = transformBehaviour,
                endLocalPos = endLocalPos
            };
        }

        public static TransformBehaviour CreateGrowingElementPieceBehaviour(Match3Game game)
        {
            TransformBehaviour transformBehaviour = game.CreateGrowingElementPieceBehaviour();
            if (transformBehaviour == null)
            {
                return transformBehaviour;
            }
            AnimateGrowingElementOnChip.Settings animateGrowingElementOnChipSettings = Match3Settings.instance.animateGrowingElementOnChipSettings;
            GGUtil.SetActive(transformBehaviour, true);
            transformBehaviour.SetSortingLayer(animateGrowingElementOnChipSettings.sortingLayer);
            transformBehaviour.SetAlpha(1f);
            return transformBehaviour;
        }

        private IEnumerator DoAnimation()
        {
            return new AnimateGrowingElementOnChip._003CDoAnimation_003Ed__12(0)
            {
                _003C_003E4__this = this
            };
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            this.deltaTime = deltaTime;
            if (this.animationEnum == null)
            {
                this.animationEnum = this.DoAnimation();
            }
            this.animationEnum.MoveNext();
        }

        private AnimateGrowingElementOnChip.InitArguments initArguments;

        private float deltaTime;

        private IEnumerator animationEnum;

        private Lock slotLock;

        public struct InitArguments
        {
            public Match3Game game;

            public ItemColor itemColor;

            public Vector3 worldPositionStart;

            public Chip chipToConnectTo;

            public Slot destinationSlot;

            public bool forceConvertChip;

            public GrowingElementChip growingElement;
        }

        [Serializable]
        public class Settings
        {
            public SpriteSortingSettings sortingLayer = new SpriteSortingSettings();

            public SpriteSortingSettings sortingLayerFly = new SpriteSortingSettings();

            public float travelDuration = 1f;

            public AnimationCurve travelCurve;

            public float startScale = 0.5f;

            public float scaleUpScale = 1f;

            public float scaleUpDuration = 1f;

            public AnimationCurve scaleUpCurve;

            public AnimationCurve rotationCurve;
        }

        private sealed class _003CScalePart_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CScalePart_003Ed__9(int _003C_003E1__state)
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
                AnimateGrowingElementOnChip animateGrowingElementOnChip = this._003C_003E4__this;
                if (num != 0)
                {
                    if (num != 1)
                    {
                        return false;
                    }
                    this._003C_003E1__state = -1;
                    if (this._003Ctime_003E5__5 >= this._003Cduration_003E5__6)
                    {
                        return false;
                    }
                }
                else
                {
                    this._003C_003E1__state = -1;
                    Quaternion identity = Quaternion.identity;
                    this._003Csettings_003E5__2 = animateGrowingElementOnChip.settings;
                    this._003CstartScale_003E5__3 = new Vector3(this._003Csettings_003E5__2.startScale, this._003Csettings_003E5__2.startScale, 1f);
                    this._003CendScale_003E5__4 = new Vector3(this._003Csettings_003E5__2.scaleUpScale, this._003Csettings_003E5__2.scaleUpScale, 1f);
                    this._003Ctime_003E5__5 = 0f;
                    this._003Cduration_003E5__6 = this._003Csettings_003E5__2.scaleUpDuration;
                }
                this._003Ctime_003E5__5 += animateGrowingElementOnChip.deltaTime;
                float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__5);
                num2 = this._003Csettings_003E5__2.scaleUpCurve.Evaluate(num2);
                this._003Csettings_003E5__2.rotationCurve.Evaluate(num2);
                Vector3 localScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__3, this._003CendScale_003E5__4, num2);
                if (this.transformBehaviour != null)
                {
                    this.transformBehaviour.localScale = localScale;
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

            public AnimateGrowingElementOnChip _003C_003E4__this;

            public TransformBehaviour transformBehaviour;

            private AnimateGrowingElementOnChip.Settings _003Csettings_003E5__2;

            private Vector3 _003CstartScale_003E5__3;

            private Vector3 _003CendScale_003E5__4;

            private float _003Ctime_003E5__5;

            private float _003Cduration_003E5__6;
        }

        private sealed class _003CTravelPart_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CTravelPart_003Ed__10(int _003C_003E1__state)
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
                AnimateGrowingElementOnChip animateGrowingElementOnChip = this._003C_003E4__this;
                if (num != 0)
                {
                    if (num != 1)
                    {
                        return false;
                    }
                    this._003C_003E1__state = -1;
                    if (this._003Ctime_003E5__4 >= this._003Cduration_003E5__5)
                    {
                        return false;
                    }
                }
                else
                {
                    this._003C_003E1__state = -1;
                    this._003Csettings_003E5__2 = animateGrowingElementOnChip.settings;
                    Vector3 vector = Vector3.zero;
                    if (this.transformBehaviour != null)
                    {
                        vector = this.transformBehaviour.localPosition;
                    }
                    this._003CstartLocalPos_003E5__3 = vector;
                    this._003Ctime_003E5__4 = 0f;
                    this._003Cduration_003E5__5 = this._003Csettings_003E5__2.travelDuration;
                }
                this._003Ctime_003E5__4 += animateGrowingElementOnChip.deltaTime;
                float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__5, this._003Ctime_003E5__4);
                num2 = this._003Csettings_003E5__2.travelCurve.Evaluate(num2);
                Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartLocalPos_003E5__3, this.endLocalPos, num2);
                if (this.transformBehaviour != null)
                {
                    this.transformBehaviour.localPosition = localPosition;
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

            public AnimateGrowingElementOnChip _003C_003E4__this;

            public TransformBehaviour transformBehaviour;

            public Vector3 endLocalPos;

            private AnimateGrowingElementOnChip.Settings _003Csettings_003E5__2;

            private Vector3 _003CstartLocalPos_003E5__3;

            private float _003Ctime_003E5__4;

            private float _003Cduration_003E5__5;
        }

        private sealed class _003CDoAnimation_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CDoAnimation_003Ed__12(int _003C_003E1__state)
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
                AnimateGrowingElementOnChip animateGrowingElementOnChip = this._003C_003E4__this;
                switch (num)
                {
                    case 0:
                        {
                            this._003C_003E1__state = -1;
                            this._003Cgame_003E5__2 = animateGrowingElementOnChip.initArguments.game;
                            this._003CtransformBehaviour_003E5__3 = this._003Cgame_003E5__2.CreateGrowingElementPieceBehaviour();
                            Vector3 localPosition = this._003Cgame_003E5__2.WorldToBoardPosition(animateGrowingElementOnChip.initArguments.worldPositionStart);
                            if (this._003CtransformBehaviour_003E5__3 != null)
                            {
                                GGUtil.SetActive(this._003CtransformBehaviour_003E5__3, true);
                                this._003CtransformBehaviour_003E5__3.SetSortingLayer(animateGrowingElementOnChip.settings.sortingLayer);
                                this._003CtransformBehaviour_003E5__3.SetAlpha(1f);
                                this._003CtransformBehaviour_003E5__3.localPosition = localPosition;
                            }
                            this._003Canimation_003E5__4 = null;
                            this._003CenumList_003E5__5 = new EnumeratorsList();
                            this._003CenumList_003E5__5.Add(animateGrowingElementOnChip.ScalePart(this._003CtransformBehaviour_003E5__3), 0f, null, null, false);
                            break;
                        }
                    case 1:
                        this._003C_003E1__state = -1;
                        break;
                    case 2:
                        this._003C_003E1__state = -1;
                        goto IL_1CC;
                    default:
                        return false;
                }
                if (this._003CenumList_003E5__5.Update())
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 1;
                    return true;
                }
                if (this._003CtransformBehaviour_003E5__3 != null)
                {
                    this._003CtransformBehaviour_003E5__3.SetSortingLayer(animateGrowingElementOnChip.settings.sortingLayerFly);
                }
                this._003CdestinationSlot_003E5__6 = animateGrowingElementOnChip.initArguments.destinationSlot;
                if (this._003CdestinationSlot_003E5__6 == null)
                {
                    this._003CdestinationSlot_003E5__6 = animateGrowingElementOnChip.initArguments.growingElement.RandomDesiredSlot();
                    if (this._003CdestinationSlot_003E5__6 == null)
                    {
                        this._003CtransformBehaviour_003E5__3.ForceRemoveFromGame();
                        animateGrowingElementOnChip.slotLock.UnlockAll();
                        animateGrowingElementOnChip.isAlive = false;
                        return false;
                    }
                    animateGrowingElementOnChip.slotLock.LockSlot(animateGrowingElementOnChip.initArguments.destinationSlot);
                }
                this._003Canimation_003E5__4 = animateGrowingElementOnChip.TravelPart(this._003CtransformBehaviour_003E5__3, this._003CdestinationSlot_003E5__6.localPositionOfCenter);
                IL_1CC:
                if (!this._003Canimation_003E5__4.MoveNext())
                {
                    animateGrowingElementOnChip.slotLock.UnlockAll();
                    Chip chip = animateGrowingElementOnChip.initArguments.chipToConnectTo;
                    if (chip != null && chip.slot != this._003CdestinationSlot_003E5__6)
                    {
                        chip = null;
                    }
                    if (chip == null || animateGrowingElementOnChip.initArguments.forceConvertChip)
                    {
                        Chip slotComponent = this._003CdestinationSlot_003E5__6.GetSlotComponent<Chip>();
                        if (slotComponent != null)
                        {
                            slotComponent.RemoveFromGame();
                        }
                        chip = this._003Cgame_003E5__2.CreateChipInSlot(this._003CdestinationSlot_003E5__6, animateGrowingElementOnChip.initArguments.itemColor);
                    }
                    if (chip != null)
                    {
                        chip.AttachGrowingElement(this._003CtransformBehaviour_003E5__3);
                    }
                    else if (this._003CtransformBehaviour_003E5__3 != null)
                    {
                        this._003CtransformBehaviour_003E5__3.ForceRemoveFromGame();
                    }
                    animateGrowingElementOnChip.isAlive = false;
                    return false;
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

            public AnimateGrowingElementOnChip _003C_003E4__this;

            private Match3Game _003Cgame_003E5__2;

            private TransformBehaviour _003CtransformBehaviour_003E5__3;

            private IEnumerator _003Canimation_003E5__4;

            private EnumeratorsList _003CenumList_003E5__5;

            private Slot _003CdestinationSlot_003E5__6;
        }
    }
}
