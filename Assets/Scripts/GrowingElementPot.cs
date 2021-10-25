using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class GrowingElementPot : MonoBehaviour
{
	private GrowingElementPot.Settings settings
	{
		get
		{
			return Match3Settings.instance.growingElementPotSettings;
		}
	}

	public Vector3 WorldPositionForFlower
	{
		get
		{
			return this.flowerTransform.position;
		}
	}

	public void SetActve(bool active)
	{
		GGUtil.SetActive(this.flowerTransform, active);
	}

	public void AnimateIn()
	{
		this.animationEnum = this.DoAnimateIn();
	}

	public void StopAnimation()
	{
		this.animationEnum = null;
		GGUtil.SetScale(this.scaleTransform, this.settings.endScale);
	}

	public IEnumerator DoAnimateIn()
	{
		return new GrowingElementPot._003CDoAnimateIn_003Ed__12(0)
		{
			_003C_003E4__this = this
		};
	}

	public void Update()
	{
		if (this.animationEnum == null)
		{
			return;
		}
		if (!this.animationEnum.MoveNext())
		{
			this.animationEnum = null;
		}
	}

	[SerializeField]
	private Transform scaleTransform;

	[SerializeField]
	private Transform flowerTransform;

	[SerializeField]
	private Transform stemTransform;

	private IEnumerator animationEnum;

	[Serializable]
	public class Settings
	{
		public Vector3 startScale;

		public Vector3 endScale = Vector3.one;

		public AnimationCurve scaleCurve;

		public float scaleDuration;
	}

	private sealed class _003CDoAnimateIn_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimateIn_003Ed__12(int _003C_003E1__state)
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
			GrowingElementPot growingElementPot = this._003C_003E4__this;
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
			}
			if (this._003Ctime_003E5__2 > growingElementPot.settings.scaleDuration)
			{
				return false;
			}
			float deltaTime = Time.deltaTime;
			this._003Ctime_003E5__2 += deltaTime;
			float time = Mathf.InverseLerp(0f, growingElementPot.settings.scaleDuration, this._003Ctime_003E5__2);
			float t = growingElementPot.settings.scaleCurve.Evaluate(time);
			Vector3 scale = Vector3.Lerp(growingElementPot.settings.startScale, growingElementPot.settings.endScale, t);
			GGUtil.SetScale(growingElementPot.scaleTransform, scale);
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

		public GrowingElementPot _003C_003E4__this;

		private float _003Ctime_003E5__2;
	}
}
