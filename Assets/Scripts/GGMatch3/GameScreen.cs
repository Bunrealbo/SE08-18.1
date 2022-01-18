using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGMatch3
{
	public class GameScreen : MonoBehaviour, IRemoveFromHistoryEventListener
	{
		private bool isStarterLoaded
		{
			get
			{
				return this.gameScene != null && this.gameScene.starter != null;
			}
		}

		private LevelDefinition CloneLevelAndApplyChanges(LevelDefinition level, Match3GameParams initParams)
		{
			level = level.Clone();
			level.ExchangeBurriedElementsForSmallOnes();
			if (!GGTest.showAdaptiveShowMatch)
			{
				return level;
			}
			Match3StagesDB.Stage stage = initParams.stage;
			if (stage != null)
			{
				if (stage.index < 16)
				{
					return level;
				}
				if (stage.difficulty != Match3StagesDB.Stage.Difficulty.Normal)
				{
					if (stage.index < 30)
					{
						return level;
					}
					if (stage.timesPlayed >= 1)
					{
						return level;
					}
				}
				if (stage.index < 30)
				{
					if (stage.timesPlayed >= 2)
					{
						return level;
					}
					if (stage.index % 3 == 0)
					{
						level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
						if (level.suggestMoveType == SuggestMoveType.Normal)
						{
							level.suggestMoveType = SuggestMoveType.GoodOnFirstAndLast2;
						}
					}
					else
					{
						level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
					}
				}
				else if (stage.index <= 70)
				{
					if (stage.index % 2 == 0 && stage.timesPlayed <= 2)
					{
						level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
						if (level.suggestMoveType == SuggestMoveType.Normal)
						{
							level.suggestMoveType = SuggestMoveType.GoodOnFirstAndLast2;
						}
					}
					else
					{
						level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
					}
				}
				else if (stage.timesPlayed <= 2)
				{
					level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
					if (level.suggestMoveType == SuggestMoveType.Normal)
					{
						level.suggestMoveType = SuggestMoveType.GoodOnFirstAndLast2;
					}
				}
				else
				{
					level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
				}
			}
			return level;
		}

		public void Show(Match3GameParams initParams)
		{
			initParams.level = this.CloneLevelAndApplyChanges(initParams.level, initParams);
			for (int i = 0; i < initParams.levelsList.Count; i++)
			{
				LevelDefinition levelDefinition = initParams.levelsList[i];
				levelDefinition = this.CloneLevelAndApplyChanges(levelDefinition, initParams);
				initParams.levelsList[i] = levelDefinition;
			}
			this.initParams = initParams;
			GGSoundSystem.Play(GGSoundSystem.MusicType.GameMusic);
			NavigationManager.instance.Push(base.gameObject, false);
			if (initParams == null)
			{
				initParams = new Match3GameParams();
				initParams.level = ScriptableObjectSingleton<LevelDB>.instance.levels[0];
			}
			this.Init();
		}

		public void OnRemovedFromNavigationHistory()
		{
			this.DestroyCreatedGameObjects();
		}

		private void Init()
		{
			this.HideAll();
			this.loadingGameSceneEnum = null;
			GGUtil.ChangeText(this.levelLabel, this.initParams.levelIndex + 1);
			GGUtil.SetActive(this.background, !this.initParams.disableBackground);
			if (this.isStarterLoaded)
			{
				this.StartGame();
				return;
			}
			this.LoadGameScene();
		}

        public int GetGameLevel()
        {
            return this.initParams.levelIndex;
        }

        public void DestroyCreatedGameObjects()
		{
			if (!this.isStarterLoaded)
			{
				return;
			}
			this.gameScene.starter.DestroyCreatedGameObjects();
			this.stageState = new GameScreen.StageState();
		}

		private void LoadGameScene()
		{
			GGUtil.SetActive(this.hideAll, false);
			this.loadingStyle.Apply();
			this.loadingGameSceneEnum = this.DoLoadGameScene();
		}

		private IEnumerator DoLoadGameScene()
		{
			return new GameScreen._003CDoLoadGameScene_003Ed__36(0)
			{
				_003C_003E4__this = this
			};
		}

		private Vector2 ScreenWorldSize()
		{
			RectTransform component = base.GetComponent<RectTransform>();
			Vector3[] array = new Vector3[4];
			component.GetWorldCorners(array);
			return new Vector2(array[2].x - array[0].x, array[2].y - array[0].y);
		}

		public void ShowConfetti()
		{
			GGUtil.Show(this.conffettiParticle);
		}

		private void StartGame()
		{
			Match3GameStarter starter = this.gameScene.starter;
			starter.DestroyCreatedGameObjects();
			List<LevelDefinition> list = new List<LevelDefinition>();
			if (this.initParams.levelsList.Count > 0)
			{
				list.AddRange(this.initParams.levelsList);
			}
			else
			{
				list.Add(this.initParams.level);
			}
			GGUtil.Hide(this.conffettiParticle);
			this.tutorialHand.Hide();
			GGUtil.Hide(this.rankedBoostersStartAnimation);
			GGUtil.Show(this.exitButton);
			Vector2 vector = this.ScreenWorldSize();
			this.stageState = new GameScreen.StageState();
			GGUtil.Hide(this.powerupPlacement);
			List<GameScreen.GameProgress> gameProgressList = this.stageState.gameProgressList;
			for (int i = 0; i < list.Count; i++)
			{
				LevelDefinition level = list[i];
				Match3Game match3Game = starter.CreateGame();
				GameScreen.GameProgress gameProgress = new GameScreen.GameProgress();
				gameProgress.game = match3Game;
				gameProgressList.Add(gameProgress);
				Vector3 offset = Vector3.right * vector.x * (float)i;
				gameProgress.offset = offset;
				this.HideAll();
				GGUtil.SetActive(this.visibleObjects, true);
				Camera camera = NavigationManager.instance.GetCamera();
				match3Game.Init(camera, this, this.initParams);
				match3Game.CreateBoard(new Match3Game.CreateBoardArguments
				{
					level = level,
					offset = offset
				});
				if (this.initParams.stage != null)
				{
					match3Game.SetStageDifficulty(this.initParams.stage.difficulty);
				}
				this.stageState.goals.Add(match3Game.goals);
			}
			Match3Game game = gameProgressList[0].game;
			this.HideAll();
			this.goalsPanel.Init(this.stageState);
			this.powerupsPanel.Init(this);
			this.stageState.runnerEnumerator = this.GameRunner();
			this.stageState.runnerEnumerator.MoveNext();
		}

		private IEnumerator GameRunner()
		{
			return new GameScreen._003CGameRunner_003Ed__41(0)
			{
				_003C_003E4__this = this
			};
		}

		public void Callback_ShowActivatePowerup(PowerupsPanelPowerup panelPowerup)
		{
			GameScreen.GameProgress currentGameProgress = this.stageState.currentGameProgress;
			if (currentGameProgress == null)
			{
				return;
			}
			if (currentGameProgress.isDone)
			{
				return;
			}
			PowerupsDB.PowerupDefinition powerup = panelPowerup.powerup;
			currentGameProgress.game.Callback_ShowActivatePowerup(powerup);
		}

		public void Match3GameCallback_OnGameOutOfMoves(GameCompleteParams completeParams)
		{
			Match3Game game = completeParams.game;
			OutOfMovesDialog @object = NavigationManager.instance.GetObject<OutOfMovesDialog>();
			if (@object == null)
			{
				return;
			}
			BuyMovesPricesConfig.OfferConfig offer = ScriptableObjectSingleton<BuyMovesPricesConfig>.instance.GetOffer(completeParams.stageState);
			@object.Show(offer, game, this.stageState.goals, new OutOfMovesDialog.OutOfMovesDelegate(this.OutOfMovesCallback_OnPlayOnOfferYes), new OutOfMovesDialog.OutOfMovesDelegate(this.OutOfMovesCallback_OnPlayOnOfferNo));
		}

		private void OutOfMovesCallback_OnPlayOnOfferYes(OutOfMovesDialog dialog)
		{
			BuyMovesPricesConfig.OfferConfig offer = dialog.offer;
			WalletManager walletManager = GGPlayerSettings.instance.walletManager;
			if (!walletManager.CanBuyItemWithPrice(offer.price))
			{
				NavigationManager.instance.GetObject<CurrencyPurchaseDialog>().Show(ScriptableObjectSingleton<OffersDB>.instance, null);
				return;
			}
			Match3Game game = dialog.game;
			walletManager.BuyItem(offer.price);
			game.ContinueGameAfterOffer(offer);
			this.goalsPanel.UpdateMovesCount();
			dialog.Hide();
			new Analytics.MovesBoughtEvent
			{
				stageState = this.stageState,
				offer = offer
			}.Send();
		}

		private void OutOfMovesCallback_OnPlayOnOfferNo(OutOfMovesDialog dialog)
		{
			dialog.Hide();
			GameScreen.GameProgress currentGameProgress = this.stageState.currentGameProgress;
			currentGameProgress.isDone = true;
			currentGameProgress.completeParams = new GameCompleteParams
			{
				isWin = false,
				stageState = this.stageState,
				game = currentGameProgress.game
			};
		}

		public void Match3GameCallback_OnGameWon(GameCompleteParams completeParams)
		{
			Match3Game game = completeParams.game;
			GameScreen.GameProgress gameProgress = this.stageState.GameProgressForGame(game);
			if (gameProgress == null || gameProgress.game != game)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"GAME PROGRESS NULL isNull ",
					(gameProgress == null).ToString(),
					" listCount ",
					this.stageState.gameProgressList.Count
				}));
				return;
			}
			gameProgress.completeParams = completeParams;
			gameProgress.isDone = true;
		}

		public void HideVisibleObjects()
		{
			GGUtil.SetActive(this.visibleObjects, false);
		}

		public void HideAll()
		{
			GGUtil.Hide(this.shuffleContainer);
			GGUtil.Hide(this.movesContainer);
			GGUtil.Hide(this.wellDoneContainer);
			GGUtil.Hide(this.tapToContinueContainer);
			GGUtil.Hide(this.tutorialMask);
			GGUtil.Hide(this.powerupPlacement);
			if (this.shuffleContainer != null)
			{
				this.shuffleContainer.Reset();
			}
		}

		private void Update()
		{
			if (this.stageState.runnerEnumerator != null && !this.stageState.runnerEnumerator.MoveNext())
			{
				this.stageState.runnerEnumerator = null;
			}
			if (this.loadingGameSceneEnum == null)
			{
				return;
			}
			if (!this.loadingGameSceneEnum.MoveNext())
			{
				this.loadingGameSceneEnum = null;
			}
		}

		public void ButtonCallback_OnExitButtonPressed()
		{
			NavigationManager instance = NavigationManager.instance;
			GameScreen.GameProgress currentGameProgress = this.stageState.currentGameProgress;
			if (currentGameProgress == null)
			{
				return;
			}
			Match3Game game = currentGameProgress.game;
			if (this.stageState.userMovesCount > 0)
			{
				game.SuspendGame();
				ExitGameConfirmDialog.ExitGameConfirmArguments arguments = default(ExitGameConfirmDialog.ExitGameConfirmArguments);
				arguments.goals = this.stageState.goals;
				arguments.game = game;
				arguments.onCompleteCallback = new Action<bool>(this.ExitGameConfirmDialogCallback_OnExit);
				instance.GetObject<ExitGameConfirmDialog>().Show(arguments);
				return;
			}
			game.QuitGame();
			currentGameProgress.isDone = true;
			currentGameProgress.completeParams = new GameCompleteParams
			{
				isWin = false,
				game = game,
				stageState = this.stageState
			};
		}

		private void ExitGameConfirmDialogCallback_OnExit(bool shouldExit)
		{
			GameScreen.GameProgress currentGameProgress = this.stageState.currentGameProgress;
			Match3Game game = currentGameProgress.game;
			if (currentGameProgress == null)
			{
				return;
			}
			if (!shouldExit)
			{
				game.ResumeGame();
				return;
			}
			currentGameProgress.isDone = true;
			currentGameProgress.completeParams = new GameCompleteParams
			{
				isWin = false,
				game = game,
				stageState = this.stageState
			};
		}

		[SerializeField]
		private Transform conffettiParticle;

		[SerializeField]
		private Transform background;

		[SerializeField]
		private List<RectTransform> visibleObjects = new List<RectTransform>();

		[SerializeField]
		public string gameSceneName;

		[SerializeField]
		public GoalsPanel goalsPanel;

		[SerializeField]
		public PowerupsPanel powerupsPanel;

		[SerializeField]
		public ShuffleContainer shuffleContainer;

		public MovesContainer movesContainer;

		[SerializeField]
		public WellDoneContainer wellDoneContainer;

		[SerializeField]
		public GameTapToContinueContainer tapToContinueContainer;

		[SerializeField]
		private TextMeshProUGUI levelLabel;

		[SerializeField]
		public TutorialHandController tutorialHand;

		[SerializeField]
		public RankedBoostersStartAnimation rankedBoostersStartAnimation;

		[SerializeField]
		private List<RectTransform> hideAll = new List<RectTransform>();

		[SerializeField]
		private VisualStyleSet loadingStyle = new VisualStyleSet();

		[SerializeField]
		private VisualStyleSet loadedStyle = new VisualStyleSet();

		[SerializeField]
		public InputHandler inputHandler;

		[SerializeField]
		public PowerupPlacementHandler powerupPlacement;

		[SerializeField]
		public RectTransform gamePlayAreaContainer;

		[SerializeField]
		public RectTransform tutorialMask;

		[SerializeField]
		public RectTransform exitButton;

		private Match3GameParams initParams;

		private IEnumerator loadingGameSceneEnum;

		public GameScreen.StageState stageState = new GameScreen.StageState();

		private GameScreen.Match3GameScene gameScene;

		public class GameProgress
		{
			public Match3Game game;

			public GameCompleteParams completeParams;

			public bool isDone;

			public Vector3 offset;
		}

		public class StageState
		{
			public int additionalMoves
			{
				get
				{
					int num = 0;
					for (int i = 0; i < this.gameProgressList.Count; i++)
					{
						GameScreen.GameProgress gameProgress = this.gameProgressList[i];
						num += gameProgress.game.board.totalAdditionalMoves;
					}
					return num;
				}
			}

			public int userMovesCount
			{
				get
				{
					int num = 0;
					for (int i = 0; i < this.gameProgressList.Count; i++)
					{
						GameScreen.GameProgress gameProgress = this.gameProgressList[i];
						num += gameProgress.game.board.userMovesCount;
					}
					return num;
				}
			}

			public int userScore
			{
				get
				{
					int num = 0;
					for (int i = 0; i < this.gameProgressList.Count; i++)
					{
						GameScreen.GameProgress gameProgress = this.gameProgressList[i];
						num += gameProgress.game.board.userScore;
					}
					return num;
				}
			}

			public int MovesRemaining
			{
				get
				{
					return this.goals.TotalMovesCount + this.additionalMoves - this.userMovesCount;
				}
			}

			public GameScreen.GameProgress GameProgressForGame(Match3Game game)
			{
				for (int i = 0; i < this.gameProgressList.Count; i++)
				{
					GameScreen.GameProgress gameProgress = this.gameProgressList[i];
					if (gameProgress.game == game)
					{
						return gameProgress;
					}
				}
				return null;
			}

			public GameScreen.GameProgress currentGameProgress
			{
				get
				{
					if (this.currentGameProgressIndex >= this.gameProgressList.Count || this.currentGameProgressIndex < 0)
					{
						return null;
					}
					return this.gameProgressList[this.currentGameProgressIndex];
				}
			}

			public List<GameScreen.GameProgress> gameProgressList = new List<GameScreen.GameProgress>();

			public IEnumerator runnerEnumerator;

			public MultiLevelGoals goals = new MultiLevelGoals();

			public int currentGameProgressIndex;

			public int hammersUsed;

			public int powerHammersUsed;
		}

		public class Match3GameScene
		{
			public Match3GameScene(Scene scene, Match3GameStarter starter)
			{
				this.scene = scene;
				this.starter = starter;
			}

			public Scene scene;

			public Match3GameStarter starter;
		}

		[Serializable]
		public class MultiLevelAanimationSettings
		{
			public float durationPerScreen;

			public AnimationCurve moveAnimationCurve;

			public float initialDelay;
		}

		private sealed class _003CDoLoadGameScene_003Ed__36 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoLoadGameScene_003Ed__36(int _003C_003E1__state)
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
				GameScreen gameScreen = this._003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					this._003C_003E1__state = -1;
				}
				else
				{
					this._003C_003E1__state = -1;
					this._003CasyncOperation_003E5__2 = SceneManager.LoadSceneAsync(gameScreen.gameSceneName, LoadSceneMode.Additive);
					this._003CasyncOperation_003E5__2.allowSceneActivation = true;
				}
				if (this._003CasyncOperation_003E5__2.isDone)
				{
					Scene sceneByName = SceneManager.GetSceneByName(gameScreen.gameSceneName);
					Match3GameStarter starter = null;
					GameObject[] rootGameObjects = sceneByName.GetRootGameObjects();
					for (int i = 0; i < rootGameObjects.Length; i++)
					{
						Match3GameStarter component = rootGameObjects[i].GetComponent<Match3GameStarter>();
						if (component != null)
						{
							starter = component;
							break;
						}
					}
					gameScreen.gameScene = new GameScreen.Match3GameScene(sceneByName, starter);
					GGUtil.SetActive(gameScreen.hideAll, false);
					gameScreen.loadedStyle.Apply();
					gameScreen.StartGame();
					return false;
				}
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
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

			public GameScreen _003C_003E4__this;

			private AsyncOperation _003CasyncOperation_003E5__2;
		}

		private sealed class _003CGameRunner_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CGameRunner_003Ed__41(int _003C_003E1__state)
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
				GameScreen gameScreen = this._003C_003E4__this;
				switch (num)
				{
				case 0:
				{
					this._003C_003E1__state = -1;
					this._003Cdialog_003E5__2 = BuyPowerupDialog.instance;
					this._003Cdialog_003E5__2.Hide();
					this._003CgameProgressList_003E5__3 = gameScreen.stageState.gameProgressList;
					RectTransform gamePlayAreaContainer = gameScreen.gamePlayAreaContainer;
					this._003CisBoostersPlaced_003E5__4 = false;
					this._003Ci_003E5__5 = 0;
					goto IL_570;
				}
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_346;
				case 3:
					this._003C_003E1__state = -1;
					goto IL_48C;
				case 4:
					this._003C_003E1__state = -1;
					goto IL_526;
				default:
					return false;
				}
				IL_231:
				if (this._003Ctime_003E5__12 <= this._003CanimSettings_003E5__9.initialDelay)
				{
					this._003Ctime_003E5__12 += Time.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				IL_244:
				this._003Ctime_003E5__12 -= this._003CanimSettings_003E5__9.initialDelay;
				this._003CtotalDuration_003E5__13 = this._003CanimSettings_003E5__9.durationPerScreen * (float)this._003CgameProgressList_003E5__3.Count;
				IL_346:
				if (this._003Ctime_003E5__12 <= this._003CtotalDuration_003E5__13)
				{
					this._003Ctime_003E5__12 += Time.deltaTime;
					float num2 = Mathf.InverseLerp(0f, this._003CtotalDuration_003E5__13, this._003Ctime_003E5__12);
					num2 = this._003CanimSettings_003E5__9.moveAnimationCurve.Evaluate(num2);
					Vector3 position = Vector3.LerpUnclamped(this._003CstartOffset_003E5__10, this._003CendOffset_003E5__11, num2);
					for (int i = 0; i < this._003CgameProgressList_003E5__3.Count; i++)
					{
						Match3Game game = this._003CgameProgressList_003E5__3[i].game;
						if (!(game == null))
						{
							Transform transform = game.transform;
							position.z = transform.position.z;
							transform.position = position;
						}
					}
					this._003C_003E2__current = null;
					this._003C_003E1__state = 2;
					return true;
				}
				this._003CanimSettings_003E5__9 = null;
				this._003CstartOffset_003E5__10 = default(Vector3);
				this._003CendOffset_003E5__11 = default(Vector3);
				IL_376:
				if (this._003CisFirstGame_003E5__8)
				{
					goto IL_4B5;
				}
				this._003CtotalDuration_003E5__13 = 0f;
				GameScreen.GameProgress gameProgress = this._003CgameProgressList_003E5__3[this._003Ci_003E5__5 - 1];
				this._003CendOffset_003E5__11 = -gameProgress.offset;
				this._003CstartOffset_003E5__10 = -this._003CgameProgress_003E5__6.offset;
				this._003Ctime_003E5__12 = 1f;
				IL_48C:
				if (this._003CtotalDuration_003E5__13 <= this._003Ctime_003E5__12)
				{
					this._003CtotalDuration_003E5__13 += Time.deltaTime;
					float t = Mathf.InverseLerp(0f, this._003Ctime_003E5__12, this._003CtotalDuration_003E5__13);
					Vector3 position2 = Vector3.LerpUnclamped(this._003CendOffset_003E5__11, this._003CstartOffset_003E5__10, t);
					for (int j = 0; j < this._003CgameProgressList_003E5__3.Count; j++)
					{
						Match3Game game2 = this._003CgameProgressList_003E5__3[j].game;
						if (!(game2 == null))
						{
							Transform transform2 = game2.transform;
							position2.z = transform2.position.z;
							transform2.position = position2;
						}
					}
					this._003C_003E2__current = null;
					this._003C_003E1__state = 3;
					return true;
				}
				this._003CendOffset_003E5__11 = default(Vector3);
				this._003CstartOffset_003E5__10 = default(Vector3);
				IL_4B5:
				Match3Game.StartGameArguments arguments = default(Match3Game.StartGameArguments);
				arguments.putBoosters = false;
				LevelDefinition level = this._003Cgame_003E5__7.level;
				if (level != null && !level.isPowerupPlacementSuspended && !this._003CisBoostersPlaced_003E5__4)
				{
					this._003CisBoostersPlaced_003E5__4 = true;
					arguments.putBoosters = true;
				}
				this._003Cdialog_003E5__2.Hide();
				this._003Cgame_003E5__7.StartGame(arguments);
				IL_526:
				if (!this._003CgameProgress_003E5__6.isDone)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 4;
					return true;
				}
				this._003Cgame_003E5__7.SuspendGame();
				if (!this._003CgameProgress_003E5__6.completeParams.isWin)
				{
					goto IL_586;
				}
				this._003CgameProgress_003E5__6 = null;
				this._003Cgame_003E5__7 = null;
				int num3 = this._003Ci_003E5__5;
				this._003Ci_003E5__5 = num3 + 1;
				IL_570:
				if (this._003Ci_003E5__5 < this._003CgameProgressList_003E5__3.Count)
				{
					gameScreen.stageState.currentGameProgressIndex = this._003Ci_003E5__5;
					this._003CgameProgress_003E5__6 = this._003CgameProgressList_003E5__3[this._003Ci_003E5__5];
					this._003Cgame_003E5__7 = this._003CgameProgress_003E5__6.game;
					this._003CisFirstGame_003E5__8 = (this._003Ci_003E5__5 == 0);
					if (this._003CisFirstGame_003E5__8 && gameScreen.initParams.listener != null)
					{
						GameStartedParams gameStartedParams = new GameStartedParams();
						gameStartedParams.stageState = gameScreen.stageState;
						gameStartedParams.game = this._003Cgame_003E5__7;
						gameScreen.initParams.listener.OnGameStarted(gameStartedParams);
						if (gameScreen.initParams.stage != null)
						{
							gameScreen.initParams.stage.OnGameStarted(gameStartedParams);
						}
					}
					if (!this._003CisFirstGame_003E5__8 || this._003CgameProgressList_003E5__3.Count <= 1)
					{
						goto IL_376;
					}
					this._003CanimSettings_003E5__9 = Match3Settings.instance.multiLevelAnimationSettings;
					GameScreen.GameProgress gameProgress2 = this._003CgameProgressList_003E5__3[this._003CgameProgressList_003E5__3.Count - 1];
					this._003CstartOffset_003E5__10 = -gameProgress2.offset;
					this._003CendOffset_003E5__11 = Vector3.zero;
					Vector3 position = this._003CstartOffset_003E5__10;
					for (int k = 0; k < this._003CgameProgressList_003E5__3.Count; k++)
					{
						Match3Game game3 = this._003CgameProgressList_003E5__3[k].game;
						if (!(game3 == null))
						{
							Transform transform3 = game3.transform;
							position.z = transform3.position.z;
							transform3.position = position;
						}
					}
					this._003Ctime_003E5__12 = 0f;
					if (this._003CanimSettings_003E5__9.initialDelay > 0f)
					{
						goto IL_231;
					}
					goto IL_244;
				}
				IL_586:
				GGUtil.Hide(gameScreen.exitButton);
				this._003Cdialog_003E5__2.Hide();
				GameScreen.GameProgress currentGameProgress = gameScreen.stageState.currentGameProgress;
				if (gameScreen.initParams.listener != null)
				{
					gameScreen.initParams.listener.OnGameComplete(currentGameProgress.completeParams);
				}
				return false;
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

			public GameScreen _003C_003E4__this;

			private BuyPowerupDialog _003Cdialog_003E5__2;

			private List<GameScreen.GameProgress> _003CgameProgressList_003E5__3;

			private bool _003CisBoostersPlaced_003E5__4;

			private int _003Ci_003E5__5;

			private GameScreen.GameProgress _003CgameProgress_003E5__6;

			private Match3Game _003Cgame_003E5__7;

			private bool _003CisFirstGame_003E5__8;

			private GameScreen.MultiLevelAanimationSettings _003CanimSettings_003E5__9;

			private Vector3 _003CstartOffset_003E5__10;

			private Vector3 _003CendOffset_003E5__11;

			private float _003Ctime_003E5__12;

			private float _003CtotalDuration_003E5__13;
		}
	}
}
