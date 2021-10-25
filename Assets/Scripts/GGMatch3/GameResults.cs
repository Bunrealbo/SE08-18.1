using System;
using System.Collections.Generic;

namespace GGMatch3
{
	[Serializable]
	public class GameResults
	{
		public string levelName;

		public int repeats;

		public List<GameResults.GameResult> gameResults = new List<GameResults.GameResult>();

		[Serializable]
		public class GameResult
		{
			public int randomSeed;

			public int numberOfMoves;

			public bool isComplete;

			public GameStats gameStats;
		}
	}
}
