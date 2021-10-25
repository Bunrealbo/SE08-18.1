using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace GGMatch3
{
	public class TestExecutor : Match3GameListener
	{
		private List<GameResults.GameResult> gameResults
		{
			get
			{
				return this.results.gameResults;
			}
		}

		public void OnGameComplete(GameCompleteParams completeParams)
		{
			GameResults.GameResult gameResult = new GameResults.GameResult();
			gameResult.isComplete = true;
			Match3Game game = completeParams.game;
			gameResult.randomSeed = game.initParams.randomSeed;
			gameResult.numberOfMoves = completeParams.stageState.userMovesCount;
			gameResult.gameStats = game.board.gameStats;
			this.gameResults.Add(gameResult);
			NavigationManager instance = NavigationManager.instance;
			if (instance == null)
			{
				return;
			}
			instance.Pop(true);
			string resultString = "COMPLETE: " + Mathf.RoundToInt((float)this.gameResults.Count / (float)this.arguments.repeatTimes * 100f) + "%";
			this.arguments.visualizer.resultString = resultString;
		}

		public void OnGameStarted(GameStartedParams startedParams)
		{
		}

		private void Clear()
		{
			this.results = new GameResults();
			this.countGamesPerMoves.Clear();
		}

		public void StopTesting()
		{
			this.Clear();
			this.executionEnum = null;
		}

		public void StartTesting(TestExecutor.ExecuteArguments arguments)
		{
			this.arguments = arguments;
			this.Clear();
			this.results.repeats = arguments.repeatTimes;
			this.results.levelName = arguments.visualizer.levelName;
			this.executionEnum = this.DoExecution();
		}

		private IEnumerator DoExecution()
		{
			return new TestExecutor._003CDoExecution_003Ed__13(0)
			{
				_003C_003E4__this = this
			};
		}

		public void Update()
		{
			if (this.executionEnum == null)
			{
				return;
			}
			if (!this.executionEnum.MoveNext())
			{
				this.executionEnum = null;
			}
		}

		private GameResults results = new GameResults();

		private Dictionary<int, int> countGamesPerMoves = new Dictionary<int, int>();

		private TestExecutor.ExecuteArguments arguments;

		private IEnumerator executionEnum;

		public struct ExecuteArguments
		{
			public int repeatTimes;

			public LevelEditorVisualizer visualizer;
		}

		public struct ResultData
		{
			public int moveCount;

			public int count;
		}

		[Serializable]
		private sealed class _003C_003Ec
		{
			internal int _003CDoExecution_003Eb__13_0(TestExecutor.ResultData x, TestExecutor.ResultData y)
			{
				return x.moveCount.CompareTo(y.moveCount);
			}

			public static readonly TestExecutor._003C_003Ec _003C_003E9 = new TestExecutor._003C_003Ec();

			public static Comparison<TestExecutor.ResultData> _003C_003E9__13_0;
		}

		private sealed class _003CDoExecution_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoExecution_003Ed__13(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			bool IEnumerator.MoveNext()
			{
				int num = this._003C_003E1__state;
				TestExecutor testExecutor = this._003C_003E4__this;
				if (num == 0)
				{
					this._003C_003E1__state = -1;
					this._003Cvisualizer_003E5__2 = testExecutor.arguments.visualizer;
					this._003CstartTime_003E5__3 = DateTime.Now;
					this._003CrepeatTimes_003E5__4 = testExecutor.arguments.repeatTimes;
					this._003Ci_003E5__5 = 0;
					goto IL_183;
				}
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				IL_140:
				if (testExecutor.gameResults.Count <= this._003Ci_003E5__5)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				int num2 = this._003Ci_003E5__5;
				this._003Ci_003E5__5 = num2 + 1;
				IL_183:
				if (this._003Ci_003E5__5 >= this._003CrepeatTimes_003E5__4)
				{
					testExecutor.arguments.visualizer.StopGame();
					for (int i = 0; i < testExecutor.gameResults.Count; i++)
					{
						GameResults.GameResult gameResult = testExecutor.gameResults[i];
						if (!testExecutor.countGamesPerMoves.ContainsKey(gameResult.numberOfMoves))
						{
							testExecutor.countGamesPerMoves.Add(gameResult.numberOfMoves, 0);
						}
						testExecutor.countGamesPerMoves[gameResult.numberOfMoves] = testExecutor.countGamesPerMoves[gameResult.numberOfMoves] + 1;
					}
					List<TestExecutor.ResultData> list = new List<TestExecutor.ResultData>();
					foreach (KeyValuePair<int, int> keyValuePair in testExecutor.countGamesPerMoves)
					{
						list.Add(new TestExecutor.ResultData
						{
							moveCount = keyValuePair.Key,
							count = keyValuePair.Value
						});
					}
					list.Sort(new Comparison<TestExecutor.ResultData>(TestExecutor._003C_003Ec._003C_003E9._003CDoExecution_003Eb__13_0));
					StringBuilder stringBuilder = new StringBuilder();
					for (int j = 0; j < list.Count; j++)
					{
						TestExecutor.ResultData resultData = list[j];
						stringBuilder.AppendLine(resultData.moveCount + "," + resultData.count);
					}
					UnityEngine.Debug.Log("DURATION " + (DateTime.Now - this._003CstartTime_003E5__3).TotalSeconds + "sec");
					string text = stringBuilder.ToString();
					UnityEngine.Debug.Log(text);
					testExecutor.arguments.visualizer.resultString = text;
					testExecutor.arguments.visualizer.lastResult = testExecutor.results;
					testExecutor.Clear();
					return false;
				}
				Match3GameParams match3GameParams = new Match3GameParams();
				match3GameParams.level = this._003Cvisualizer_003E5__2.level;
				match3GameParams.listener = testExecutor;
				match3GameParams.isAIPlayer = true;
				match3GameParams.iterations = this._003Cvisualizer_003E5__2.stepsPerFrame;
				match3GameParams.timeScale = 10000f;
				match3GameParams.isHudDissabled = this._003Cvisualizer_003E5__2.isHudDissabled;
				match3GameParams.disableParticles = true;
				if (this._003Cvisualizer_003E5__2.humanVisibleDebug)
				{
					match3GameParams.iterations = 1;
					match3GameParams.timeScale = 1f;
					match3GameParams.isHudDissabled = false;
					match3GameParams.isAIDebug = true;
				}
				match3GameParams.setRandomSeed = true;
				int randomSeed = (int)DateTime.Now.Ticks % 1000;
				if (this._003Cvisualizer_003E5__2.setRandomSeed)
				{
					randomSeed = this._003Cvisualizer_003E5__2.randomSeed;
				}
				match3GameParams.randomSeed = randomSeed;
				testExecutor.arguments.visualizer.PlayGame(match3GameParams);
				goto IL_140;
			}

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this._003C_003E2__current;
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this._003C_003E2__current;
				}
			}

			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TestExecutor _003C_003E4__this;

			private LevelEditorVisualizer _003Cvisualizer_003E5__2;

			private DateTime _003CstartTime_003E5__3;

			private int _003CrepeatTimes_003E5__4;

			private int _003Ci_003E5__5;
		}
	}
}
