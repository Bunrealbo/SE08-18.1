using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class CarModelSubpart : MonoBehaviour
{
    public void RemoveAllModificationsOfVariant(int variant)
    {
        CarModelSubpart._003C_003Ec__DisplayClass7_0 _003C_003Ec__DisplayClass7_ = new CarModelSubpart._003C_003Ec__DisplayClass7_0();
        _003C_003Ec__DisplayClass7_.variant = variant;
        this.variantModifications.RemoveAll(new Predicate<VariantModification>(_003C_003Ec__DisplayClass7_._003CRemoveAllModificationsOfVariant_003Eb__0));
    }

    public bool HasVariantForGroup(CarModelInfo.VariantGroup group)
    {
        for (int i = 0; i < this.variants.Count; i++)
        {
            if (this.variants[i].info.groupName == group.name)
            {
                return true;
            }
        }
        for (int j = 0; j < this.variantModifications.Count; j++)
        {
            if (this.variantModifications[j].groupName == group.name)
            {
                return true;
            }
        }
        return false;
    }

    public CarModelInfo.VariantGroup firstVariantGroup
    {
        get
        {
            for (int i = 0; i < this.variants.Count; i++)
            {
                CarModelInfo.VariantGroup variantGroup = this.variants[i].variantGroup;
                if (variantGroup != null)
                {
                    return variantGroup;
                }
            }
            for (int j = 0; j < this.variantModifications.Count; j++)
            {
                VariantModification variantModification = this.variantModifications[j];
                CarModelInfo.VariantGroup variantGroup2 = this.part.model.modelInfo.GetVariantGroup(variantModification.groupName);
                if (variantGroup2 != null)
                {
                    return variantGroup2;
                }
            }
            return null;
        }
    }

    public Vector3 buttonHandlePosition
    {
        get
        {
            if (this.handleTransform_ != null)
            {
                return this.handleTransform_.position;
            }
            return base.transform.position;
        }
    }

    public Transform handleTransform
    {
        get
        {
            if (this.handleTransform_ != null)
            {
                return this.handleTransform_;
            }
            return base.transform;
        }
    }

    public string displayName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(this.subpartInfo.displayName))
            {
                return this.subpartInfo.displayName;
            }
            return base.transform.name;
        }
    }

    private CarModelSubpart.BlinkSettings blinkSettings
    {
        get
        {
            return ScriptableObjectSingleton<CarsDB>.instance.subpartBlinkSettings;
        }
    }

    private CarModelSubpart.Settings settings
    {
        get
        {
            return ScriptableObjectSingleton<CarsDB>.instance.subpartInSettings;
        }
    }

    public void Init(CarModelPart part)
    {
        this.scaleAtStart = base.transform.localScale;
        this.localPositionAtStart = base.transform.localPosition;
        this.part = part;
        this.handleTransform_ = null;
        this.variantHandle = null;
        this.variants.Clear();
        this.nutTransforms.Clear();
        if (this.subpartInfo.rotateSettings.enabled)
        {
            this.subpartInfo.rotateSettings.forwardDirection = base.transform.forward;
        }
        foreach (object obj in base.transform)
        {
            Transform transform = (Transform)obj;
            string text = transform.name.ToLower();
            if (text.Contains("_nut"))
            {
                this.nutTransforms.Add(transform);
            }
            else if (text.Contains("_collider"))
            {
                GGUtil.Hide(transform);
            }
            else if (text.Contains("_variant_handle"))
            {
                this.variantHandle = transform;
            }
            else if (text.Contains("_handle"))
            {
                this.handleTransform_ = transform;
            }
            else if (text.Contains("_variant"))
            {
                SubpartVariant subpartVariant = transform.GetComponent<SubpartVariant>();
                if (subpartVariant == null)
                {
                    subpartVariant = transform.gameObject.AddComponent<SubpartVariant>();
                }
                subpartVariant.Init(this);
                subpartVariant.info.index = this.variants.Count;
                this.variants.Add(subpartVariant);
            }
            else if (text.Contains("_rotate"))
            {
                this.subpartInfo.rotateSettings.forwardDirection = transform.forward;
            }
        }
    }

    public IEnumerator InAnimationScaleBounce()
    {
        return new CarModelSubpart._003CInAnimationScaleBounce_003Ed__30(0)
        {
            _003C_003E4__this = this
        };
    }

    public void Hide()
    {
        GGUtil.SetActive(this, false);
    }

    public void Show(bool force = false)
    {
        bool active = !this.subpartInfo.removing || force;
        GGUtil.SetActive(this, active);
        this.ShowActiveVariant();
        for (int i = 0; i < this.variantModifications.Count; i++)
        {
            VariantModification variantModification = this.variantModifications[i];
            CarModelInfo.VariantGroup variantGroup = this.part.model.modelInfo.GetVariantGroup(variantModification.groupName);
            int index = 0;
            if (variantGroup != null)
            {
                index = variantGroup.selectedVariationIndex;
            }
            if (variantModification.IsApplicable(index))
            {
                variantModification.Apply(false);
            }
        }
    }

    public void ApplyVariantModification(int selectedVariationIndex, bool useSharedMaterial)
    {
        for (int i = 0; i < this.variantModifications.Count; i++)
        {
            VariantModification variantModification = this.variantModifications[i];
            if (variantModification.IsApplicable(selectedVariationIndex))
            {
                variantModification.Apply(useSharedMaterial);
            }
        }
    }

    private void ShowActiveVariant()
    {
        for (int i = 0; i < this.variants.Count; i++)
        {
            this.variants[i].ShowIfInActiveVariant();
        }
    }

    public void SetOffsetPosition()
    {
        base.transform.localPosition = this.localPositionAtStart + this.subpartInfo.offset;
    }

    public void SetOffsetPosition(float p)
    {
        base.transform.localPosition = Vector3.Lerp(this.localPositionAtStart, this.localPositionAtStart + this.subpartInfo.offset, p);
    }

    public void SetExplodeOffset(float nTime, float distance)
    {
        float magnitude = this.subpartInfo.offset.magnitude;
        Vector3 a = Vector3.up;
        if (magnitude > Mathf.Epsilon)
        {
            a = this.subpartInfo.offset / magnitude;
        }
        base.transform.localPosition = Vector3.Lerp(this.localPositionAtStart, this.localPositionAtStart + a * distance, nTime);
    }

    public static void ShowChange(List<CarModelSubpart> subparts, float scale = 1f)
    {
        for (int i = 0; i < subparts.Count; i++)
        {
            subparts[i].ShowChange(scale);
        }
    }

    public void ChangeRotation()
    {
        this.openAnimation = this.DoChangeRotation();
        this.openAnimation.MoveNext();
    }

    private IEnumerator DoChangeRotation()
    {
        return new CarModelSubpart._003CDoChangeRotation_003Ed__41(0)
        {
            _003C_003E4__this = this
        };
    }

    public void ShowChange(float scale)
    {
        this.activeAnimation = this.DoShowChange(scale);
        this.activeAnimation.MoveNext();
    }

    public bool HasNutAnimations
    {
        get
        {
            return this.nutTransforms.Count > 0;
        }
    }

    public IEnumerator DoShowChange(float scaleMult)
    {
        return new CarModelSubpart._003CDoShowChange_003Ed__45(0)
        {
            _003C_003E4__this = this,
            scaleMult = scaleMult
        };
    }

    public IEnumerator ShowNutAnimations()
    {
        return new CarModelSubpart._003CShowNutAnimations_003Ed__46(0)
        {
            _003C_003E4__this = this
        };
    }

    public IEnumerator ShowRemoveNutAnimations(AssembleCarScreen screen)
    {
        return new CarModelSubpart._003CShowRemoveNutAnimations_003Ed__47(0)
        {
            _003C_003E4__this = this,
            screen = screen
        };
    }

    public IEnumerator ShowNutAnimations(AssembleCarScreen screen)
    {
        return new CarModelSubpart._003CShowNutAnimations_003Ed__48(0)
        {
            _003C_003E4__this = this,
            screen = screen
        };
    }

    public IEnumerator InAnimationOffset(float percent)
    {
        return new CarModelSubpart._003CInAnimationOffset_003Ed__49(0)
        {
            _003C_003E4__this = this,
            percent = percent
        };
    }

    public IEnumerator InAnimationScaleOffset()
    {
        return new CarModelSubpart._003CInAnimationScaleOffset_003Ed__50(0)
        {
            _003C_003E4__this = this
        };
    }

    public IEnumerator InAnimation(float delay = 0f)
    {
        return new CarModelSubpart._003CInAnimation_003Ed__51(0)
        {
            _003C_003E4__this = this,
            delay = delay
        };
    }

    private void Update()
    {
        if (this.activeAnimation != null)
        {
            this.activeAnimation.MoveNext();
        }
        if (this.openAnimation != null)
        {
            this.openAnimation.MoveNext();
        }
    }

    [SerializeField]
    private Vector3 scaleAtStart;

    [SerializeField]
    private Vector3 localPositionAtStart;

    [SerializeField]
    public CarModelPart part;

    [SerializeField]
    private Transform handleTransform_;

    [SerializeField]
    private List<SubpartVariant> variants = new List<SubpartVariant>();

    [SerializeField]
    public List<VariantModification> variantModifications = new List<VariantModification>();

    [SerializeField]
    private List<Transform> nutTransforms = new List<Transform>();

    [SerializeField]
    public string defaultVariantGroupName;

    private IEnumerator activeAnimation;

    private IEnumerator openAnimation;

    [SerializeField]
    public Transform variantHandle;

    [SerializeField]
    public string variantGroupName;

    [SerializeField]
    public CarSubPartInfo subpartInfo = new CarSubPartInfo();

    private bool isOpen;

    [Serializable]
    public class Settings
    {
        public float inDuration = 1f;

        public float fromScale;

        public float toScale = 1f;

        public AnimationCurve scaleCurve;

        public float moveDuration;

        public AnimationCurve moveCurve;

        public float openDuration = 1f;
    }

    [Serializable]
    public class BlinkSettings
    {
        public float inDuration = 1f;

        public float fromScale;

        public float toScale = 1f;

        public AnimationCurve scaleCurve;

        public float moveDuration;

        public float fromScaleChange = 0.99f;

        public float changeOffset = 0.1f;

        public AnimationCurve moveCurve;
    }

    private sealed class _003C_003Ec__DisplayClass7_0
    {
        internal bool _003CRemoveAllModificationsOfVariant_003Eb__0(VariantModification mod)
        {
            return mod.variantIndex == this.variant;
        }

        public int variant;
    }

    private sealed class _003CInAnimationScaleBounce_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CInAnimationScaleBounce_003Ed__30(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            if (num != 0)
            {
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
            }
            else
            {
                this._003C_003E1__state = -1;
                this._003Csettings_003E5__2 = carModelSubpart.settings;
                this._003Ctime_003E5__3 = 0f;
            }
            if (this._003Ctime_003E5__3 >= this._003Csettings_003E5__2.inDuration)
            {
                return false;
            }
            this._003Ctime_003E5__3 += Time.deltaTime;
            float time = Mathf.InverseLerp(0f, this._003Csettings_003E5__2.inDuration, this._003Ctime_003E5__3);
            float t = this._003Csettings_003E5__2.scaleCurve.Evaluate(time);
            float d = Mathf.LerpUnclamped(this._003Csettings_003E5__2.fromScale, this._003Csettings_003E5__2.toScale, t);
            Vector3 localScale = carModelSubpart.scaleAtStart * d;
            carModelSubpart.transform.localScale = localScale;
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

        public CarModelSubpart _003C_003E4__this;

        private CarModelSubpart.Settings _003Csettings_003E5__2;

        private float _003Ctime_003E5__3;
    }

    private sealed class _003CDoChangeRotation_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoChangeRotation_003Ed__41(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            if (num != 0)
            {
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
            }
            else
            {
                this._003C_003E1__state = -1;
                this._003Ctime_003E5__2 = 0f;
                CarSubPartInfo.RotateSettings rotateSettings = carModelSubpart.subpartInfo.rotateSettings;
                this._003Cduration_003E5__3 = carModelSubpart.settings.openDuration;
                this._003CstartRotation_003E5__4 = carModelSubpart.transform.localRotation;
                float angle = carModelSubpart.isOpen ? rotateSettings.initialAngle : rotateSettings.outAngle;
                carModelSubpart.isOpen = !carModelSubpart.isOpen;
                this._003CendRotation_003E5__5 = Quaternion.AngleAxis(angle, rotateSettings.axis);
            }
            if (this._003Ctime_003E5__2 >= this._003Cduration_003E5__3)
            {
                carModelSubpart.transform.localRotation = this._003CendRotation_003E5__5;
                return false;
            }
            this._003Ctime_003E5__2 += Time.deltaTime;
            float t = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
            carModelSubpart.transform.localRotation = Quaternion.Lerp(this._003CstartRotation_003E5__4, this._003CendRotation_003E5__5, t);
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

        public CarModelSubpart _003C_003E4__this;

        private float _003Ctime_003E5__2;

        private float _003Cduration_003E5__3;

        private Quaternion _003CstartRotation_003E5__4;

        private Quaternion _003CendRotation_003E5__5;
    }

    private sealed class _003CDoShowChange_003Ed__45 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShowChange_003Ed__45(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    {
                        this._003C_003E1__state = -1;
                        CarModelSubpart.BlinkSettings blinkSettings = carModelSubpart.blinkSettings;
                        this._003Ctime_003E5__2 = 0f;
                        this._003Cduration_003E5__3 = blinkSettings.inDuration;
                        Vector3 normalized = carModelSubpart.subpartInfo.offset.normalized;
                        if (carModelSubpart.subpartInfo.overrideChangeAnimOffset)
                        {
                            normalized = carModelSubpart.subpartInfo.changeAnimOffset.normalized;
                        }
                        this._003CstartPosition_003E5__4 = carModelSubpart.localPositionAtStart + normalized * blinkSettings.changeOffset * this.scaleMult;
                        this._003Ccurve_003E5__5 = blinkSettings.moveCurve;
                        this._003CendPosition_003E5__6 = carModelSubpart.localPositionAtStart;
                        break;
                    }
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    {
                        this._003C_003E1__state = -1;
                        CarModelSubpart.BlinkSettings blinkSettings = carModelSubpart.blinkSettings;
                        if (this._003Ctime_003E5__2 >= blinkSettings.inDuration)
                        {
                            return false;
                        }
                        this._003Ctime_003E5__2 += Time.deltaTime;
                        float time = Mathf.InverseLerp(0f, blinkSettings.inDuration, this._003Ctime_003E5__2);
                        float t = blinkSettings.scaleCurve.Evaluate(time);
                        float d = Mathf.LerpUnclamped(blinkSettings.fromScaleChange, blinkSettings.toScale, t);
                        Vector3 localScale = carModelSubpart.scaleAtStart * d;
                        carModelSubpart.transform.localScale = localScale;
                        this._003C_003E2__current = null;
                        this._003C_003E1__state = 2;
                        return true;
                    }
                default:
                    return false;
            }
            if (this._003Ctime_003E5__2 <= this._003Cduration_003E5__3)
            {
                this._003Ctime_003E5__2 += Time.deltaTime;
                float time2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
                float t2 = this._003Ccurve_003E5__5.Evaluate(time2);
                Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPosition_003E5__4, this._003CendPosition_003E5__6, t2);
                carModelSubpart.transform.localPosition = localPosition;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            this._003CstartPosition_003E5__4 = default(Vector3);
            this._003Ccurve_003E5__5 = null;
            this._003CendPosition_003E5__6 = default(Vector3);
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

        public CarModelSubpart _003C_003E4__this;

        public float scaleMult;

        private float _003Ctime_003E5__2;

        private float _003Cduration_003E5__3;

        private Vector3 _003CstartPosition_003E5__4;

        private AnimationCurve _003Ccurve_003E5__5;

        private Vector3 _003CendPosition_003E5__6;
    }

    private sealed class _003CShowNutAnimations_003Ed__46 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CShowNutAnimations_003Ed__46(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            if (num == 0)
            {
                this._003C_003E1__state = -1;
                this._003Cnuts_003E5__2 = carModelSubpart.part.model.nuts;
                this._003Cnuts_003E5__2.Clear();
                new List<CarNut>();
                this._003CoffsetDirection_003E5__3 = carModelSubpart.subpartInfo.offset.normalized;
                this._003Ci_003E5__4 = 0;
                goto IL_16B;
            }
            if (num != 1)
            {
                return false;
            }
            this._003C_003E1__state = -1;
            IL_145:
            if (this._003CrotationAnimation_003E5__5.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            this._003CrotationAnimation_003E5__5 = null;
            int num2 = this._003Ci_003E5__4;
            this._003Ci_003E5__4 = num2 + 1;
            IL_16B:
            if (this._003Ci_003E5__4 >= carModelSubpart.nutTransforms.Count)
            {
                this._003Cnuts_003E5__2.Clear();
                return false;
            }
            Transform transform = carModelSubpart.nutTransforms[this._003Ci_003E5__4];
            CarNut carNut = this._003Cnuts_003E5__2.NextNut();
            Vector3 position = transform.position;
            UnityEngine.Debug.Log("OFFSET DIRECTION " + this._003CoffsetDirection_003E5__3);
            Quaternion rotation = Quaternion.LookRotation(this._003CoffsetDirection_003E5__3);
            carNut.Init();
            carNut.transform.position = transform.position + this._003CoffsetDirection_003E5__3 * carNut.nutSize;
            carNut.SetRotation(rotation);
            GGUtil.Show(carNut);
            Vector3 fromPosition = transform.position + this._003CoffsetDirection_003E5__3 * carNut.nutSize;
            Vector3 position2 = transform.position;
            this._003CrotationAnimation_003E5__5 = carNut.DoRotateIn(fromPosition, position2, 0.5f);
            goto IL_145;
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

        public CarModelSubpart _003C_003E4__this;

        private CarNutsPool _003Cnuts_003E5__2;

        private Vector3 _003CoffsetDirection_003E5__3;

        private int _003Ci_003E5__4;

        private IEnumerator _003CrotationAnimation_003E5__5;
    }

    private sealed class _003C_003Ec__DisplayClass47_0
    {
        public AssembleCarScreen screen;

        public CarModelSubpart _003C_003E4__this;

        public TalkingDialog talkingDialog;
    }

    private sealed class _003C_003Ec__DisplayClass47_1
    {
        internal void _003CShowRemoveNutAnimations_003Eb__0(ScrewdriverTool.PressArguments p0)
        {
            this.time += Time.deltaTime;
            this.CS_0024_003C_003E8__locals1.screen.tutorialHand.Hide();
            if (this.CS_0024_003C_003E8__locals1._003C_003E4__this.subpartInfo.hideToSayWhenWorking)
            {
                this.CS_0024_003C_003E8__locals1.talkingDialog.Hide();
            }
        }

        public float time;

        public CarModelSubpart._003C_003Ec__DisplayClass47_0 CS_0024_003C_003E8__locals1;
    }

    private sealed class _003CShowRemoveNutAnimations_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CShowRemoveNutAnimations_003Ed__47(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            if (num == 0)
            {
                this._003C_003E1__state = -1;
                this._003C_003E8__1 = new CarModelSubpart._003C_003Ec__DisplayClass47_0();
                this._003C_003E8__1.screen = this.screen;
                this._003C_003E8__1._003C_003E4__this = this._003C_003E4__this;
                this._003Cnuts_003E5__2 = carModelSubpart.part.model.nuts;
                this._003Cnuts_003E5__2.Clear();
                this._003CnutsList_003E5__3 = new List<CarNut>();
                this._003CoffsetDirection_003E5__4 = carModelSubpart.subpartInfo.offset.normalized;
                NavigationManager instance = NavigationManager.instance;
                this._003C_003E8__1.talkingDialog = instance.GetObject<TalkingDialog>();
                List<string> toSayBefore = carModelSubpart.subpartInfo.toSayBefore;
                if (toSayBefore.Count > 0)
                {
                    this._003C_003E8__1.talkingDialog.ShowSingleLine(toSayBefore[0]);
                }
                for (int i = 0; i < carModelSubpart.nutTransforms.Count; i++)
                {
                    Transform transform = carModelSubpart.nutTransforms[i];
                    CarNut carNut = this._003Cnuts_003E5__2.NextNut();
                    Vector3 position = transform.position;
                    Quaternion rotation = Quaternion.LookRotation(this._003CoffsetDirection_003E5__4);
                    carNut.Init();
                    carNut.transform.position = transform.position;
                    carNut.SetRotation(rotation);
                    GGUtil.Show(carNut);
                    this._003CnutsList_003E5__3.Add(carNut);
                }
                this._003Ci_003E5__5 = 0;
                goto IL_47D;
            }
            if (num != 1)
            {
                return false;
            }
            this._003C_003E1__state = -1;
            IL_3C0:
            if (this._003C_003E8__2.time < this._003Cduration_003E5__8)
            {
                float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__8, this._003C_003E8__2.time);
                this._003Cnut_003E5__6.SetRotateIn(this._003CfromPosition_003E5__11, this._003CtoPosition_003E5__10, num2);
                this._003Cdrill_003E5__9.transform.position = Vector3.Lerp(this._003CfromPosition_003E5__11, this._003CtoPosition_003E5__10, num2);
                this._003Cdrill_003E5__9.SetActive(this._003CscrewdriverTool_003E5__7.isPressed);
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            this._003Cnut_003E5__6.SetRotateIn(this._003CfromPosition_003E5__11, this._003CtoPosition_003E5__10, 1f);
            this._003CscrewdriverTool_003E5__7.Hide();
            this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen.tutorialHand.Hide();
            GGUtil.Hide(this._003Cnut_003E5__6);
            this._003C_003E8__2.CS_0024_003C_003E8__locals1.talkingDialog.Hide();
            this._003C_003E8__2 = null;
            this._003Cnut_003E5__6 = null;
            this._003CscrewdriverTool_003E5__7 = null;
            this._003Cdrill_003E5__9 = null;
            this._003CtoPosition_003E5__10 = default(Vector3);
            this._003CfromPosition_003E5__11 = default(Vector3);
            int num3 = this._003Ci_003E5__5;
            this._003Ci_003E5__5 = num3 + 1;
            IL_47D:
            if (this._003Ci_003E5__5 >= this._003CnutsList_003E5__3.Count)
            {
                this._003Cnuts_003E5__2.Clear();
                return false;
            }
            this._003C_003E8__2 = new CarModelSubpart._003C_003Ec__DisplayClass47_1();
            this._003C_003E8__2.CS_0024_003C_003E8__locals1 = this._003C_003E8__1;
            Transform transform2 = carModelSubpart.nutTransforms[this._003Ci_003E5__5];
            this._003Cnut_003E5__6 = this._003CnutsList_003E5__3[this._003Ci_003E5__5];
            if (this._003Ci_003E5__5 == 0 && carModelSubpart.subpartInfo.showNutTutorialHand)
            {
                TutorialHandController.InitArguments initArguments = default(TutorialHandController.InitArguments);
                Transform transform3 = this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen.transform;
                initArguments.endLocalPosition = Vector3.zero;
                initArguments.startLocalPosition = Vector3.zero;
                initArguments.settings = Match3Settings.instance.tutorialHandSettings;
                initArguments.repeat = true;
                this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen.tutorialHand.Show(initArguments);
            }
            this._003CscrewdriverTool_003E5__7 = this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen.screwdriverTool;
            ScrewdriverTool.InitArguments initArguments2 = default(ScrewdriverTool.InitArguments);
            initArguments2.inputHandler = this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen.inputHandler;
            this._003C_003E8__2.time = 0f;
            this._003Cduration_003E5__8 = 1f;
            initArguments2.onPress = (Action<ScrewdriverTool.PressArguments>)Delegate.Combine(initArguments2.onPress, new Action<ScrewdriverTool.PressArguments>(this._003C_003E8__2._003CShowRemoveNutAnimations_003Eb__0));
            this._003CscrewdriverTool_003E5__7.Init(initArguments2);
            this._003Cdrill_003E5__9 = this._003CscrewdriverTool_003E5__7.GetDrillModel(this._003C_003E8__2.CS_0024_003C_003E8__locals1.screen.scene);
            this._003CtoPosition_003E5__10 = transform2.position + this._003CoffsetDirection_003E5__4 * this._003Cnut_003E5__6.nutSize;
            this._003CfromPosition_003E5__11 = transform2.position;
            this._003Cdrill_003E5__9.Show(this._003CfromPosition_003E5__11, Quaternion.LookRotation(this._003CfromPosition_003E5__11 - this._003CtoPosition_003E5__10));
            goto IL_3C0;
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

        public CarModelSubpart _003C_003E4__this;

        private CarModelSubpart._003C_003Ec__DisplayClass47_0 _003C_003E8__1;

        private CarModelSubpart._003C_003Ec__DisplayClass47_1 _003C_003E8__2;

        private CarNutsPool _003Cnuts_003E5__2;

        private List<CarNut> _003CnutsList_003E5__3;

        private Vector3 _003CoffsetDirection_003E5__4;

        private int _003Ci_003E5__5;

        private CarNut _003Cnut_003E5__6;

        private ScrewdriverTool _003CscrewdriverTool_003E5__7;

        private float _003Cduration_003E5__8;

        private DrillModel _003Cdrill_003E5__9;

        private Vector3 _003CtoPosition_003E5__10;

        private Vector3 _003CfromPosition_003E5__11;
    }

    private sealed class _003C_003Ec__DisplayClass48_0
    {
        internal void _003CShowNutAnimations_003Eb__0(ScrewdriverTool.PressArguments p0)
        {
            this.time += Time.deltaTime;
        }

        public float time;
    }

    private sealed class _003CShowNutAnimations_003Ed__48 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CShowNutAnimations_003Ed__48(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            if (num == 0)
            {
                this._003C_003E1__state = -1;
                this._003Cnuts_003E5__2 = carModelSubpart.part.model.nuts;
                this._003Cnuts_003E5__2.Clear();
                new List<CarNut>();
                this._003CoffsetDirection_003E5__3 = carModelSubpart.subpartInfo.offset.normalized;
                this._003Ci_003E5__4 = 0;
                goto IL_2FA;
            }
            if (num != 1)
            {
                return false;
            }
            this._003C_003E1__state = -1;
            IL_277:
            if (this._003C_003E8__1.time < this._003Cduration_003E5__8)
            {
                float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__8, this._003C_003E8__1.time);
                this._003Cnut_003E5__5.SetRotateIn(this._003CfromPosition_003E5__9, this._003CtoPosition_003E5__10, num2);
                this._003Cdrill_003E5__7.transform.position = Vector3.Lerp(this._003CfromPosition_003E5__9, this._003CtoPosition_003E5__10, num2);
                this._003Cdrill_003E5__7.SetActive(this._003CscrewdriverTool_003E5__6.isPressed);
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            this._003Cnut_003E5__5.SetRotateIn(this._003CfromPosition_003E5__9, this._003CtoPosition_003E5__10, 1f);
            this._003CscrewdriverTool_003E5__6.Hide();
            this._003C_003E8__1 = null;
            this._003Cnut_003E5__5 = null;
            this._003CscrewdriverTool_003E5__6 = null;
            this._003Cdrill_003E5__7 = null;
            this._003CfromPosition_003E5__9 = default(Vector3);
            this._003CtoPosition_003E5__10 = default(Vector3);
            int num3 = this._003Ci_003E5__4;
            this._003Ci_003E5__4 = num3 + 1;
            IL_2FA:
            if (this._003Ci_003E5__4 >= carModelSubpart.nutTransforms.Count)
            {
                this._003Cnuts_003E5__2.Clear();
                return false;
            }
            this._003C_003E8__1 = new CarModelSubpart._003C_003Ec__DisplayClass48_0();
            Transform transform = carModelSubpart.nutTransforms[this._003Ci_003E5__4];
            this._003Cnut_003E5__5 = this._003Cnuts_003E5__2.NextNut();
            Vector3 position = transform.position;
            Quaternion rotation = Quaternion.LookRotation(this._003CoffsetDirection_003E5__3);
            this._003Cnut_003E5__5.Init();
            this._003Cnut_003E5__5.transform.position = transform.position + this._003CoffsetDirection_003E5__3 * this._003Cnut_003E5__5.nutSize;
            this._003Cnut_003E5__5.SetRotation(rotation);
            GGUtil.Show(this._003Cnut_003E5__5);
            this._003CscrewdriverTool_003E5__6 = this.screen.screwdriverTool;
            ScrewdriverTool.InitArguments initArguments = default(ScrewdriverTool.InitArguments);
            initArguments.inputHandler = this.screen.inputHandler;
            this._003Cdrill_003E5__7 = this._003CscrewdriverTool_003E5__6.GetDrillModel(this.screen.scene);
            this._003C_003E8__1.time = 0f;
            this._003Cduration_003E5__8 = 1f;
            initArguments.onPress = (Action<ScrewdriverTool.PressArguments>)Delegate.Combine(initArguments.onPress, new Action<ScrewdriverTool.PressArguments>(this._003C_003E8__1._003CShowNutAnimations_003Eb__0));
            this._003CscrewdriverTool_003E5__6.Init(initArguments);
            this._003CfromPosition_003E5__9 = transform.position + this._003CoffsetDirection_003E5__3 * this._003Cnut_003E5__5.nutSize;
            this._003CtoPosition_003E5__10 = transform.position;
            this._003Cdrill_003E5__7.Show(this._003CfromPosition_003E5__9, Quaternion.LookRotation(this._003CtoPosition_003E5__10 - this._003CfromPosition_003E5__9));
            goto IL_277;
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

        public CarModelSubpart _003C_003E4__this;

        public AssembleCarScreen screen;

        private CarModelSubpart._003C_003Ec__DisplayClass48_0 _003C_003E8__1;

        private CarNutsPool _003Cnuts_003E5__2;

        private Vector3 _003CoffsetDirection_003E5__3;

        private int _003Ci_003E5__4;

        private CarNut _003Cnut_003E5__5;

        private ScrewdriverTool _003CscrewdriverTool_003E5__6;

        private DrillModel _003Cdrill_003E5__7;

        private float _003Cduration_003E5__8;

        private Vector3 _003CfromPosition_003E5__9;

        private Vector3 _003CtoPosition_003E5__10;
    }

    private sealed class _003CInAnimationOffset_003Ed__49 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CInAnimationOffset_003Ed__49(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            if (num != 0)
            {
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
            }
            else
            {
                this._003C_003E1__state = -1;
                CarModelSubpart.Settings settings = carModelSubpart.settings;
                this._003Ctime_003E5__2 = 0f;
                this._003Cduration_003E5__3 = settings.moveDuration;
                this._003Ccurve_003E5__4 = settings.moveCurve;
                this._003CstartPosition_003E5__5 = carModelSubpart.localPositionAtStart + Vector3.Lerp(Vector3.zero, carModelSubpart.subpartInfo.offset, this.percent);
                this._003CendPosition_003E5__6 = carModelSubpart.localPositionAtStart;
            }
            if (this._003Ctime_003E5__2 > this._003Cduration_003E5__3)
            {
                return false;
            }
            this._003Ctime_003E5__2 += Time.deltaTime;
            float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
            float t = this._003Ccurve_003E5__4.Evaluate(time);
            Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPosition_003E5__5, this._003CendPosition_003E5__6, t);
            carModelSubpart.transform.localPosition = localPosition;
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

        public CarModelSubpart _003C_003E4__this;

        public float percent;

        private float _003Ctime_003E5__2;

        private float _003Cduration_003E5__3;

        private AnimationCurve _003Ccurve_003E5__4;

        private Vector3 _003CstartPosition_003E5__5;

        private Vector3 _003CendPosition_003E5__6;
    }

    private sealed class _003CInAnimationScaleOffset_003Ed__50 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CInAnimationScaleOffset_003Ed__50(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            if (num != 0)
            {
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
            }
            else
            {
                this._003C_003E1__state = -1;
                CarModelSubpart.Settings settings = carModelSubpart.settings;
                this._003Ctime_003E5__2 = 0f;
                this._003Cduration_003E5__3 = settings.moveDuration;
                this._003Ccurve_003E5__4 = settings.moveCurve;
                this._003CstartPosition_003E5__5 = carModelSubpart.localPositionAtStart + carModelSubpart.subpartInfo.offset;
                this._003CendPosition_003E5__6 = carModelSubpart.localPositionAtStart;
            }
            if (this._003Ctime_003E5__2 > this._003Cduration_003E5__3)
            {
                return false;
            }
            this._003Ctime_003E5__2 += Time.deltaTime;
            float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
            float t = this._003Ccurve_003E5__4.Evaluate(time);
            Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPosition_003E5__5, this._003CendPosition_003E5__6, t);
            carModelSubpart.transform.localPosition = localPosition;
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

        public CarModelSubpart _003C_003E4__this;

        private float _003Ctime_003E5__2;

        private float _003Cduration_003E5__3;

        private AnimationCurve _003Ccurve_003E5__4;

        private Vector3 _003CstartPosition_003E5__5;

        private Vector3 _003CendPosition_003E5__6;
    }

    private sealed class _003CInAnimation_003Ed__51 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CInAnimation_003Ed__51(int _003C_003E1__state)
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
            CarModelSubpart carModelSubpart = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    if (this.delay <= 0f)
                    {
                        goto IL_80;
                    }
                    carModelSubpart.Hide();
                    this._003Ctime_003E5__3 = 0f;
                    break;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_D3;
                default:
                    return false;
            }
            if (this._003Ctime_003E5__3 <= this.delay)
            {
                this._003Ctime_003E5__3 += Time.deltaTime;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            IL_80:
            carModelSubpart.Show(false);
            this._003CanimationEnum_003E5__2 = null;
            if (carModelSubpart.part.partInfo.animType == CarPartInfo.AnimType.ScaleBounce)
            {
                this._003CanimationEnum_003E5__2 = carModelSubpart.InAnimationScaleBounce();
            }
            else
            {
                this._003CanimationEnum_003E5__2 = carModelSubpart.InAnimationScaleOffset();
            }
            IL_D3:
            if (!this._003CanimationEnum_003E5__2.MoveNext())
            {
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

        public float delay;

        public CarModelSubpart _003C_003E4__this;

        private IEnumerator _003CanimationEnum_003E5__2;

        private float _003Ctime_003E5__3;
    }
}
