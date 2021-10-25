using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;

public class OutOfMovesDialog : MonoBehaviour
{
	public void Show(BuyMovesPricesConfig.OfferConfig offer, Match3Game game, MultiLevelGoals goals, OutOfMovesDialog.OutOfMovesDelegate onYes, OutOfMovesDialog.OutOfMovesDelegate onNo)
	{
		this.offer = offer;
		this.game = game;
		this.goals = goals;
		this.onYes = onYes;
		this.onNo = onNo;
		NavigationManager.instance.Push(base.gameObject, true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
	}

	public void Hide()
	{
		NavigationManager.instance.Pop(true);
	}

	public void OnBuyClicked()
	{
		if (this.onYes != null)
		{
			this.onYes(this);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	public void OnNotBuyClicked()
	{
		if (this.onNo != null)
		{
			this.onNo(this);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void Init(BuyMovesPricesConfig.OfferConfig offer)
	{
		if(this.buttonLabel != null)
			this.buttonLabel.text = string.Format(this.playButtonFormat, offer.price.cost);
		if(this.movesCounterLabel != null)
			this.movesCounterLabel.text = string.Format(this.movesCountFormat, offer.movesCount);
		List<MultiLevelGoals.Goal> activeGoals = this.goals.GetActiveGoals();
		if (this.rankedBoosters != null)
		{
			this.rankedBoosters.Init(this.game.initParams.giftBoosterLevel);
		}
		this.goalsPool.Clear();
		for (int i = 0; i < activeGoals.Count; i++)
		{
			MultiLevelGoals.Goal goal = activeGoals[i];
			GoalsPanelGoal goalsPanelGoal = this.goalsPool.Next<GoalsPanelGoal>(true);
			goalsPanelGoal.Init(goal);
			goalsPanelGoal.UpdateCollectedCount();
		}
		this.goalsPool.HideNotUsed();
		this.powerupsPool.Clear();
		for (int j = 0; j < offer.powerups.Count; j++)
		{
			BuyMovesPricesConfig.OfferConfig.PowerupDefinition powerupDefinition = offer.powerups[j];
			this.powerupsPool.Next<OutOfMovesDialogPowerup>(true).Init(powerupDefinition.powerupType, powerupDefinition.count);
		}
		this.powerupsPool.HideNotUsed();
		if(this.coinsLabel != null)
			this.coinsLabel.text = GGPlayerSettings.instance.walletManager.CurrencyCount(CurrencyType.coins).ToString();
	}

	public void OnEnable()
	{
		this.Init(this.offer);
	}

	[SerializeField]
	private string playButtonFormat;

	[SerializeField]
	private string movesCountFormat;

	[SerializeField]
	private TextMeshProUGUI buttonLabel;

	[SerializeField]
	private TextMeshProUGUI movesCounterLabel;

	[SerializeField]
	private TextMeshProUGUI coinsLabel;

	[SerializeField]
	private ComponentPool goalsPool = new ComponentPool();

	[SerializeField]
	private ComponentPool powerupsPool = new ComponentPool();

	[SerializeField]
	private RankedBoostersContainer rankedBoosters;

	[NonSerialized]
	public Match3Game game;

	private OutOfMovesDialog.OutOfMovesDelegate onYes;

	private OutOfMovesDialog.OutOfMovesDelegate onNo;

	public BuyMovesPricesConfig.OfferConfig offer;

	private MultiLevelGoals goals;

	public delegate void OutOfMovesDelegate(OutOfMovesDialog dialog);
}
