using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

public class PreGameDialogBoosterPrefab : MonoBehaviour
{
	public void Init(BoosterConfig boosterConfig, PreGameDialog screen, bool resetSelection = false)
	{
		this.screen = screen;
		this.booster.config = boosterConfig;
		GGUtil.SetActive(this.widgetsToHide, false);
		if (resetSelection)
		{
			this.booster.active = false;
		}
		long num = PlayerInventory.instance.OwnedCount(boosterConfig.boosterType);
		PlayerInventory.instance.UsedCount(boosterConfig.boosterType);
		if (num > 0L)
		{
			this.activeStyle.Apply();
		}
		else
		{
			this.notActiveStyle.Apply();
		}
		for (int i = 0; i < this.namedSprites.Count; i++)
		{
			PreGameDialogBoosterPrefab.NamedSprite namedSprite = this.namedSprites[i];
			if (namedSprite.IsMatching(this.booster))
			{
				namedSprite.SetActive(true);
				namedSprite.SetLabel(string.Format("x{0}", num));
				namedSprite.SetStyle(num > 0L);
			}
		}
		bool active = num > 0L && !this.booster.active && screen.stage.timesPlayed > 0;
		GGUtil.SetActive(this.arrowAnimation, active);
		GGUtil.SetActive(this.activeImage, this.booster.active);
	}

	public bool IsActive()
	{
		return this.booster.active;
	}

	public BoosterConfig GetBooster()
	{
		return this.booster.config;
	}

	public void ButtonCallback_OnClick()
	{
		this.screen.OnBoosterClicked(this.booster);
		this.Init(this.booster.config, this.screen, false);
	}

	[SerializeField]
	private Image activeImage;

	[SerializeField]
	public List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	public List<PreGameDialogBoosterPrefab.NamedSprite> namedSprites = new List<PreGameDialogBoosterPrefab.NamedSprite>();

	[SerializeField]
	private Transform arrowAnimation;

	[SerializeField]
	private PowerupsPanelPowerup.ItemStyleSet activeStyle = new PowerupsPanelPowerup.ItemStyleSet();

	[SerializeField]
	private PowerupsPanelPowerup.ItemStyleSet notActiveStyle = new PowerupsPanelPowerup.ItemStyleSet();

	private PreGameDialog screen;

	private PreGameDialogBoosterPrefab.BoosterDefinition booster = new PreGameDialogBoosterPrefab.BoosterDefinition();

	public class BoosterDefinition
	{
		public BoosterConfig config;

		public bool active;
	}

	[Serializable]
	public class NamedSprite
	{
		public bool IsMatching(PreGameDialogBoosterPrefab.BoosterDefinition booster)
		{
			return booster.config.boosterType == this.type;
		}

		public void SetActive(bool active)
		{
			GGUtil.SetActive(this.visualConfig.transform, active);
			if (active)
			{
				this.visualConfig.Init(this.chipTypeUsedForRepresentation);
			}
		}

		public void SetLabel(string text)
		{
			this.visualConfig.SetLabel(text);
		}

		public void SetStyle(bool owned)
		{
			this.visualConfig.SetStyle(owned);
		}

		public PreGameDialogBoosterPrefabVisualConfig visualConfig;

		public BoosterType type;

		public ChipType chipTypeUsedForRepresentation;
	}
}
