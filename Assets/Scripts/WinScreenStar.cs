using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenStar : MonoBehaviour
{
	public void Show(WinScreen winScreen)
	{
		this.winScreen = winScreen;
		GGUtil.SetActive(this, true);
		GGUtil.SetAlpha(this.image, 1f);
		base.transform.localPosition = Vector3.zero;
		base.transform.localScale = Vector3.one;
		base.transform.localRotation = Quaternion.identity;
	}

	public IEnumerator DoMoveTo(RectTransform moveToTransform)
	{
		return new WinScreenStar._003CDoMoveTo_003Ed__3(0)
		{
			_003C_003E4__this = this,
			moveToTransform = moveToTransform
		};
	}

	private WinScreen.Settings settings
	{
		get
		{
			return Match3Settings.instance.winScreenSettings;
		}
	}

	[SerializeField]
	private Image image;

	private WinScreen winScreen;

	private sealed class _003CDoMoveTo_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoMoveTo_003Ed__3(int _003C_003E1__state)
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
			WinScreenStar winScreenStar = this._003C_003E4__this;
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
				this._003CmyTransform_003E5__2 = winScreenStar.image.transform;
				Vector3 vector = winScreenStar.image.transform.parent.InverseTransformPoint(this.moveToTransform.position);
				vector.z = 0f;
				WinScreen.Settings settings = winScreenStar.settings;
				this._003CstartPositionLocal_003E5__3 = this._003CmyTransform_003E5__2.localPosition;
				this._003CendPositionLocal_003E5__4 = vector;
				this._003CstartAngle_003E5__5 = 0;
				this._003CendAngle_003E5__6 = settings.starRotationAngle;
				this._003CstartScale_003E5__7 = 1;
				this._003CendScale_003E5__8 = settings.starEndScale;
				this._003Ctime_003E5__9 = 0f;
				this._003Cduration_003E5__10 = settings.starTravelDuration;
			}
			if (this._003Ctime_003E5__9 > this._003Cduration_003E5__10)
			{
				winScreenStar.winScreen.currencyPanel.DisplayForCurrency(CurrencyType.diamonds).ShowShineParticle();
				GGSoundSystem.Play(GGSoundSystem.SFXType.RecieveStar);
				return false;
			}
			this._003Ctime_003E5__9 += Time.deltaTime;
			float t = Mathf.InverseLerp(0f, this._003Cduration_003E5__10, this._003Ctime_003E5__9);
			float angle = Mathf.Lerp((float)this._003CstartAngle_003E5__5, this._003CendAngle_003E5__6, t);
			float d = Mathf.Lerp((float)this._003CstartScale_003E5__7, this._003CendScale_003E5__8, t);
			Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPositionLocal_003E5__3, this._003CendPositionLocal_003E5__4, t);
			this._003CmyTransform_003E5__2.localPosition = localPosition;
			this._003CmyTransform_003E5__2.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			this._003CmyTransform_003E5__2.localScale = Vector3.one * d;
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

		public WinScreenStar _003C_003E4__this;

		public RectTransform moveToTransform;

		private Transform _003CmyTransform_003E5__2;

		private Vector3 _003CstartPositionLocal_003E5__3;

		private Vector3 _003CendPositionLocal_003E5__4;

		private int _003CstartAngle_003E5__5;

		private float _003CendAngle_003E5__6;

		private int _003CstartScale_003E5__7;

		private float _003CendScale_003E5__8;

		private float _003Ctime_003E5__9;

		private float _003Cduration_003E5__10;
	}
}
