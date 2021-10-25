using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGMatch3
{
	[ExecuteInEditMode]
	public class LevelEditorVisualizer : MonoBehaviour, Match3GameListener
	{
		public bool isShowDifficiltiesVisible
		{
			get
			{
				return this.showDifficulties && !this.justEditMode;
			}
		}

		public bool isShowLevelsVisible
		{
			get
			{
				return !this.justEditMode;
			}
		}

		public bool isShowStagesVisible
		{
			get
			{
				return !this.justEditMode;
			}
		}

		public LevelDB levelDB
		{
			get
			{
				if (string.IsNullOrEmpty(this.levelDBName))
				{
					return ScriptableObjectSingleton<LevelDB>.instance;
				}
				if (this.loadedLevelDB != null && this.loadedLevelDBName == this.levelDBName)
				{
					return this.loadedLevelDB;
				}
				this.loadedLevelDBName = this.levelDBName;
				this.loadedLevelDB = Resources.Load<LevelDB>(this.levelDBName);
				if (this.loadedLevelDB == null)
				{
					this.loadedLevelDB = ScriptableObjectSingleton<LevelDB>.instance;
				}
				return this.loadedLevelDB;
			}
		}

		public string levelName
		{
			get
			{
				return this.levelDB.currentLevelName;
			}
			set
			{
				UnityEngine.Debug.Log("Changed level Name " + value);
				this.levelDB.currentLevelName = value;
			}
		}

		public LevelDefinition level
		{
			get
			{
				LevelDefinition levelDefinition = this.levelDB.Get(this.levelName);
				if (levelDefinition == null)
				{
					levelDefinition = this.levelDB.levels[0];
				}
				return levelDefinition;
			}
		}

		public bool IsWorldToPositionOnBoard(Vector3 wordPos)
		{
			LevelDefinition level = this.level;
			IntVector2 intVector = this.WorldToBoardPosition(wordPos);
			return intVector.x >= 0 && intVector.x < level.size.width && intVector.y >= 0 && intVector.y < level.size.height;
		}

		public IntVector2 WorldToBoardPositionClamped(Vector3 wordPos)
		{
			LevelDefinition level = this.level;
			IntVector2 intVector = this.WorldToBoardPosition(wordPos);
			intVector.x = Mathf.Clamp(intVector.x, 0, level.size.width - 1);
			intVector.y = Mathf.Clamp(intVector.y, 0, level.size.height - 1);
			return intVector;
		}

		public IntVector2 WorldToBoardPosition(Vector3 wordPos)
		{
			Vector3 a = this.container.InverseTransformPoint(wordPos);
			LevelDefinition level = this.level;
			float num = this.slotPool.prefabWidth * this.innerContainer.localScale.x;
			float num2 = this.slotPool.prefabHeight * this.innerContainer.localScale.y;
			float num3 = num * (float)level.size.width;
			float num4 = num2 * (float)level.size.height;
			Vector3 b = new Vector3(-num3 * 0.5f, -num4 * 0.5f, 0f);
			Vector3 vector = a - b;
			int x = Mathf.FloorToInt(vector.x / num);
			int y = Mathf.FloorToInt(vector.y / num2);
			return new IntVector2(x, y);
		}

		public void HideMarker()
		{
			if (!this.markerSlot.gameObject.activeSelf)
			{
				return;
			}
			this.markerSlot.gameObject.SetActive(false);
		}

		public void SetSlot(IntVector2 position, LevelDefinition.SlotDefinition slotDefinition)
		{
			this.level.SetSlot(position, slotDefinition.Clone());
		}

		public void ShowMarker(LevelDefinition.SlotDefinition slotDefiniton)
		{
			LevelDefinition level = this.level;
			float prefabWidth = this.slotPool.prefabWidth;
			float prefabHeight = this.slotPool.prefabHeight;
			float num = prefabWidth * (float)level.size.width;
			float num2 = prefabHeight * (float)level.size.height;
			Vector3 a = new Vector3(-num * 0.5f, -num2 * 0.5f, 0f);
			this.markerSlot.Init(level, slotDefiniton);
			this.markerSlot.gameObject.SetActive(true);
			this.markerSlot.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
			Vector3 localPosition = a + Vector3.right * (((float)slotDefiniton.position.x + 0.5f) * prefabWidth) + Vector3.up * (((float)slotDefiniton.position.y + 0.5f) * prefabHeight);
			this.markerSlot.GetComponent<RectTransform>().localPosition = localPosition;
		}

		public void Refresh()
		{
			this.ShowLevel(this.level);
		}

		public Vector3 GetLocalPosition(LevelDefinition level, IntVector2 position)
		{
			float prefabWidth = this.slotPool.prefabWidth;
			float prefabHeight = this.slotPool.prefabHeight;
			float num = prefabWidth * (float)level.size.width;
			float num2 = prefabHeight * (float)level.size.height;
			return new Vector3(-num * 0.5f, -num2 * 0.5f, 0f) + Vector3.right * (((float)position.x + 0.5f) * prefabWidth) + Vector3.up * (((float)position.y + 0.5f) * prefabHeight);
		}

		public void ShowLevel(LevelDefinition level)
		{
			this.lastShownLevelIndex = level.versionIndex;
			this.lastShownLevelName = level.name;
			level.EnsureSizeAndInit();
			float prefabWidth = this.slotPool.prefabWidth;
			float prefabHeight = this.slotPool.prefabHeight;
			float num = prefabWidth * (float)level.size.width;
			float num2 = prefabHeight * (float)level.size.height;
			float num3 = Mathf.Max(this.container.sizeDelta.x / num, this.container.sizeDelta.y / num2);
			this.innerContainer.localScale = new Vector3(num3, num3, 1f);
			this.slotPool.Init();
			this.slotPool.Clear(false);
			this.burriedElementPool.Init();
			this.burriedElementPool.Clear(false);
			this.monsterElementPool.Init();
			this.monsterElementPool.Clear(false);
			Vector3 a = new Vector3(-num * 0.5f, -num2 * 0.5f, 0f);
			List<LevelDefinition.SlotDefinition> slots = level.slots;
			for (int i = 0; i < slots.Count; i++)
			{
				LevelDefinition.SlotDefinition slotDefinition = slots[i];
				LevelEditorSlot levelEditorSlot = this.slotPool.Next<LevelEditorSlot>(true);
				levelEditorSlot.Init(level, slotDefinition);
				Transform component = levelEditorSlot.GetComponent<RectTransform>();
				Vector3 localPosition = a + Vector3.right * (((float)slotDefinition.position.x + 0.5f) * prefabWidth) + Vector3.up * (((float)slotDefinition.position.y + 0.5f) * prefabHeight);
				component.localPosition = localPosition;
			}
			List<LevelDefinition.BurriedElement> elements = level.burriedElements.elements;
			for (int j = 0; j < elements.Count; j++)
			{
				LevelDefinition.BurriedElement burriedElement = elements[j];
				this.burriedElementPool.Next<LevelEditorBurriedElement>(true).Init(this, level, burriedElement);
			}
			List<LevelDefinition.MonsterElement> elements2 = level.monsterElements.elements;
			for (int k = 0; k < elements2.Count; k++)
			{
				LevelDefinition.MonsterElement monsterElement = elements2[k];
				this.monsterElementPool.Next<LevelEditorMonster>(true).Init(this, level, monsterElement);
			}
			this.slotPool.HideNotActive();
			this.burriedElementPool.HideNotActive();
			this.monsterElementPool.HideNotActive();
			this.generatorSetupPool.Clear(false);
			List<GeneratorSetup> generatorSetups = level.generatorSetups;
			for (int l = 0; l < generatorSetups.Count; l++)
			{
				GeneratorSetup generatorSetup = generatorSetups[l];
				LevelDefinition.SlotDefinition slot = level.GetSlot(generatorSetup.position);
				if (slot != null && slot.generatorSettings.isGeneratorOn)
				{
					LevelEditorGeneratorSetup levelEditorGeneratorSetup = this.generatorSetupPool.Next<LevelEditorGeneratorSetup>(true);
					Vector3 startPositionForGeneratorSetup = this.GetStartPositionForGeneratorSetup(l);
					levelEditorGeneratorSetup.Init(generatorSetup, startPositionForGeneratorSetup);
					levelEditorGeneratorSetup.transform.localPosition = Vector3.zero;
				}
			}
			this.generatorSetupPool.HideNotActive();
		}

		public LevelEditorVisualizer.GeneratorSetupHit GetGeneratorSetupHit(Vector3 worldPos)
		{
			LevelEditorVisualizer.GeneratorSetupHit result = default(LevelEditorVisualizer.GeneratorSetupHit);
			Vector3 vector = this.innerContainer.InverseTransformPoint(worldPos);
			List<GeneratorSetup> generatorSetups = this.level.generatorSetups;
			IntVector2 intVector = this.WorldToBoardPosition(worldPos);
			if (intVector.x < 0 || intVector.x >= this.level.size.width)
			{
				return result;
			}
			float chipHeight = this.generatorSetupPool.prefab.GetComponent<LevelEditorGeneratorSetup>().ChipHeight;
			for (int i = 0; i < generatorSetups.Count; i++)
			{
				GeneratorSetup generatorSetup = generatorSetups[i];
				if (generatorSetup.position.x == intVector.x)
				{
					Vector3 vector2 = this.GetStartPositionForGeneratorSetup(i) + Vector3.down * chipHeight * 0.5f;
					int num = Mathf.FloorToInt((vector.y - vector2.y) / chipHeight);
					if (num >= 0 && num <= generatorSetup.chips.Count)
					{
						result.isHit = true;
						result.generatorSetupIndex = i;
						result.generatorSetupChipIndex = num;
						return result;
					}
				}
			}
			return result;
		}

		public Vector3 GetStartPositionForGeneratorSetup(int generatorSetupIndex)
		{
			List<GeneratorSetup> generatorSetups = this.level.generatorSetups;
			GeneratorSetup generatorSetup = generatorSetups[generatorSetupIndex];
			this.generatorSetupsInSameRow.Clear();
			for (int i = 0; i < generatorSetups.Count; i++)
			{
				GeneratorSetup generatorSetup2 = generatorSetups[i];
				LevelDefinition.SlotDefinition slot = this.level.GetSlot(generatorSetup2.position);
				if (slot != null && slot.generatorSettings.isGeneratorOn && generatorSetup2.position.x == generatorSetup.position.x && generatorSetup2.position.y <= generatorSetup.position.y && (generatorSetup2.position.y != generatorSetup.position.y || generatorSetupIndex > i))
				{
					this.generatorSetupsInSameRow.Add(generatorSetup2);
				}
			}
			Vector3 a = this.GetLocalPosition(this.level, new IntVector2(generatorSetup.position.x, this.level.size.height));
			float chipHeight = this.generatorSetupPool.prefab.GetComponent<LevelEditorGeneratorSetup>().ChipHeight;
			float num = 50f;
			for (int j = 0; j < this.generatorSetupsInSameRow.Count; j++)
			{
				GeneratorSetup generatorSetup3 = this.generatorSetupsInSameRow[j];
				a += Vector3.up * (num + (float)(generatorSetup3.chips.Count + 1) * chipHeight);
			}
			return a + Vector3.up * num;
		}

		public void GrowLevel(int cellsToAdd)
		{
			this.level.size.Clone();
			List<LevelDefinition.SlotDefinition> list = new List<LevelDefinition.SlotDefinition>();
			list.AddRange(this.level.slots);
			this.level.size.width += cellsToAdd * 2;
			this.level.size.height += cellsToAdd * 2;
			this.level.slots.Clear();
			this.level.EnsureSizeAndInit();
			this.CopyWithOffset(list, this.level, new IntVector2(cellsToAdd, cellsToAdd));
		}

		public void GrowLevelWidth(int cellsToAdd)
		{
			this.level.size.Clone();
			List<LevelDefinition.SlotDefinition> list = new List<LevelDefinition.SlotDefinition>();
			list.AddRange(this.level.slots);
			this.level.size.width += cellsToAdd * 2;
			this.level.slots.Clear();
			this.level.EnsureSizeAndInit();
			this.CopyWithOffset(list, this.level, new IntVector2(cellsToAdd, 0));
		}

		public void GrowLevelHeight(int cellsToAdd)
		{
			this.level.size.Clone();
			List<LevelDefinition.SlotDefinition> list = new List<LevelDefinition.SlotDefinition>();
			list.AddRange(this.level.slots);
			this.level.size.height += cellsToAdd * 2;
			this.level.slots.Clear();
			this.level.EnsureSizeAndInit();
			this.CopyWithOffset(list, this.level, new IntVector2(0, cellsToAdd));
		}

		public void ClearLevel()
		{
			this.level.slots.Clear();
			this.level.burriedElements.elements.Clear();
			this.level.tutorialMatches.Clear();
			this.level.generatorSetups.Clear();
			this.level.EnsureSizeAndInit();
		}

		public void TrimLevel()
		{
			IntVector2 intVector = new IntVector2(this.level.size.width - 1, this.level.size.height - 1);
			IntVector2 intVector2 = new IntVector2(0, 0);
			for (int i = 0; i < this.level.size.width; i++)
			{
				for (int j = 0; j < this.level.size.height; j++)
				{
					if (this.level.GetSlot(new IntVector2(i, j)).slotType != SlotType.Empty)
					{
						intVector.x = Mathf.Min(i, intVector.x);
						intVector.y = Mathf.Min(j, intVector.y);
						intVector2.x = Mathf.Max(i, intVector2.x);
						intVector2.y = Mathf.Max(j, intVector2.y);
					}
				}
			}
			int num = intVector2.x - intVector.x + 1;
			int num2 = intVector2.y - intVector.y + 1;
			int b = 3;
			num = Mathf.Max(num, b);
			num2 = Mathf.Max(num2, b);
			if (num == this.level.size.width && num2 == this.level.size.height)
			{
				return;
			}
			this.level.size.Clone();
			List<LevelDefinition.SlotDefinition> list = new List<LevelDefinition.SlotDefinition>();
			list.AddRange(this.level.slots);
			this.level.size.width = num;
			this.level.size.height = num2;
			this.level.slots.Clear();
			this.level.EnsureSizeAndInit();
			IntVector2 offset = new IntVector2(-intVector.x, -intVector.y);
			this.CopyWithOffsetBounds(list, this.level, offset);
		}

		private void CopyWithOffsetBounds(List<LevelDefinition.SlotDefinition> copyFrom, LevelDefinition level, IntVector2 offset)
		{
			for (int i = 0; i < copyFrom.Count; i++)
			{
				LevelDefinition.SlotDefinition slotDefinition = copyFrom[i];
				slotDefinition.position += offset;
				if (slotDefinition.position.x >= 0 && slotDefinition.position.y >= 0 && slotDefinition.position.x < level.size.width && slotDefinition.position.y < level.size.height)
				{
					level.SetSlot(slotDefinition.position, slotDefinition);
				}
			}
			List<GeneratorSetup> generatorSetups = level.generatorSetups;
			for (int j = 0; j < generatorSetups.Count; j++)
			{
				generatorSetups[j].position += offset;
			}
			List<LevelDefinition.TutorialMatch> tutorialMatches = level.tutorialMatches;
			for (int k = 0; k < tutorialMatches.Count; k++)
			{
				tutorialMatches[k].OffsetAllSlots(offset);
			}
			level.burriedElements.MoveByOffset(offset);
			level.monsterElements.MoveByOffset(offset);
		}

		private void CopyWithOffset(List<LevelDefinition.SlotDefinition> copyFrom, LevelDefinition level, IntVector2 offset)
		{
			for (int i = 0; i < copyFrom.Count; i++)
			{
				LevelDefinition.SlotDefinition slotDefinition = copyFrom[i];
				slotDefinition.position += offset;
				level.SetSlot(slotDefinition.position, slotDefinition);
			}
			List<GeneratorSetup> generatorSetups = level.generatorSetups;
			for (int j = 0; j < generatorSetups.Count; j++)
			{
				generatorSetups[j].position += offset;
			}
			List<LevelDefinition.TutorialMatch> tutorialMatches = level.tutorialMatches;
			for (int k = 0; k < tutorialMatches.Count; k++)
			{
				tutorialMatches[k].OffsetAllSlots(offset);
			}
			level.burriedElements.MoveByOffset(offset);
			level.monsterElements.MoveByOffset(offset);
		}

		public void PopulateBoardRandom()
		{
			PopulateBoard.BoardRepresentation boardRepresentation = new PopulateBoard.BoardRepresentation();
			boardRepresentation.Init(this.level);
			PopulateBoard.Params @params = new PopulateBoard.Params();
			@params.randomProvider = new RandomProvider();
			for (int i = 0; i < this.level.numChips; i++)
			{
				@params.availableColors.Add((ItemColor)i);
			}
			@params.maxPotentialMatches = Match3Settings.instance.maxPotentialMatchesAtStart;
			PopulateBoard populateBoard = new PopulateBoard();
			populateBoard.RandomPopulate(boardRepresentation, @params);
			List<LevelDefinition.SlotDefinition> slots = this.level.slots;
			for (int j = 0; j < slots.Count; j++)
			{
				LevelDefinition.SlotDefinition slotDefinition = slots[j];
				if (slotDefinition.slotType != SlotType.Empty && slotDefinition.chipType == ChipType.RandomChip)
				{
					PopulateBoard.BoardRepresentation.RepresentationSlot slot = populateBoard.board.GetSlot(slotDefinition.position);
					if (slot != null && slot.isGenerated)
					{
						slotDefinition.chipType = ChipType.Chip;
						slotDefinition.itemColor = slot.itemColor;
					}
				}
			}
		}

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				this.buttonsContainer.gameObject.SetActive(true);
				this.SetLabel(this.buttonLabel, "Play Game");
			}
		}

		private void Update()
		{
			if (this.level == null)
			{
				return;
			}
			if (this.lastShownLevelIndex != this.level.versionIndex || this.lastShownLevelName != this.level.name)
			{
				this.ShowLevel(this.level);
			}
			if (Application.isPlaying)
			{
				this.test.Update();
			}
		}

		public void OnGameComplete(GameCompleteParams completeParams)
		{
			this.StopGame();
		}

		public void OnGameStarted(GameStartedParams startedParams)
		{
		}

		public void PlayGame()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			NavigationManager.instance.GetObject<GameScreen>().Show(new Match3GameParams
			{
				level = this.level,
				listener = this,
				setRandomSeed = this.setRandomSeed,
				randomSeed = this.randomSeed
			});
			this.container.gameObject.SetActive(false);
			this.SetLabel(this.buttonLabel, "Stop Game");
			this.isGamePlaying = true;
		}

		public void StopMultipleTests()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.test.StopTesting();
			this.StopGame();
		}

		public void PlayMultipleTests()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			TestExecutor.ExecuteArguments arguments = default(TestExecutor.ExecuteArguments);
			arguments.repeatTimes = this.repeatTimes;
			arguments.visualizer = this;
			this.test.StartTesting(arguments);
		}

		public void PlayTestGame()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			NavigationManager.instance.GetObject<GameScreen>().Show(new Match3GameParams
			{
				level = this.level,
				listener = this,
				isAIPlayer = true,
				iterations = this.stepsPerFrame,
				isHudDissabled = this.isHudDissabled,
				timeScale = 10000f
			});
			this.container.gameObject.SetActive(false);
			this.SetLabel(this.buttonLabel, "Stop Game");
			this.isGamePlaying = true;
		}

		public void PlayGame(Match3GameParams initParams)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			NavigationManager.instance.GetObject<GameScreen>().Show(initParams);
			this.container.gameObject.SetActive(false);
			this.SetLabel(this.buttonLabel, "Stop Game");
			this.isGamePlaying = true;
		}

		private void SetLabel(Text label, string text)
		{
			if (label == null)
			{
				return;
			}
			if (label.text == text)
			{
				return;
			}
			label.text = text;
		}

		public void StopGame()
		{
			this.container.gameObject.SetActive(true);
			this.SetLabel(this.buttonLabel, "Play Game");
			this.isGamePlaying = false;
			NavigationManager instance = NavigationManager.instance;
			if (instance == null)
			{
				return;
			}
			instance.Pop(true);
		}

		public void ClearPools()
		{
			this.slotPool.DestroyObjectsFromPool();
			this.burriedElementPool.DestroyObjectsFromPool();
			this.generatorSetupPool.DestroyObjectsFromPool();
			this.monsterElementPool.DestroyObjectsFromPool();
			LevelEditorSlot levelEditorSlot = this.markerSlot;
			LevelEditorSlot component = UnityEngine.Object.Instantiate<GameObject>(this.slotPool.prefab, levelEditorSlot.transform.parent).GetComponent<LevelEditorSlot>();
			component.transform.localPosition = levelEditorSlot.transform.localPosition;
			component.transform.localScale = levelEditorSlot.transform.localScale;
			this.markerSlot = component;
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(levelEditorSlot.gameObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(levelEditorSlot.gameObject);
			}
			this.ShowLevel(this.level);
		}

		public void Callback_TogglePlay()
		{
			if (this.isGamePlaying)
			{
				this.StopGame();
				return;
			}
			this.PlayGame();
		}

		[SerializeField]
		public StagesAnalyticsDB analyticsDB;

		[SerializeField]
		public string levelDBName;

		[SerializeField]
		public List<string> possibleLevelDB = new List<string>();

		public TestExecutor test = new TestExecutor();

		[SerializeField]
		public bool justEditMode;

		[SerializeField]
		public bool showDifficulties;

		[SerializeField]
		public bool limitStages;

		[SerializeField]
		public int minStage;

		[SerializeField]
		public int maxStage;

		[NonSerialized]
		public bool isGamePlaying;

		[SerializeField]
		private LevelEditorVisualizer.UIElementPool slotPool = new LevelEditorVisualizer.UIElementPool();

		[SerializeField]
		private LevelEditorVisualizer.UIElementPool burriedElementPool = new LevelEditorVisualizer.UIElementPool();

		[SerializeField]
		private LevelEditorVisualizer.UIElementPool monsterElementPool = new LevelEditorVisualizer.UIElementPool();

		[SerializeField]
		private LevelEditorVisualizer.UIElementPool generatorSetupPool = new LevelEditorVisualizer.UIElementPool();

		[SerializeField]
		public RectTransform container;

		[SerializeField]
		public RectTransform screenContainer;

		[SerializeField]
		private RectTransform buttonsContainer;

		[SerializeField]
		private Text buttonLabel;

		[SerializeField]
		private RectTransform innerContainer;

		[SerializeField]
		private LevelEditorSlot markerSlot;

		[SerializeField]
		public LevelDefinition.SlotDefinition markerSlotDefinition = new LevelDefinition.SlotDefinition();

		[SerializeField]
		public int repeatTimes = 10;

		[SerializeField]
		public bool isHudDissabled = true;

		[SerializeField]
		public int stepsPerFrame = 1000;

		[SerializeField]
		public bool humanVisibleDebug;

		[SerializeField]
		public bool setRandomSeed;

		[SerializeField]
		public int randomSeed;

		[SerializeField]
		public GameResults lastResult;

		[SerializeField]
		public string resultString;

		private LevelDB loadedLevelDB;

		private string loadedLevelDBName;

		[NonSerialized]
		public long lastShownLevelIndex;

		[NonSerialized]
		public string lastShownLevelName;

		private List<GeneratorSetup> generatorSetupsInSameRow = new List<GeneratorSetup>();

		[Serializable]
		public class UIElementPool
		{
			public float prefabWidth
			{
				get
				{
					if (this.prefab == null)
					{
						return 0f;
					}
					RectTransform component = this.prefab.GetComponent<RectTransform>();
					if (component == null)
					{
						return 0f;
					}
					return component.sizeDelta.x;
				}
			}

			public float prefabHeight
			{
				get
				{
					if (this.prefab == null)
					{
						return 0f;
					}
					RectTransform component = this.prefab.GetComponent<RectTransform>();
					if (component == null)
					{
						return 0f;
					}
					return component.sizeDelta.y;
				}
			}

			public void DestroyObjectsFromPool()
			{
				this.Clear(false);
				for (int i = 0; i < this.notUsedObjects.Count; i++)
				{
					GameObject obj = this.notUsedObjects[i];
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(obj);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(obj);
					}
				}
				this.notUsedObjects.Clear();
			}

			public void Init()
			{
				this.prefab.SetActive(false);
			}

			public void Clear(bool hideNotActive)
			{
				for (int i = this.usedObjects.Count - 1; i >= 0; i--)
				{
					GameObject gameObject = this.usedObjects[i];
					if (hideNotActive)
					{
						gameObject.SetActive(false);
					}
					this.notUsedObjects.Add(gameObject);
				}
				this.usedObjects.Clear();
			}

			public void HideNotActive()
			{
				for (int i = 0; i < this.notUsedObjects.Count; i++)
				{
					this.notUsedObjects[i].SetActive(false);
				}
			}

			public T Next<T>(bool activate = true) where T : MonoBehaviour
			{
				GameObject gameObject;
				if (this.notUsedObjects.Count > 0)
				{
					int index = this.notUsedObjects.Count - 1;
					gameObject = this.notUsedObjects[index];
					this.notUsedObjects.RemoveAt(index);
				}
				else
				{
					gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab, this.parent);
				}
				this.usedObjects.Add(gameObject);
				if (activate)
				{
					gameObject.SetActive(true);
				}
				return gameObject.GetComponent<T>();
			}

			public GameObject prefab;

			public Transform parent;

			public List<GameObject> usedObjects = new List<GameObject>();

			public List<GameObject> notUsedObjects = new List<GameObject>();
		}

		public struct GeneratorSetupHit
		{
			public bool isHit;

			public int generatorSetupIndex;

			public int generatorSetupChipIndex;
		}
	}
}
