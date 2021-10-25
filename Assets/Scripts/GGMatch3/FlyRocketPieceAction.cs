using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
    public class FlyRocketPieceAction : BoardAction
    {
        private FlyRocketPieceAction.Settings settings
        {
            get
            {
                return Match3Settings.instance.flyRocketPieceSettings;
            }
        }

        public void Init(FlyRocketPieceAction.Params initParams)
        {
            this.initParams = initParams;
            this.individualLock = this.lockContainer.NewLock();
            this.individualLock.isSlotGravitySuspended = true;
            this.prelock = this.lockContainer.NewLock();
            this.prelock.isSlotGravitySuspended = true;
            this.prelock.isChipGravitySuspended = true;
            this.prelock.isChipGeneratorSuspended = true;
            this.prelock.isAboutToBeDestroyed = true;
            this.prelock.isAvailableForDiscoBombSuspended = true;
            Match3Game game = initParams.game;
            this.pathSlots = this.GetPathSlots();
            if (initParams.prelock)
            {
                this.prelock.LockSlots(this.pathSlots);
            }
            Slot slot = initParams.game.GetSlot(initParams.position);
            this.hasCarpet = initParams.isHavingCarpet;
            if (slot != null && slot.canCarpetSpreadFromHere)
            {
                this.hasCarpet = true;
            }
            this.affectedSlots.Clear();
            Slot slot2 = game.LastSlotOnDirection(slot, initParams.direction);
            IntVector2 intVector = initParams.position;
            for (; ; )
            {
                Slot slot3 = game.GetSlot(intVector);
                if (!game.board.IsInBoard(intVector))
                {
                    break;
                }
                intVector += initParams.direction;
                if (slot3 != null && (!(intVector == initParams.position) || !initParams.ignoreOriginSlot))
                {
                    FlyRocketPieceAction.AffectedSlot affectedSlot = default(FlyRocketPieceAction.AffectedSlot);
                    affectedSlot.slot = slot3;
                    if (initParams.affectorExport != null)
                    {
                        affectedSlot.bolt = initParams.affectorExport.GetLigtingBoltForSlots(initParams.position, slot3.position);
                    }
                    if (affectedSlot.slot == slot2 && this.settings.putBoltOnLastSlot && affectedSlot.bolt == null)
                    {
                        LightingBolt lightingBolt = game.CreateLightingBoltPowerup();
                        lightingBolt.Init(slot, affectedSlot.slot);
                        affectedSlot.bolt = lightingBolt;
                        this.createdBolt = true;
                    }
                    this.affectedSlots.Add(affectedSlot);
                }
            }
            if (!initParams.ignoreOriginSlot)
            {
                game.particles.CreateParticles(game.LocalPositionOfCenter(initParams.position), Match3Particles.PositionType.OnRocketStart, (initParams.direction.x != 0) ? ChipType.HorizontalRocket : ChipType.VerticalRocket, ItemColor.Unknown);
            }
        }

        private void ClearAffectedSlots()
        {
            for (int i = 0; i < this.affectedSlots.Count; i++)
            {
                FlyRocketPieceAction.AffectedSlot affectedSlot = this.affectedSlots[i];
                if (affectedSlot.bolt != null)
                {
                    affectedSlot.bolt.RemoveFromGame();
                }
            }
            this.affectedSlots.Clear();
        }

        private RocketPieceBehaviour CreateRocketPiece()
        {
            Match3Game game = this.initParams.game;
            RocketPieceBehaviour rocketPieceBehaviour = game.CreateRocketPiece();
            if (rocketPieceBehaviour == null)
            {
                return null;
            }
            Vector3 localPosition = game.LocalPositionOfCenter(this.initParams.position);
            rocketPieceBehaviour.localPosition = localPosition;
            rocketPieceBehaviour.SetDirection(this.initParams.direction);
            return rocketPieceBehaviour;
        }

        public List<Slot> GetPathSlots()
        {
            List<Slot> list = new List<Slot>();
            Match3Game game = this.initParams.game;
            IntVector2 position = this.initParams.position;
            IntVector2 direction = this.initParams.direction;
            IntVector2 size = game.board.size;
            IntVector2 intVector = position;
            list.Clear();
            while (game.board.IsInBoard(intVector))
            {
                Slot slot = game.board.GetSlot(intVector);
                if (slot != null)
                {
                    list.Add(slot);
                }
                intVector += direction;
            }
            return list;
        }

        private bool UpdateLockedSlots()
        {
            float num = this.deltaTime;
            float additionalTimeToKeepLock = this.settings.additionalTimeToKeepLock;
            bool result = false;
            for (int i = 0; i < this.lockedSlotsList.Count; i++)
            {
                FlyRocketPieceAction.LockedSlot lockedSlot = this.lockedSlotsList[i];
                if (!lockedSlot.isUnlocked)
                {
                    lockedSlot.timeLocked += num;
                    if (lockedSlot.timeLocked >= additionalTimeToKeepLock)
                    {
                        lockedSlot.isUnlocked = true;
                        this.individualLock.Unlock(lockedSlot.slot);
                    }
                    this.lockedSlotsList[i] = lockedSlot;
                    result = true;
                }
            }
            return result;
        }

        private IEnumerator DoFly()
        {
            return new FlyRocketPieceAction._003CDoFly_003Ed__22(0)
            {
                _003C_003E4__this = this
            };
        }

        public override void OnStart(ActionManager manager)
        {
            base.OnStart(manager);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            this.deltaTime = deltaTime * this.settings.timeScale;
            if (this.animation == null)
            {
                this.animation = this.DoFly();
            }
            this.animation.MoveNext();
        }

        private void ApplyDisplaceAfterDestroy(Slot slot)
        {
            IntVector2[] array = IntVector2.leftRight;
            if (Mathf.Abs(this.initParams.direction.x) > Mathf.Abs(this.initParams.direction.y))
            {
                array = IntVector2.upDown;
            }
            foreach (IntVector2 b in array)
            {
                Slot slot2 = this.initParams.game.GetSlot(slot.position + b);
                if (slot2 != null)
                {
                    slot2.offsetPosition = (slot2.localPositionOfCenter - slot.localPositionOfCenter).normalized * this.settings.shockWaveOffset;
                    slot2.positionIntegrator.currentPosition = slot2.offsetPosition;
                }
            }
        }

        private FlyRocketPieceAction.Params initParams;

        private Lock prelock;

        private float deltaTime;

        private IEnumerator animation;

        private RocketPieceBehaviour rocketPiece;

        private List<Slot> pathSlots;

        private Lock individualLock;

        private bool createdBolt;

        private List<FlyRocketPieceAction.AffectedSlot> affectedSlots = new List<FlyRocketPieceAction.AffectedSlot>();

        private List<FlyRocketPieceAction.LockedSlot> lockedSlotsList = new List<FlyRocketPieceAction.LockedSlot>();

        private bool hasCarpet;

        public struct Params
        {
            public Match3Game game;

            public IntVector2 position;

            public IntVector2 direction;

            public bool prelock;

            public bool isInstant;

            public bool ignoreOriginSlot;

            public bool canUseScale;

            public bool isHavingCarpet;

            public Match3Game.InputAffectorExport affectorExport;
        }

        [Serializable]
        public class Settings
        {
            public FloatRange speedRange;

            public FloatRange accelerationTime;

            public AnimationCurve velocityChangeCurve;

            public float timeScale = 1f;

            public float lightIntensity = 0.5f;

            public FloatRange lightIntensityRange = new FloatRange(1.5f, 0.75f);

            public float maxLightDistance = 6f;

            public float lightDuration = 1f;

            public Vector3 scale = Vector3.one;

            public Vector3 scaleMin = Vector3.one;

            public Vector3 scaleMax = Vector3.one;

            public float initialDisplace;

            public float initialDelay;

            public float additionalTimeToKeepLock;

            public float keepParticlesFor = 2f;

            public int slotsOutside = 5;

            public float shockWaveOffset = 0.02f;

            public float timeAheadDestroy;

            public bool putBoltOnLastSlot;

            public float boltDelay;

            public bool keepBaltsDistance;

            public float minBoltDuration;

            public bool useCameraShake;

            public GeneralSettings.CameraShakeSettings cameraShake = new GeneralSettings.CameraShakeSettings();
        }

        public struct AffectedSlot
        {
            public Slot slot;

            public LightingBolt bolt;
        }

        public struct LockedSlot
        {
            public Slot slot;

            public float timeLocked;

            public bool isUnlocked;
        }

        private sealed class _003CDoFly_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CDoFly_003Ed__22(int _003C_003E1__state)
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
                FlyRocketPieceAction flyRocketPieceAction = this._003C_003E4__this;
                switch (num)
                {
                    case 0:
                        this._003C_003E1__state = -1;
                        this._003Ctime_003E5__2 = 0f;
                        if (flyRocketPieceAction.createdBolt && flyRocketPieceAction.settings.boltDelay > 0f)
                        {
                            goto IL_86;
                        }
                        goto IL_99;
                    case 1:
                        this._003C_003E1__state = -1;
                        goto IL_86;
                    case 2:
                        this._003C_003E1__state = -1;
                        goto IL_1E7;
                    case 3:
                        this._003C_003E1__state = -1;
                        break;
                    case 4:
                        this._003C_003E1__state = -1;
                        goto IL_7D1;
                    case 5:
                        this._003C_003E1__state = -1;
                        goto IL_803;
                    default:
                        return false;
                }
               
                    IL_253:
                    this._003Ctime_003E5__2 += flyRocketPieceAction.deltaTime;
                    if (flyRocketPieceAction.initParams.isInstant)
                    {
                        this._003Ctime_003E5__2 += 1000f;
                    }
                    float time = flyRocketPieceAction.settings.accelerationTime.InverseLerp(this._003Ctime_003E5__2);
                    float t = flyRocketPieceAction.settings.velocityChangeCurve.Evaluate(time);
                    float d = flyRocketPieceAction.settings.speedRange.Lerp(t);
                    if (flyRocketPieceAction.initParams.canUseScale)
                    {
                        Vector3 vector = Vector3.Lerp(flyRocketPieceAction.settings.scaleMin, flyRocketPieceAction.settings.scaleMax, t);
                        if (flyRocketPieceAction.initParams.direction.y != 0)
                        {
                            vector.x = vector.y;
                            vector.y = vector.x;
                        }
                        if (flyRocketPieceAction.rocketPiece != null)
                        {
                            flyRocketPieceAction.rocketPiece.localScale = vector;
                        }
                    }
                    this._003CcurrentPositon_003E5__4 += this._003Cdirection_003E5__5 * d * flyRocketPieceAction.deltaTime;
                    IntVector2 position = this._003Cgame_003E5__3.BoardPositionFromLocalPositionRound(this._003CcurrentPositon_003E5__4);
                    Vector3 vector2 = this._003CcurrentPositon_003E5__4 + this._003Cdirection_003E5__5 * d * flyRocketPieceAction.settings.timeAheadDestroy;
                    IntVector2 intVector = this._003Cgame_003E5__3.BoardPositionFromLocalPositionRound(vector2);
                    int num2 = Mathf.Min(this._003ClastSlotPosition_003E5__7.x, intVector.x);
                    int num3 = Mathf.Max(this._003ClastSlotPosition_003E5__7.x, intVector.x);
                    int num4 = Mathf.Min(this._003ClastSlotPosition_003E5__7.y, intVector.y);
                    int num5 = Mathf.Max(this._003ClastSlotPosition_003E5__7.y, intVector.y);
                    bool flag = false;
                    for (int i = num2; i <= num3; i++)
                    {
                        for (int j = num4; j <= num5; j++)
                        {
                            IntVector2 intVector2 = new IntVector2(i, j);
                            Slot slot = this._003Cgame_003E5__3.GetSlot(intVector2);
                            if (slot != null && !this._003CvisitedSlots_003E5__6.Contains(slot))
                            {
                                if (slot.canCarpetSpreadFromHere)
                                {
                                    this._003CdestroyParams_003E5__8.isHavingCarpet = true;
                                }
                                if (Vector3.Dot(this._003Cdirection_003E5__5, vector2 - slot.localPositionOfCenter) >= 0f)
                                {
                                    int num6 = Mathf.Abs(intVector2.x - flyRocketPieceAction.initParams.position.x) + Mathf.Abs(intVector2.y - flyRocketPieceAction.initParams.position.y);
                                    slot.light.AddLightWithDuration(flyRocketPieceAction.settings.lightIntensityRange.Lerp(Mathf.InverseLerp(0f, flyRocketPieceAction.settings.maxLightDistance, (float)num6)), flyRocketPieceAction.settings.lightDuration);
                                    if (flyRocketPieceAction.settings.additionalTimeToKeepLock > 0f)
                                    {
                                        FlyRocketPieceAction.LockedSlot item = default(FlyRocketPieceAction.LockedSlot);
                                        item.slot = slot;
                                        flyRocketPieceAction.individualLock.LockSlot(slot);
                                        flyRocketPieceAction.lockedSlotsList.Add(item);
                                    }
                                    if (flyRocketPieceAction.initParams.ignoreOriginSlot && intVector2 == flyRocketPieceAction.initParams.position)
                                    {
                                        this._003CvisitedSlots_003E5__6.Add(slot);
                                    }
                                    else
                                    {
                                        for (int k = 0; k < flyRocketPieceAction.affectedSlots.Count; k++)
                                        {
                                            FlyRocketPieceAction.AffectedSlot affectedSlot = flyRocketPieceAction.affectedSlots[k];
                                            if (affectedSlot.slot == slot)
                                            {
                                                if (affectedSlot.bolt != null && !flyRocketPieceAction.settings.keepBaltsDistance)
                                                {
                                                    affectedSlot.bolt.RemoveFromGame();
                                                    affectedSlot.bolt = null;
                                                }
                                                flyRocketPieceAction.affectedSlots[k] = affectedSlot;
                                            }
                                        }
                                        slot.OnDestroySlot(this._003CdestroyParams_003E5__8);
                                        flyRocketPieceAction.prelock.Unlock(slot);
                                        this._003CvisitedSlots_003E5__6.Add(slot);
                                        flyRocketPieceAction.ApplyDisplaceAfterDestroy(slot);
                                        if (this._003CdestroyParams_003E5__8.isRocketStopped)
                                        {
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                    }
                    this._003ClastSlotPosition_003E5__7 = position;
                    if (flyRocketPieceAction.rocketPiece != null)
                    {
                        flyRocketPieceAction.rocketPiece.localPosition = this._003CcurrentPositon_003E5__4;
                    }
                    for (int l = 0; l < flyRocketPieceAction.affectedSlots.Count; l++)
                    {
                        FlyRocketPieceAction.AffectedSlot affectedSlot2 = flyRocketPieceAction.affectedSlots[l];
                        if (!(affectedSlot2.bolt == null) && !flyRocketPieceAction.settings.keepBaltsDistance)
                        {
                            affectedSlot2.bolt.SetStartPosition(this._003CcurrentPositon_003E5__4 + this._003Cdirection_003E5__5 * this._003Cgame_003E5__3.slotPhysicalSize.x * 0.5f);
                        }
                    }
                    flyRocketPieceAction.UpdateLockedSlots();
                    if (flag || (!this._003Cgame_003E5__3.board.IsInBoard(position) && this._003Cgame_003E5__3.board.DistanceOutsideBoard(position) >= flyRocketPieceAction.settings.slotsOutside))
                    {
                        goto IL_761;
                    }
                
                while (flyRocketPieceAction.initParams.isInstant);
                this._003C_003E2__current = null;
                this._003C_003E1__state = 3;
                return true;
                IL_761:
                if (flyRocketPieceAction.rocketPiece != null)
                {
                    flyRocketPieceAction.rocketPiece.RemoveFromGameAfter(flyRocketPieceAction.settings.keepParticlesFor);
                    flyRocketPieceAction.rocketPiece = null;
                }
                if (flyRocketPieceAction.settings.minBoltDuration > 0f)
                {
                    goto IL_7D1;
                }
                goto IL_7E4;
                IL_86:
                if (this._003Ctime_003E5__2 < flyRocketPieceAction.settings.boltDelay)
                {
                    this._003Ctime_003E5__2 += flyRocketPieceAction.deltaTime;
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 1;
                    return true;
                }
                IL_99:
                if (flyRocketPieceAction.settings.useCameraShake)
                {
                    flyRocketPieceAction.initParams.game.ShakeCamera(flyRocketPieceAction.settings.cameraShake);
                }
                if (flyRocketPieceAction.rocketPiece == null)
                {
                    flyRocketPieceAction.rocketPiece = flyRocketPieceAction.CreateRocketPiece();
                }
                flyRocketPieceAction.lockedSlotsList.Clear();
                this._003Cgame_003E5__3 = flyRocketPieceAction.initParams.game;
                Vector3 localPosition = this._003Cgame_003E5__3.LocalPositionOfCenter(flyRocketPieceAction.initParams.position);
                this._003CcurrentPositon_003E5__4 = localPosition;
                this._003Cdirection_003E5__5 = new Vector3((float)flyRocketPieceAction.initParams.direction.x, (float)flyRocketPieceAction.initParams.direction.y, 0f);
                this._003CcurrentPositon_003E5__4 += this._003Cdirection_003E5__5 * flyRocketPieceAction.settings.initialDisplace;
                this._003CvisitedSlots_003E5__6 = new List<Slot>();
                this._003ClastSlotPosition_003E5__7 = flyRocketPieceAction.initParams.position;
                if (!(flyRocketPieceAction.rocketPiece != null))
                {
                    goto IL_1FA;
                }
                flyRocketPieceAction.rocketPiece.localPosition = localPosition;
                if (flyRocketPieceAction.initParams.isInstant)
                {
                    goto IL_1FA;
                }
                this._003Ctime_003E5__2 = 0f;
                IL_1E7:
                if (this._003Ctime_003E5__2 < flyRocketPieceAction.settings.initialDelay)
                {
                    this._003Ctime_003E5__2 += Time.deltaTime;
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 2;
                    return true;
                }
                IL_1FA:
                this._003CdestroyParams_003E5__8 = new SlotDestroyParams();
                this._003CdestroyParams_003E5__8.isHitByBomb = true;
                this._003CdestroyParams_003E5__8.bombType = ((Mathf.Abs(flyRocketPieceAction.initParams.direction.y) != 0) ? ChipType.VerticalRocket : ChipType.HorizontalRocket);
                this._003CdestroyParams_003E5__8.isHavingCarpet = flyRocketPieceAction.hasCarpet;
                this._003Ctime_003E5__2 = 0f;
                goto IL_253;
                IL_7D1:
                if (this._003Ctime_003E5__2 < flyRocketPieceAction.settings.minBoltDuration)
                {
                    this._003Ctime_003E5__2 += flyRocketPieceAction.deltaTime;
                    flyRocketPieceAction.UpdateLockedSlots();
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 4;
                    return true;
                }
                IL_7E4:
                flyRocketPieceAction.ClearAffectedSlots();
                IL_803:
                if (!flyRocketPieceAction.UpdateLockedSlots())
                {
                    flyRocketPieceAction.individualLock.UnlockAll();
                    flyRocketPieceAction.prelock.UnlockAll();
                    flyRocketPieceAction.isAlive = false;
                    return false;
                }
                this._003C_003E2__current = null;
                this._003C_003E1__state = 5;
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

            public FlyRocketPieceAction _003C_003E4__this;

            private float _003Ctime_003E5__2;

            private Match3Game _003Cgame_003E5__3;

            private Vector3 _003CcurrentPositon_003E5__4;

            private Vector3 _003Cdirection_003E5__5;

            private List<Slot> _003CvisitedSlots_003E5__6;

            private IntVector2 _003ClastSlotPosition_003E5__7;

            private SlotDestroyParams _003CdestroyParams_003E5__8;
        }
    }
}
