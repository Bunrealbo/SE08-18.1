using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class MultiLevelGoals
	{
		public int TotalMovesCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.goalsList.Count; i++)
				{
					Match3Goals match3Goals = this.goalsList[i];
					num += match3Goals.TotalMovesCount;
				}
				return num;
			}
		}

		public List<MultiLevelGoals.Goal> GetActiveGoals()
		{
			this.activeGoals.Clear();
			for (int i = 0; i < this.allGoals.Count; i++)
			{
				MultiLevelGoals.Goal goal = this.allGoals[i];
				if (!goal.isComplete)
				{
					this.activeGoals.Add(goal);
				}
			}
			return this.activeGoals;
		}

		private MultiLevelGoals.Goal GetOrCreateGoal(GoalConfig goalConfig)
		{
			for (int i = 0; i < this.allGoals.Count; i++)
			{
				MultiLevelGoals.Goal goal = this.allGoals[i];
				if (goal.config.IsCompatible(goalConfig))
				{
					return goal;
				}
			}
			MultiLevelGoals.Goal goal2 = new MultiLevelGoals.Goal();
			goal2.config = goalConfig;
			this.allGoals.Add(goal2);
			return goal2;
		}

		public void Add(Match3Goals goals)
		{
			this.goalsList.Add(goals);
			List<Match3Goals.GoalBase> goals2 = goals.goals;
			for (int i = 0; i < goals2.Count; i++)
			{
				Match3Goals.GoalBase goalBase = goals2[i];
				this.GetOrCreateGoal(goalBase.config).goals.Add(goalBase);
			}
		}

		private List<Match3Goals> goalsList = new List<Match3Goals>();

		public List<MultiLevelGoals.Goal> allGoals = new List<MultiLevelGoals.Goal>();

		private List<MultiLevelGoals.Goal> activeGoals = new List<MultiLevelGoals.Goal>();

		public class Goal
		{
			public bool isComplete
			{
				get
				{
					return this.RemainingCount <= 0;
				}
			}

			public bool IsCompatible(Match3Goals.GoalBase goal)
			{
				return goal != null && goal.config.IsCompatible(this.config);
			}

			public int RemainingCount
			{
				get
				{
					int num = 0;
					for (int i = 0; i < this.goals.Count; i++)
					{
						Match3Goals.GoalBase goalBase = this.goals[i];
						num += goalBase.RemainingCount;
					}
					return num;
				}
			}

			public GoalConfig config;

			public List<Match3Goals.GoalBase> goals = new List<Match3Goals.GoalBase>();
		}
	}
}
