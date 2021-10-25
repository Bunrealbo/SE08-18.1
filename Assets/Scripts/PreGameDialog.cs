using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;

public class PreGameDialog : MonoBehaviour
{
	public void Show(Match3StagesDB.Stage stage, DecorateRoomScreen screen, Action onHide = null, Action<Match3GameParams> onStart = null)
	{
		this.screen = screen;
		this.onHide = onHide;
		this.stage = stage;
		this.onStart = onStart;
		NavigationManager.instance.Push(base.gameObject, true);
	}

	public void Init(Match3StagesDB.Stage stage, DecorateRoomScreen screen, Action onHide = null, Action<Match3GameParams> onStart = null)
	{
		this.screen = screen;
		this.onHide = onHide;
		this.onStart = onStart;
		this.stage = stage;
		if (this.rankedBooster != null)
		{
			this.rankedBooster.Init(ScriptableObjectSingleton<GiftsDefinitionDB>.instance.buildupBooster.currentBoosterLevel);
		}
		GGUtil.SetActive(this.widgetsToHide, false);
		GoalsDefinition goals = stage.levelReference.level.goals;
		GGUtil.ChangeText(this.stageNameLabel, string.Format("Level {0}", stage.index + 1));
		if (stage.shouldUseStarDialog)
		{
			GGUtil.ChangeText(this.stageNameLabel, "Get a Star");
			this.starStyle.Apply();
		}
		else
		{
			this.boostersStyle.Apply();
		}
		this.goalsPool.Clear();
		this.goalsPool.HideNotUsed();
		List<GoalConfig> goals2;
		if (stage.multiLevelReference.Count > 0)
		{
			PreGameDialog.StageGoalsConfig stageGoalsConfig = new PreGameDialog.StageGoalsConfig();
			for (int i = 0; i < stage.multiLevelReference.Count; i++)
			{
				LevelDefinition level = stage.multiLevelReference[i].level;
				for (int j = 0; j < level.goals.goals.Count; j++)
				{
					GoalConfig newGoal = level.goals.goals[j];
					stageGoalsConfig.AddGoal(newGoal);
				}
			}
			goals2 = stageGoalsConfig.goals;
		}
		else
		{
			goals2 = stage.levelReference.level.goals.goals;
		}
		GGUtil.Hide(this.levelDifficultyWidgets);
		if (stage.difficulty == Match3StagesDB.Stage.Difficulty.Normal)
		{
			this.normalDifficultySlyle.Apply();
		}
		else if (stage.difficulty == Match3StagesDB.Stage.Difficulty.Hard)
		{
			this.hardDifficultySlyle.Apply();
		}
		else if (stage.difficulty == Match3StagesDB.Stage.Difficulty.Nightmare)
		{
			this.nightmareDifficultySlyle.Apply();
		}
		for (int k = 0; k < goals2.Count; k++)
		{
			GoalConfig config = goals2[k];
			this.goalsPool.Next<PreGameDialogGoalsPrefab>(true).Init(config, stage);
		}
		List<BoosterConfig> list = new List<BoosterConfig>();
		for (int l = 0; l < Match3StagesDB.instance.defaultBoosters.Count; l++)
		{
			BoosterConfig boosterConfig = Match3StagesDB.instance.defaultBoosters[l];
			if (!stage.forbittenBoosters.Contains(boosterConfig.boosterType))
			{
				list.Add(boosterConfig);
			}
		}
		this.boostersPool.Clear();
		this.boostersPool.HideNotUsed();
		this.usedBoosters.Clear();
		for (int m = 0; m < list.Count; m++)
		{
			BoosterConfig boosterConfig2 = list[m];
			PreGameDialogBoosterPrefab preGameDialogBoosterPrefab = this.boostersPool.Next<PreGameDialogBoosterPrefab>(true);
			preGameDialogBoosterPrefab.Init(boosterConfig2, this, true);
			this.usedBoosters.Add(preGameDialogBoosterPrefab);
		}
		this.collectableGoalStyle.Apply();
	}

	public void ButtonCallback_OnPlayButtonClicked()
	{
		GGUtil.SetActive(base.gameObject, false);
		NavigationManager.instance.Pop(true);
		Match3GameParams match3GameParams = new Match3GameParams();
		for (int i = 0; i < this.usedBoosters.Count; i++)
		{
			PreGameDialogBoosterPrefab preGameDialogBoosterPrefab = this.usedBoosters[i];
			if (PlayerInventory.instance.OwnedCount(preGameDialogBoosterPrefab.GetBooster().boosterType) > 0L && preGameDialogBoosterPrefab.IsActive())
			{
				match3GameParams.activeBoosters.Add(preGameDialogBoosterPrefab.GetBooster());
				match3GameParams.boughtBoosters.Add(preGameDialogBoosterPrefab.GetBooster());
			}
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
		if (this.onStart != null)
		{
			this.onStart(match3GameParams);
		}
		if (this.screen != null)
		{
			this.screen.StartGame(match3GameParams);
		}
	}

	public void OnBoosterClicked(PreGameDialogBoosterPrefab.BoosterDefinition booster)
	{
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
		if (PlayerInventory.instance.OwnedCount(booster.config.boosterType) > 0L)
		{
			booster.active = !booster.active;
			return;
		}
		NavigationManager.instance.GetObject<CurrencyPurchaseDialog>().Show(ScriptableObjectSingleton<OffersDB>.instance, null);
	}

	public void ButtonCallback_Hide()
	{
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
		if (this.onHide != null)
		{
			this.onHide();
		}
		GGUtil.SetActive(base.gameObject, false);
		NavigationManager.instance.Pop(true);
	}

	private void OnEnable()
	{
		this.Init(this.stage, this.screen, this.onHide, this.onStart);
	}

	[SerializeField]
	private List<Transform> levelDifficultyWidgets = new List<Transform>();

	[SerializeField]
	private VisualStyleSet normalDifficultySlyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet hardDifficultySlyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet nightmareDifficultySlyle = new VisualStyleSet();

	[SerializeField]
	private TextMeshProUGUI stageNameLabel;

	[SerializeField]
	private ComponentPool goalsPool = new ComponentPool();

	[SerializeField]
	private ComponentPool boostersPool = new ComponentPool();

	private DecorateRoomScreen screen;

	private List<PreGameDialogBoosterPrefab> usedBoosters = new List<PreGameDialogBoosterPrefab>();

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private VisualStyleSet starStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet boostersStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet collectableGoalStyle;

	[SerializeField]
	private VisualStyleSet messageGoalStyleA;

	[SerializeField]
	private VisualStyleSet messageGoalStyleB;

	[SerializeField]
	private RankedBoostersContainer rankedBooster;

	public Action onHide;

	public Action<Match3GameParams> onStart;

	public Match3StagesDB.Stage stage;

	public class StageGoalsConfig
	{
		public void AddGoal(GoalConfig newGoal)
		{
			for (int i = 0; i < this.goals.Count; i++)
			{
				GoalConfig goalConfig = this.goals[i];
				if (goalConfig.IsCompatible(newGoal))
				{
					goalConfig.collectCount += newGoal.collectCount;
					return;
				}
			}
			this.goals.Add(newGoal.Clone());
		}

		public List<GoalConfig> goals = new List<GoalConfig>();
	}
}
