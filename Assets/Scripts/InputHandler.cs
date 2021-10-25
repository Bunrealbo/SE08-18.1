using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public event InputHandler.OnClickDelegate onClick;

	public void Clear()
	{
		for (int i = 0; i < this.inputPointers.Count; i++)
		{
			this.inputPointers[i].isDown = false;
		}
	}

	public bool IsAnyDown
	{
		get
		{
			for (int i = 0; i < this.inputPointers.Count; i++)
			{
				if (this.inputPointers[i].isDown)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool IsDown(int pointerId)
	{
		InputHandler.PointerData pointerData = this.GetPointerData(pointerId);
		return pointerData != null && pointerData.isDown;
	}

	public Vector2 Position(int pointerId)
	{
		InputHandler.PointerData pointerData = this.GetPointerData(pointerId);
		if (pointerData == null)
		{
			return Vector2.zero;
		}
		return pointerData.position;
	}

	public InputHandler.PointerData FirstDownPointer()
	{
		for (int i = 0; i < this.inputPointers.Count; i++)
		{
			InputHandler.PointerData pointerData = this.inputPointers[i];
			if (pointerData.isDown)
			{
				return pointerData;
			}
		}
		return null;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.UpdateMousePosition(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.UpdateMousePosition(eventData);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		this.UpdateMousePosition(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		int pointerId = eventData.pointerId;
		InputHandler.PointerData orCreatePointerData = this.GetOrCreatePointerData(pointerId);
		orCreatePointerData.downFrame = Time.frameCount;
		orCreatePointerData.isDown = true;
		orCreatePointerData.position = eventData.position;
		orCreatePointerData.startPosition = orCreatePointerData.position;
		orCreatePointerData.downTime = Time.unscaledTime;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		int pointerId = eventData.pointerId;
		InputHandler.PointerData orCreatePointerData = this.GetOrCreatePointerData(pointerId);
		orCreatePointerData.isDown = false;
		orCreatePointerData.position = eventData.position;
		float num = Vector2.Distance(orCreatePointerData.startPosition, orCreatePointerData.position);
		float num2 = Time.unscaledTime - orCreatePointerData.downTime;
		int num3 = Time.frameCount - orCreatePointerData.downFrame;
		bool flag = num2 < this.minTime || num3 < this.minFrames;
		bool flag2 = num <= (float)this.minDistance;
		if (flag && flag2 && this.onClick != null)
		{
			this.onClick(orCreatePointerData.position);
		}
	}

	private void UpdateMousePosition(PointerEventData eventData)
	{
		int pointerId = eventData.pointerId;
		this.GetOrCreatePointerData(pointerId).position = eventData.position;
	}

	private InputHandler.PointerData GetOrCreatePointerData(int pointerId)
	{
		InputHandler.PointerData pointerData = this.GetPointerData(pointerId);
		if (pointerData != null)
		{
			return pointerData;
		}
		return this.AddPointerData(pointerId);
	}

	private InputHandler.PointerData GetPointerData(int pointerId)
	{
		for (int i = 0; i < this.inputPointers.Count; i++)
		{
			InputHandler.PointerData pointerData = this.inputPointers[i];
			if (pointerData.pointerId == pointerId)
			{
				return pointerData;
			}
		}
		return null;
	}

	private InputHandler.PointerData AddPointerData(int pointerId)
	{
		InputHandler.PointerData pointerData = new InputHandler.PointerData();
		pointerData.pointerId = pointerId;
		this.inputPointers.Add(pointerData);
		return pointerData;
	}

	[SerializeField]
	private int minDistance = 6;

	[SerializeField]
	private float minTime = 0.3f;

	[SerializeField]
	private int minFrames = 3;

	private List<InputHandler.PointerData> inputPointers = new List<InputHandler.PointerData>();

	public delegate void OnClickDelegate(Vector2 screenPosition);

	public class PointerData
	{
		public int pointerId;

		public Vector2 position;

		public bool isDown;

		public int downFrame;

		public Vector2 startPosition;

		public float downTime;
	}
}
