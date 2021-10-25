using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CurrencyPrefabAnimation : MonoBehaviour
{
    public void Init()
    {
        this.canvasGroup.alpha = this.alphaCurve.Evaluate(0f);
        this.scaler.LocalScale(this.targetTransform, this.scaleCurve.Evaluate(0f));
    }

    public void Play(float delay, Action onEnd = null)
    {
        this.animateInEnum = this.DoPlay(delay, onEnd);
    }

    public void Stop()
    {
        this.animateInEnum = null;
    }

    public IEnumerator DoPlay(float delay, Action onEnd = null)
    {
        return new CurrencyPrefabAnimation._003CDoPlay_003Ed__10(0)
        {
            _003C_003E4__this = this,
            delay = delay,
            onEnd = onEnd
        };
    }

    public void Update()
    {
        if (this.animateInEnum != null)
        {
            this.animateInEnum.MoveNext();
        }
    }

    [SerializeField]
    private AnimationCurve scaleCurve;

    [SerializeField]
    private AnimationCurve alphaCurve;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float duration;

    [SerializeField]
    private CurrencyPrefabAnimation.Scaler scaler = new CurrencyPrefabAnimation.Scaler();

    public IEnumerator animateInEnum;

    [Serializable]
    public class Scaler
    {
        public void LocalScale(Transform trans, float scale)
        {
            Vector3 localScale = trans.localScale;
            if (this.scaleX)
            {
                localScale.x = scale;
            }
            if (this.scaleY)
            {
                localScale.y = scale;
            }
            if (this.scaleZ)
            {
                localScale.z = scale;
            }
            trans.localScale = localScale;
        }

        [SerializeField]
        private bool scaleX = true;

        [SerializeField]
        private bool scaleY = true;

        [SerializeField]
        private bool scaleZ = true;
    }

    private sealed class _003CDoPlay_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoPlay_003Ed__10(int _003C_003E1__state)
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
            CurrencyPrefabAnimation currencyPrefabAnimation = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    {
                        this._003C_003E1__state = -1;
                        currencyPrefabAnimation.canvasGroup.alpha = currencyPrefabAnimation.alphaCurve.Evaluate(0f);
                        float scale = currencyPrefabAnimation.scaleCurve.Evaluate(0f);
                        currencyPrefabAnimation.scaler.LocalScale(currencyPrefabAnimation.targetTransform, scale);
                        this._003Ctime_003E5__2 = 0f;
                        break;
                    }
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_12B;
                default:
                    return false;
            }
            if (this._003Ctime_003E5__2 < this.delay)
            {
                this._003Ctime_003E5__2 += Time.deltaTime;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            this._003Ctime_003E5__2 = 0f;
            IL_12B:
            if (this._003Ctime_003E5__2 >= currencyPrefabAnimation.duration)
            {
                currencyPrefabAnimation.animateInEnum = null;
                if (this.onEnd != null)
                {
                    this.onEnd();
                }
                return false;
            }
            this._003Ctime_003E5__2 += Time.deltaTime;
            float time = this._003Ctime_003E5__2 / currencyPrefabAnimation.duration;
            float alpha = currencyPrefabAnimation.alphaCurve.Evaluate(time);
            float scale2 = currencyPrefabAnimation.scaleCurve.Evaluate(time);
            currencyPrefabAnimation.scaler.LocalScale(currencyPrefabAnimation.targetTransform, scale2);
            currencyPrefabAnimation.canvasGroup.alpha = alpha;
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

        public CurrencyPrefabAnimation _003C_003E4__this;

        public float delay;

        public Action onEnd;

        private float _003Ctime_003E5__2;
    }
}
