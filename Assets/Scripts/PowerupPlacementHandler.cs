using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;

public class PowerupPlacementHandler : MonoBehaviour
{
	public void Show(PowerupPlacementHandler.InitArguments initArguments)
	{
		this.initArguments = initArguments;
		this.inputHandler.Clear();
		GGUtil.Show(this);
		Match3Game game = initArguments.game;
		this.state.isActive = true;
		LevelDefinitionTilesSlotsProvider provider = new LevelDefinitionTilesSlotsProvider(game.createBoardArguments.level);
		Match3Game.TutorialSlotHighlighter tutorialHighlighter = game.tutorialHighlighter;
		tutorialHighlighter.Show(provider);
		tutorialHighlighter.SetTutorialBackgroundActive(false);
		GGUtil.SetActive(this.widgetsToHide, false);
		PowerupPlacementHandler.PowerupDefinition definition = this.GetDefinition(initArguments.powerup.type);
		if (definition != null)
		{
			definition.Show();
		}
		GGUtil.ChangeText(this.descriptionLabel, initArguments.powerup.description);
	}

	private PowerupPlacementHandler.PowerupDefinition GetDefinition(PowerupType powerupType)
	{
		for (int i = 0; i < this.powerups.Count; i++)
		{
			PowerupPlacementHandler.PowerupDefinition powerupDefinition = this.powerups[i];
			if (powerupDefinition.powerupType == powerupType)
			{
				return powerupDefinition;
			}
		}
		return null;
	}

	private void Hide()
	{
		Match3Game game = this.initArguments.game;
		if (game != null)
		{
			game.tutorialHighlighter.Hide();
			game.gameScreen.inputHandler.Clear();
		}
		GGUtil.Hide(this);
		this.state.isActive = false;
	}

	private void Update()
	{
		if (!this.state.isActive)
		{
			return;
		}
		InputHandler.PointerData pointerData = this.inputHandler.FirstDownPointer();
		if (pointerData != null)
		{
			this.OnPress(pointerData);
		}
	}

	private void OnComplete(PowerupPlacementHandler.PlacementCompleteArguments completeArguments)
	{
		if (this.initArguments.onComplete == null)
		{
			return;
		}
		this.initArguments.onComplete(completeArguments);
	}

	private void OnPress(InputHandler.PointerData pointer)
	{
		PowerupPlacementHandler.PlacementCompleteArguments completeArguments = default(PowerupPlacementHandler.PlacementCompleteArguments);
		completeArguments.initArguments = this.initArguments;
		Vector2 position = pointer.position;
		Match3Game game = this.initArguments.game;
		Slot slotFromMousePosition = game.input.GetSlotFromMousePosition(pointer.position);
		completeArguments.targetSlot = slotFromMousePosition;
		if (slotFromMousePosition == null)
		{
			game.Play(GGSoundSystem.SFXType.CancelPress);
			this.Hide();
			this.OnComplete(completeArguments);
			return;
		}
		PowerupsDB.PowerupDefinition powerup = this.initArguments.powerup;
		powerup.ownedCount = Math.Max(0L, powerup.ownedCount - 1L);
		game.gameScreen.powerupsPanel.Refresh();
		this.Hide();
		completeArguments.isSuccess = true;
		this.OnComplete(completeArguments);
		game.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	[SerializeField]
	private InputHandler inputHandler;

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private List<PowerupPlacementHandler.PowerupDefinition> powerups = new List<PowerupPlacementHandler.PowerupDefinition>();

	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	private PowerupPlacementHandler.InitArguments initArguments;

	private PowerupPlacementHandler.State state;

	[Serializable]
	private class PowerupDefinition
	{
		public void Show()
		{
			GGUtil.Show(this.container);
		}

		public PowerupType powerupType;

		public RectTransform container;
	}

	public struct PlacementCompleteArguments
	{
		public bool isCancel
		{
			get
			{
				return !this.isSuccess;
			}
		}

		public PowerupPlacementHandler.InitArguments initArguments;

		public bool isSuccess;

		public Slot targetSlot;
	}

	public struct InitArguments
	{
		public PowerupsDB.PowerupDefinition powerup;

		public Match3Game game;

		public Action<PowerupPlacementHandler.PlacementCompleteArguments> onComplete;
	}

	private struct State
	{
		public bool isActive;
	}
}
