using System;
using System.Collections.Generic;
using UnityEngine;

public class StagesAnalyticsDB : ScriptableObject
{
	public StagesAnalyticsDB.StageData GetOrCreateStageData(int index)
	{
		StagesAnalyticsDB.StageData stageData = this.GetStageData(index);
		if (stageData != null)
		{
			return stageData;
		}
		stageData = new StagesAnalyticsDB.StageData();
		stageData.stageIndex = index;
		this.stages.Add(stageData);
		return stageData;
	}

	public StagesAnalyticsDB.StageData GetStageData(int index)
	{
		for (int i = 0; i < this.stages.Count; i++)
		{
			StagesAnalyticsDB.StageData stageData = this.stages[i];
			if (stageData.stageIndex == index)
			{
				return stageData;
			}
		}
		return null;
	}

	[SerializeField]
	public List<StagesAnalyticsDB.StageData> stages = new List<StagesAnalyticsDB.StageData>();

	[Serializable]
	public class StageData
	{
		public int stageIndex;

		[SerializeField]
		public List<StagesAnalyticsDB.StageData.MovesData> moves = new List<StagesAnalyticsDB.StageData.MovesData>();

		[SerializeField]
		public List<StagesAnalyticsDB.StageData.PlayedData> playedData = new List<StagesAnalyticsDB.StageData.PlayedData>();

		[SerializeField]
		public int usersStarted;

		[Serializable]
		public class MovesData
		{
			[SerializeField]
			public StagesAnalyticsDB.StageData.MovesData.PassedType passed;

			[SerializeField]
			public int moves;

			[SerializeField]
			public int games;

			public enum PassedType
			{
				Passed,
				NotPassed
			}
		}

		[Serializable]
		public class PlayedData
		{
			[SerializeField]
			public int timesPlayed;

			[SerializeField]
			public int games;
		}
	}
}
