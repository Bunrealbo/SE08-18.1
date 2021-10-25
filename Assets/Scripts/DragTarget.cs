using System;
using GGMatch3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTarget : MonoBehaviour
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

	public void Init(ConfirmPurchasePanel panel, Sprite sprite)
	{
		this.panel = panel;
		this.image.sprite = sprite;
	}

	public void OnDrop(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		Vector3 dragButtonWorldPosition = this.uiCamera.ScreenToWorldPoint(pointerEventData.position);
		if (this.panel.IsTargetIn(dragButtonWorldPosition))
		{
			this.panel.OnPurchaseConfirmed();
		}
	}

	private Camera uiCamera_;

	[SerializeField]
	private Image image;

	private ConfirmPurchasePanel panel;
}
