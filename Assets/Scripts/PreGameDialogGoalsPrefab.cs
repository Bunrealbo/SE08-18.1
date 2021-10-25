using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class PreGameDialogGoalsPrefab : MonoBehaviour
{
	public void Init(GoalConfig config, Match3StagesDB.Stage stage)
	{
		GGUtil.SetActive(this.widgetsToHide, false);
		string label;
		if (config.isCollectAllPresentAtStart)
		{
			List<Match3StagesDB.LevelReference> allLevelReferences = stage.allLevelReferences;
			int num = 0;
			for (int i = 0; i < allLevelReferences.Count; i++)
			{
				LevelDefinition level = allLevelReferences[i].level;
				num += level.CountChips(config.chipType);
			}
			label = num.ToString();
		}
		else
		{
			label = config.collectCount.ToString();
		}
		ChipDisplaySettings chipDisplaySettings = Match3Settings.instance.GetChipDisplaySettings(config.chipType, config.itemColor);
		if (chipDisplaySettings != null)
		{
			GGUtil.Show(this.genericIconWithText);
			this.genericIconWithText.SetSprite(chipDisplaySettings.displaySprite);
			this.genericIconWithText.SetLabel(label);
			return;
		}
		for (int j = 0; j < this.namedSprites.Count; j++)
		{
			PreGameDialogGoalsPrefab.NamedSprite namedSprite = this.namedSprites[j];
			if (namedSprite.IsMatching(config))
			{
				namedSprite.SetActive(true);
				namedSprite.SetLabel(label);
			}
			else
			{
				namedSprite.SetActive(false);
			}
		}
	}

	[SerializeField]
	private List<PreGameDialogGoalsPrefab.NamedSprite> namedSprites = new List<PreGameDialogGoalsPrefab.NamedSprite>();

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private PreGameDialogGoalPrefabVisualConfig genericIconWithText;

	[Serializable]
	public class NamedSprite
	{
		public bool IsMatching(GoalConfig config)
		{
			if (this.goalType != config.goalType)
			{
				return false;
			}
			if (this.useItemColor)
			{
				return this.chipType == config.chipType && this.itemColor == config.itemColor;
			}
			return this.chipType == config.chipType;
		}

		public void SetActive(bool flag)
		{
			GGUtil.SetActive(this.iconWithText.transform, flag);
		}

		public void SetLabel(string text)
		{
			this.iconWithText.SetLabel(text);
		}

		public ChipType chipType;

		public GoalType goalType;

		public ItemColor itemColor;

		public bool useItemColor;

		public PreGameDialogGoalPrefabVisualConfig iconWithText;
	}
}
