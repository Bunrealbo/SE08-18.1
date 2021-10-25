using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class CarModelPart : MonoBehaviour
{
    public Vector3 buttonHandlePosition
    {
        get
        {
            if (this.buttonHandleTransform != null)
            {
                return this.buttonHandleTransform.position;
            }
            return base.transform.position;
        }
    }

    public Vector3 directionHandlePosition
    {
        get
        {
            if (this.subparts.Count == 0)
            {
                return this.buttonHandlePosition;
            }
            return this.buttonHandlePosition + this.subparts[0].subpartInfo.offset;
        }
    }

    public bool shouldShow
    {
        get
        {
            if (!this.partInfo.isOwned)
            {
                return false;
            }
            for (int i = 0; i < this.partInfo.hideWhenAnyActive.Count; i++)
            {
                if (this.partInfo.hideWhenAnyActive[i].partInfo.isOwned)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public List<CarModelSubpart> subpartsWithVariantHandles
    {
        get
        {
            this.subpartsWithVariantHandles_.Clear();
            for (int i = 0; i < this.subparts.Count; i++)
            {
                CarModelSubpart carModelSubpart = this.subparts[i];
                if (!(carModelSubpart.variantHandle == null))
                {
                    this.subpartsWithVariantHandles_.Add(carModelSubpart);
                }
            }
            return this.subpartsWithVariantHandles_;
        }
    }

    public List<CarModelSubpart> subpartsWithInteraction
    {
        get
        {
            this.subpartsWithVariantHandles_.Clear();
            for (int i = 0; i < this.subparts.Count; i++)
            {
                CarModelSubpart carModelSubpart = this.subparts[i];
                if (carModelSubpart.subpartInfo.rotateSettings.enabled)
                {
                    this.subpartsWithVariantHandles_.Add(carModelSubpart);
                }
            }
            return this.subpartsWithVariantHandles_;
        }
    }

    public CarModelInfo.VariantGroup firstVariantGroup
    {
        get
        {
            for (int i = 0; i < this.subparts.Count; i++)
            {
                CarModelInfo.VariantGroup firstVariantGroup = this.subparts[i].firstVariantGroup;
                if (firstVariantGroup != null)
                {
                    return firstVariantGroup;
                }
            }
            return null;
        }
    }

    public void SetExplodeOffset(float nTime)
    {
        bool active = nTime <= 0f;
        GGUtil.SetActive(this.colliderRoot, active);
        float distanceFromCenter = ScriptableObjectSingleton<CarsDB>.instance.explosionSettings.distanceFromCenter;
        for (int i = 0; i < this.subparts.Count; i++)
        {
            this.subparts[i].SetExplodeOffset(nTime, distanceFromCenter);
        }
    }

    public void Init(CarModel model)
    {
        this.colliderRoot = null;
        this.model = model;
        this.partInfo.name = base.name;
        this.paintTransformations.Clear();
        this.buttonHandleTransform = null;
        this.subparts.Clear();
        foreach (object obj in base.transform)
        {
            Transform transform = (Transform)obj;
            string text = transform.name.ToLower();
            if (text.Contains("_collider"))
            {
                GGUtil.SetActive(transform, false);
            }
            else if (text.Contains("_handle"))
            {
                this.buttonHandleTransform = transform;
            }
            else if (!text.Contains("_ignore"))
            {
                PaintTransformation component = transform.GetComponent<PaintTransformation>();
                if (component != null)
                {
                    this.paintTransformations.Add(component);
                    component.Init();
                    GGUtil.Hide(component);
                }
                else
                {
                    CarModelSubpart carModelSubpart = transform.GetComponent<CarModelSubpart>();
                    if (carModelSubpart == null)
                    {
                        carModelSubpart = transform.gameObject.AddComponent<CarModelSubpart>();
                    }
                    carModelSubpart.Init(this);
                    this.subparts.Add(carModelSubpart);
                }
            }
        }
    }

    private IEnumerator RemoveSubpart(CarModelSubpart subpart, AssembleCarScreen screen)
    {
        return new CarModelPart._003CRemoveSubpart_003Ed__21(0)
        {
            subpart = subpart,
            screen = screen
        };
    }

    public void ShowSubpartsIfRemoving()
    {
        for (int i = 0; i < this.subparts.Count; i++)
        {
            CarModelSubpart carModelSubpart = this.subparts[i];
            if (carModelSubpart.subpartInfo.removing)
            {
                carModelSubpart.Show(true);
            }
        }
    }

    public IEnumerator AnimateIn(AssembleCarScreen screen)
    {
        return new CarModelPart._003CAnimateIn_003Ed__23(0)
        {
            _003C_003E4__this = this,
            screen = screen
        };
    }

    public void InitForRuntime(RoomsBackend.RoomAccessor backend)
    {
        this.partInfo.InitForRuntime(this.model, backend);
    }

    public void HideSubparts()
    {
        for (int i = 0; i < this.subparts.Count; i++)
        {
            GGUtil.Hide(this.subparts[i]);
        }
    }

    public void SetActiveIfOwned()
    {
        GGUtil.SetActive(this, this.shouldShow);
        for (int i = 0; i < this.subparts.Count; i++)
        {
            this.subparts[i].Show(false);
        }
        GGUtil.SetActive(this.colliderRoot, this.shouldShow);
    }

    [SerializeField]
    public List<CarModelSubpart> subparts = new List<CarModelSubpart>();

    [SerializeField]
    public CarModel model;

    [SerializeField]
    public CarPartInfo partInfo = new CarPartInfo();

    [SerializeField]
    private Transform buttonHandleTransform;

    [SerializeField]
    public List<PaintTransformation> paintTransformations = new List<PaintTransformation>();

    [SerializeField]
    public Transform colliderRoot;

    private List<CarModelSubpart> subpartsWithVariantHandles_ = new List<CarModelSubpart>();

    private sealed class _003C_003Ec__DisplayClass21_0
    {
        internal void _003CRemoveSubpart_003Eb__0(CarConfirmPurchase.InitArguments p0)
        {
            this.isDone = true;
        }

        internal void _003CRemoveSubpart_003Eb__1()
        {
            this.isDone = true;
        }

        internal void _003CRemoveSubpart_003Eb__2()
        {
            float num = Mathf.Max(0.1f, this.screen.confirmPurchase.DistancePercent());
            this.subpart.SetOffsetPosition(1f - num);
        }

        public bool isDone;

        public AssembleCarScreen screen;

        public CarModelSubpart subpart;
    }

    private sealed class _003CRemoveSubpart_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CRemoveSubpart_003Ed__21(int _003C_003E1__state)
        {
            this._003C_003E1__state = _003C_003E1__state;
        }

        [DebuggerHidden]
        void IDisposable.Dispose()
        {
        }

        bool IEnumerator.MoveNext()
        {
            switch (this._003C_003E1__state)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new CarModelPart._003C_003Ec__DisplayClass21_0();
                    this._003C_003E8__1.screen = this.screen;
                    this._003C_003E8__1.subpart = this.subpart;
                    this._003CenumAnimation_003E5__2 = this._003C_003E8__1.subpart.ShowRemoveNutAnimations(this._003C_003E8__1.screen);
                    break;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_291;
                default:
                    return false;
            }
            if (this._003CenumAnimation_003E5__2.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            NavigationManager instance = NavigationManager.instance;
            this._003CtalkingDialog_003E5__3 = instance.GetObject<TalkingDialog>();
            List<string> toSayBeforeWork = this._003C_003E8__1.subpart.subpartInfo.toSayBeforeWork;
            if (toSayBeforeWork.Count > 0)
            {
                this._003CtalkingDialog_003E5__3.ShowSingleLine(toSayBeforeWork[0]);
            }
            this._003C_003E8__1.isDone = false;
            CarConfirmPurchase.InitArguments initArguments = default(CarConfirmPurchase.InitArguments);
            initArguments.screen = this._003C_003E8__1.screen;
            initArguments.displayName = this._003C_003E8__1.subpart.displayName;
            initArguments.carPart = null;
            initArguments.updateDirection = true;
            initArguments.useDistanceToFindIfInside = true;
            initArguments.buttonHandlePosition = this._003C_003E8__1.subpart.buttonHandlePosition;
            initArguments.directionHandlePosition = this._003C_003E8__1.subpart.handleTransform.TransformPoint(this._003C_003E8__1.subpart.subpartInfo.offset);
            initArguments.directionHandlePosition = this._003C_003E8__1.subpart.buttonHandlePosition;
            initArguments.buttonHandlePosition = this._003C_003E8__1.subpart.handleTransform.TransformPoint(this._003C_003E8__1.subpart.subpartInfo.offset);
            initArguments.onSuccess = new Action<CarConfirmPurchase.InitArguments>(this._003C_003E8__1._003CRemoveSubpart_003Eb__0);
            initArguments.onCancel = new Action(this._003C_003E8__1._003CRemoveSubpart_003Eb__1);
            if (this._003C_003E8__1.subpart.subpartInfo.directControl)
            {
                initArguments.useMinDistanceToConfirm = true;
                initArguments.minDistance = 0.1f;
                initArguments.directionHandlePosition = this._003C_003E8__1.subpart.handleTransform.TransformPoint(this._003C_003E8__1.subpart.subpartInfo.offset);
                initArguments.onDrag = new Action(this._003C_003E8__1._003CRemoveSubpart_003Eb__2);
            }
            this._003C_003E8__1.screen.confirmPurchase.Show(initArguments);
            IL_291:
            if (this._003C_003E8__1.isDone)
            {
                this._003CtalkingDialog_003E5__3.Hide();
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

        public AssembleCarScreen screen;

        public CarModelSubpart subpart;

        private CarModelPart._003C_003Ec__DisplayClass21_0 _003C_003E8__1;

        private IEnumerator _003CenumAnimation_003E5__2;

        private TalkingDialog _003CtalkingDialog_003E5__3;
    }

    private sealed class _003C_003Ec__DisplayClass23_0
    {
        internal void _003CAnimateIn_003Eb__3(CarVariationPanel p0)
        {
            CarModelSubpart.ShowChange(this._003C_003E4__this.model.AllOwnedSubpartsInVariantGroup(this.groupToShow), 1f);
        }

        public AssembleCarScreen screen;

        public CarModelPart _003C_003E4__this;

        public CarModelInfo.VariantGroup groupToShow;
    }

    private sealed class _003C_003Ec__DisplayClass23_1
    {
        public float animationProgressPercent;

        public CarModelSubpart item;

        public CarModelPart._003C_003Ec__DisplayClass23_0 CS_0024_003C_003E8__locals1;
    }

    private sealed class _003C_003Ec__DisplayClass23_2
    {
        internal void _003CAnimateIn_003Eb__0(CarConfirmPurchase.InitArguments p0)
        {
            this.isDone = true;
        }

        internal void _003CAnimateIn_003Eb__1()
        {
            this.isDone = true;
        }

        internal void _003CAnimateIn_003Eb__2()
        {
            float num = Mathf.Max(0.1f, this.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.screen.confirmPurchase.DistancePercent());
            this.CS_0024_003C_003E8__locals2.animationProgressPercent = num;
            this.CS_0024_003C_003E8__locals2.item.SetOffsetPosition(num);
            if (this.CS_0024_003C_003E8__locals2.item.subpartInfo.hideToSayWhenWorking)
            {
                this.talkingDialog.Hide();
            }
        }

        public bool isDone;

        public TalkingDialog talkingDialog;

        public CarModelPart._003C_003Ec__DisplayClass23_1 CS_0024_003C_003E8__locals2;
    }

    private sealed class _003C_003Ec__DisplayClass23_3
    {
        internal void _003CAnimateIn_003Eb__4(CarVariationPanel p0)
        {
            CarModelSubpart.ShowChange(this.CS_0024_003C_003E8__locals3._003C_003E4__this.model.AllOwnedSubpartsInVariantGroup(this.CS_0024_003C_003E8__locals3.groupToShow), 1f);
            this.isVariantChosen = true;
        }

        public bool isVariantChosen;

        public CarModelPart._003C_003Ec__DisplayClass23_0 CS_0024_003C_003E8__locals3;
    }

    private sealed class _003CAnimateIn_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CAnimateIn_003Ed__23(int _003C_003E1__state)
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
            CarModelPart carModelPart = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new CarModelPart._003C_003Ec__DisplayClass23_0();
                    this._003C_003E8__1.screen = this.screen;
                    this._003C_003E8__1._003C_003E4__this = this._003C_003E4__this;
                    this._003CenumList_003E5__2 = new EnumeratorsList();
                    for (int i = 0; i < carModelPart.subparts.Count; i++)
                    {
                        carModelPart.subparts[i].Hide();
                    }
                    if (carModelPart.partInfo.confirmEachSubpart)
                    {
                        for (int j = 0; j < carModelPart.subparts.Count; j++)
                        {
                            CarModelSubpart carModelSubpart = carModelPart.subparts[j];
                            if (carModelSubpart.subpartInfo.removing)
                            {
                                carModelSubpart.Show(true);
                            }
                        }
                        this._003Ci_003E5__3 = 0;
                        goto IL_67F;
                    }
                    this._003CenumList_003E5__2.Clear();
                    for (int k = 0; k < carModelPart.subparts.Count; k++)
                    {
                        CarModelSubpart carModelSubpart2 = carModelPart.subparts[k];
                        this._003CenumList_003E5__2.Add(carModelSubpart2.InAnimation((float)k * carModelPart.partInfo.delaySubpartAnimation), 0f, null, null, false);
                    }
                    goto IL_712;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_4B3;
                case 3:
                    this._003C_003E1__state = -1;
                    goto IL_56F;
                case 4:
                    this._003C_003E1__state = -1;
                    goto IL_5E1;
                case 5:
                    this._003C_003E1__state = -1;
                    goto IL_659;
                case 6:
                    this._003C_003E1__state = -1;
                    goto IL_712;
                case 7:
                    this._003C_003E1__state = -1;
                    goto IL_79E;
                case 8:
                    this._003C_003E1__state = -1;
                    goto IL_954;
                default:
                    return false;
            }
            IL_1F7:
            if (!this._003CremoveEnum_003E5__5.MoveNext())
            {
                this._003C_003E8__2.item.Show(false);
                goto IL_66D;
            }
            this._003C_003E2__current = null;
            this._003C_003E1__state = 1;
            return true;
            IL_4B3:
            if (!this._003C_003E8__3.isDone)
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
            }
            this._003C_003E8__3.talkingDialog.Hide();
            this._003C_003E8__3 = null;
            this._003C_003E8__2.item.Show(false);
            this._003CenumList_003E5__2.Clear();
            if (this._003CskipAnimation_003E5__4)
            {
                this._003CenumList_003E5__2.Add(this._003C_003E8__2.item.InAnimationOffset(this._003C_003E8__2.animationProgressPercent), 0f, null, null, false);
            }
            else
            {
                this._003CenumList_003E5__2.Add(this._003C_003E8__2.item.InAnimation(0f), 0f, null, null, false);
            }
            IL_56F:
            if (this._003CenumList_003E5__2.Update())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 3;
                return true;
            }
            if (!this._003C_003E8__2.item.subpartInfo.showChangeAnimAfterIn)
            {
                goto IL_5EE;
            }
            this._003CenumList_003E5__2.Clear();
            this._003CenumList_003E5__2.Add(this._003C_003E8__2.item.DoShowChange(1f), 0f, null, null, false);
            IL_5E1:
            if (this._003CenumList_003E5__2.Update())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 4;
                return true;
            }
            IL_5EE:
            if (!this._003C_003E8__2.item.HasNutAnimations)
            {
                goto IL_666;
            }
            this._003CenumList_003E5__2.Clear();
            this._003CenumList_003E5__2.Add(this._003C_003E8__2.item.ShowNutAnimations(this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen), 0f, null, null, false);
            IL_659:
            if (this._003CenumList_003E5__2.Update())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 5;
                return true;
            }
            IL_666:
            this._003C_003E8__2 = null;
            IL_66D:
            int num2 = this._003Ci_003E5__3;
            this._003Ci_003E5__3 = num2 + 1;
            IL_67F:
            if (this._003Ci_003E5__3 >= carModelPart.subparts.Count)
            {
                goto IL_7D3;
            }
            this._003C_003E8__2 = new CarModelPart._003C_003Ec__DisplayClass23_1();
            this._003C_003E8__2.CS_0024_003C_003E8__locals1 = this._003C_003E8__1;
            this._003C_003E8__2.item = carModelPart.subparts[this._003Ci_003E5__3];
            this._003CskipAnimation_003E5__4 = false;
            this._003C_003E8__2.animationProgressPercent = 0f;
            CarCamera.Settings carCamera = this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen.scene.camera.GetCarCamera(this._003C_003E8__2.item.subpartInfo.cameraName);
            if (carCamera != null)
            {
                this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen.scene.camera.AnimateIntoSettings(carCamera);
            }
            if (this._003C_003E8__2.item.subpartInfo.removing)
            {
                this._003CremoveEnum_003E5__5 = carModelPart.RemoveSubpart(this._003C_003E8__2.item, this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen);
                goto IL_1F7;
            }
            this._003C_003E8__3 = new CarModelPart._003C_003Ec__DisplayClass23_2();
            this._003C_003E8__3.CS_0024_003C_003E8__locals2 = this._003C_003E8__2;
            NavigationManager instance = NavigationManager.instance;
            this._003C_003E8__3.talkingDialog = instance.GetObject<TalkingDialog>();
            List<string> toSayBefore = this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.toSayBefore;
            if (toSayBefore.Count > 0)
            {
                this._003C_003E8__3.talkingDialog.ShowSingleLine(toSayBefore[0]);
            }
            this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.SetOffsetPosition(0f);
            this._003C_003E8__3.isDone = false;
            CarConfirmPurchase.InitArguments initArguments = default(CarConfirmPurchase.InitArguments);
            initArguments.screen = this._003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.screen;
            initArguments.buttonHandlePosition = this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.buttonHandlePosition;
            initArguments.displayName = this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.displayName;
            initArguments.carPart = null;
            initArguments.updateDirection = true;
            initArguments.useDistanceToFindIfInside = true;
            initArguments.directionHandlePosition = this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.handleTransform.TransformPoint(this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.offset);
            initArguments.onSuccess = new Action<CarConfirmPurchase.InitArguments>(this._003C_003E8__3._003CAnimateIn_003Eb__0);
            initArguments.onCancel = new Action(this._003C_003E8__3._003CAnimateIn_003Eb__1);
            if (this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.showAtStart)
            {
                this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.Show(true);
                this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.SetOffsetPosition();
                GGUtil.SetActive(this._003C_003E8__3.CS_0024_003C_003E8__locals2.item, true);
            }
            if (this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.directControl)
            {
                initArguments.useMinDistanceToConfirm = true;
                initArguments.minDistance = 0.1f;
                initArguments.directionHandlePosition = this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.handleTransform.TransformPoint(this._003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.offset);
                initArguments.onDrag = new Action(this._003C_003E8__3._003CAnimateIn_003Eb__2);
                this._003CskipAnimation_003E5__4 = true;
            }
            this._003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.screen.confirmPurchase.Show(initArguments);
            goto IL_4B3;
            IL_712:
            if (!this._003CenumList_003E5__2.Update())
            {
                this._003Ci_003E5__3 = 0;
                goto IL_7BD;
            }
            this._003C_003E2__current = null;
            this._003C_003E1__state = 6;
            return true;
            IL_79E:
            if (this._003CenumList_003E5__2.Update())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 7;
                return true;
            }
            IL_7AB:
            num2 = this._003Ci_003E5__3;
            this._003Ci_003E5__3 = num2 + 1;
            IL_7BD:
            if (this._003Ci_003E5__3 < carModelPart.subparts.Count)
            {
                this._003CenumList_003E5__2.Clear();
                CarModelSubpart carModelSubpart3 = carModelPart.subparts[this._003Ci_003E5__3];
                if (carModelSubpart3.HasNutAnimations)
                {
                    this._003CenumList_003E5__2.Clear();
                    this._003CenumList_003E5__2.Add(carModelSubpart3.ShowNutAnimations(this._003C_003E8__1.screen), 0f, null, null, false);
                    goto IL_79E;
                }
                goto IL_7AB;
            }
            IL_7D3:
            this._003C_003E8__1.groupToShow = null;
            this._003C_003E8__1.groupToShow = carModelPart.model.modelInfo.GetVariantGroup(carModelPart.partInfo.variantGroupToShowAfterPurchase);
            if (this._003C_003E8__1.groupToShow == null)
            {
                return false;
            }
            this._003C_003E8__4 = new CarModelPart._003C_003Ec__DisplayClass23_3();
            this._003C_003E8__4.CS_0024_003C_003E8__locals3 = this._003C_003E8__1;
            this._003C_003E8__4.isVariantChosen = false;
            CarVariationPanel.InitParams initParams = default(CarVariationPanel.InitParams);
            initParams.screen = this._003C_003E8__4.CS_0024_003C_003E8__locals3.screen;
            initParams.variantGroup = this._003C_003E8__4.CS_0024_003C_003E8__locals3.groupToShow;
            initParams.inputHandler = this._003C_003E8__4.CS_0024_003C_003E8__locals3.screen.inputHandler;
            initParams.onChange = new Action<CarVariationPanel>(this._003C_003E8__4.CS_0024_003C_003E8__locals3._003CAnimateIn_003Eb__3);
            initParams.onClosed = new Action<CarVariationPanel>(this._003C_003E8__4._003CAnimateIn_003Eb__4);
            CarCamera.Settings carCamera2 = this._003C_003E8__4.CS_0024_003C_003E8__locals3.screen.scene.camera.GetCarCamera(this._003C_003E8__4.CS_0024_003C_003E8__locals3.groupToShow.cameraName);
            if (carCamera2 != null)
            {
                this._003C_003E8__4.CS_0024_003C_003E8__locals3.screen.scene.camera.AnimateIntoSettings(carCamera2);
            }
            this._003C_003E8__4.CS_0024_003C_003E8__locals3.screen.variationPanel.Show(initParams);
            IL_954:
            if (!this._003C_003E8__4.isVariantChosen)
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 8;
                return true;
            }
            this._003C_003E8__4 = null;
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

        public AssembleCarScreen screen;

        public CarModelPart _003C_003E4__this;

        private CarModelPart._003C_003Ec__DisplayClass23_0 _003C_003E8__1;

        private CarModelPart._003C_003Ec__DisplayClass23_1 _003C_003E8__2;

        private CarModelPart._003C_003Ec__DisplayClass23_2 _003C_003E8__3;

        private CarModelPart._003C_003Ec__DisplayClass23_3 _003C_003E8__4;

        private EnumeratorsList _003CenumList_003E5__2;

        private int _003Ci_003E5__3;

        private bool _003CskipAnimation_003E5__4;

        private IEnumerator _003CremoveEnum_003E5__5;
    }
}
