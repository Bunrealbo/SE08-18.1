using System;

namespace GGMatch3
{
	public struct ActionScore
	{
		public int GoalsAndObstaclesScore(int goalsFactor)
		{
			return this.goalsCount * goalsFactor + this.obstaclesDestroyed;
		}

		public bool isZero
		{
			get
			{
				return this.goalsCount == 0 && this.powerupsCreated == 0 && this.obstaclesDestroyed == 0;
			}
		}

		public static ActionScore operator +(ActionScore a, ActionScore b)
		{
			return new ActionScore
			{
				goalsCount = a.goalsCount + b.goalsCount,
				powerupsCreated = a.powerupsCreated + b.powerupsCreated,
				obstaclesDestroyed = a.obstaclesDestroyed + b.obstaclesDestroyed
			};
		}

		public string ToDebugString()
		{
			return string.Format("(goals: {0}, powerups: {1}, obstacles: {2})", this.goalsCount, this.powerupsCreated, this.obstaclesDestroyed);
		}

		public int goalsCount;

		public int powerupsCreated;

		public int obstaclesDestroyed;
	}
}
