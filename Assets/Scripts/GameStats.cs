using System;
using System.Collections.Generic;
using GGMatch3;

[Serializable]
public class GameStats
{
	public List<GameStats.Move> moves = new List<GameStats.Move>();

	public int initialSeed;

	public int goalsFromPowerups;

	public int goalsFromUserMatches;

	public int goalsFromInertion;

	public int matchesFromUser;

	public int matchesFromInertion;

	public int powerupsCreatedFromUser;

	public int powerupsCreatedFromInertion;

	public int powerupsUsedBySwipe;

	public int powerupsMixed;

	public int powerupsUsedByTap;

	public int noUsefulMovesTurns;

	public enum MoveType
	{
		Unknown,
		Match,
		PowerupActivation,
		PowerupMix,
		PowerupTap
	}

	[Serializable]
	public class Move
	{
		public IntVector2 fromPosition;

		public IntVector2 toPosition;

		public GameStats.MoveType moveType;

		public long frameWhenActivated;
	}
}
