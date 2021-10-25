using System;

namespace GGMatch3
{
	[Serializable]
	public class GoalConfig
	{
		public GoalConfig Clone()
		{
			return new GoalConfig
			{
				goalType = this.goalType,
				chipType = this.chipType,
				itemColor = this.itemColor,
				collectCount = this.collectCount
			};
		}

		public bool IsCompatible(GoalConfig goalConfig)
		{
			return this.goalType == goalConfig.goalType && this.chipType == goalConfig.chipType && (this.chipType != ChipType.Chip || this.itemColor == goalConfig.itemColor);
		}

		public bool isCollectAllPresentAtStart
		{
			get
			{
				return this.collectCount <= 0;
			}
		}

		public GoalType goalType;

		public ChipType chipType;

		public ItemColor itemColor;

		public int collectCount;
	}
}
