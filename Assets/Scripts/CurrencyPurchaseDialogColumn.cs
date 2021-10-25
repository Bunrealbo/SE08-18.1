using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPurchaseDialogColumn : MonoBehaviour
{
	public Vector2 GetSize()
	{
		return this.containingRectangle.rect.size;
	}

	public void Init(List<RectTransform> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			items[i].parent = this.parent;
		}
	}

	[SerializeField]
	private RectTransform parent;

	[SerializeField]
	private RectTransform containingRectangle;
}
