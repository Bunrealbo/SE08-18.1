using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GGMatch3
{
	public class GoalsPanel : MonoBehaviour
	{
		public void Init(GameScreen.StageState stageState)
		{
			this.visibilityHelper.SetActive(this.widgetsToHide, false);
			this.goalsDisplayStyle.Apply(this.visibilityHelper);
			this.visibilityHelper.Complete();
			this.stageState = stageState;
			Vector2 prefabSizeDelta = this.goalsPool.prefabSizeDelta;
			List<MultiLevelGoals.Goal> allGoals = stageState.goals.allGoals;
			Vector2 sizeDelta = this.goalsContainer.sizeDelta;
			Vector3 a = new Vector3(0f, prefabSizeDelta.y * ((float)allGoals.Count * 0.5f - 0.5f), 0f);
			this.goalsPool.Clear();
			this.uiGoalsList.Clear();
			for (int i = 0; i < allGoals.Count; i++)
			{
				MultiLevelGoals.Goal goal = allGoals[i];
				GoalsPanelGoal goalsPanelGoal = this.goalsPool.Next<GoalsPanelGoal>(true);
				goalsPanelGoal.transform.localPosition = a + Vector3.down * (prefabSizeDelta.y * (float)i);
				goalsPanelGoal.Init(goal);
				this.uiGoalsList.Add(goalsPanelGoal);
			}
			this.goalsPool.HideNotUsed();
			this.UpdateMovesCount();
			this.currentDisplayedScore = (this.desiredScore = stageState.userScore);
			this.UpdateScore();
		}

		public void UpdateScore()
		{
			this.desiredScore = this.stageState.userScore;
		}

		public void ShowCoins()
		{
			this.visibilityHelper.SetActive(this.widgetsToHide, false);
			this.coinsDisplayStyle.Apply(this.visibilityHelper);
			this.visibilityHelper.Complete();
		}

		public void SetCoinsCount(long coinsCount)
		{
			GGUtil.ChangeText(this.coinsCountLabel, coinsCount);
		}

		public GoalsPanelGoal GetGoal(Match3Goals.GoalBase goal)
		{
			if (goal == null)
			{
				return null;
			}
			for (int i = 0; i < this.uiGoalsList.Count; i++)
			{
				GoalsPanelGoal goalsPanelGoal = this.uiGoalsList[i];
				if (goalsPanelGoal.goal.IsCompatible(goal))
				{
					return goalsPanelGoal;
				}
			}
			return null;
		}

		public void UpdateMovesCount()
		{
			int movesRemaining = this.stageState.MovesRemaining;
			GGUtil.ChangeText(this.movesCountLabel, movesRemaining.ToString());
		}

		private void Update()
		{
			if (this.currentDisplayedScore < this.desiredScore)
			{
				GeneralSettings generalSettings = Match3Settings.instance.generalSettings;
				float a = (float)this.currentDisplayedScore + generalSettings.scoreSpeed * Time.deltaTime;
				float b = Mathf.Lerp((float)this.currentDisplayedScore, (float)this.desiredScore, Time.deltaTime * generalSettings.lerpSpeed);
				float f = Mathf.Max(a, b);
				this.currentDisplayedScore = Mathf.Min(Mathf.RoundToInt(f), this.desiredScore);
				GGUtil.ChangeText(this.pointsLabel, this.currentDisplayedScore.ToString());
			}
		}

		[SerializeField]
		private List<Transform> widgetsToHide = new List<Transform>();

		[SerializeField]
		private VisualStyleSet goalsDisplayStyle = new VisualStyleSet();

		[SerializeField]
		private VisualStyleSet coinsDisplayStyle = new VisualStyleSet();

		[SerializeField]
		private ComponentPool goalsPool = new ComponentPool();

		[SerializeField]
		public TextMeshProUGUI movesCountLabel;

		[SerializeField]
		private RectTransform goalsContainer;

		[SerializeField]
		public RectTransform coinsTransform;

		[SerializeField]
		private TextMeshProUGUI coinsCountLabel;

		private List<GoalsPanelGoal> uiGoalsList = new List<GoalsPanelGoal>();

		private GameScreen.StageState stageState;

		private VisibilityHelper visibilityHelper = new VisibilityHelper();

		[SerializeField]
		private TextMeshProUGUI pointsLabel;

		private int currentDisplayedScore;

		private int desiredScore;
	}
}
