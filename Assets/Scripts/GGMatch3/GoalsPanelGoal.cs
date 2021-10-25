using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GGMatch3
{
	public class GoalsPanelGoal : MonoBehaviour
	{
		public void Init(MultiLevelGoals.Goal goal)
		{
			this.goal = goal;
			GGUtil.SetActive(this.widgetsToHide, false);
			ChipType chipType = goal.config.chipType;
			ItemColor itemColor = goal.config.itemColor;
			ChipDisplaySettings chipDisplaySettings = Match3Settings.instance.GetChipDisplaySettings(chipType, itemColor);
			if (chipDisplaySettings != null)
			{
				GGUtil.SetActive(this.genericChipImage, true);
				GGUtil.SetSprite(this.genericChipImage, chipDisplaySettings.displaySprite);
			}
			else
			{
				for (int i = 0; i < this.chips.Count; i++)
				{
					GoalsPanelGoal.ChipDescriptor chipDescriptor = this.chips[i];
					GGUtil.SetActive(chipDescriptor.container, chipType == ChipType.Chip && chipDescriptor.color == itemColor);
				}
				for (int j = 0; j < this.chipsTypes.Count; j++)
				{
					GoalsPanelGoal.ChipTypeDescriptor chipTypeDescriptor = this.chipsTypes[j];
					GGUtil.SetActive(chipTypeDescriptor.container, chipTypeDescriptor.chipType == chipType);
				}
			}
			GGUtil.ChangeText(this.collectedCount, goal.RemainingCount.ToString());
			GGUtil.Hide(this.completeTickMark);
			GGUtil.Show(this.collectedCount);
		}

		public void UpdateCollectedCount()
		{
			int num = Mathf.Max(this.goal.RemainingCount, 0);
			bool flag = num <= 0;
			GGUtil.ChangeText(this.collectedCount, num.ToString());
			GGUtil.SetActive(this.collectedCount, !flag);
			GGUtil.SetActive(this.completeTickMark, flag);
		}

		[SerializeField]
		private Image genericChipImage;

		[SerializeField]
		private List<GoalsPanelGoal.ChipDescriptor> chips = new List<GoalsPanelGoal.ChipDescriptor>();

		[SerializeField]
		private List<GoalsPanelGoal.ChipTypeDescriptor> chipsTypes = new List<GoalsPanelGoal.ChipTypeDescriptor>();

		[SerializeField]
		private List<RectTransform> widgetsToHide = new List<RectTransform>();

		[SerializeField]
		private TextMeshProUGUI collectedCount;

		[SerializeField]
		private RectTransform completeTickMark;

		[NonSerialized]
		public MultiLevelGoals.Goal goal;

		[Serializable]
		public class ChipDescriptor
		{
			public ItemColor color;

			public RectTransform container;
		}

		[Serializable]
		public class ChipTypeDescriptor
		{
			public ChipType chipType;

			public RectTransform container;
		}
	}
}
