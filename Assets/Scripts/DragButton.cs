using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	private Camera uiCamera
	{
		get
		{
			if (this.uiCamera_ == null)
			{
				this.uiCamera_ = NavigationManager.instance.GetCamera();
			}
			return this.uiCamera_;
		}
	}

	public DragButton.Settings settings
	{
		get
		{
			return Match3Settings.instance.dragButtonSettings;
		}
	}

	public void Init(ConfirmPurchasePanel panel, Sprite sprite)
	{
		base.transform.localPosition = Vector3.zero;
		this.panel = panel;
		this.trailRenderer.sortingLayerName = "UI";
		this.trailRenderer.Clear();
		this.dragging = false;
		this.ResetOffsetTransform();
		base.transform.localScale = Vector3.one;
	}

	private void ResetOffsetTransform()
	{
		this.offsetTransform.localPosition = Vector3.zero;
		this.arrowOffsetTransform.localPosition = Vector3.zero;
		this.animationEnum = this.DoAnimate();
	}

	public void StopAnimation()
	{
		this.offsetTransform.localPosition = Vector3.zero;
		this.arrowOffsetTransform.localPosition = Vector3.zero;
		this.animationEnum = null;
	}

	private IEnumerator DoAnimate()
	{
		return new DragButton._003CDoAnimate_003Ed__16(0)
		{
			_003C_003E4__this = this
		};
	}

	public void OnDragStart(BaseEventData data)
	{
		this.panel.OnDragStart();
		this.trailRenderer.Clear();
		this.dragging = true;
		base.transform.localScale = Vector3.one * this.settings.pressScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.isDrag = false;
		base.transform.localScale = Vector3.one * this.settings.pressScale;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		base.transform.localScale = Vector3.one;
		if (!this.isDrag)
		{
			this.panel.OnButtonClick();
		}
	}

	public void OnDrag(BaseEventData data)
	{
		this.isDrag = true;
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.pointerDrag != base.gameObject)
		{
			return;
		}
		base.transform.position = this.uiCamera.ScreenToWorldPoint(pointerEventData.position);
		this.panel.OnDrag();
	}

	public void OnDragEnd(BaseEventData data)
	{
		if ((data as PointerEventData).pointerDrag != base.gameObject)
		{
			return;
		}
		base.transform.localPosition = Vector3.zero;
		this.panel.OnDragEnd();
		this.dragging = false;
		this.ResetOffsetTransform();
		base.transform.localScale = Vector3.one;
	}

	private void Update()
	{
		if (this.dragging)
		{
			return;
		}
		if (this.animationEnum != null)
		{
			this.animationEnum.MoveNext();
		}
	}

	private Camera uiCamera_;

	[SerializeField]
	private TrailRenderer trailRenderer;

	[SerializeField]
	private RectTransform offsetTransform;

	[SerializeField]
	private RectTransform arrowOffsetTransform;

	private ConfirmPurchasePanel panel;

	private bool dragging;

	private IEnumerator animationEnum;

	private bool isDrag;

	[Serializable]
	public class Settings
	{
		public float bobDuration;

		public float bobDisplace;

		public AnimationCurve animationCurve;

		public float pressScale = 1.3f;
	}

	private sealed class _003CDoAnimate_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimate_003Ed__16(int _003C_003E1__state)
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
			DragButton dragButton = this._003C_003E4__this;
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
			this._003Ctime_003E5__2 += Time.deltaTime;
			float num2 = Mathf.PingPong(this._003Ctime_003E5__2, dragButton.settings.bobDuration);
			num2 = dragButton.settings.animationCurve.Evaluate(num2);
			Vector3 normalized = dragButton.transform.InverseTransformPoint(dragButton.panel.dragTarget.transform.position).normalized;
			float d = Mathf.LerpUnclamped(0f, dragButton.settings.bobDisplace, num2);
			dragButton.offsetTransform.localPosition = normalized * d;
			dragButton.arrowOffsetTransform.localPosition = Vector3.up * d;
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

		public DragButton _003C_003E4__this;

		private float _003Ctime_003E5__2;
	}
}
