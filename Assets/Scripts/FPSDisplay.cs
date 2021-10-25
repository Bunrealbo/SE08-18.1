using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private void Update()
    {
        if (this.enumerator == null)
        {
            this.enumerator = this.DoFPSDisplay();
        }
        this.enumerator.MoveNext();
    }

    private IEnumerator DoFPSDisplay()
    {
        return new FPSDisplay._003CDoFPSDisplay_003Ed__5(0)
        {
            _003C_003E4__this = this
        };
    }

    [SerializeField]
    private TextMeshProUGUI label;

    [SerializeField]
    private int iterationsBeforeDisplay = 1;

    [SerializeField]
    private int targetFrameRate = 60;

    private IEnumerator enumerator;

    private sealed class _003CDoFPSDisplay_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoFPSDisplay_003Ed__5(int _003C_003E1__state)
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
            FPSDisplay fpsdisplay = this._003C_003E4__this;
            if (num != 0)
            {
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
                int num2 = this._003Ci_003E5__3;
                this._003Ci_003E5__3 = num2 + 1;
                goto IL_90;
            }
            else
            {
                this._003C_003E1__state = -1;
            }
            IL_1E:
            this._003CtotalFps_003E5__2 = 0f;
            if (fpsdisplay.iterationsBeforeDisplay <= 0)
            {
                fpsdisplay.iterationsBeforeDisplay = 1;
            }
            Application.targetFrameRate = fpsdisplay.targetFrameRate;
            this._003Ci_003E5__3 = 0;
            IL_90:
            if (this._003Ci_003E5__3 >= fpsdisplay.iterationsBeforeDisplay)
            {
                float num3 = this._003CtotalFps_003E5__2 / (float)fpsdisplay.iterationsBeforeDisplay;
                GGUtil.ChangeText(fpsdisplay.label, string.Format("{0:0.}", num3));
                goto IL_1E;
            }
            float unscaledDeltaTime = Time.unscaledDeltaTime;
            this._003CtotalFps_003E5__2 += 1f / unscaledDeltaTime;
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

        public FPSDisplay _003C_003E4__this;

        private float _003CtotalFps_003E5__2;

        private int _003Ci_003E5__3;
    }
}
