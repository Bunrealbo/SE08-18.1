using System;
using System.Collections.Generic;
using GGMatch3;

public class Match3GameParams
{
	public int BoughtBoosterCount(BoosterType boosterType)
	{
		List<BoosterConfig> list = this.boughtBoosters;
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].boosterType == boosterType)
			{
				num++;
			}
		}
		return num;
	}

	public LevelDefinition level;

	public List<LevelDefinition> levelsList = new List<LevelDefinition>();

	public List<BoosterConfig> activeBoosters = new List<BoosterConfig>();

	public List<BoosterConfig> boughtBoosters = new List<BoosterConfig>();

	public Match3GameListener listener;

	public Match3StagesDB.Stage stage;

	public int iterations = 1;

	public float timeScale = 1f;

	public bool isAIPlayer;

	public bool isAIDebug;

	public bool isHudDissabled;

	public int levelIndex;

	public bool disableParticles;

	public int giftBoosterLevel;

	public bool setRandomSeed;

	public int randomSeed;

	public bool disableBackground;
}
