using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class ExitGameConfirmDialog : MonoBehaviour
{
	public void Show(ExitGameConfirmDialog.ExitGameConfirmArguments arguments)
	{
		this.arguments = arguments;
		NavigationManager.instance.Push(base.gameObject, true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
	}

	public void Init()
	{
		this.goalPrefabsPool.Clear();
		this.goalPrefabsPool.HideNotUsed();
		List<MultiLevelGoals.Goal> activeGoals = this.arguments.goals.GetActiveGoals();
		for (int i = 0; i < activeGoals.Count; i++)
		{
			MultiLevelGoals.Goal goal = activeGoals[i];
			this.goalPrefabsPool.Next<GoalsPanelGoal>(true).Init(goal);
		}
		if (this.rankedContainer != null)
		{
			this.rankedContainer.Init(this.arguments.game.initParams.giftBoosterLevel);
		}
	}

	public void OnEnable()
	{
		this.Init();
	}

	public void ButtonCallback_OnExit()
	{
		NavigationManager.instance.Pop(true);
		if (this.arguments.onCompleteCallback != null)
		{
			this.arguments.onCompleteCallback(false);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void ButtonCallback_OnQuit()
	{
		NavigationManager.instance.Pop(true);
		if (this.arguments.onCompleteCallback != null)
		{
			this.arguments.onCompleteCallback(true);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonConfirm);
	}

	[SerializeField]
	private ComponentPool goalPrefabsPool = new ComponentPool();

	[SerializeField]
	private RankedBoostersContainer rankedContainer;

	private ExitGameConfirmDialog.ExitGameConfirmArguments arguments;

	public struct ExitGameConfirmArguments
	{
		public Action<bool> onCompleteCallback;

		public MultiLevelGoals goals;

		public Match3Game game;
	}
}
