using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class DecorateRoomSceneVisualItem : MonoBehaviour
{
    public DecorateRoomSceneVisualItem.Settings settings
    {
        get
        {
            return Match3Settings.instance.visualItemAnimationSettings;
        }
    }

    public void Init(VisualObjectBehaviour visualObjectBehaviour, DecorateRoomScreen screen, int index, float delay)
    {
        this.index = index;
        visualObjectBehaviour.InitMarkers(this.markersPool.prefab);
        this.visualObjectBehaviour = visualObjectBehaviour;
        this.screen = screen;
        GraphicsSceneConfig.VisualObject visualObject = visualObjectBehaviour.visualObject;
        GGUtil.SetActive(this.widgetsToHide, false);
        bool active = visualObject.IsUnlocked(screen.scene) && !visualObject.isOwned;
        GGUtil.SetActive(this.buyButtonContanier, active);
        this.SetPositionOfBuyButton();
        visualObjectBehaviour.SetVisualState();
        visualObjectBehaviour.SetMarkersActive(false);
        this.animationCoroutine = this.DoAnimation(delay);
        this.animationCoroutine.MoveNext();
    }

    private IEnumerator DoAnimation(float delay)
    {
        return new DecorateRoomSceneVisualItem._003CDoAnimation_003Ed__13(0)
        {
            _003C_003E4__this = this,
            delay = delay
        };
    }

    private void SetPositionOfBuyButton()
    {
        Vector3 localPosition = base.transform.InverseTransformPoint(this.screen.TransformPSDToWorldPoint(this.visualObjectBehaviour.iconHandlePosition));
        localPosition.z = 0f;
        this.buyButtonContanier.localPosition = localPosition;
        this.buyButtonContanier.localScale = this.visualObjectBehaviour.iconHandleScale;
        this.buyButtonContanier.localRotation = this.visualObjectBehaviour.iconHandleRotation;
    }

    private void Update()
    {
        if (this.visualObjectBehaviour == null)
        {
            return;
        }
        this.SetPositionOfBuyButton();
        if (this.animationCoroutine != null && !this.animationCoroutine.MoveNext())
        {
            this.animationCoroutine = null;
        }
    }

    public void HideButton()
    {
        GGUtil.SetActive(this.buyButtonContanier, false);
    }

    public void ShowMarkers()
    {
        this.visualObjectBehaviour.SetMarkersActive(true);
    }

    public int Sort_MinX(DecorateRoomSceneVisualItem.PointWithIndex a, DecorateRoomSceneVisualItem.PointWithIndex b)
    {
        return a.point.x.CompareTo(b.point.x);
    }

    public int Sort_MinY(DecorateRoomSceneVisualItem.PointWithIndex a, DecorateRoomSceneVisualItem.PointWithIndex b)
    {
        return a.point.y.CompareTo(b.point.y);
    }

    public int Sort_Index(DecorateRoomSceneVisualItem.PointWithIndex a, DecorateRoomSceneVisualItem.PointWithIndex b)
    {
        return a.index.CompareTo(b.index);
    }

    public void ButtonCallback_OnBuyButton()
    {
        this.screen.VisualItemCallback_OnBuyItemPressed(this);
    }

    [SerializeField]
    private ComponentPool markersPool = new ComponentPool();

    [SerializeField]
    private RectTransform buyButtonContanier;

    [SerializeField]
    private RectTransform animationTransform;

    [SerializeField]
    private RectTransform animationOffsetTransform;

    [SerializeField]
    private List<RectTransform> widgetsToHide = new List<RectTransform>();

    [NonSerialized]
    public VisualObjectBehaviour visualObjectBehaviour;

    [NonSerialized]
    private DecorateRoomScreen screen;

    private IEnumerator animationCoroutine;

    private int index;

    private List<Vector2> pointsCachedList = new List<Vector2>();

    [Serializable]
    public class Settings
    {
        public float durationOfPop;

        public Vector3 startScale;

        public AnimationCurve popCurve;

        public float delayPerIndex = 0.05f;

        public float loopDuration;

        public Vector3 loopOffset;

        public Vector3 loopScaleOffset;

        public AnimationCurve loopCurve;
    }

    public class PointWithIndex
    {
        public Vector2 point;

        public int index;
    }

    private sealed class _003CDoAnimation_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoAnimation_003Ed__13(int _003C_003E1__state)
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
            DecorateRoomSceneVisualItem decorateRoomSceneVisualItem = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003CtransformToChange_003E5__2 = decorateRoomSceneVisualItem.animationTransform;
                    decorateRoomSceneVisualItem.animationOffsetTransform.localPosition = Vector3.zero;
                    this._003CtransformToChange_003E5__2.localScale = decorateRoomSceneVisualItem.settings.startScale;
                    this._003Ctime_003E5__3 = 0f;
                    if (this.delay <= 0f)
                    {
                        goto IL_C7;
                    }
                    break;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_10B;
                case 3:
                    this._003C_003E1__state = -1;
                    goto IL_1F3;
                case 4:
                    this._003C_003E1__state = -1;
                    goto IL_214;
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
            this._003Ctime_003E5__3 -= this.delay;
            IL_C7:
            this._003CmyDelay_003E5__4 = decorateRoomSceneVisualItem.settings.delayPerIndex * (float)decorateRoomSceneVisualItem.index;
            IL_10B:
            if (this._003Ctime_003E5__3 <= this._003CmyDelay_003E5__4)
            {
                this._003Ctime_003E5__3 += Time.deltaTime;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
            }
            this._003Ctime_003E5__3 -= this._003CmyDelay_003E5__4;
            GraphicsSceneConfig.VisualObject visualObject = decorateRoomSceneVisualItem.visualObjectBehaviour.visualObject;
            bool markersActive = !visualObject.isOwned && visualObject.IsUnlocked(decorateRoomSceneVisualItem.screen.scene);
            decorateRoomSceneVisualItem.visualObjectBehaviour.SetMarkersActive(markersActive);
            IL_1F3:
            if (this._003Ctime_003E5__3 <= decorateRoomSceneVisualItem.settings.durationOfPop)
            {
                this._003Ctime_003E5__3 += Time.deltaTime;
                float num2 = Mathf.InverseLerp(0f, decorateRoomSceneVisualItem.settings.durationOfPop, this._003Ctime_003E5__3);
                if (decorateRoomSceneVisualItem.settings.popCurve != null)
                {
                    num2 = decorateRoomSceneVisualItem.settings.popCurve.Evaluate(num2);
                }
                Vector3 localScale = Vector3.LerpUnclamped(decorateRoomSceneVisualItem.settings.startScale, Vector3.one, num2);
                this._003CtransformToChange_003E5__2.localScale = localScale;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 3;
                return true;
            }
            this._003Ctime_003E5__3 = 0f;
            IL_214:
            this._003Ctime_003E5__3 += Time.deltaTime;
            float num3 = Mathf.PingPong(this._003Ctime_003E5__3, decorateRoomSceneVisualItem.settings.loopDuration);
            num3 = decorateRoomSceneVisualItem.settings.loopCurve.Evaluate(num3);
            Vector3 localPosition = Vector3.LerpUnclamped(Vector3.zero, decorateRoomSceneVisualItem.settings.loopOffset, num3);
            Vector3 localScale2 = Vector3.LerpUnclamped(Vector3.one, decorateRoomSceneVisualItem.settings.loopScaleOffset, num3);
            decorateRoomSceneVisualItem.animationTransform.localScale = localScale2;
            decorateRoomSceneVisualItem.animationOffsetTransform.transform.localPosition = localPosition;
            this._003C_003E2__current = null;
            this._003C_003E1__state = 4;
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

        public DecorateRoomSceneVisualItem _003C_003E4__this;

        public float delay;

        private RectTransform _003CtransformToChange_003E5__2;

        private float _003Ctime_003E5__3;

        private float _003CmyDelay_003E5__4;
    }
}
