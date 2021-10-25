using System;
using System.Collections.Generic;

namespace GGMatch3
{
	[Serializable]
	public class GoalsDefinition
	{
		public GoalsDefinition Clone()
		{
			GoalsDefinition goalsDefinition = new GoalsDefinition();
			goalsDefinition.movesCount = this.movesCount;
			for (int i = 0; i < this.goals.Count; i++)
			{
				GoalConfig goalConfig = this.goals[i];
				goalsDefinition.goals.Add(goalConfig.Clone());
			}
			return goalsDefinition;
		}

		public int movesCount;

		public List<GoalConfig> goals = new List<GoalConfig>();
	}
}
