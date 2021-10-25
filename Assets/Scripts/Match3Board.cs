using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class Match3Board
{
	public void AddMatch()
	{
		this.matchesInBoard.AddMatch(this.userMovesCount);
	}

	public void ResetMatchesInBoard()
	{
		this.matchesInBoard.currentMove = this.userMovesCount;
		this.matchesInBoard.matchesCount = 1;
	}

	public int currentMoveMatches
	{
		get
		{
			return this.matchesInBoard.matchesCount;
		}
	}

	public int RandomRange(int min, int max)
	{
		return this.randomProvider.Range(min, max);
	}

	public float RandomRange(float min, float max)
	{
		return this.randomProvider.Range(min, max);
	}

	public Match3Board.ChipCreateParams RandomChip(GeneratorSlotSettings generationSettings)
	{
		Match3Board.ChipCreateParams result = default(Match3Board.ChipCreateParams);
		result.chipType = ChipType.Unknown;
		if (generationSettings == null || generationSettings.chipSettings.Count == 0)
		{
			return result;
		}
		if ((float)this.RandomRange(0, 100) > generationSettings.weight)
		{
			return result;
		}
		float num = 0f;
		for (int i = 0; i < generationSettings.chipSettings.Count; i++)
		{
			LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting = generationSettings.chipSettings[i];
			num += chipSetting.weight;
		}
		float num2 = this.RandomRange(0f, num);
		result.hasIce = generationSettings.hasIce;
		result.iceLevel = generationSettings.iceLevel;
		for (int j = 0; j < generationSettings.chipSettings.Count; j++)
		{
			LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting2 = generationSettings.chipSettings[j];
			num2 -= chipSetting2.weight;
			bool flag = j == generationSettings.chipSettings.Count - 1;
			if (num2 <= 0f || flag)
			{
				result.chipType = chipSetting2.chipType;
				result.itemColor = chipSetting2.itemColor;
				return result;
			}
		}
		return result;
	}

	public Match3Board.ChipCreateParams RandomChip(ItemColor colorToIgnore = ItemColor.Unknown)
	{
		Match3Board.ChipCreateParams result = default(Match3Board.ChipCreateParams);
		result.chipType = ChipType.Chip;
		if (this.generationSettings.isConfigured)
		{
			float num = 0f;
			for (int i = 0; i < this.generationSettings.chipSettings.Count; i++)
			{
				LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting = this.generationSettings.chipSettings[i];
				num += chipSetting.weight;
			}
			float num2 = this.RandomRange(0f, num);
			for (int j = 0; j < this.generationSettings.chipSettings.Count; j++)
			{
				LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting2 = this.generationSettings.chipSettings[j];
				num2 -= chipSetting2.weight;
				bool flag = j == this.generationSettings.chipSettings.Count - 1;
				if (num2 <= 0f || flag)
				{
					result.chipType = chipSetting2.chipType;
					result.itemColor = chipSetting2.itemColor;
					return result;
				}
			}
		}
		if (this.availableColors.Count == 0)
		{
			result.itemColor = (ItemColor)this.RandomRange(0, 5);
			return result;
		}
		int num3 = this.RandomRange(0, this.availableColors.Count);
		if (num3 < 0)
		{
			num3 = this.availableColors.Count + num3;
		}
		num3 %= this.availableColors.Count;
		ItemColor itemColor = this.availableColors[num3];
		result.itemColor = itemColor;
		return result;
	}

	public ItemColor RandomColor(ItemColor colorToIgnore = ItemColor.Unknown)
	{
		if (this.generationSettings.isConfigured)
		{
			float num = 0f;
			for (int i = 0; i < this.generationSettings.chipSettings.Count; i++)
			{
				LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting = this.generationSettings.chipSettings[i];
				num += chipSetting.weight;
			}
			float num2 = this.RandomRange(0f, num);
			for (int j = 0; j < this.generationSettings.chipSettings.Count; j++)
			{
				LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting2 = this.generationSettings.chipSettings[j];
				num2 -= chipSetting2.weight;
				bool flag = j == this.generationSettings.chipSettings.Count - 1;
				if (num2 <= 0f || flag)
				{
					return chipSetting2.itemColor;
				}
			}
		}
		if (this.availableColors.Count == 0)
		{
			return (ItemColor)this.RandomRange(0, 5);
		}
		int num3 = this.RandomRange(0, this.availableColors.Count);
		if (num3 < 0)
		{
			num3 = this.availableColors.Count + num3;
		}
		num3 %= this.availableColors.Count;
		return this.availableColors[num3];
	}

	public void Add(BoardComponent boardComponent)
	{
		this.boardComponents.Add(boardComponent);
	}

	public int totalAdditionalMoves
	{
		get
		{
			return this.additionalMoves + this.collectedAdditionalMoves;
		}
	}

	public bool isInteractionSuspendedBecausePowerupAnimation
	{
		get
		{
			return this.powerupAnimationsInProgress > 0;
		}
	}

	public bool IsOutOfBoard(IntVector2 position)
	{
		return !this.IsInBoard(position);
	}

	public bool IsInBoard(IntVector2 position)
	{
		return position.x >= 0 && position.y >= 0 && position.x < this.size.x && position.y < this.size.y;
	}

	public int DistanceOutsideBoard(IntVector2 position)
	{
		IntVector2 intVector = default(IntVector2);
		if (position.x < 0)
		{
			intVector.x = Mathf.Abs(position.x);
		}
		if (position.x >= this.size.x)
		{
			intVector.x = position.x - this.size.x + 1;
		}
		if (position.y < 0)
		{
			intVector.y = Mathf.Abs(position.y);
		}
		if (position.y >= this.size.y)
		{
			intVector.y = position.y - this.size.y + 1;
		}
		return Mathf.Max(intVector.x, intVector.y);
	}

	public void Init(IntVector2 size)
	{
		this.slots = new Slot[size.x * size.y];
		this.size = size;
		this.findMatches = new FindMatches();
		this.findMatchesOutside = new FindMatches();
		this.findMatchesOutside.Init(this);
		this.findMatches.Init(this);
	}

	public int Index(IntVector2 position)
	{
		return position.x + position.y * this.size.x;
	}

	public IntVector2 PositionFromIndex(int index)
	{
		return new IntVector2(index % this.size.x, index / this.size.x);
	}

	public void SetSlot(IntVector2 position, Slot slot)
	{
		this.slots[this.Index(position)] = slot;
	}

	public Slot GetSlot(IntVector2 position)
	{
		if (position.x >= this.size.x || position.x < 0 || position.y >= this.size.y || position.y < 0)
		{
			return null;
		}
		return this.slots[this.Index(position)];
	}

	public Match3Board.MatchesInBoard matchesInBoard = new Match3Board.MatchesInBoard();

	public Match3Board.TimedCounter matchCounter = new Match3Board.TimedCounter();

	public GameStats gameStats = new GameStats();

	public FindMatches findMatches;

	public FindMatches findMatchesOutside;

	public PopulateBoard populateBoard = new PopulateBoard();

	public PotentialMatches potentialMatches = new PotentialMatches();

	public PowerupCombines powerupCombines = new PowerupCombines();

	public PowerupActivations powerupActivations = new PowerupActivations();

	public List<ItemColor> availableColors = new List<ItemColor>();

	public RandomProvider randomProvider = new RandomProvider();

	private List<ItemColor> selectedColors = new List<ItemColor>();

	private int maxSelectedColorCount = 10;

	private int randomNumber;

	public LevelDefinition.ChipGenerationSettings generationSettings = new LevelDefinition.ChipGenerationSettings();

	public Slot[] slots;

	public List<Slot> sortedSlotsUpdateList = new List<Slot>();

	public List<BoardComponent> boardComponents = new List<BoardComponent>();

	public BubblesBoardComponent bubblesBoardComponent;

	public IntVector2 size;

	public bool isGameEnded;

	public bool isBoardSettled;

	public bool isShufflingBoard;

	public int additionalMoves;

	public int collectedAdditionalMoves;

	public bool ignoreEndConditions;

	public bool isInteractionSuspended;

	public int powerupAnimationsInProgress;

	public bool isPowerupSelectionActive;

	public bool isEndConditionReached;

	public bool isUpdateSuspended;

	public bool isGameSoundsSuspended;

	public bool isAnyConveyorMoveSuspended;

	public int moveCountWhenConveyorTookAction;

	public ActionManager actionManager = new ActionManager();

	public BurriedElements burriedElements = new BurriedElements();

	public MonsterElements monsterElements = new MonsterElements();

	public CarpetSpread carpet = new CarpetSpread();

	public int generatedChipsCount;

	public bool hasMoreMoves;

	public ActionManager nonChangeStateActionMenager = new ActionManager();

	public long currentFrameIndex;

	public float currentTime;

	public int userMovesCount;

	public int userScore;

	public int lastSettledMove;

	public long currentCoins;

	public float lastTimeWhenUserMadeMove;

	public long lastFrameWhenUserMadeMove;

	public int currentMatchesCount;

	public float currentDeltaTime;

	public bool isDirtyInCurrentFrame;

	public class MatchesInBoard
	{
		public void AddMatch(int currentMoveIndex)
		{
			if (this.currentMove != currentMoveIndex)
			{
				this.currentMove = currentMoveIndex;
				this.matchesCount = 0;
			}
			this.matchesCount++;
		}

		public int currentMove;

		public int matchesCount;
	}

	public class TimedCounter
	{
		public void OnUserMadeMove()
		{
			this.eventCount = 0;
			this.timeLeft = 15f;
		}

		public void Update(float deltaTime)
		{
			this.timeLeft -= deltaTime;
			if (this.timeLeft <= 0f)
			{
				this.eventCount = 0;
				this.timeLeft = 0f;
			}
		}

		public float timeLeft;

		public int eventCount;
	}

	public struct ChipCreateParams
	{
		public ChipType chipType;

		public ItemColor itemColor;

		public bool hasIce;

		public int iceLevel;
	}
}
