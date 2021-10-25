using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarSprayToolTarget : MonoBehaviour
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

	public void Init(Action<Vector3> onDrag)
	{
		this.onDrag = onDrag;
		GGUtil.Hide(this.sprayParticleSystem);
		GGUtil.Hide(this.widgetsToHide);
		this.notPressedStyle.Apply();
		this.dragState = default(CarSprayToolTarget.DragState);
	}

	public void OnPress(BaseEventData data)
	{
		this.OnBeginDrag(data);
	}

	public void OnRelease(BaseEventData data)
	{
		this.OnEndDrag(data);
	}

	public void OnBeginDrag(BaseEventData data)
	{
		PointerEventData position = data as PointerEventData;
		this.SetPosition(position);
		GGUtil.Show(this.sprayParticleSystem);
		ParticleSystemHelper.SetEmmisionActiveRecursive(this.sprayParticleSystem, true);
		GGUtil.Hide(this.widgetsToHide);
		this.pressedStyle.Apply();
		this.dragState.isDragging = true;
	}

	public void OnEndDrag(BaseEventData data)
	{
		PointerEventData position = data as PointerEventData;
		this.SetPosition(position);
		ParticleSystemHelper.SetEmmisionActiveRecursive(this.sprayParticleSystem, false);
		GGUtil.Hide(this.widgetsToHide);
		this.notPressedStyle.Apply();
		this.dragState.isDragging = false;
	}

	public void OnDrag(BaseEventData data)
	{
		PointerEventData position = data as PointerEventData;
		this.SetPosition(position);
	}

	private void SetPosition(PointerEventData pointerData)
	{
		if (pointerData == null)
		{
			return;
		}
		Transform parent = base.transform.parent;
		base.transform.localPosition = parent.InverseTransformPoint(this.uiCamera.ScreenToWorldPoint(pointerData.position));
		this.dragState.lastScreenPosition = pointerData.position;
		if (this.onDrag != null)
		{
			this.onDrag(pointerData.position);
		}
	}

	[SerializeField]
	private ParticleSystem sprayParticleSystem;

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private VisualStyleSet notPressedStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet pressedStyle = new VisualStyleSet();

	private Camera uiCamera_;

	private Action<Vector3> onDrag;

	[NonSerialized]
	public CarSprayToolTarget.DragState dragState;

	public struct DragState
	{
		public bool isDragging;

		public Vector3 lastScreenPosition;
	}
}
