using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyGiftsScreenGiftCard : MonoBehaviour
{
	public void Init(int giftIndex, float giftScale, bool isSelected)
	{
		GGUtil.Hide(this.widgetsToHide);
		GGUtil.ChangeText(this.label, string.Format("Day {0}", giftIndex + 1));
		this.gift.localScale = Vector3.one * giftScale;
		if (isSelected)
		{
			this.selectedStyle.Apply();
		}
		float alpha = isSelected ? 1f : this.alphaWenNotSelected;
		GGUtil.SetAlpha(this.canvasGroup, alpha);
	}

	[SerializeField]
	private RectTransform gift;

	[SerializeField]
	private TextMeshProUGUI label;

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private VisualStyleSet selectedStyle = new VisualStyleSet();

	[SerializeField]
	private CanvasGroup canvasGroup;

	[SerializeField]
	private float alphaWenNotSelected = 0.85f;
}
