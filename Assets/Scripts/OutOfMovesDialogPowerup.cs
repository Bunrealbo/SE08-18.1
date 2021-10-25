using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutOfMovesDialogPowerup : MonoBehaviour
{
	public void Init(ChipType powerupType, int powerupCount)
	{
		GGUtil.SetActive(this.widgetsToHide, false);
		for (int i = 0; i < this.powerups.Count; i++)
		{
			OutOfMovesDialogPowerup.PowerupContainer powerupContainer = this.powerups[i];
			if (powerupContainer.powerupType == powerupType)
			{
				GGUtil.SetActive(powerupContainer.container, true);
				if (!(powerupContainer.image == null))
				{
					ChipDisplaySettings chipDisplaySettings = Match3Settings.instance.GetChipDisplaySettings(powerupContainer.powerupType, ItemColor.Uncolored);
					if (chipDisplaySettings != null)
					{
						GGUtil.SetSprite(powerupContainer.image, chipDisplaySettings.displaySprite);
					}
				}
			}
		}
		GGUtil.ChangeText(this.countLabel, string.Format("+{0}", powerupCount));
	}

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private List<OutOfMovesDialogPowerup.PowerupContainer> powerups = new List<OutOfMovesDialogPowerup.PowerupContainer>();

	[SerializeField]
	private TextMeshProUGUI countLabel;

	[Serializable]
	public class PowerupContainer
	{
		public ChipType powerupType;

		public RectTransform container;

		public Image image;
	}
}
