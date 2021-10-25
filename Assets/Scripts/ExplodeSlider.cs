using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ExplodeSlider : MonoBehaviour
{
	public ExplodeSlider.ExplosionSettings settings
	{
		get
		{
			return ScriptableObjectSingleton<CarsDB>.instance.explosionSettings;
		}
	}

	public float value
	{
		get
		{
			return this.slider.value;
		}
	}

	public bool isExploded
	{
		get
		{
			return this.value > 0f;
		}
	}

	private CarScene scene
	{
		get
		{
			if (this.screen == null)
			{
				return null;
			}
			return this.screen.scene;
		}
	}

	public void Reset()
	{
		this.StopSlider();
		this.slider.value = 0f;
		this.UpdateButtonActive(0f);
	}

	public void Init(AssembleCarScreen screen)
	{
		this.screen = screen;
		this.scene.carModel.explodeAnimation.SetTimeTo(this.slider.value);
		ExplodeAnimation explodeAnimation = screen.scene.carModel.explodeAnimation;
		GGUtil.SetActive(this, explodeAnimation.hasParts);
		this.UpdateButtonActive(this.slider.value);
	}

	public void StopSlider()
	{
		this.sliderAnim = null;
	}

	private void UpdateButtonActive(float value)
	{
		GGUtil.SetActive(this.unexplodeButton, value > 0f);
	}

	private IEnumerator SpringOnSlider(float change)
	{
		return new ExplodeSlider._003CSpringOnSlider_003Ed__18(0)
		{
			_003C_003E4__this = this,
			change = change
		};
	}

	public IEnumerator Unexplode()
	{
		return new ExplodeSlider._003CUnexplode_003Ed__19(0)
		{
			_003C_003E4__this = this
		};
	}

	public void ButtonCallbacks_OnUnexplode()
	{
		this.sliderAnim = this.Unexplode();
	}

	public void SliderCallback_OnDragStart()
	{
		this.sliderValueOnDragStart = this.slider.value;
	}

	public void SliderCallback_OnDragEnd()
	{
		float change = this.slider.value - this.sliderValueOnDragStart;
		this.sliderAnim = this.SpringOnSlider(change);
	}

	public void SliderCallback_OnValueChanged()
	{
		if (this.scene == null)
		{
			return;
		}
		this.scene.carModel.explodeAnimation.SetTimeTo(this.slider.value);
	}

	private void Update()
	{
		if (this.sliderAnim != null && !this.sliderAnim.MoveNext())
		{
			this.sliderAnim = null;
		}
	}

	[SerializeField]
	private Slider slider;

	[SerializeField]
	private Transform unexplodeButton;

	[NonSerialized]
	private AssembleCarScreen screen;

	[NonSerialized]
	private IEnumerator sliderAnim;

	[NonSerialized]
	private float sliderValueOnDragStart;

	[Serializable]
	public class ExplosionSettings
	{
		public float minValueWhenSwitch = 0.1f;

		public float distanceFromCenter = 3f;

		public float durationToReturn = 0.5f;

		public AnimationCurve unexplodeTimeCurve;

		public AnimationCurve sliderCurve;
	}

	private sealed class _003CSpringOnSlider_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CSpringOnSlider_003Ed__18(int _003C_003E1__state)
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
			ExplodeSlider explodeSlider = this._003C_003E4__this;
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
				if (explodeSlider.scene == null)
				{
					return false;
				}
				float value = explodeSlider.slider.value;
				this._003CcurrentTime_003E5__2 = value;
				this._003CexplodeAnimation_003E5__3 = explodeSlider.scene.carModel.explodeAnimation;
				this._003CendTime_003E5__4 = this._003CexplodeAnimation_003E5__3.ClosestFullTime(this._003CcurrentTime_003E5__2, this.change);
				float num2 = 1f;
				this._003Cduration_003E5__5 = Mathf.Abs(this._003CendTime_003E5__4 - this._003CcurrentTime_003E5__2) * num2;
				this._003Ctime_003E5__6 = 0f;
			}
			if (this._003Ctime_003E5__6 > this._003Cduration_003E5__5)
			{
				explodeSlider.UpdateButtonActive(explodeSlider.slider.value);
				return false;
			}
			this._003Ctime_003E5__6 += Time.deltaTime;
			float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__5, this._003Ctime_003E5__6);
			float t = explodeSlider.settings.sliderCurve.Evaluate(time);
			float num3 = Mathf.Lerp(this._003CcurrentTime_003E5__2, this._003CendTime_003E5__4, t);
			explodeSlider.slider.value = num3;
			this._003CexplodeAnimation_003E5__3.SetTimeTo(num3);
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

		public ExplodeSlider _003C_003E4__this;

		public float change;

		private float _003CcurrentTime_003E5__2;

		private ExplodeAnimation _003CexplodeAnimation_003E5__3;

		private float _003CendTime_003E5__4;

		private float _003Cduration_003E5__5;

		private float _003Ctime_003E5__6;
	}

	private sealed class _003CUnexplode_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CUnexplode_003Ed__19(int _003C_003E1__state)
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
			ExplodeSlider explodeSlider = this._003C_003E4__this;
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
				GGUtil.SetActive(explodeSlider.unexplodeButton, false);
				this._003CstartValue_003E5__2 = explodeSlider.slider.value;
				this._003Cduration_003E5__3 = explodeSlider.settings.durationToReturn;
				this._003Ctime_003E5__4 = 0f;
			}
			if (this._003Ctime_003E5__4 >= this._003Cduration_003E5__3)
			{
				return false;
			}
			this._003Ctime_003E5__4 += Time.deltaTime;
			float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__4);
			float t = explodeSlider.settings.unexplodeTimeCurve.Evaluate(time);
			float num2 = Mathf.Lerp(this._003CstartValue_003E5__2, 0f, t);
			explodeSlider.slider.value = num2;
			explodeSlider.scene.carModel.explodeAnimation.SetTimeTo(num2);
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

		public ExplodeSlider _003C_003E4__this;

		private float _003CstartValue_003E5__2;

		private float _003Cduration_003E5__3;

		private float _003Ctime_003E5__4;
	}
}
