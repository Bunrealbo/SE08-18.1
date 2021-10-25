using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class HeuristicAIPlayer
	{
		public void Init(Match3Game game)
		{
			this.game = game;
			this.lastMoveCount = 0;
		}

		public void FindBestMove()
		{
			this.potentialMoves.Clear();
			this.game.goals.FillSlotData(this.game);
			this.game.board.potentialMatches.FindPotentialMatches(this.game);
			this.game.board.powerupActivations.Fill(this.game);
			this.game.board.powerupCombines.Fill(this.game);
			PotentialMatches potentialMatches = this.game.board.potentialMatches;
			Match3Goals goals = this.game.goals;
			List<PotentialMatches.CompoundSlotsSet> matchesList = potentialMatches.matchesList;
			for (int i = 0; i < matchesList.Count; i++)
			{
				PotentialMatches.CompoundSlotsSet compoundSlotsSet = matchesList[i];
				ActionScore actionScore = compoundSlotsSet.GetActionScore(this.game, goals);
				ActionScore actionScore2 = default(ActionScore);
				if (actionScore.powerupsCreated > 0)
				{
					ChipType createdPowerup = compoundSlotsSet.createdPowerup;
					if (createdPowerup != ChipType.DiscoBall)
					{
						List<PowerupActivations.PowerupActivation> list = this.game.board.powerupActivations.CreatePotentialActivations(createdPowerup, this.game.GetSlot(compoundSlotsSet.positionOfSlotMissingForMatch));
						for (int j = 0; j < list.Count; j++)
						{
							ActionScore actionScore3 = list[j].GetActionScore(this.game, goals);
							actionScore2.goalsCount = Mathf.Max(actionScore2.goalsCount, actionScore3.goalsCount);
							actionScore2.obstaclesDestroyed = Mathf.Max(actionScore2.obstaclesDestroyed, actionScore3.obstaclesDestroyed);
						}
					}
					else
					{
						actionScore2.goalsCount = 20;
						actionScore2.obstaclesDestroyed = 20;
					}
				}
				this.potentialMoves.Add(new HeuristicAIPlayer.PotentialMove(actionScore, compoundSlotsSet, actionScore2));
			}
			List<PowerupActivations.PowerupActivation> powerups = this.game.board.powerupActivations.powerups;
			for (int k = 0; k < powerups.Count; k++)
			{
				PowerupActivations.PowerupActivation powerupActivation = powerups[k];
				ActionScore actionScore4 = powerupActivation.GetActionScore(this.game, goals);
				this.potentialMoves.Add(new HeuristicAIPlayer.PotentialMove(actionScore4, powerupActivation));
			}
			List<PowerupCombines.PowerupCombine> combines = this.game.board.powerupCombines.combines;
			for (int l = 0; l < combines.Count; l++)
			{
				PowerupCombines.PowerupCombine powerupCombine = combines[l];
				ActionScore actionScore5 = powerupCombine.GetActionScore(this.game, goals);
				this.potentialMoves.Add(new HeuristicAIPlayer.PotentialMove(actionScore5, powerupCombine));
			}
			this.potentialMoves.Sort(new Comparison<HeuristicAIPlayer.PotentialMove>(HeuristicAIPlayer._003C_003Ec._003C_003E9._003CFindBestMove_003Eb__5_0));
			if (this.potentialMoves.Count == 0)
			{
				UnityEngine.Debug.Log("NO MOVES");
				return;
			}
			bool instant = true;
			HeuristicAIPlayer.PotentialMove potentialMove = this.potentialMoves[0];
			ActionScore moveScore = potentialMove.moveScore;
			if (moveScore.isZero && potentialMove.powerupScore.isZero)
			{
				potentialMove = this.potentialMoves[this.game.RandomRange(0, this.potentialMoves.Count)];
				this.game.board.gameStats.noUsefulMovesTurns++;
			}
			if (potentialMove.swapMatch != null)
			{
				PotentialMatches.CompoundSlotsSet swapMatch = potentialMove.swapMatch;
				this.game.TrySwitchSlots(swapMatch.positionOfSlotMissingForMatch, swapMatch.swipeSlot.position, instant);
			}
			else if (potentialMove.powerupCombine != null)
			{
				PowerupCombines.PowerupCombine powerupCombine2 = potentialMove.powerupCombine;
				this.game.TrySwitchSlots(powerupCombine2.powerupSlot.position, powerupCombine2.exchangeSlot.position, instant);
			}
			else if (potentialMove.powerupActivation != null)
			{
				PowerupActivations.PowerupActivation powerupActivation2 = potentialMove.powerupActivation;
				if (powerupActivation2.isTap)
				{
					this.game.TapOnSlot(powerupActivation2.powerupSlot.position, null);
				}
				else
				{
					this.game.TrySwitchSlots(powerupActivation2.powerupSlot.position, powerupActivation2.exchangeSlot.position, instant);
				}
			}
			bool isAIDebug = this.game.initParams.isAIDebug;
			int num = this.lastMoveCount;
			int userMovesCount = this.game.board.userMovesCount;
			this.lastMoveCount++;
			if (isAIDebug)
			{
				UnityEngine.Debug.Log("MOVE " + this.game.board.userMovesCount);
				for (int m = 0; m < this.potentialMoves.Count; m++)
				{
					HeuristicAIPlayer.PotentialMove potentialMove2 = this.potentialMoves[m];
					if (potentialMove2.swapMatch != null)
					{
						UnityEngine.Debug.LogFormat("{0} Score: {1} {2} -> {3}", new object[]
						{
							m,
							potentialMove2.moveScore.ToDebugString(),
							potentialMove2.swapMatch.swipeSlot.position,
							potentialMove2.swapMatch.positionOfSlotMissingForMatch
						});
					}
					else if (potentialMove2.powerupCombine != null)
					{
						UnityEngine.Debug.LogFormat("{0} Score: {1} {2} -> {3}", new object[]
						{
							m,
							potentialMove2.moveScore.ToDebugString(),
							potentialMove2.powerupCombine.exchangeSlot.position,
							potentialMove2.powerupCombine.powerupSlot.position
						});
					}
					else if (potentialMove2.powerupActivation != null)
					{
						if (potentialMove2.powerupActivation.isTap)
						{
							UnityEngine.Debug.LogFormat("{0} Score: {1} {2} -> TAP", new object[]
							{
								m,
								potentialMove2.moveScore.ToDebugString(),
								potentialMove2.powerupActivation.powerupSlot.position
							});
						}
						else
						{
							UnityEngine.Debug.LogFormat("{0} Score: {1} {2} -> {3}", new object[]
							{
								m,
								potentialMove2.moveScore.ToDebugString(),
								potentialMove2.powerupActivation.exchangeSlot.position,
								potentialMove2.powerupActivation.powerupSlot.position
							});
						}
					}
				}
				UnityEngine.Debug.LogError("DEBUG END " + this.game.board.userMovesCount);
			}
		}

		private Match3Game game;

		private List<HeuristicAIPlayer.PotentialMove> potentialMoves = new List<HeuristicAIPlayer.PotentialMove>();

		private int lastMoveCount;

		public struct PotentialMove
		{
			public PotentialMove(ActionScore moveScore, PotentialMatches.CompoundSlotsSet swapMatch, ActionScore powerupScore)
			{
				this.moveScore = moveScore;
				this.swapMatch = swapMatch;
				this.powerupActivation = null;
				this.powerupCombine = null;
				this.powerupScore = powerupScore;
			}

			public PotentialMove(ActionScore moveScore, PowerupActivations.PowerupActivation powerupActivation)
			{
				this.moveScore = moveScore;
				this.swapMatch = null;
				this.powerupActivation = powerupActivation;
				this.powerupCombine = null;
				this.powerupScore = default(ActionScore);
			}

			public PotentialMove(ActionScore moveScore, PowerupCombines.PowerupCombine powerupCombine)
			{
				this.moveScore = moveScore;
				this.swapMatch = null;
				this.powerupActivation = null;
				this.powerupCombine = powerupCombine;
				this.powerupScore = default(ActionScore);
			}

			public ActionScore moveScore;

			public PotentialMatches.CompoundSlotsSet swapMatch;

			public PowerupActivations.PowerupActivation powerupActivation;

			public PowerupCombines.PowerupCombine powerupCombine;

			public ActionScore powerupScore;
		}

		[Serializable]
		private sealed class _003C_003Ec
		{
			internal int _003CFindBestMove_003Eb__5_0(HeuristicAIPlayer.PotentialMove x, HeuristicAIPlayer.PotentialMove y)
			{
				ActionScore moveScore = y.moveScore;
				ActionScore moveScore2 = x.moveScore;
				ActionScore powerupScore = y.powerupScore;
				ActionScore powerupScore2 = x.powerupScore;
				int num = moveScore.goalsCount.CompareTo(moveScore2.goalsCount);
				if (num == 0)
				{
					num = moveScore.obstaclesDestroyed.CompareTo(moveScore2.obstaclesDestroyed);
				}
				if (num == 0)
				{
					num = moveScore.powerupsCreated.CompareTo(moveScore2.powerupsCreated);
				}
				if (num == 0)
				{
					num = powerupScore.goalsCount.CompareTo(powerupScore2.goalsCount);
				}
				if (num == 0)
				{
					num = powerupScore.obstaclesDestroyed.CompareTo(powerupScore2.obstaclesDestroyed);
				}
				return num;
			}

			public static readonly HeuristicAIPlayer._003C_003Ec _003C_003E9 = new HeuristicAIPlayer._003C_003Ec();

			public static Comparison<HeuristicAIPlayer.PotentialMove> _003C_003E9__5_0;
		}
	}
}
