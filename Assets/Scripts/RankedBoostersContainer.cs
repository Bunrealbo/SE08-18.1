using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

public class RankedBoostersContainer : MonoBehaviour
{
	public void Init(int rankLevel)
	{
		GGUtil.SetActive(this.mainContainer, rankLevel > 0);
		GiftsDefinitionDB.BuildupBooster.BoosterGift boosterGiftForLevel = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.buildupBooster.GetBoosterGiftForLevel(rankLevel);
		for (int i = 0; i < this.boosters.Count; i++)
		{
			RankedBoostersContainer.Booster booster = this.boosters[i];
			bool flag = i + 1 == rankLevel;
			booster.SetActive(flag);
			if (flag)
			{
				booster.SetImages(boosterGiftForLevel);
			}
		}
	}

	[SerializeField]
	private List<RankedBoostersContainer.Booster> boosters = new List<RankedBoostersContainer.Booster>();

	[SerializeField]
	private Transform mainContainer;

	[Serializable]
	public class Booster
	{
		public void SetImages(GiftsDefinitionDB.BuildupBooster.BoosterGift booster)
		{
			if (booster == null)
			{
				return;
			}
			List<BoosterConfig> boosterConfig = booster.boosterConfig;
			int num = 0;
			while (num < this.images.Count && num < boosterConfig.Count)
			{
				Image image = this.images[num];
				BoosterConfig boosterConfig2 = boosterConfig[num];
				ChipDisplaySettings chipDisplaySettings = Match3Settings.instance.GetChipDisplaySettings(boosterConfig2.chipType, ItemColor.Uncolored);
				if (chipDisplaySettings != null)
				{
					GGUtil.SetSprite(image, chipDisplaySettings.displaySprite);
				}
				num++;
			}
		}

		public void SetActive(bool active)
		{
			GGUtil.SetActive(this.container, active);
		}

		[SerializeField]
		private Transform container;

		[SerializeField]
		private List<Image> images = new List<Image>();
	}
}
