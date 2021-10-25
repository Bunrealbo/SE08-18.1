using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreGameDialogBoosterPrefabVisualConfig : MonoBehaviour
{
	public void Init(ChipType chipTypeUsedForRepresentation)
	{
		ChipDisplaySettings chipDisplaySettings = Match3Settings.instance.GetChipDisplaySettings(chipTypeUsedForRepresentation, ItemColor.Uncolored);
		if (chipDisplaySettings != null)
		{
			GGUtil.SetSprite(this.image, chipDisplaySettings.displaySprite);
		}
	}

	public void SetLabel(string text)
	{
		GGUtil.ChangeText(this.label, text);
	}

	public void SetStyle(bool owned)
	{
		GGUtil.SetActive(this.hideAtStart, false);
		if (owned)
		{
			this.ownedStyle.Apply();
			return;
		}
		this.notOwnedStyle.Apply();
	}

	[SerializeField]
	private Image image;

	[SerializeField]
	private VisualStyleSet ownedStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet notOwnedStyle = new VisualStyleSet();

	[SerializeField]
	private List<RectTransform> hideAtStart = new List<RectTransform>();

	[SerializeField]
	private TextMeshProUGUI label;
}
