using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftBoxScreenGiftItem : MonoBehaviour
{
	public void Init(GiftBoxScreen.Gift gift)
	{
		GGUtil.Hide(this.widgetsToHide);
		GGUtil.Show(this);
		GiftBoxScreenGiftItem.GiftTypeStyle applicableStyle = this.GetApplicableStyle(gift);
		if (applicableStyle == null)
		{
			UnityEngine.Debug.LogError("NO APPLICABLE STYLE!!!");
			return;
		}
		applicableStyle.Apply();
		if (gift.giftType == GiftBoxScreen.GiftType.Energy)
		{
			GGUtil.ChangeText(applicableStyle.label, string.Format("{0}hr", gift.hours));
			return;
		}
		GGUtil.ChangeText(applicableStyle.label, gift.amount.ToString());
	}

	private GiftBoxScreenGiftItem.GiftTypeStyle GetApplicableStyle(GiftBoxScreen.Gift gift)
	{
		for (int i = 0; i < this.giftStyles.Count; i++)
		{
			GiftBoxScreenGiftItem.GiftTypeStyle giftTypeStyle = this.giftStyles[i];
			if (giftTypeStyle.isApplicable(gift))
			{
				return giftTypeStyle;
			}
		}
		return null;
	}

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private List<GiftBoxScreenGiftItem.GiftTypeStyle> giftStyles = new List<GiftBoxScreenGiftItem.GiftTypeStyle>();

	[Serializable]
	public class GiftTypeStyle
	{
		public bool isApplicable(GiftBoxScreen.Gift gift)
		{
			if (this.giftType != gift.giftType)
			{
				return false;
			}
			if (this.giftType == GiftBoxScreen.GiftType.Booster)
			{
				return this.boosterType == gift.boosterType;
			}
			return this.giftType != GiftBoxScreen.GiftType.Powerup || this.powerupType == gift.powerupType;
		}

		public void Apply()
		{
			this.style.Apply();
			if (this.imageRepresentation == null)
			{
				return;
			}
			ChipDisplaySettings chipDisplaySettings = Match3Settings.instance.GetChipDisplaySettings(this.chipTypeUsedForRepresentation, ItemColor.Uncolored);
			if (chipDisplaySettings == null)
			{
				return;
			}
			this.imageRepresentation.sprite = chipDisplaySettings.displaySprite;
		}

		[SerializeField]
		private GiftBoxScreen.GiftType giftType;

		[SerializeField]
		private PowerupType powerupType;

		[SerializeField]
		private BoosterType boosterType;

		[SerializeField]
		private ChipType chipTypeUsedForRepresentation;

		[SerializeField]
		private Image imageRepresentation;

		[SerializeField]
		private VisualStyleSet style = new VisualStyleSet();

		public TextMeshProUGUI label;
	}
}
