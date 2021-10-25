using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class GrowingElementBehaviour : SlotComponentBehavoiour
{
	public void Init()
	{
		this.SetLevel(0);
		GGUtil.SetActive(this, true);
	}

	public void StopAllAnimations()
	{
		for (int i = 0; i < this.growingElements.Count; i++)
		{
			this.growingElements[i].StopAnimation();
		}
	}

	private GrowingElementPot GetElementPot(int elementIndex)
	{
		return this.growingElements[Mathf.Clamp(elementIndex, 0, this.growingElements.Count - 1)];
	}

	public void StartAnimationFor(int elementIndex)
	{
		this.GetElementPot(elementIndex).AnimateIn();
	}

	public void SetLevel(int level)
	{
		for (int i = 0; i < this.growingElements.Count; i++)
		{
			this.growingElements[i].SetActve(level > i);
		}
	}

	public Vector3 WorldPositionForElement(int elementIndex)
	{
		return this.GetElementPot(elementIndex).WorldPositionForFlower;
	}

	[SerializeField]
	private List<GrowingElementPot> growingElements = new List<GrowingElementPot>();
}
