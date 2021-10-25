using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class FlyingSaucerBehaviour : MonoBehaviour
{
	public void Init(ChipType chipType, ItemColor itemColor)
	{
		for (int i = 0; i < this.visualStyles.Count; i++)
		{
			FlyingSaucerBehaviour.VisualStyle visualStyle = this.visualStyles[i];
			visualStyle.SetActive(visualStyle.IsApplicable(chipType, itemColor));
		}
		ChipDisplaySettings chipDisplaySettings = Match3Settings.instance.GetChipDisplaySettings(chipType, itemColor);
		if (this.iconSprite != null && chipDisplaySettings != null)
		{
			this.iconSprite.sprite = chipDisplaySettings.displaySprite;
		}
	}

	public void RemoveFromGame()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[SerializeField]
	private SpriteRenderer iconSprite;

	[SerializeField]
	private List<FlyingSaucerBehaviour.VisualStyle> visualStyles = new List<FlyingSaucerBehaviour.VisualStyle>();

	[Serializable]
	public class VisualStyle
	{
		public bool IsApplicable(ChipType chipType, ItemColor itemColor)
		{
			bool flag = chipType == ChipType.Chip;
			return this.chipType == chipType && (!flag || this.itemColor == itemColor);
		}

		public void SetActive(bool active)
		{
			GGUtil.SetActive(this.visualItems, active);
		}

		[SerializeField]
		private ChipType chipType;

		[SerializeField]
		private ItemColor itemColor;

		[SerializeField]
		private List<Transform> visualItems = new List<Transform>();
	}
}
